using System.Collections.Generic;
using TA.SnapCap.HardwareSimulator;

using Machine.Specifications;
using TA.SnapCap.Specifications.Contexts;

namespace TA.SnapCap.Specifications
    {
    [Subject(typeof(SimulatorStateMachine), "electroluminescent panel")]
    internal class when_the_lamp_is_turned_on : with_simulator_context
        {
        Establish context = () => Context = ContextBuilder.WithOpenChannel().InClosedState()
            .WithPropertyChangeNotifications((sender, args) => PropertyChanges.Add(args.PropertyName))
            .Build();
        Because of = () => Context.Simulator.LampOnRequested();
        It should_change_the_expected_properties = () => PropertyChanges.ShouldEqual(ExpectedPropertyChanges);
        It should_not_change_state = () => Context.StateChanges.ShouldBeEmpty();
        It should_turn_the_lamp_on = () => Context.Simulator.LampOn.ShouldBeTrue();
        static List<string> PropertyChanges = new List<string>();
        static List<string> ExpectedPropertyChanges = new List<string> {"LampOn"};
        }

    [Subject(typeof(SimulatorStateMachine), "electroluminescent panel")]
    internal class when_the_lamp_is_turned_off : with_simulator_context
        {
        Establish context = () => Context = ContextBuilder.WithOpenChannel().InClosedState()
            .WithPropertyChangeNotifications((sender, args) => PropertyChanges.Add(args.PropertyName))
            .WithLampOn()
            .Build();
        Because of = () => Context.Simulator.LampOffRequested();
        It should_change_the_expected_properties = () => PropertyChanges.ShouldEqual(ExpectedPropertyChanges);
        It should_not_change_state = () => Context.StateChanges.ShouldBeEmpty();
        It should_turn_the_lamp_on = () => Context.Simulator.LampOn.ShouldBeFalse();
        static List<string> PropertyChanges = new List<string>();
        static List<string> ExpectedPropertyChanges = new List<string> {"LampOn"};
        }

    [Subject(typeof(SimulatorStateMachine), "electroluminescent panel")]
    internal class when_the_brightness_is_set_nonzero : with_simulator_context
        {
        Establish context = () => Context = ContextBuilder.WithOpenChannel().InClosedState()
            .WithPropertyChangeNotifications((sender, args) => PropertyChanges.Add(args.PropertyName))
            .Build();
        Because of = () => Context.Simulator.SetLampBrightness(128);
        It should_change_the_expected_properties = () => PropertyChanges.ShouldEqual(ExpectedPropertyChanges);
        It should_not_change_state = () => Context.StateChanges.ShouldBeEmpty();
        It should_not_turn_the_lamp_on = () => Context.Simulator.LampOn.ShouldBeFalse();
        static List<string> PropertyChanges = new List<string>();
        static List<string> ExpectedPropertyChanges = new List<string> { "LampBrightness" };
        }
    }