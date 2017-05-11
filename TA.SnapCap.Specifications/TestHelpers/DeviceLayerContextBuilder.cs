// This file is part of the TA.SnapCap project
// 
// Copyright © 2007-2017 Tigra Astronomy, all rights reserved.
// 
// File: DeviceLayerContextBuilder.cs  Created: 2017-05-07@12:52
// Last modified: 2017-05-10@18:40 by Tim Long

using System.Collections.Generic;
using TA.SnapCap.DeviceInterface;
using TA.SnapCap.Specifications.Fakes;

namespace TA.SnapCap.Specifications.TestHelpers
{
    class DeviceLayerContextBuilder
    {
        readonly List<string> fakeResponses = new List<string>();
        FakeCommunicationChannel channel;
        ITransactionProcessorFactory factory;

        public FakeTransactionProcessor TransactionProcessor { get; private set; }

        public DeviceController Build()
        {
            channel = new FakeCommunicationChannel(string.Empty);
            TransactionProcessor = new FakeTransactionProcessor(fakeResponses);
            factory = new UnitTestTransactionProcessorFactory(channel, TransactionProcessor);
            return new DeviceController(factory);
        }

        public DeviceLayerContextBuilder WithResponses(params string[] responses)
        {
            fakeResponses.AddRange(responses);
            return this;
        }
    }
}