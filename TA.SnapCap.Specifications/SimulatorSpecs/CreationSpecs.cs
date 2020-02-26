// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: ChannelCreationSpecs.cs  Last modified: 2020-02-24@17:39 by Tim Long

using System.Linq;
using Machine.Specifications;
using TA.DigitalDomeworks.HardwareSimulator;
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
        }

    [Subject(typeof(SimulatorCommunicationsChannel), "create from connection string")]
    internal class when_creating_a_fast_simulator : with_simulator_context
        {
        Establish context = () => Context = Builder
            .WithFastSimulator()
            .Build();
        It should_be_fast = () => Context.Endpoint.Realtime.ShouldBeFalse();
        }

    [Subject(typeof(SimulatorStateMachine), "create")]
    internal class when_creating_the_simulator_state_machine : with_simulator_context

        {
        Establish context = () => Context=Builder
            .WithFastSimulator()
            .Build();
        Because of = () => Context.Channel.Open();
        It should_have_passed_through_closing_state = () => Context.StateChanges.Single().StateName.ShouldEqual(nameof(StateClosing));
        }
    }