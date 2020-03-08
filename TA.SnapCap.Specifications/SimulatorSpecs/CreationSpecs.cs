// This file is part of the TA.SnapCap project
// 
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
// 
// File: CreationSpecs.cs  Last modified: 2020-02-27@21:37 by Tim Long

using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using TA.DigitalDomeworks.HardwareSimulator;
using TA.SnapCap.DeviceInterface;
using TA.SnapCap.Specifications.Contexts;

namespace TA.SnapCap.Specifications.SimulatorSpecs
    {
    [Subject(typeof(SimulatorCommunicationsChannel), "create from connection string")]
    internal class when_creating_a_realtime_simulator : with_simulator_context
        {
        Establish context = () => Context = Builder
            .WithRealtimeSimulator()
            .Build();
        It should_be_real_time = () => Context.Endpoint.Realtime.ShouldBeTrue();
        // We don' test for other states because it would take too long.
        }

    [Subject(typeof(SimulatorCommunicationsChannel), "create from connection string")]
    internal class when_creating_a_fast_simulator : with_simulator_context
        {
        static readonly IEnumerable<string> expectedStates = new List<string>
                {nameof(StateClosing), nameof(StateClosed)};
        Establish context = () => Context = Builder
            .WithFastSimulator()
            .Build();
        Because of = () => OpenChannelAndWaitUntilStopped();
        It should_be_fast = () => Context.Endpoint.Realtime.ShouldBeFalse();
        It should_have_passed_through_closing_then_closed_states =
            () => Context.StateChanges.ShouldBeLike(expectedStates);
        }

    [Subject(typeof(SimulatorStateMachine), "Opening")]
    internal class when_closed_and_an_open_request_is_received : with_simulator_context
        {
        Establish context = () => Context = Builder
            .WithFastSimulator()
            .WithOpenChannel()
            .InStoppedState()
            .Build();
        Because of = () => Context.Channel.Send(Protocol.OpenCover.ToString());
        It should_transition_through_opening_state = () => Context.StateChanges.First().ShouldEqual(nameof(StateOpening));
        }
    }