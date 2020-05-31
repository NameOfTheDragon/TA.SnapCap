// This file is part of the TA.DigitalDomeworks project
//
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
//
// File: SimulatorCommunicationsChannel.cs  Last modified: 2018-03-28@22:45 by Tim Long

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NodaTime;
using TA.Ascom.ReactiveCommunications;

namespace TA.SnapCap.HardwareSimulator
    {
    /// <summary>
    ///     Provides a direct in-memory communications link to the hardware simulator.
    /// </summary>
    public class SimulatorCommunicationsChannel : ICommunicationChannel
        {
        internal readonly InputParser inputParser;

        internal SimulatorStateMachine Simulator { get; }

        /// <summary>
        ///     Creates a simulator communications channel from a valid endpoint.
        /// </summary>
        /// <param name="endpoint">A valid simulator endpoint.</param>
        public SimulatorCommunicationsChannel(SimulatorEndpoint endpoint, SimulatorStateMachine machine, InputParser parser)
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

        /// <summary>
        ///     Keeps a log of all commands sent to the simulator.
        /// </summary>
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