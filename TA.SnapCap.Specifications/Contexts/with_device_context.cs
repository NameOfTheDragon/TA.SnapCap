// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using Machine.Specifications;
using TA.SnapCap.DeviceInterface;
using TA.SnapCap.Specifications.Fakes;
using TA.SnapCap.Specifications.TestHelpers;

namespace TA.SnapCap.Specifications
    {
    #region Context base classes
    internal class with_device_context
        {
        private Establish context = () => ContextBuilder = new DeviceLayerContextBuilder();
        protected static DeviceLayerContextBuilder ContextBuilder;
        protected static DeviceController Controller;

        protected static FakeTransactionProcessor Processor => ContextBuilder.TransactionProcessor;
        }
    #endregion
    }