using System;
using TA.Utils.Core;
using Timtek.ReactiveCommunications;

namespace TA.SnapCap.Specifications.TestHelpers
    {
    internal class TestableDeviceTransaction : DeviceTransaction
        {
        readonly DeviceTransaction sourceTransaction;

        public TestableDeviceTransaction(DeviceTransaction sourceTransaction) : base(sourceTransaction.Command)
            {
            this.sourceTransaction = sourceTransaction;
            }

        public override void ObserveResponse(IObservable<char> source)
            {
            throw new NotImplementedException();
            }

        void SetResponse(string response)
            {
            Response = Maybe<string>.From(response);
            }

        internal void SignalCompletion(string fakeResponse)
            {
            SetResponse(fakeResponse);
            OnCompleted();
            }

        public void SignalError(string error)
            {
            OnError(new TimeoutException(error));
            }

        internal void MakeHot()
        {
            
        }
    }
    }