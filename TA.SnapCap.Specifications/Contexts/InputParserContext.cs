// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System.Reactive.Linq;
using System.Text;
using TA.SnapCap.HardwareSimulator;
using TA.SnapCap.SharedTypes;

namespace TA.SnapCap.Specifications.Contexts
    {
    internal class InputParserContext
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
            var command = new StringBuilder();
            command.Append(Protocol.CommandInitiator);
            command.Append(commandCode);
            command.AppendLine();
            SimulateReceivedCommand(command.ToString());
            }
        }
    }