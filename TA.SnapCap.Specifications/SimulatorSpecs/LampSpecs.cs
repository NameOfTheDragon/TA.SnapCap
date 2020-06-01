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
        It should_reply = () => Context.Response.ShouldEqual("*L000\r\n");
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
        It should_reply = () => Context.Response.ShouldEqual("*D000\r\n");
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
        It should_reply = () => Context.Response.ShouldEqual("*B128\r\n");
        static List<string> PropertyChanges = new List<string>();
        static List<string> ExpectedPropertyChanges = new List<string> { "LampBrightness" };
        }

    [Subject(typeof(SimulatorStateMachine), "electroluminescent panel")]
    internal class when_the_brightness_is_queried : with_simulator_context
        {
        Establish context = () => Context = ContextBuilder.WithOpenChannel().InClosedState()
            .WithLampBrightness(123)
            .WithPropertyChangeNotifications((sender, args) => PropertyChanges.Add(args.PropertyName))
            .Build();
        Because of = () => Context.Simulator.GetLampBrightness();
        It should_not_change_any_properties = () => PropertyChanges.ShouldBeEmpty();
        It should_not_change_state = () => Context.StateChanges.ShouldBeEmpty();
        It should_not_turn_the_lamp_on = () => Context.Simulator.LampOn.ShouldBeFalse();
        It should_reply = () => Context.Response.ShouldEqual("*J123\r\n");
        static List<string> PropertyChanges = new List<string>();
        }
    }