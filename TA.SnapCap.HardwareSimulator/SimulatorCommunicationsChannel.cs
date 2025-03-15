// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NodaTime;
using Timtek.ReactiveCommunications;

namespace TA.SnapCap.HardwareSimulator
    {
    /// <summary>Provides a direct in-memory communications link to the hardware simulator.</summary>
    public class SimulatorCommunicationsChannel : ICommunicationChannel
        {
        internal readonly InputParser inputParser;

        /// <summary>Creates a simulator communications channel from a valid endpoint.</summary>
        /// <param name="endpoint">A valid simulator endpoint.</param>
        public SimulatorCommunicationsChannel(SimulatorEndpoint endpoint, SimulatorStateMachine machine,
            InputParser parser)
            {
            /*
             * ToDo: InputParser is really not needed here except to initialize the
             * state machine. Can this be injected there instead?
             */

            Contract.Requires(endpoint != null);
            Endpoint = endpoint;
            Simulator = machine;
            inputParser = parser;
            }

        internal SimulatorStateMachine Simulator { get; }

        /// <summary>Keeps a log of all commands sent to the simulator.</summary>
        public List<string> SendLog { get; } = new List<string>();

        /// <inheritdoc />
        public void Dispose()
            {
            Simulator?.InputObserver.OnCompleted();
            }

        /// <inheritdoc />
        public void Open()
            {
            IsOpen = true;
            Simulator.Initialize(new StateClosing(Simulator), inputParser);
            }

        /// <inheritdoc />
        public void Close()
            {
            IsOpen = false;
            }

        /// <inheritdoc />
        public void Send(string txData)
            {
            SendLog.Add(txData);
            foreach (var c in txData) Simulator.InputObserver.OnNext(c);
            }

        /// <inheritdoc />
        public IObservable<char> ObservableReceivedCharacters => Simulator.ObservableResponses;

        /// <inheritdoc />
        public bool IsOpen { get; internal set; }

        /// <inheritdoc />
        public DeviceEndpoint Endpoint { get; }
        }
    }