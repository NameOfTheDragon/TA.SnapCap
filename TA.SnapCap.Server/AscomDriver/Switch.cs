// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: Switch.cs  Last modified: 2020-02-15@19:31 by Tim Long

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using ASCOM;
using ASCOM.DeviceInterface;
using JetBrains.Annotations;
using TA.SnapCap.Aspects;
using TA.SnapCap.DeviceInterface;
using TA.SnapCap.Server.Properties;

#if DEBUG_IN_EXTERNAL_APP
using System.Windows.Forms;
#endif

namespace TA.SnapCap.Server.AscomDriver
    {
    [ProgId(SharedResources.SwitchDriverId)]
    [Guid("dd351fb1-ad95-4901-9672-777b93d0fe24")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [UsedImplicitly]
    [ServedClassName(SharedResources.DriverName)]
    [NLogTraceWithArguments]
    public class Switch : AscomDriverBase, ISwitchV2
        {
        private readonly IDictionary<short, ISnapCapSwitch> switches = new Dictionary<short, ISnapCapSwitch>();

        /// <summary>Initializes a new instance of the <see cref="Switch" /> class.</summary>
        public Switch()
            {
#if DEBUG_IN_EXTERNAL_APP
            MessageBox.Show("Attach debugger now");
#endif
            //HandleAssemblyResolveEvents();
            //device = CompositionRoot.GetDeviceLayer();
            clientId = SharedResources.ConnectionManager.RegisterClient(SharedResources.SwitchDriverId);
            }

        public bool CanWrite(short id)
            {
            return true;
            }

        [MustBeConnected]
        public bool GetSwitch(short id)
            {
            return switches[id].GetState();
            }

        public string GetSwitchDescription(short id)
            {
            return switches[id].Description;
            }

        public string GetSwitchName(short id)
            {
            return switches[id].Name;
            }

        /// <summary>Returns the value for switch device id as a double</summary>
        [MustBeConnected]
        public double GetSwitchValue(short id)
            {
            return switches[id].GetValue();
            }

        /// <summary>The ASCOM interface version number that this device supports.</summary>
        public override short InterfaceVersion => 2;

        public short MaxSwitch => (short) switches.Count;

        /// <summary>Returns the maximum value for this switch device.</summary>
        public double MaxSwitchValue(short id)
            {
            return switches[id].MaximumValue;
            }

        /// <summary>Returns the minimum value for this switch device.</summary>
        public double MinSwitchValue(short id)
            {
            return switches[id].MinimumValue;
            }

        [MustBeConnected]
        public void SetSwitch(short id, bool state)
            {
            switches[id].SetValue(state);
            }

        public void SetSwitchName(short id, string name)
            {
            Settings.Default.SwitchNames[id] = string.IsNullOrWhiteSpace(name) ? $"Switch {id}" : name;
            Settings.Default.Save();
            }

        /// <summary>Set the value for this device as a double.</summary>
        [MustBeConnected]
        public void SetSwitchValue(short id, double value)
            {
            switches[id].SetValue(value);
            }

        /// <summary>
        ///     Returns the step size that this device supports (the difference between successive values of
        ///     the device).
        /// </summary>
        public double SwitchStep(short id)
            {
            return switches[id].Precision;
            }

        /// <summary>Connects to the device.</summary>
        /// <exception cref="ASCOM.DriverException">
        ///     Failed to connect. Open apparently succeeded but then the
        ///     device reported that is was offline.
        /// </exception>
        protected override void Connect()
            {
            base.Connect();
            switches.Clear();
            switches.Add(0, new SnapCapOpenClose("Open/Close", "Controls the SnapCap cover position", device));
            switches.Add(1,
                new SnapCapFlatPanel("Brightness", "Controls the electroluminescent panel brightness", device));
            }

        private void CreateSwitchNames()
            {
            Settings.Default.SwitchNames = new StringCollection();
            for (var i = 0; i < MaxSwitch; i++)
                {
                Settings.Default.SwitchNames.Add($"Relay {i}");
                }
            }

        /// <summary>Disconnects from the device.</summary>
        protected override void Disconnect()
            {
            base.Disconnect();
            switches.Clear();
            }

        /// <summary>Finalizes this instance (called prior to garbage collection by the CLR)</summary>
        ~Switch()
            {
            Dispose(false);
            }
        }
    }