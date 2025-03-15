// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System.Collections.Generic;
using FakeItEasy;
using TA.SnapCap.DeviceInterface;
using TA.SnapCap.Specifications.Fakes;

namespace TA.SnapCap.Specifications.TestHelpers
    {
    internal class DeviceLayerContextBuilder
        {
        private readonly List<string> fakeResponses = new List<string>();
        private FakeCommunicationChannel channel;

        public FakeTransactionProcessor TransactionProcessor { get; private set; }

        public DeviceController Build()
            {
            channel = new FakeCommunicationChannel(string.Empty);
            TransactionProcessor = new FakeTransactionProcessor(fakeResponses);
            return new DeviceController(channel, TransactionProcessor);
            }

        public DeviceLayerContextBuilder WithResponses(params string[] responses)
            {
            fakeResponses.AddRange(responses);
            return this;
            }
        }
    }