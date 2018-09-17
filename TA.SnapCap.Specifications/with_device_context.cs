// This file is part of the TA.SnapCap project
// 
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: with_device_context.cs  Last modified: 2017-05-07@04:47 by Tim Long

using Machine.Specifications;
using TA.SnapCap.DeviceInterface;
using TA.SnapCap.Specifications.Fakes;
using TA.SnapCap.Specifications.TestHelpers;

namespace TA.SnapCap.Specifications
    {
    class with_device_context
        {
        protected static DeviceLayerContextBuilder ContextBuilder;
        protected static DeviceController Controller;
        Establish context = () => ContextBuilder = new DeviceLayerContextBuilder();

        protected static FakeTransactionProcessor Processor => ContextBuilder.TransactionProcessor;
        }
    }