// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System.Collections.Generic;
using TA.SnapCap.Specifications.TestHelpers;
using Timtek.ReactiveCommunications;

namespace TA.SnapCap.Specifications.Fakes
    {
    internal class FakeTransactionProcessor : ITransactionProcessor
        {
        private readonly IEnumerator<string> responseEnumerator;

        public FakeTransactionProcessor(IEnumerable<string> fakeResponses)
            {
            responseEnumerator = fakeResponses.GetEnumerator();
            }

        public List<DeviceTransaction> TransactionHistory { get; } = new List<DeviceTransaction>();

        public void CommitTransaction(DeviceTransaction transaction)
            {
            var moreResponses = responseEnumerator.MoveNext();
            if (moreResponses)
                transaction.SimulateCompletionWithResponse(responseEnumerator.Current);
            else
                transaction.TimedOut("Timeout");
            TransactionHistory.Add(transaction);
            }
        }
    }