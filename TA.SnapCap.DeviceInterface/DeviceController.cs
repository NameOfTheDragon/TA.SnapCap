// This file is part of the TA.SnapCap project
//
// Copyright © 2007-2017 Tigra Astronomy, all rights reserved.
//
// File: DeviceController.cs  Created: 2017-05-07@12:52
// Last modified: 2017-05-11@02:52 by Tim Long

using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NLog;
using NLog.Fluent;
using PostSharp.Patterns.Model;
using TA.Ascom.ReactiveCommunications;
using TA.SnapCap.SharedTypes;

namespace TA.SnapCap.DeviceInterface
{
    [NotifyPropertyChanged]
    public class DeviceController : IDisposable, INotifyPropertyChanged
    {
        private const double BrightnessRange = 255.0 - 24.0;
        private readonly ITransactionProcessorFactory factory;
        private readonly Logger log = LogManager.GetCurrentClassLogger();

        private bool disposed;

        [NotNull] private CancellationTokenSource monitorStateCancellation = new CancellationTokenSource();

        private ITransactionProcessor transactionProcessor;

        public DeviceController(ITransactionProcessorFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        ///     Gets the brightness setting of the electroluminescent panel, as a percentage
        ///     in the range 0 to 100. This value is updated whenever the brightness is read or written.
        /// </summary>
        public int Brightness { get; private set; } = 50;

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

        internal static byte BrightnessFromPercent(double percentBrightness)
        {
            if (percentBrightness >= 100.0)
                return 255;
            if (percentBrightness <= 0.0)
                return 0;
            var fractionOfUnity = percentBrightness / 100.0;
            var brightness = (int) Math.Ceiling(BrightnessRange * fractionOfUnity);
            return (byte) (brightness + 24);
        }

        internal static int BrightnessToPercent(byte brightness)
        {
            if (brightness < 24)
                return 0;
            var offsetBrightness = brightness - 24;

            var fractionOfUnity = offsetBrightness / BrightnessRange;
            return (int) (fractionOfUnity * 100.0);
        }

        /// <summary>
        ///     Close the connection to the target system. This should never fail.
        /// </summary>
        public void Close()
        {
            log.Warn("Close requested");
        try
            {
            monitorStateCancellation.Cancel(); // Cancel any background monitoring.
            }
        catch (Exception e)
            {
            // Only log the exception because closing cannot fail.
            Log.Warn().Exception(e).Message("Error when cancelling monitoring task: {message} ", e.Message).Write();
            }
        if (!IsOnline)
            {
            log.Warn("Ignoring Close request because already closed");
            return;
            }
        log.Info($"Closing device endpoint: {factory.Endpoint}");
        factory.DestroyTransactionProcessor();
        log.Info("====== Channel closed: the device is now disconnected ======");
        OnPropertyChanged(nameof(IsOnline));
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

        /// <summary>
        ///     Gets the electroluminescent panel brightness setting, in the range 0..255.
        /// </summary>
        /// <seealso cref="Brightness" />
        public ushort GetBrightness()
        {
            var transaction = TransactSimpleCommand(Protocol.GetBrightness);
            var brightness = byte.Parse(transaction.ResponsePayload);
            Brightness = BrightnessToPercent(brightness);
            return brightness;
        }

        public SnapCapState GetState()
        {
            Brightness = GetBrightness();
            var transaction = TransactSimpleCommand(Protocol.GetStatus);
            return SnapCapState.FromResponsePayload(transaction.ResponsePayload);
        }

        private async void MonitorState(CancellationToken cancel)
        {
            /*
             * We delay for a short time to allow any startup tasks to complete.
             * This also allows the method to immediately return while monitoring occurs asynchronously
             */
            await Task.Delay(TimeSpan.FromSeconds(5), cancel).ConfigureAwait(false);
            while (!cancel.IsCancellationRequested)
            {
                try
                {
                    var delayTask = Task.Delay(TimeSpan.FromSeconds(5), cancel);
                    var transactionTask = PollDeviceState(cancel);
                    Task.WaitAll(new[] {delayTask, transactionTask}, cancel);
                }
                catch (Exception e)
                {
                    log.Error($"Error in state monitoring task: {e.Message}");
                }
            }
        }

        private void MonitorStateWorker(object state)
        {
            var cancel = (CancellationToken) state;
            while (!cancel.IsCancellationRequested)
            {
                try
                {
                    Task.Delay(TimeSpan.FromSeconds(1), cancel).Wait(cancel);
                    PollDeviceState(cancel).Wait(cancel);
                }
                catch (TaskCanceledException)
                {
                    log.Warn($"Monitoring task cancelled");
                }
                catch (Exception e)
                {
                    log.Error($"Error in State Monitor Worker: {e.Message}");
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([NotNull] string propertyName)
        {
            log.Debug($"NotifyPropertyChanged: {propertyName}");
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
            ThreadPool.QueueUserWorkItem(MonitorStateWorker, monitorStateCancellation.Token);
            OnPropertyChanged(nameof(IsOnline));
        }

        public void OpenCap()
        {
            TransactSimpleCommand(Protocol.OpenCover);
        }

        public async Task PerformOnConnectTasks()
        {
            await PollDeviceState(monitorStateCancellation.Token).ConfigureAwait(false);
            var brightness = GetBrightness();
            Brightness = BrightnessToPercent((byte) brightness);
        }

        private async Task PollDeviceState(CancellationToken cancel)
        {
            var transaction = TransactionFactory.Create(Protocol.GetStatus);
            transactionProcessor?.CommitTransaction(transaction);
            await transaction.WaitForCompletionOrTimeoutAsync(cancel).ConfigureAwait(false);
            if (transaction.Failed)
                throw new TransactionException(transaction.ToString());
            var state = SnapCapState.FromResponsePayload(transaction.ResponsePayload);
            UpdateStateProperties(state);
        }

        public void SetBrightness(byte brightness)
        {
            TransactSimpleCommand(Protocol.SetBrightness, brightness);
            Brightness = BrightnessToPercent(brightness);
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