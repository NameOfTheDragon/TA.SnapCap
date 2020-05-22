// This file is part of the TA.SnapCap project
// 
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
// 
// File: CommandProtocolSpecs.cs  Last modified: 2020-05-18@17:02 by Tim Long

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
        Establish context = () => Context = ContextBuilder.InClosedState().WithOpenChannel().Build();
        Because of = () => Context.Channel.Send(Protocol.GetCommandString(Protocol.GetStatus));
        It should_receive_a_status_response = () => Context.Response.ShouldEqual(Expected);
        const string Expected = "*S002\r\n";
        }
    [Subject(typeof(SimulatorStateMachine), "Command/Response processing")]
    internal class when_requesting_status_from_an_open_idle_snapcap : with_simulator_context
        {
        Establish context = () => Context = ContextBuilder.InOpenState().WithOpenChannel().Build();
        Because of = () => Context.Channel.Send(Protocol.GetCommandString(Protocol.GetStatus));
        It should_receive_a_status_response = () => Context.Response.ShouldEqual(Expected);
        const string Expected = "*S001\r\n";
        }
    }