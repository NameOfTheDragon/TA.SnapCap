// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using FakeItEasy;
using Machine.Specifications;
using TA.SnapCap.HardwareSimulator;
using TA.SnapCap.SharedTypes;
using TA.SnapCap.Specifications.Contexts;

namespace TA.SnapCap.Specifications.SimulatorSpecs
    {
    [Subject(typeof(InputParser), "Tokenization and parsing of the input stream")]
    internal class when_an_open_command_is_received : with_input_parser_context
        {
        private Establish context = () => Context = ContextBuilder.Build();
        private Because of = () => Context.SimulateReceivedCommand(Protocol.OpenCover);
        private It should_trigger_the_open_action =
            () => A.CallTo(() => Context.FakeStateMachine.OpenRequested()).MustHaveHappenedOnceExactly();
        }

    [Subject(typeof(InputParser), "Tokenization and parsing of the input stream")]
    internal class when_a_lamp_on_command_is_received : with_input_parser_context
        {
        private Establish context = () => Context = ContextBuilder.Build();
        private Because of = () => Context.SimulateReceivedCommand(Protocol.ElpOn);
        private It should_trigger_the_lamp_on_action =
            () => A.CallTo(() => Context.FakeStateMachine.LampOnRequested()).MustHaveHappenedOnceExactly();
        }

    [Subject(typeof(InputParser), "Tokenization and parsing of the input stream")]
    internal class when_a_lamp_off_command_is_received : with_input_parser_context
        {
        private Establish context = () => Context = ContextBuilder.Build();
        private Because of = () => Context.SimulateReceivedCommand(Protocol.ElpOff);
        private It should_trigger_the_lamp_off_action =
            () => A.CallTo(() => Context.FakeStateMachine.LampOffRequested()).MustHaveHappenedOnceExactly();
        }

    [Subject(typeof(InputParser), "Tokenization and parsing of the input stream")]
    internal class when_a_lamp_brightness_command_is_received : with_input_parser_context
        {
        private Establish context = () => Context = ContextBuilder.Build();
        private Because of = () => Context.SimulateReceivedCommand(">B100\r\n");
        private It should_trigger_the_lamp_brightness_action =
            () => A.CallTo(() => Context.FakeStateMachine.SetLampBrightness(100)).MustHaveHappenedOnceExactly();
        }

    [Subject(typeof(InputParser), "Tokenization and parsing of the input stream")]
    internal class when_a_halt_command_is_received : with_input_parser_context
        {
        private Establish context = () => Context = ContextBuilder.Build();
        private Because of = () => Context.SimulateReceivedCommand(Protocol.Halt);
        private It should_trigger_the_halt_action =
            () => A.CallTo(() => Context.FakeStateMachine.HaltRequested()).MustHaveHappenedOnceExactly();
        }
    }