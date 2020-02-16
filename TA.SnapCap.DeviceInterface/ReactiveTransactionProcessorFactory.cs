// This file is part of the TA.SnapCap project
//
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
//
// File: ReactiveTransactionProcessorFactory.cs  Last modified: 2017-05-06@19:42 by Tim Long

using System;
using System.Threading;
using TA.Ascom.ReactiveCommunications;

namespace TA.SnapCap.DeviceInterface
    {
    public class ReactiveTransactionProcessorFactory : ITransactionProcessorFactory
        {
        private TransactionObserver observer;
        private ReactiveTransactionProcessor processor;

        public ReactiveTransactionProcessorFactory(string connectionString)
            {
            ConnectionString = connectionString;
            }

        public string ConnectionString { get; }

        public ICommunicationChannel Channel { get; private set; }

        /// <summary>
        ///     Creates the transaction processor ready for use. Also creates and initialises the
        ///     device endpoint and the communications channel and opens the channel.
        /// </summary>
        /// <returns>ITransactionProcessor.</returns>
        public ITransactionProcessor CreateTransactionProcessor()
            {
            var factory = new ChannelFactory();
            Channel = factory.FromConnectionString(ConnectionString);
            observer = new TransactionObserver(Channel);
            processor = new ReactiveTransactionProcessor();
            processor.SubscribeTransactionObserver(observer, TimeSpan.FromMilliseconds(100));
            Channel.Open();
            Thread.Sleep(TimeSpan.FromSeconds(3));
            return processor;
            }

        /// <summary>
        ///     Destroys the transaction processor and its dependencies. Ensures that the
        ///     <see cref="Channel" /> is closed. Once this method has been called, the
        ///     <see cref="Channel" /> and <see cref="Endpoint" /> properties will be null. A new
        ///     connection to the same endpoint can be created by calling
        ///     <see cref="CreateTransactionProcessor" /> again.
        /// </summary>
        public void DestroyTransactionProcessor()
            {
            processor?.Dispose();
            processor = null; // [Sentinel]
            observer = null;
            if (Channel?.IsOpen ?? false)
                Channel.Close();
            Channel?.Dispose();
            Channel = null; // [Sentinel]
            GC.Collect(3, GCCollectionMode.Forced, blocking: true);
            }

        public DeviceEndpoint Endpoint => Channel.Endpoint;
        }
    }