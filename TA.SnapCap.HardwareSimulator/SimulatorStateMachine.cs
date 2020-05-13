// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: SimulatorStateMachine.cs  Last modified: 2020-02-25@23:51 by Tim Long

using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NLog.Fluent;
using NodaTime;

namespace TA.SnapCap.HardwareSimulator
    {
    /// <summary>Class SimulatorStateMachine. This class cannot be inherited.</summary>
    public sealed class SimulatorStateMachine : ISimulatorStateTriggers
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
        private InputParser parser;
        private readonly IClock timeSource;
        private readonly Subject<char> transmitSubject = new Subject<char>();

        /// <summary>
        ///     Characters received from the serial port, which accumulate until a valid command has been
        ///     received.
        /// </summary>
        internal StringBuilder ReceivedChars = new StringBuilder();

        /// <summary>Initializes a new instance of the <see cref="SimulatorStateMachine" /> class.</summary>
        /// <param name="realTime">
        ///     When <c>true</c> the simulator introduces pauses that are representative of real equipment.
        ///     When <c>false</c>, the simulation proceeds at an accelerated pace with no pauses.
        /// </param>
        /// <param name="timeSource">A source of the current time.</param>
        public SimulatorStateMachine(bool realTime, IClock timeSource)
            {
            Contract.Requires(timeSource != null);
            RealTime = realTime;
            this.timeSource = timeSource;
            }

        internal SimulatorState CurrentState { get; set; }

        /// <summary>
        ///     An observable sequence of characters that simulates data arriving from the dome controller to
        ///     the PC serial port.
        /// </summary>
        public IObservable<char> ObservableResponses => transmitSubject.AsObservable();

        /// <summary>
        ///     Simulate sending characters to the dome controller by calling the observer's
        ///     <see cref="IObserver{T}.OnNext" /> method.
        /// </summary>
        public IObserver<char> InputObserver => receiveSubject.AsObserver();

        /// <summary>
        ///     A flag indicating whether the simulation should proceed in real time (<c>true</c>) or at an
        ///     accelerated pace (<c>false</c>).
        /// </summary>
        public bool RealTime { get; }

        public bool MotorEnergized { get; set; }
        public MotorDirection MotorDirection { get; set; }

        public Task WhenStopped()
            {
            return InReadyState.AsTask();
            }

        /// <summary>Transitions the state machine to the specified new state.</summary>
        /// <param name="newState">The new state.</param>
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

        private void EndOfSimulation()
            {
            transmitSubject.OnCompleted();
            }

        private void InputStimulus(char c)
            {
            //SimulatorState.CurrentState.Stimulus(c);
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

        #region Disposable pattern
        /// <summary>Releases the resources used by this object.</summary>
        /// <param name="disposing">
        ///     true to release both managed and unmanaged resources; false to release only
        ///     unmanaged resources.
        /// </param>
        private void Dispose(bool disposing)
            {
            if (disposing)
                {
                transmitSubject?.OnCompleted();
                }
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
        public void OpenRequested() => CurrentState.OpenRequested();
        #endregion

        /// <summary>Called to initialize the state machine and set it into the starting state..</summary>
        public void Initialize(SimulatorState startState, InputParser parser)
            {
            Transition(startState);
            this.parser = parser;
            var receiveObservable = receiveSubject.AsObservable();
            parser.SubscribeTo(receiveObservable);
            }

        /// <summary>
        /// Gets a task that will complete either immediately (for a fast simulator) or after the specified time (for a realtime simulator).
        /// </summary>
        /// <param name="delay">The delay.</param>
        /// <returns>Task.</returns>
        public Task SimulatedDelay(TimeSpan delay)
            {
            if (RealTime)
                return Task.Delay(delay);
            else
                return Task.CompletedTask;
            }

        public void SignalStopped()
            {
            InReadyState.Set();
            }
        }
    }