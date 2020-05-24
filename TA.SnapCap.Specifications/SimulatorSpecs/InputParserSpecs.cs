// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: InputParserSpecs.cs  Last modified: 2020-05-24@15:30 by Tim Long

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
        Establish context = () => Context = ContextBuilder.Build();
        Because of = () => Context.SimulateReceivedCommand(Protocol.OpenCover);
        It should_trigger_the_open_action =
            () => A.CallTo(() => Context.FakeStateMachine.OpenRequested()).MustHaveHappenedOnceExactly();
        }

    [Subject(typeof(InputParser), "Tokenization and parsing of the input stream")]
    internal class when_a_lamp_on_command_is_received : with_input_parser_context
        {
        Establish context = () => Context = ContextBuilder.Build();
        Because of = () => Context.SimulateReceivedCommand(Protocol.ElpOn);
        It should_trigger_the_lamp_on_action =
            () => A.CallTo(() => Context.FakeStateMachine.LampOnRequested()).MustHaveHappenedOnceExactly();
        }

    [Subject(typeof(InputParser), "Tokenization and parsing of the input stream")]
    internal class when_a_lamp_off_command_is_received : with_input_parser_context
        {
        Establish context = () => Context = ContextBuilder.Build();
        Because of = () => Context.SimulateReceivedCommand(Protocol.ElpOff);
        It should_trigger_the_lamp_off_action =
            () => A.CallTo(() => Context.FakeStateMachine.LampOffRequested()).MustHaveHappenedOnceExactly();
        }

    [Subject(typeof(InputParser), "Tokenization and parsing of the input stream")]
    internal class when_a_lamp_brightness_command_is_received : with_input_parser_context
        {
        Establish context = () => Context = ContextBuilder.Build();
        Because of = () => Context.SimulateReceivedCommand(">B100\r\n");
        It should_trigger_the_lamp_brightness_action =
            () => A.CallTo(() => Context.FakeStateMachine.SetLampBrightness(100)).MustHaveHappenedOnceExactly();
        }
    }