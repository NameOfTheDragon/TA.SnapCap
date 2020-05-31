using FakeItEasy;
using TA.SnapCap.HardwareSimulator;

namespace TA.SnapCap.Specifications.Contexts
    {
    class InputParserContextBuilder
        {
        public InputParserContext Build()
            {
            var context = new InputParserContext();
            context.FakeStateMachine = A.Fake<ISimulatorStateTriggers>();
            context.Parser = new InputParser();
            return context;
            }
        }
    }