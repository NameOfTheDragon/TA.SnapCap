// This file is part of the TA.SnapCap project
// 
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: UnitTestTransactionProcessorFactory.cs  Last modified: 2017-05-07@04:19 by Tim Long

using TA.Ascom.ReactiveCommunications;
using TA.SnapCap.DeviceInterface;

namespace TA.SnapCap.Specifications.Fakes
    {
    class UnitTestTransactionProcessorFactory : ITransactionProcessorFactory
        {
        readonly ITransactionProcessor fakeProcessor;

        public UnitTestTransactionProcessorFactory(ICommunicationChannel fakeChannel,
            ITransactionProcessor fakeProcessor)
            {
            Endpoint = new FakeEndpoint();
            Channel = fakeChannel;
            this.fakeProcessor = fakeProcessor;
            }

        public bool CreateCalled { get; private set; }

        public bool DestroyCalled { get; private set; }

        public ICommunicationChannel Channel { get; }

        /// <summary>
        ///     Creates the transaction processor ready for use. Also creates and initialises the
        ///     device endpoint and the communications channel and opens the channel.
        /// </summary>
        /// <returns>ITransactionProcessor.</returns>
        public ITransactionProcessor CreateTransactionProcessor()
            {
            CreateCalled = true;
            Channel.Open();
            return fakeProcessor;
            }

        public void DestroyTransactionProcessor()
            {
            Channel.Close();
            DestroyCalled = true;
            }

        public DeviceEndpoint Endpoint { get; }
        }
    }