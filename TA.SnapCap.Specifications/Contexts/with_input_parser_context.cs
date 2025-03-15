﻿// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using Machine.Specifications;

namespace TA.SnapCap.Specifications.Contexts
    {
    #region Context base classes
    internal class with_input_parser_context
        {
        private Establish context = () => ContextBuilder = new InputParserContextBuilder();
        protected static InputParserContext Context;
        protected static InputParserContextBuilder ContextBuilder;
        }
    #endregion
    }