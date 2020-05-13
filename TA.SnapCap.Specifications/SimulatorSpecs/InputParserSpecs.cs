using System.Reactive.Linq;
using FakeItEasy;
using Machine.Specifications;
using TA.SnapCap.HardwareSimulator;

namespace TA.SnapCap.Specifications.SimulatorSpecs
    {
    [Subject(typeof(InputParser), "Tokenization and parsing of the input stream")]
    internal class when_an_open_command_is_received
        {
        const string openCommand = ">O\r\n";
        Establish context = () =>
            {
            fakeStateMachine = A.Fake<ISimulatorStateTriggers>();
            parser = new InputParser(fakeStateMachine);
            };
        Because of = () => parser.SubscribeTo(openCommand.ToObservable());
        It should_trigger_the_open_action = () => A.CallTo(()=>fakeStateMachine.OpenRequested()).MustHaveHappenedOnceExactly();
        static ISimulatorStateTriggers fakeStateMachine;
        static InputParser parser;
        }

    }
