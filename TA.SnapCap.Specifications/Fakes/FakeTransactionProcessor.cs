// This file is part of the AWR Drive System ASCOM Driver project
// 
// Copyright © 2010-2016 Tigra Astronomy, all rights reserved.
// 
// File: FakeTransactionProcessor.cs  Last modified: 2016-01-21@16:02 by Tim Long

using System.Collections.Generic;
using TA.Ascom.ReactiveCommunications;
using TA.SnapCap.Specifications.TestHelpers;

namespace TA.SnapCap.Specifications.Fakes
    {
    internal class FakeTransactionProcessor : ITransactionProcessor
        {
        readonly IEnumerator<string> responseEnumerator;

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