// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using Machine.Specifications;
using TA.SnapCap.HardwareSimulator;
using TA.SnapCap.SharedTypes;
using TA.SnapCap.Specifications.Contexts;

namespace TA.SnapCap.Specifications
    {
    /*
     * S    *S000   Report status as follows:
     *              First digit: Motor on (1) or off (0)
     *              Second digit: Lamp on (1) or off (0)
     *              Third digit: system state.
     *                  OPEN 1
     *                  CLOSED 2
     *                  TIMEOUT 3
     *                  OPEN_CIRCUIT 4
     *                  OVERCURRENT 5
     *                  USERABORT 6
     */
    [Subject(typeof(SimulatorStateMachine), "Command/Response processing")]
    internal class when_requesting_status_from_a_closed_idle_snapcap : with_simulator_context
        {
        private Establish context = () => Context = ContextBuilder.InClosedState().WithOpenChannel().Build();
        private Because of = () => Context.Channel.Send(Protocol.GetCommandString(Protocol.GetStatus));
        private It should_receive_a_status_response = () => Context.Response.ShouldEqual(Expected);
        private const string Expected = "*S002\r\n";
        }

    [Subject(typeof(SimulatorStateMachine), "Command/Response processing")]
    internal class when_requesting_status_from_an_open_idle_snapcap : with_simulator_context
        {
        private Establish context = () => Context = ContextBuilder.InOpenState().WithOpenChannel().Build();
        private Because of = () => Context.Channel.Send(Protocol.GetCommandString(Protocol.GetStatus));
        private It should_receive_a_status_response = () => Context.Response.ShouldEqual(Expected);
        private const string Expected = "*S001\r\n";
        }
    }