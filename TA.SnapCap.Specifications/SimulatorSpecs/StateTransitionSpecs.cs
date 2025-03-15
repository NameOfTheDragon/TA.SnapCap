// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System.Collections.Generic;
using System.ComponentModel;
using Machine.Specifications;
using TA.SnapCap.HardwareSimulator;
using TA.SnapCap.Specifications.Contexts;

namespace TA.SnapCap.Specifications
    {
    [Subject(typeof(SimulatorStateMachine), "state transitions")]
    internal class when_first_connected : with_simulator_context
        {
        private Establish context = () => Context = ContextBuilder.Build();
        private Because of = () => Context.Channel.Open();
        private It should_pass_through_closing_then_closed =
            () => Context.StateChanges.ShouldEqual(ExpectedStateChanges);
        private static List<string> ExpectedStateChanges = new List<string>
                { nameof(StateClosing), nameof(StateClosed) };
        }

    [Subject(typeof(SimulatorStateMachine), "state transitions")]
    internal class when_closed_and_open_is_requested : with_simulator_context
        {
        private Establish context = () => Context = ContextBuilder.WithOpenChannel().InClosedState()
            .WithPropertyChangeNotifications((sender, args) => PropertyChanges.Add(args.PropertyName))
            .Build();
        private Because of = async () =>
            {
            Context.Simulator.OpenRequested();
            await Context.Simulator.WhenStopped();
            };
        private It should_change_the_expected_properties = () => PropertyChanges.ShouldEqual(ExpectedPropertyChanges);
        private It should_open_the_cover = () => Context.Simulator.SystemStatus.ShouldEqual(SystemStatus.Open);
        private It should_stop_the_motor = () => Context.Simulator.MotorEnergized.ShouldBeFalse();
        private It should_pass_through_opening_and_open = () => Context.StateChanges.ShouldEqual(ExpectedStates);
        private It should_reply = () => Context.Response.ShouldEqual("*O000\r\n");
        private static List<string> ExpectedPropertyChanges =
            new List<string> { "MotorEnergized", "MotorEnergized", "SystemStatus" };
        private static List<string> ExpectedStates = new List<string> { nameof(StateOpening), nameof(StateOpen) };
        //ToDo: property change order is not deterministic - come up with a better test that is order agnostic
        private static List<string> PropertyChanges = new List<string>();
        }

    [Subject(typeof(SimulatorStateMachine), "state transitions")]
    internal class when_open_and_close_is_requested : with_simulator_context
        {
        private Establish context = () => Context = ContextBuilder.WithOpenChannel().InOpenState()
            .WithPropertyChangeNotifications((sender, args) => PropertyChanges.Add(args.PropertyName))
            .Build();
        private Because of = async () =>
            {
            Context.Simulator.CloseRequested();
            await Context.Simulator.WhenStopped();
            };
        private It should_change_the_expected_properties = () => PropertyChanges.ShouldEqual(ExpectedPropertyChanges);
        private It should_close_the_cover = () => Context.Simulator.SystemStatus.ShouldEqual(SystemStatus.Closed);
        private It should_stop_the_motor = () => Context.Simulator.MotorEnergized.ShouldBeFalse();
        private It should_pass_through_closing_and_closed = () => Context.StateChanges.ShouldEqual(ExpectedStates);
        private It should_reply = () => Context.Response.ShouldEqual("*C000\r\n");
        private static List<string> ExpectedPropertyChanges =
            new List<string> { "MotorDirection", "MotorEnergized", "MotorEnergized", "SystemStatus" };
        private static List<string> ExpectedStates = new List<string> { nameof(StateClosing), nameof(StateClosed) };
        //ToDo: property change order is not deterministic - come up with a better test that is order agnostic
        private static List<string> PropertyChanges = new List<string>();
        }

    //[Subject(typeof(SimulatorStateMachine), "status reporting")]
    //internal class when_open : with_simulator_context
    //    {
    //    Establish context = () => Context = ContextBuilder.WithOpenChannel().InOpenState()
    //        .Build();
    //    private Because of;
    //    private It should_report_open;
    //    }
    }