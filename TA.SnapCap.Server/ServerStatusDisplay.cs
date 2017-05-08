// This file is part of the TA.SnapCap project
// 
// Copyright © 2007-2017 Tigra Astronomy, all rights reserved.
// 
// File: ServerStatusDisplay.cs  Created: 2017-05-07@12:52
// Last modified: 2017-05-08@19:15 by Tim Long

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;
using ASCOM.Controls;
using NLog;
using TA.Ascom.ReactiveCommunications.Diagnostics;
using TA.SnapCap.DeviceInterface;
using TA.SnapCap.Server.Properties;

namespace TA.SnapCap.Server
{
    public partial class ServerStatusDisplay : Form
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private List<Annunciator> annunciators;
        private IDisposable clientStatusSubscription;
        private IDisposable illuminationStateSubscription;
        private IDisposable motorStateSubscription;

        public ServerStatusDisplay()
        {
            InitializeComponent();
        }

        private void CalibrateCommand_Click(object sender, EventArgs e)
        {
            if (!SharedResources.ConnectionManager.MaybeControllerInstance.Any())
                return;
            //SharedResources.ConnectionManager.MaybeControllerInstance.Single().CalibrateFocuserAsync();
        }

        /// <summary>
        ///     Begins or terminates UI updates depending on the number of online clients.
        /// </summary>
        private void ConfigureUiPropertyNotifications()
        {
            var clientsOnline = SharedResources.ConnectionManager.OnlineClientCount;
            if (clientsOnline > 0)
                SubscribePropertyChangeNotifications();
            else
                UnsubscribePropertyChangeNotifications();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            clientStatusSubscription?.Dispose();
            UnsubscribePropertyChangeNotifications();
            var clients = SharedResources.ConnectionManager.Clients;
            foreach (var client in clients)
            {
                SharedResources.ConnectionManager.GoOffline(client.ClientId);
            }
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            annunciators = new List<Annunciator>
            {
                ConnectedAnnunciator,
                MotorAnnunciator,
                IlluminationAnnunciator,
                BrightnessAnnunciator,
                DispositionAnnunciator
            };
            foreach (var annunciator in annunciators)
            {
                annunciator.Enabled = true;
                annunciator.Mute = true;
            }
            var clientStatusObservable = Observable.FromEventPattern<EventHandler<EventArgs>, EventArgs>(
                handler => SharedResources.ConnectionManager.ClientStatusChanged += handler,
                handler => SharedResources.ConnectionManager.ClientStatusChanged -= handler);
            clientStatusSubscription = clientStatusObservable
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(ObserveClientStatusChanged);
            ObserveClientStatusChanged(null); // This sets the initial UI state before any notifications arrive
        }

        private void frmMain_LocationChanged(object sender, EventArgs e)
        {
            Settings.Default.Save();
        }


        private void ObserveClientStatusChanged(EventPattern<EventArgs> eventPattern)
        {
            SetUiButtonState();
            SetUiDeviceConnectedState();
            var clientStatus = SharedResources.ConnectionManager.Clients;
            try
            {
                ClientStatus.BeginUpdate();
                ClientStatus.Items.Clear();
                foreach (var client in clientStatus)
                {
                    ClientStatus.Items.Add(client);
                }
            }
            finally
            {
                ClientStatus.EndUpdate();
            }
            registeredClientCount.Text = clientStatus.Count().ToString();
            OnlineClients.Text = clientStatus.Count(p => p.Online).ToString();
            ConfigureUiPropertyNotifications();
        }


        private void SetPaCommand_Click(object sender, EventArgs e)
        {
            if (!SharedResources.ConnectionManager.MaybeControllerInstance.Any())
                return;
            //SharedResources.ConnectionManager.MaybeControllerInstance.Single().SetRotatorPositionAngle(0.0);
        }

        private void SetUiButtonState()
        {
        }

        /// <summary>
        ///     Enables each device activity annunciator if there are any clients connected to that
        ///     device.
        /// </summary>
        private void SetUiDeviceConnectedState()
        {
        }

        private void SetupCommand_Click(object sender, EventArgs e)
        {
            SharedResources.DoSetupDialog(default(Guid));
        }

        /// <summary>
        ///     Creates subscriptions that observe property change notifications and update the UI as changes occur.
        /// </summary>
        private void SubscribePropertyChangeNotifications()
        {
            if (!SharedResources.ConnectionManager.MaybeControllerInstance.Any())
                return;
            var controller = SharedResources.ConnectionManager.MaybeControllerInstance.Single();
            //var relayStateChangeEvents = Observable
            //    .FromEventPattern<EventHandler<RelayStateChangedEventArgs>, RelayStateChangedEventArgs>(
            //        handler => handler,
            //        handler => handler);

            //relayStateChangedSubscription = relayStateChangeEvents
            //    .Select(element => element.EventArgs)
            //    .ObserveOn(SynchronizationContext.Current)
            //    .Trace("Annunciators")
            //    .Subscribe(UpdateRelayAnnunciator);

            var observableMotorState = controller.GetObservableValueFor(p => p.MotorRunning);
            motorStateSubscription = observableMotorState
                .Trace("Motor State")
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(p => MotorAnnunciator.Mute = !p);
            var observableIlluminationState = controller.GetObservableValueFor(p => p.Illuminated);
            illuminationStateSubscription = observableIlluminationState
                .Trace("Illumination")
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(p => IlluminationAnnunciator.Mute = !p);
        }

        /// <summary>
        ///     Stops observing the controller property change notifications.
        /// </summary>
        private void UnsubscribePropertyChangeNotifications()
        {
            // Dispose any observable subscriptions
            motorStateSubscription?.Dispose();
            illuminationStateSubscription?.Dispose();
        }

        private void UpdateRelayAnnunciator(RelayStateChangedEventArgs args)
        {
            log.Info($"Relay {args.RelayNumber} changed to {args.NewState}");
            if (annunciators == null || annunciators.Count == 0)
            {
                log.Warn(
                    $"Relay {args.RelayNumber} changed to {args.NewState} but there are no annunciators to update");
                return;
            }
            var annunciator = annunciators[args.RelayNumber];
            annunciator.Enabled = args.NewState;
        }
    }
}