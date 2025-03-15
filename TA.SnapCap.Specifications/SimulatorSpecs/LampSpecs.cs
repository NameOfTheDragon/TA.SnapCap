// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System.Collections.Generic;
using TA.SnapCap.HardwareSimulator;
using Machine.Specifications;
using TA.SnapCap.Specifications.Contexts;
using TA.SnapCap.SharedTypes;

namespace TA.SnapCap.Specifications
    {
    [Subject(typeof(SimulatorStateMachine), "electroluminescent panel")]
    internal class when_the_lamp_is_turned_on : with_simulator_context
        {
        private Establish context = () => Context = ContextBuilder.WithOpenChannel().InClosedState()
            .WithPropertyChangeNotifications((sender, args) => PropertyChanges.Add(args.PropertyName))
            .Build();
        private Because of = () => Context.Simulator.LampOnRequested();
        private It should_change_the_expected_properties = () => PropertyChanges.ShouldEqual(ExpectedPropertyChanges);
        private It should_not_change_state = () => Context.StateChanges.ShouldBeEmpty();
        private It should_turn_the_lamp_on = () => Context.Simulator.LampOn.ShouldBeTrue();
        private It should_reply = () => Context.Response.ShouldEqual("*L000\r\n");
        private static List<string> ExpectedPropertyChanges = new List<string> { "LampOn" };
        private static List<string> PropertyChanges = new List<string>();
        }

    [Subject(typeof(SimulatorStateMachine), "electroluminescent panel")]
    internal class when_the_lamp_is_turned_off : with_simulator_context
        {
        private Establish context = () => Context = ContextBuilder.WithOpenChannel().InClosedState()
            .WithPropertyChangeNotifications((sender, args) => PropertyChanges.Add(args.PropertyName))
            .WithLampOn()
            .Build();
        private Because of = () => Context.Simulator.LampOffRequested();
        private It should_change_the_expected_properties = () => PropertyChanges.ShouldEqual(ExpectedPropertyChanges);
        private It should_not_change_state = () => Context.StateChanges.ShouldBeEmpty();
        private It should_turn_the_lamp_on = () => Context.Simulator.LampOn.ShouldBeFalse();
        private It should_reply = () => Context.Response.ShouldEqual("*D000\r\n");
        private static List<string> ExpectedPropertyChanges = new List<string> { "LampOn" };
        private static List<string> PropertyChanges = new List<string>();
        }

    [Subject(typeof(SimulatorStateMachine), "electroluminescent panel")]
    internal class when_the_brightness_is_set_nonzero : with_simulator_context
        {
        private Establish context = () => Context = ContextBuilder.WithOpenChannel().InClosedState()
            .WithPropertyChangeNotifications((sender, args) => PropertyChanges.Add(args.PropertyName))
            .Build();
        private Because of = () => Context.Simulator.SetLampBrightness(128);
        private It should_change_the_expected_properties = () => PropertyChanges.ShouldEqual(ExpectedPropertyChanges);
        private It should_not_change_state = () => Context.StateChanges.ShouldBeEmpty();
        private It should_not_turn_the_lamp_on = () => Context.Simulator.LampOn.ShouldBeFalse();
        private It should_reply = () => Context.Response.ShouldEqual("*B128\r\n");
        private static List<string> ExpectedPropertyChanges = new List<string> { "LampBrightness" };
        private static List<string> PropertyChanges = new List<string>();
        }

    [Subject(typeof(SimulatorStateMachine), "electroluminescent panel")]
    internal class when_the_brightness_is_queried : with_simulator_context
        {
        private Establish context = () => Context = ContextBuilder.WithOpenChannel().InClosedState()
            .WithLampBrightness(123)
            .WithPropertyChangeNotifications((sender, args) => PropertyChanges.Add(args.PropertyName))
            .Build();
        private Because of = () => Context.Simulator.GetLampBrightness();
        private It should_not_change_any_properties = () => PropertyChanges.ShouldBeEmpty();
        private It should_not_change_state = () => Context.StateChanges.ShouldBeEmpty();
        private It should_not_turn_the_lamp_on = () => Context.Simulator.LampOn.ShouldBeFalse();
        private It should_reply = () => Context.Response.ShouldEqual("*J123\r\n");
        private static List<string> PropertyChanges = new List<string>();
        }

    internal class when_converting_device_brightness_to_ASCOM
        {
        private It should_return_0_for_0_percent = () => 0.ToAscomBrightness().ShouldEqual(0);
        private It should_return_maxBrightness_for_100_percent = () =>
            100.ToAscomBrightness().ShouldEqual(ValueConverterExtensions.AscomMaxBrightness);
        private It should_return_2_for_1_percent = () => 1.ToAscomBrightness().ShouldEqual(2);
        }
    }