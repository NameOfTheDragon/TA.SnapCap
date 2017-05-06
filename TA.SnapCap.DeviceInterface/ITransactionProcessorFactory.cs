// This file is part of the TA.SnapCap project
// 
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: ITransactionProcessorFactory.cs  Last modified: 2017-05-06@19:42 by Tim Long

using TA.Ascom.ReactiveCommunications;

namespace TA.SnapCap.DeviceInterface
    {
    public interface ITransactionProcessorFactory
        {
        ICommunicationChannel Channel { get; }

        DeviceEndpoint Endpoint { get; }

        /// <summary>
        ///     Creates the transaction processor ready for use. Also creates and initialises the
        ///     device endpoint and the communications channel and opens the channel.
        /// </summary>
        /// <returns>ITransactionProcessor.</returns>
        ITransactionProcessor CreateTransactionProcessor();

        /// <summary>
        ///     Destroys the transaction processor and its dependencies. Ensures that the
        ///     <see cref="ReactiveTransactionProcessorFactory.Channel" /> is closed. Once this method has been called, the
        ///     <see cref="ReactiveTransactionProcessorFactory.Channel" /> and
        ///     <see cref="ReactiveTransactionProcessorFactory.Endpoint" /> properties will be null. A new
        ///     connection to the same endpoint can be created by calling
        ///     <see cref="ReactiveTransactionProcessorFactory.CreateTransactionProcessor" /> again.
        /// </summary>
        void DestroyTransactionProcessor();
        }
    }