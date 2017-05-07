// This file is part of the TA.SnapCap project
// 
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: DeviceController.cs  Last modified: 2017-05-07@15:22 by Tim Long

using System;
using NLog;
using TA.Ascom.ReactiveCommunications;

namespace TA.SnapCap.DeviceInterface
{
    public class DeviceController : IDisposable
    {
        private readonly ITransactionProcessorFactory factory;
        private readonly Logger log = LogManager.GetCurrentClassLogger();

        private bool disposed;
        private ITransactionProcessor transactionProcessor;

        public DeviceController(ITransactionProcessorFactory factory)
        {
            this.factory = factory;
        }

        public bool IsOnline => transactionProcessor != null && (factory?.Channel?.IsOpen ?? false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void ClearRelay(ushort id)
        {
            var transaction = new WriteRelayTransaction(id, false);
            transactionProcessor.CommitTransaction(transaction);
            transaction.WaitForCompletionOrTimeout();
            RaiseRelayStateChanged(id, false);
        }

        /// <summary>
        ///     Close the connection to the AWR system. This should never fail.
        /// </summary>
        public void Close()
        {
            log.Warn("Close requested");
            if (!IsOnline)
            {
                log.Warn("Ignoring Close request because already closed");
                return;
            }
            log.Info($"Closing device endpoint: {factory.Endpoint}");
            factory.DestroyTransactionProcessor();
            log.Info("====== Channel closed: the device is now disconnected ======");
        }

        protected virtual void Dispose(bool fromUserCode)
        {
            if (!disposed)
                if (fromUserCode)
                    Close();
            disposed = true;

            // ToDo: Call the base class's Dispose(Boolean) method, if available.
            // base.Dispose(fromUserCode);
        }

        // The IDisposable pattern, as described at
        // http://www.codeproject.com/Articles/15360/Implementing-IDisposable-and-the-Dispose-Pattern-P


        /// <summary>
        ///     Finalizes this instance (called prior to garbage collection by the CLR)
        /// </summary>
        ~DeviceController()
        {
            Dispose(false);
        }

        public SnapCapState GetState()
        {
            var transaction = TransactSimpleCommand(Protocol.GetStatus);
            return SnapCapState.FromResponsePayload(transaction.ResponsePayload);
        }

        /// <summary>
        ///     Opens the transaction pipeline for sending and receiving and performs initial state synchronization with the drive
        ///     system.
        /// </summary>
        public void Open()
        {
            log.Info($"Opening device endpoint: {factory.Endpoint}");
            transactionProcessor = factory.CreateTransactionProcessor();
            log.Info("====== Initialization completed successfully : Device is now ready to accept commands ======");
        }


        public void PerformOnConnectTasks()
        {
            //ToDo: perform any tasks that must occur as soon as the communication channel is connected.
        }

        protected void RaiseRelayStateChanged(int relay, bool newState)
        {
            var args = new RelayStateChangedEventArgs(relay, newState);
            RelayStateChanged?.Invoke(this, args);
        }

        public event EventHandler<RelayStateChangedEventArgs> RelayStateChanged;

        public void SetRelay(ushort id)
        {
            var transaction = new WriteRelayTransaction(id, true);
            transactionProcessor.CommitTransaction(transaction);
            transaction.WaitForCompletionOrTimeout();
            RaiseRelayStateChanged(id, true);
        }

        public void OpenCap()
        {
            TransactSimpleCommand(Protocol.OpenCover);
        }

        public void CloseCap()
        {
            TransactSimpleCommand(Protocol.CloseCover);
        }

        public ushort GetBrightness()
        {
            var transaction = TransactSimpleCommand(Protocol.GetBrightness);
            var brightness = ushort.Parse(transaction.ResponsePayload);
            return brightness;
        }

        public void ElectroluminescentPanelOff()
        {
            TransactSimpleCommand(Protocol.ElpOff);
        }

        public void SetBrightness(byte brightness)
        {
            TransactSimpleCommand(Protocol.SetBrightness, brightness);
        }

        public void ElectroluminescentPanelOn()
        {
            TransactSimpleCommand(Protocol.ElpOn);
        }

        private SnapCapTransaction TransactSimpleCommand(char command, byte? payload = null)
        {
            var transaction = TransactionFactory.Create(command, payload);
            transactionProcessor.CommitTransaction(transaction);
            transaction.WaitForCompletionOrTimeout();
            if (transaction.Failed)
                throw new TransactionException($"Transaction failed: {transaction}");
            return transaction;
        }
    }
}