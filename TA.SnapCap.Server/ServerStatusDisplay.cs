// This file is part of the TA.SnapCap project
// 
// Copyright © 2007-2017 Tigra Astronomy, all rights reserved.
// 
// File: ServerStatusDisplay.cs  Created: 2017-05-07@12:52
// Last modified: 2017-05-11@01:41 by Tim Long

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;
using NLog;
using TA.Ascom.ReactiveCommunications.Diagnostics;
using TA.SnapCap.DeviceInterface;
using TA.SnapCap.Server.Properties;
using TA.WinFormsControls;

namespace TA.SnapCap.Server
{
    public partial class ServerStatusDisplay : Form
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private List<Annunciator> annunciators;
        private IDisposable clientStatusSubscription;
        private IDisposable connectedSubscription;
        private IDisposable dispositionSubscription;
        private IDisposable illuminationStateSubscription;
        private IDisposable motorStateSubscription;
        private IDisposable brightnessSubscription;

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
            {
                annunciators.ForEach(p => p.Enabled = true);
                SubscribePropertyChangeNotifications();
            }
            else
            {
                UnsubscribePropertyChangeNotifications();
                annunciators.ForEach(p => p.Enabled = false);
            }
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
                annunciator.Enabled = false;
            }
            var clientStatusObservable = Observable.FromEventPattern<EventHandler<EventArgs>, EventArgs>(
                handler => SharedResources.ConnectionManager.ClientStatusChanged += handler,
                handler => SharedResources.ConnectionManager.ClientStatusChanged -= handler);
            clientStatusSubscription = clientStatusObservable
                .ObserveOn(SynchronizationContext.Current)
                .Trace("Client Status Changed")
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
                .ObserveOn(SynchronizationContext.Current)
                .Trace("Motor State")
                .Subscribe(isRunning => MotorAnnunciator.Enabled = isRunning);
            var observableIlluminationState = controller.GetObservableValueFor(p => p.Illuminated);
            illuminationStateSubscription = observableIlluminationState
                .ObserveOn(SynchronizationContext.Current)
                .Trace("Illumination")
                .Subscribe(b => IlluminationAnnunciator.Enabled = b);
            connectedSubscription = controller.GetObservableValueFor(p => p.IsOnline)
                .ObserveOn(SynchronizationContext.Current)
                .Trace("IsOnline")
                .Subscribe(isOnline => ConnectedAnnunciator.Enabled = isOnline);
            dispositionSubscription = controller.GetObservableValueFor(p => p.Disposition)
                .ObserveOn(SynchronizationContext.Current)
                .Trace("Disposition")
                .Subscribe(UpdateDisposition);
            brightnessSubscription = controller.GetObservableValueFor(p => p.Brightness)
                .ObserveOn(SynchronizationContext.Current)
                .Trace("Brightness")
                .Subscribe(value=>BrightnessAnnunciator.Text=$"{value:000}%");
        }

        /// <summary>
        ///     Stops observing the controller property change notifications.
        /// </summary>
        private void UnsubscribePropertyChangeNotifications()
        {
            // Dispose any observable subscriptions
            motorStateSubscription?.Dispose();
            illuminationStateSubscription?.Dispose();
            connectedSubscription?.Dispose();
            dispositionSubscription?.Dispose();
            brightnessSubscription?.Dispose();
        }

        private void UpdateDisposition(SnapCapDisposition disposition)
        {
            DispositionAnnunciator.Text = disposition.ToString();
            switch (disposition)
            {
                case SnapCapDisposition.Timeout:
                case SnapCapDisposition.OpenCircuit:
                case SnapCapDisposition.Overcurrent:
                    DispositionAnnunciator.Cadence = CadencePattern.BlinkAlarm;
                    DispositionAnnunciator.ForeColor = Color.OrangeRed;
                    break;
                case SnapCapDisposition.UserAbort:
                    DispositionAnnunciator.Cadence = CadencePattern.BlinkSlow;
                    DispositionAnnunciator.ForeColor = Color.Yellow;
                    break;
                case SnapCapDisposition.Open:
                case SnapCapDisposition.Closed:
                default:
                    DispositionAnnunciator.Cadence = CadencePattern.SteadyOn;
                    DispositionAnnunciator.ForeColor = Color.DarkSeaGreen;
                    break;
            }
        }
    }
}