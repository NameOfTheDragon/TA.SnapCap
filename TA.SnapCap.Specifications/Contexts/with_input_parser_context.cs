using Machine.Specifications;

namespace TA.SnapCap.Specifications.Contexts
    {
    internal class with_input_parser_context
        {
        protected static InputParserContext Context;
        protected static InputParserContextBuilder ContextBuilder;
        Establish context = () => ContextBuilder = new InputParserContextBuilder();
        }
    }