// This file is part of the TA.SnapCap project
// 
// Copyright © 2007-2017 Tigra Astronomy, all rights reserved.
// 
// File: DeviceController.cs  Created: 2017-05-07@12:52
// Last modified: 2017-05-08@18:17 by Tim Long

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NLog;
using PostSharp.Patterns.Model;
using TA.Ascom.ReactiveCommunications;

namespace TA.SnapCap.DeviceInterface
{
    [NotifyPropertyChanged]
    public class DeviceController : IDisposable, INotifyPropertyChanged
    {
        private readonly ITransactionProcessorFactory factory;
        private readonly Logger log = LogManager.GetCurrentClassLogger();

        private bool disposed;

        [NotNull] private CancellationTokenSource monitorStateCancellation = new CancellationTokenSource();

        private ITransactionProcessor transactionProcessor;

        public DeviceController(ITransactionProcessorFactory factory)
        {
            this.factory = factory;
        }

        public SnapCapDisposition Disposition { get; private set; }
        public bool Illuminated { get; private set; }

        [SafeForDependencyAnalysis]
        public bool IsOnline => transactionProcessor != null && (factory?.Channel?.IsOpen ?? false);

        public bool MotorRunning { get; private set; }

        public SnapCapState State { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Close the connection to the AWR system. This should never fail.
        /// </summary>
        public void Close()
        {
            log.Warn("Close requested");
            monitorStateCancellation.Cancel(); // Cancel any background monitoring.
            if (!IsOnline)
            {
                log.Warn("Ignoring Close request because already closed");
                return;
            }
            log.Info($"Closing device endpoint: {factory.Endpoint}");
            factory.DestroyTransactionProcessor();
            log.Info("====== Channel closed: the device is now disconnected ======");
        }

        public void CloseCap()
        {
            TransactSimpleCommand(Protocol.CloseCover);
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

        public void ElectroluminescentPanelOff()
        {
            TransactSimpleCommand(Protocol.ElpOff);
        }

        public void ElectroluminescentPanelOn()
        {
            TransactSimpleCommand(Protocol.ElpOn);
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

        public ushort GetBrightness()
        {
            var transaction = TransactSimpleCommand(Protocol.GetBrightness);
            var brightness = ushort.Parse(transaction.ResponsePayload);
            return brightness;
        }

        public SnapCapState GetState()
        {
            var transaction = TransactSimpleCommand(Protocol.GetStatus);
            return SnapCapState.FromResponsePayload(transaction.ResponsePayload);
        }

        private async void MonitorState(CancellationToken cancel)
        {
            /*
             * We delay for a short time to allow any startup tasks to complete.
             * This also allows the method to immediately return while monitoring occurs asynchronously
             */
            await Task.Delay(TimeSpan.FromSeconds(5));
            while (!cancel.IsCancellationRequested)
            {
                try
                {
                    var delayTask = Task.Delay(TimeSpan.FromSeconds(1));
                    var transaction = TransactionFactory.Create(Protocol.GetStatus);
                    transactionProcessor?.CommitTransaction(transaction);
                    var transactionTask = transaction.WaitForCompletionOrTimeoutAsync(cancel);
                    Task.WaitAll(new[] {delayTask, transactionTask}, cancel);
                    if (transaction.Failed)
                        throw new TransactionException(transaction.ToString());
                    var state = SnapCapState.FromResponsePayload(transaction.ResponsePayload);
                    UpdateStateProperties(state);
                }
                catch (Exception e)
                {
                    log.Error($"Error in state monitoring task: {e.Message}");
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
            monitorStateCancellation = new CancellationTokenSource();
            MonitorState(monitorStateCancellation.Token);
        }

        public void OpenCap()
        {
            TransactSimpleCommand(Protocol.OpenCover);
        }

        public void PerformOnConnectTasks()
        {
            //ToDo: perform any tasks that must occur as soon as the communication channel is connected.
        }

        public void SetBrightness(byte brightness)
        {
            TransactSimpleCommand(Protocol.SetBrightness, brightness);
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

        private void UpdateStateProperties(SnapCapState state)
        {
            MotorRunning = state.MotorRunning;
            Illuminated = state.Illuminated;
            Disposition = state.Disposition;
        }
    }
}