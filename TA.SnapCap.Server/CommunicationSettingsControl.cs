// This file is part of the TA.SnapCap project
// 
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: CommunicationSettingsControl.cs  Last modified: 2017-05-06@20:05 by Tim Long

using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using TA.NexDome.Server;
using TA.SnapCap.HardwareSimulator;
using TA.SnapCap.Server.Properties;

namespace TA.SnapCap.Server
    {
    public partial class CommunicationSettingsControl : UserControl
        {
        private ClickCommand channelTypeClickCommand;

        public CommunicationSettingsControl()
            {
            InitializeComponent();
            var currentSelection = Settings.Default.CommPortName;
            var ports = new SortedSet<string>(SerialPort.GetPortNames());
            if (!ports.Contains(currentSelection))
                {
                ports.Add(currentSelection);
                }
            CommPortName.Items.Clear();
            CommPortName.Items.AddRange(ports.ToArray());
            var currentIndex = CommPortName.Items.IndexOf(currentSelection);
            CommPortName.SelectedIndex = currentIndex;
            }

        public void Save()
            {
            RegenerateConnectionString();
            Settings.Default.Save();
            }

        private void RegenerateConnectionString()
            {
            var candidateConnectionString = UseSimulatorCheckbox.Checked
                ? "Simulator:Realtime"
                : $"{Settings.Default.CommPortName}:{Settings.Default.SerialParameters}";
            Settings.Default.ConnectionString = candidateConnectionString;
            }

        private void CommunicationSettingsControl_Load(object sender, System.EventArgs e)
            {
            channelTypeClickCommand = CommPortName.AttachCommand(() => { }, () => !UseSimulatorCheckbox.Checked);
            }

        private void UseSimulatorCheckbox_CheckedChanged(object sender, System.EventArgs e)
            {
            channelTypeClickCommand.CanExecuteChanged();
            RegenerateConnectionString();
            }

        private void CommPortName_Changed(object sender, System.EventArgs e)
            {
            RegenerateConnectionString();
            }
        }
    }