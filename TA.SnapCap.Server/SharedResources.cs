// This file is part of the TA.SnapCap project
// 
// Copyright � 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: SharedResources.cs  Last modified: 2017-05-06@20:23 by Tim Long

using System;
using System.Windows.Forms;
using NLog;
using TA.SnapCap.DeviceInterface;
using TA.SnapCap.Server.Properties;

namespace TA.SnapCap.Server
    {
    /// <summary>
    ///     The resources shared by all drivers and devices, in this example it's a serial port with
    ///     a shared SendMessage method an idea for locking the message and handling connecting is
    ///     given. In reality extensive changes will probably be needed. Multiple drivers means that
    ///     several applications connect to the same hardware device, aka a hub. Multiple devices
    ///     means that there are more than one instance of the hardware, such as two focusers. In
    ///     this case there needs to be multiple instances of the hardware connector, each with it's
    ///     own connection count.
    /// </summary>
    public static class SharedResources
        {
        /// <summary>
        ///     ASCOM DeviceID (COM ProgID) for the rotator driver.
        /// </summary>
        public const string SwitchDriverId = "ASCOM.SnapCap.Switch";
        /// <summary>
        ///     Driver description for the rotator driver.
        /// </summary>
        public const string SwitchDriverName = "SnapCap from Gemini Telescope";

        private static readonly ILogger Log;

        static SharedResources()
            {
            Log = LogManager.GetCurrentClassLogger();
            ConnectionManager = CreateConnectionManager();
            }

        /// <summary>
        ///     Gets the connection manager.
        /// </summary>
        /// <value>The connection manager.</value>
        public static ClientConnectionManager ConnectionManager { get; }

        private static ClientConnectionManager CreateConnectionManager()
            {
            Log.Info("Creating ClientConnectionManager");
            return new ClientConnectionManager(
                CreateTransactionProcessorFactory());
            }

        private static ITransactionProcessorFactory CreateTransactionProcessorFactory()
            {
            Log.Warn(
                $"Creating transaction processor factory with connection string {Settings.Default.ConnectionString}");
            return new ReactiveTransactionProcessorFactory(Settings.Default.ConnectionString ?? "(not set)");
            }

        public static void DoSetupDialog(Guid clientId)
            {
            var oldConnectionString = Settings.Default.ConnectionString;
            Log.Info($"SetupDialog requested by client {clientId}");
            using (var dialogForm = new SetupDialogForm())
                {
                var result = dialogForm.ShowDialog();
                switch (result)
                    {
                        case DialogResult.OK:
                            Log.Info($"SetupDialog successful, saving settings");
                            Settings.Default.Save();
                            var newConnectionString = Settings.Default.ConnectionString;
                            if (oldConnectionString != newConnectionString)
                                {
                                Log.Warn(
                                    $"Connection string has changed from {oldConnectionString} to {newConnectionString} - replacing the TansactionProcessorFactory");
                                ConnectionManager.TransactionProcessorFactory = CreateTransactionProcessorFactory();
                                }
                            break;
                        default:
                            Log.Warn("SetupDialog cancelled or failed - reverting to previous settings");
                            Settings.Default.Reload();
                            break;
                    }
                }
            }
        }
    }