// This file is part of the TA.SnapCap project
// 
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: CommunicationsStackBuilder.cs  Last modified: 2017-05-06@19:42 by Tim Long

using System;
using TA.Ascom.ReactiveCommunications;

namespace TA.SnapCap.DeviceInterface
    {
    /// <summary>
    ///     Factory methods for creating various parts of the communications stack.
    /// </summary>
    public static class CommunicationsStackBuilder
        {
        public static ICommunicationChannel BuildChannel(DeviceEndpoint endpoint)
            {
            if (endpoint is SerialDeviceEndpoint)
                return new SerialCommunicationChannel(endpoint);
            throw new NotSupportedException($"There is no supported channel type for the endpoint: {endpoint}")
                {
                Data = {["endpoint"] = endpoint}
                };
            }

        public static TransactionObserver BuildTransactionObserver(ICommunicationChannel channel)
            {
            return new TransactionObserver(channel);
            }

        public static ITransactionProcessor BuildTransactionProcessor(TransactionObserver observer)
            {
            var processor = new ReactiveTransactionProcessor();
            processor.SubscribeTransactionObserver(observer);
            return processor;
            }
        }
    }