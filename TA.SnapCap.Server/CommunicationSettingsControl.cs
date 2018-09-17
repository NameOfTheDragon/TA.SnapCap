// This file is part of the TA.SnapCap project
// 
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: CommunicationSettingsControl.cs  Last modified: 2017-05-06@20:05 by Tim Long

using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using TA.SnapCap.Server.Properties;

namespace TA.SnapCap.Server
    {
    public partial class CommunicationSettingsControl : UserControl
        {
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
            Settings.Default.ConnectionString = $"{Settings.Default.CommPortName}:{Settings.Default.SerialParameters}";
            Settings.Default.Save();
            }
        }
    }