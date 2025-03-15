// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NLog;
using NLog.Fluent;
using NodaTime;

namespace TA.SnapCap.HardwareSimulator
    {
    /// <summary>Class SimulatorStateMachine. This class cannot be inherited.</summary>
    public sealed class SimulatorStateMachine : ISimulatorStateTriggers, INotifyPropertyChanged
        {
        /// <summary>Custom delegate signature for state machine static events.</summary>
        public delegate void StateEventHandler(StateEventArgs e);

        /// <summary>
        ///     Thread synchronization object which tells waiting threads when the state machine is in the
        ///     'idle' or 'ready' state.
        /// </summary>
        public readonly ManualResetEvent InReadyState = new ManualResetEvent(false);

        private readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly Subject<char> receiveSubject = new Subject<char>();
        private readonly IClock timeSource;
        private readonly Subject<char> transmitSubject = new Subject<char>();
        private uint lampBrightness;
        private bool lampOn;
        private MotorDirection motorDirection;
        private bool motorEnergized;
        private InputParser parser;

        /// <summary>
        ///     Characters received from the serial port, which accumulate until a valid command has been
        ///     received.
        /// </summary>
        internal StringBuilder ReceivedChars = new StringBuilder();

        private CancellationTokenSource simulatedDelayCancellation = new CancellationTokenSource();
        private SystemStatus systemStatus;

        /// <summary>Initializes a new instance of the <see cref="SimulatorStateMachine" /> class.</summary>
        /// <param name="realTime">
        ///     When <c>true</c> the simulator introduces pauses that are representative of real equipment.
        ///     When <c>false</c>, the simulation proceeds at an accelerated pace with no pauses.
        /// </param>
        /// <param name="timeSource">A source of the current time.</param>
        public SimulatorStateMachine(bool realTime, IClock timeSource, InputParser parser)
            {
            Contract.Requires(timeSource != null);
            RealTime = realTime;
            this.timeSource = timeSource;
            this.parser = parser;
            }

        [IgnoreAutoChangeNotification]
        internal SimulatorState CurrentState { get; set; }

        /// <summary>
        ///     An observable sequence of characters that simulates data arriving from the dome controller to
        ///     the PC serial port.
        /// </summary>
        [IgnoreAutoChangeNotification]
        public IObservable<char> ObservableResponses => transmitSubject.AsObservable();

        /// <summary>
        ///     Simulate sending characters to the dome controller by calling the observer's
        ///     <see cref="IObserver{T}.OnNext" /> method.
        /// </summary>
        [IgnoreAutoChangeNotification]
        public IObserver<char> InputObserver => receiveSubject.AsObserver();

        /// <summary>
        ///     A flag indicating whether the simulation should proceed in real time (<c>true</c>) or at an
        ///     accelerated pace (<c>false</c>).
        /// </summary>
        public bool RealTime { get; }

        public bool MotorEnergized
            {
            get => motorEnergized;
            set => SetField(ref motorEnergized, value);
            }

        public MotorDirection MotorDirection
            {
            get => motorDirection;
            set => SetField(ref motorDirection, value);
            }

        /// <summary>Gets or sets a value indicating whether the lamp is currently turned on.</summary>
        /// <value><c>true</c> if the lamp is on; otherwise, <c>false</c>.</value>
        /// <remarks>
        ///     Changing this property triggers the necessary state transitions and updates the simulator's
        ///     internal state to reflect the lamp's status.
        /// </remarks>
        public bool LampOn
            {
            get => lampOn;
            set => SetField(ref lampOn, value);
            }

        public SystemStatus SystemStatus
            {
            get => systemStatus;
            internal set => SetField(ref systemStatus, value);
            }

        public uint LampBrightness
            {
            get => lampBrightness;
            set => SetField(ref lampBrightness, value);
            }

        public void CloseRequested()
            {
            CurrentState.CloseRequested();
            }

        /// <inheritdoc />
        public void HaltRequested()
            {
            CurrentState.HaltRequested();
            }

        /// <inheritdoc />
        public void LampOnRequested()
            {
            CurrentState.LampOnRequested();
            }

        /// <inheritdoc />
        public void LampOffRequested()
            {
            CurrentState.LampOffRequested();
            }

        /// <inheritdoc />
        public void SetLampBrightness(uint brightness)
            {
            CurrentState.SetLampBrightness(brightness);
            }

        public void GetLampBrightness()
            {
            CurrentState.GetLampBrightness();
            }

        public Task WhenStopped()
            {
            return InReadyState.AsTask();
            }

        /// <summary>Transitions the state machine to the specified new state.</summary>
        /// <param name="newState">The new state to transition to. Must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="newState" /> is <c>null</c>.</exception>
        /// <remarks>
        ///     This method handles the transition between states by invoking the <c>OnExit</c> method of the
        ///     current state and the <c>OnEnter</c> method of the new state. It also updates the
        ///     <c>CurrentState</c> property and raises a state change notification.
        /// </remarks>
        public void Transition(SimulatorState newState)
            {
            if (newState == null)
                throw new ArgumentNullException("newState", "Must reference a valid state instance.");
            InReadyState.Reset();
            if (CurrentState != null)
                try
                    {
                    CurrentState.OnExit();
                    }
                catch (Exception ex)
                    {
                    Log.Error().Exception(ex)
                        .Message("Exception in {state}.OnExit(): {message}", CurrentState.Name, ex.Message)
                        .Write();
                    }

            Log.Debug().Message("Transitioning from {oldState} => {newState}", CurrentState, newState).Write();
            CurrentState = newState;
            RaiseStateChanged(newState.Name);
            try
                {
                newState.OnEnter();
                }
            catch (Exception ex)
                {
                Log.Error().Exception(ex)
                    .Message("Exception in {state}.OnExit(): {message}", newState.Name, ex.Message)
                    .Write();
                }
            }

        /// <summary>Occurs when a state transition has occurred.</summary>
        public event StateEventHandler StateChanged;

        /// <summary>Raises the <see cref="StateChanged" /> event.</summary>
        /// <param name="e">The <see cref="StateEventArgs" /> instance containing the event data.</param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        private void RaiseStateChanged(string newState)
            {
            Contract.Requires(newState != null);
            var args = new StateEventArgs(newState);
            StateChanged?.Invoke(args);
            }

        /// <summary>Signals the end of the simulation by completing the transmission subject.</summary>
        /// <remarks>
        ///     This method is used to indicate that no further data will be transmitted and to notify any
        ///     observers of the completion of the transmission sequence.
        /// </remarks>
        private void EndOfSimulation()
            {
            transmitSubject.OnCompleted();
            }

        /// <summary>
        ///     Writes the tx data followed by a newline sequence. No need to invoke the SentData event because
        ///     Write() will do it.
        /// </summary>
        /// <param name="txData">The tx data.</param>
        internal void WriteLine(string txData)
            {
            if (!txData.EndsWith(Environment.NewLine))
                txData += Environment.NewLine;
            // Send the string one character at a time to the transmitSubject.
            foreach (var c in txData) transmitSubject.OnNext(c);
            }

        internal void SendResponse(string message)
            {
            WriteLine(message);
            }

        internal string BuildStatusResponse()
            {
            var response = new StringBuilder("*S");
            response.Append(MotorEnergized ? '1' : '0');
            response.Append(LampOn ? '1' : '0');
            response.Append((int)SystemStatus);
            response.AppendLine();
            return response.ToString();
            }

        /// <summary>Initializes the state machine with the specified starting state and input parser.</summary>
        /// <param name="startState">The initial state to transition the state machine into.</param>
        /// <param name="parser">The input parser to associate with the state machine.</param>
        /// <exception cref="ArgumentException">
        ///     Thrown when the provided <paramref name="parser" /> is not the same instance as the existing
        ///     parser.
        /// </exception>
        public void Initialize(SimulatorState startState, InputParser parser)
            {
            Transition(startState);
            if (!ReferenceEquals(parser, this.parser))
                throw new ArgumentException("Different parsers injected");
            this.parser = parser;
            var receiveObservable = receiveSubject.AsObservable();
            parser.SubscribeTo(receiveObservable, this);
            }

        /// <summary>
        ///     Simulates a delay that either completes immediately for a fast simulator or after the specified
        ///     duration for a real-time simulator.
        /// </summary>
        /// <param name="delay">The duration of the delay to simulate.</param>
        /// <returns>A <see cref="Task" /> that represents the simulated delay.</returns>
        public Task SimulatedDelay(TimeSpan delay)
            {
            simulatedDelayCancellation?.Cancel();
            simulatedDelayCancellation = new CancellationTokenSource();
            if (RealTime)
                return Task.Delay(delay, simulatedDelayCancellation.Token);
            return Task.CompletedTask;
            }

        public void CancelSimulatedDelay()
            {
            simulatedDelayCancellation.Cancel();
            }

        public void SignalStopped()
            {
            InReadyState.Set();
            }

        internal void SetSystemState(SystemStatus value)
            {
            SystemStatus = value;
            }

        #region Disposable pattern
        /// <summary>Releases the resources used by this object.</summary>
        /// <param name="disposing">
        ///     true to release both managed and unmanaged resources; false to release only
        ///     unmanaged resources.
        /// </param>
        private void Dispose(bool disposing)
            {
            if (disposing) transmitSubject?.OnCompleted();
            }

        /// <summary>User request to dispose the object releases both managed and unmanaged resources.</summary>
        public void Dispose()
            {
            Dispose(true);
            GC.SuppressFinalize(this);
            }

        /// <summary>
        ///     Releases unmanaged resources and performs other cleanup operations before the
        ///     <see cref="SimulatorStateMachine" /> is reclaimed by garbage collection.
        /// </summary>
        ~SimulatorStateMachine()
            {
            Dispose(false);
            }
        #endregion Disposable pattern

        #region State triggers
        /// <inheritdoc />
        public void OpenRequested()
            {
            CurrentState.OpenRequested();
            }

        /// <inheritdoc />
        public void QueryStatusRequested()
            {
            CurrentState.QueryStatusRequested();
            }
        #endregion

        #region INotifyPropertyChanged implementation (provided by Resharper)
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

        /// <summary>
        ///     Sets the specified field to the given value and raises the <see cref="PropertyChanged" /> event
        ///     if the value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the field.</typeparam>
        /// <param name="field">A reference to the field to be updated.</param>
        /// <param name="value">The new value to set.</param>
        /// <param name="propertyName">
        ///     The name of the property that changed. This parameter is optional and is automatically provided
        ///     by the compiler when called from a property setter.
        /// </param>
        /// <returns><c>true</c> if the field value was changed; otherwise, <c>false</c>.</returns>
        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
            {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
            }
        }
    #endregion INotifyPropertyChanged implementation (provided by Resharper)

    /// <summary>
    ///     Represents an attribute that indicates the associated member should ignore automatic change
    ///     notifications.
    /// </summary>
    /// <remarks>
    ///     This attribute is used to mark properties or fields that should not trigger automatic
    ///     notifications when their values change. It is typically applied in scenarios where change
    ///     notifications are managed manually or are not required.
    /// </remarks>
    internal class IgnoreAutoChangeNotificationAttribute : Attribute { }
    }