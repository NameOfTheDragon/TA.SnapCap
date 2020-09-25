// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: InputParserContext.cs  Last modified: 2020-05-24@15:29 by Tim Long

using System.Reactive.Linq;
using System.Text;
using TA.SnapCap.HardwareSimulator;
using TA.SnapCap.SharedTypes;

namespace TA.SnapCap.Specifications.Contexts
    {
    class InputParserContext
        {
        public ISimulatorStateTriggers FakeStateMachine { get; set; }

        public InputParser Parser { get; set; }

        /// <summary>
        ///     Simulates a command being received via the communications channel. The supplied string should
        ///     contain a well-formed command string.
        /// </summary>
        /// <param name="receivedCommand">The received command.</param>
        public void SimulateReceivedCommand(string receivedCommand)
            {
            Parser.SubscribeTo(receivedCommand.ToObservable(), FakeStateMachine);
            }

        /// <summary>
        ///     Simulates the specified command code being received from the communications channel. The
        ///     supplied code should be a constant taken from <see cref="Protocol" />. The supplied code is
        ///     used to construct a well-formed command string.
        /// </summary>
        /// <param name="commandCode">The command code.</param>
        public void SimulateReceivedCommand(char commandCode)
            {
            StringBuilder command = new StringBuilder();
            command.Append(Protocol.CommandInitiator);
            command.Append(commandCode);
            command.AppendLine();
            SimulateReceivedCommand(command.ToString());
            }
        }
    }