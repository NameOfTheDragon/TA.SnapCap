
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
        Establish context = () => Context=ContextBuilder.Build();
        Because of = () => Context.Channel.Open();
        It should_pass_through_closing_then_closed = () => Context.StateChanges.ShouldEqual(ExpectedStateChanges);
        static List<string> ExpectedStateChanges = new List<string> {nameof(StateClosing), nameof(StateClosed)};
        }

    [Subject(typeof(SimulatorStateMachine), "state transitions")]
    internal class when_closed_and_open_is_requested : with_simulator_context
        {
        Establish context = () => Context = ContextBuilder.WithOpenChannel().InClosedState()
            .WithPropertyChangeNotifications((sender,args) => PropertyChanges.Add(args.PropertyName))
            .Build();
        Because of = () => Context.Simulator.OpenRequested();
        It should_change_the_expected_properties = () => PropertyChanges.ShouldEqual(ExpectedPropertyChanges);
        It should_open_the_cover = () => Context.Simulator.SystemStatus.ShouldEqual( SystemStatus.Open);
        It should_stop_the_motor = () => Context.Simulator.MotorEnergized.ShouldBeFalse();
        It should_pass_through_opening_and_open = () => Context.StateChanges.ShouldEqual(ExpectedStates);
        //ToDo: property change order is not deterministic - come up with a better test that is order agnostic
        static List<string> PropertyChanges = new List<string>();
        static List<string> ExpectedPropertyChanges = new List<string> {"MotorEnergized","SystemStatus"};
        static List<string> ExpectedStates = new List<string> {nameof(StateOpening), nameof(StateOpen)};
        }

    [Subject(typeof(SimulatorStateMachine), "state transitions")]
    internal class when_open_and_close_is_requested : with_simulator_context
        {
        Establish context = () => Context = ContextBuilder.WithOpenChannel().InOpenState()
            .WithPropertyChangeNotifications((sender,args) => PropertyChanges.Add(args.PropertyName))
            .Build();
        Because of = () => Context.Simulator.CloseRequested();
        It should_change_the_expected_properties = () => PropertyChanges.ShouldEqual(ExpectedPropertyChanges);
        It should_close_the_cover = () => Context.Simulator.SystemStatus.ShouldEqual( SystemStatus.Closed);
        It should_stop_the_motor = () => Context.Simulator.MotorEnergized.ShouldBeFalse();
        It should_pass_through_closing_and_closed = () => Context.StateChanges.ShouldEqual(ExpectedStates);
        //ToDo: property change order is not deterministic - come up with a better test that is order agnostic
        static List<string> PropertyChanges = new List<string>();
        static List<string> ExpectedPropertyChanges =
            new List<string> {"MotorDirection", "MotorEnergized", "SystemStatus"};
        static List<string> ExpectedStates = new List<string> {nameof(StateClosing), nameof(StateClosed)};
        }
    }