// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: ServerStatusDisplay.cs  Last modified: 2020-06-02@21:56 by Tim Long

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;
using NLog;
using TA.SnapCap.DeviceInterface;
using TA.SnapCap.Server.Properties;
using Timtek.ReactiveCommunications.Diagnostics;
using Timtek.WinForms;

namespace TA.SnapCap.Server
    {
    public partial class ServerStatusDisplay : Form
        {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private List<Annunciator> annunciators;
        private IDisposable clientStatusSubscription;
        private List<IDisposable> disposables = new List<IDisposable>();

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

        /// <summary>Begins or terminates UI updates depending on the number of online clients.</summary>
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

        private void SetUiButtonState() { }

        /// <summary>
        ///     Enables each device activity annunciator if there are any clients connected to that
        ///     device.
        /// </summary>
        private void SetUiDeviceConnectedState() { }

        private void SetupCommand_Click(object sender, EventArgs e)
            {
            SharedResources.DoSetupDialog(default);
            }

        /// <summary>
        ///     Creates subscriptions that observe property change notifications and update the UI as changes
        ///     occur.
        /// </summary>
        private void SubscribePropertyChangeNotifications()
            {
            if (!SharedResources.ConnectionManager.MaybeControllerInstance.Any())
                return;
            var controller = SharedResources.ConnectionManager.MaybeControllerInstance.Single();
            disposables.Add(
                controller.GetObservableValueFor(p => p.MotorRunning)
                    .ObserveOn(SynchronizationContext.Current)
                    .Trace("Motor State")
                    .Subscribe(isRunning => MotorAnnunciator.Enabled = isRunning)
                );
            disposables.Add(
                controller.GetObservableValueFor(p => p.Illuminated)
                    .ObserveOn(SynchronizationContext.Current)
                    .Trace("Illumination")
                    .Subscribe(b => ConfigureLampDisplay(controller))
                );
            disposables.Add(
                controller.GetObservableValueFor(p => p.LampWarmingUp)
                    .ObserveOn(SynchronizationContext.Current)
                    .Trace("WarmUp")
                    .Subscribe(b => ConfigureLampDisplay(controller))
                );
            disposables.Add(
                controller.GetObservableValueFor(p => p.IsOnline)
                    .ObserveOn(SynchronizationContext.Current)
                    .Trace("IsOnline")
                    .Subscribe(isOnline => ConnectedAnnunciator.Enabled = isOnline)
                );
            disposables.Add(
                controller.GetObservableValueFor(p => p.Disposition)
                    .ObserveOn(SynchronizationContext.Current)
                    .Trace("Disposition")
                    .Subscribe(UpdateDisposition)
            );
            disposables.Add(
                controller.GetObservableValueFor(p => p.Brightness)
                .ObserveOn(SynchronizationContext.Current)
                .Trace("Brightness")
                .Subscribe(value => BrightnessAnnunciator.Text = $"{value:000}%")
                );
            }

        private void ConfigureLampDisplay(DeviceController controller)
            {
            if (controller.Illuminated)
                {
                if (controller.LampWarmingUp)
                    {
                    IlluminationAnnunciator.ActiveColor = Color.PaleGoldenrod;
                    IlluminationAnnunciator.Cadence = CadencePattern.BlinkFast;
                    }
                else
                    {
                    IlluminationAnnunciator.ActiveColor = Color.DarkSeaGreen;
                    IlluminationAnnunciator.Cadence = CadencePattern.Wink;
                    }
                IlluminationAnnunciator.Mute = false;
                IlluminationAnnunciator.Enabled = true;
                }
            else
                {
                IlluminationAnnunciator.Mute = true;
                }
            }

        /// <summary>Stops observing the controller property change notifications.</summary>
        private void UnsubscribePropertyChangeNotifications()
            {
            disposables?.ForEach(p=>p.Dispose());
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