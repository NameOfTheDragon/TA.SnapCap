// This file is part of the TA.SnapCap project
// 
// Copyright © 2007-2017 Tigra Astronomy, all rights reserved.
// 
// File: Switch.cs  Created: 2017-05-07@12:52
// Last modified: 2017-05-11@02:57 by Tim Long

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.InteropServices;
using ASCOM;
using ASCOM.DeviceInterface;
using JetBrains.Annotations;
using NLog;
using TA.SnapCap.Aspects;
using TA.SnapCap.DeviceInterface;
using TA.SnapCap.Server;
using TA.SnapCap.Server.Properties;
using NotImplementedException = System.NotImplementedException;

#if DEBUG_IN_EXTERNAL_APP
using System.Windows.Forms;

#endif

namespace TA.SnapCap.AscomSwitch
{
    [ProgId(SharedResources.SwitchDriverId)]
    [Guid("dd351fb1-ad95-4901-9672-777b93d0fe24")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [UsedImplicitly]
    [ServedClassName(SharedResources.SwitchDriverName)]
    [NLogTraceWithArguments]
    public class Switch : ReferenceCountedObjectBase, ISwitchV2, IDisposable, IAscomDriver
    {
        private readonly Guid clientId;
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly IDictionary<short, ISnapCapSwitch> switches = new Dictionary<short, ISnapCapSwitch>();
        private DeviceController device;
        private bool disposed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Switch" /> class.
        /// </summary>
        public Switch()
        {
#if DEBUG_IN_EXTERNAL_APP
            MessageBox.Show("Attach debugger now");
#endif
            //HandleAssemblyResolveEvents();
            //device = CompositionRoot.GetDeviceLayer();
            clientId = SharedResources.ConnectionManager.RegisterClient(SharedResources.SwitchDriverId);
        }

        internal bool IsOnline => device?.IsOnline ?? false;

        [MustBeConnected]
        public string Action(string ActionName, string ActionParameters)
        {
            throw new NotImplementedException();
        }

        public bool CanWrite(short id)
        {
            return true;
        }

        public void CommandBlind(string Command, bool Raw = false)
        {
            throw new NotImplementedException();
        }

        public bool CommandBool(string Command, bool Raw = false)
        {
            throw new NotImplementedException();
        }

        public string CommandString(string Command, bool Raw = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gets or sets the connection state.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        public bool Connected
            {
            get => IsOnline;
            set
                {
                if (value)
                    Connect();
                else
                    Disconnect();
                }
            }

        /// <summary>
        ///     Returns a description of the device, such as manufacturer and modelnumber. Any ASCII characters may be used.
        /// </summary>
        public string Description => "SnapCap Controller";

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Descriptive and version information about this ASCOM driver.
        /// </summary>
        public string DriverInfo => @"ASCOM Switch driver for GTD SnapCap
Professionally developed by Tigra Astronomy";

        /// <summary>
        ///     A string containing only the major and minor version of the driver.
        /// </summary>
        [NotNull]
        public string DriverVersion
            {
            get
                {
                var myAssembly = Assembly.GetExecutingAssembly();
                var myVersion = myAssembly.GetName().Version;
                return $"{myVersion.Major}.{myVersion.Minor}";
                }
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

        /// <summary>
        ///     Returns the value for switch device id as a double
        /// </summary>
        [MustBeConnected]
        public double GetSwitchValue(short id)
        {
            return switches[id].GetValue();
        }

        /// <summary>
        ///     The ASCOM interface version number that this device supports.
        /// </summary>
        public short InterfaceVersion => 2;

        public short MaxSwitch => (short) switches.Count;

        /// <summary>
        ///     Returns the maximum value for this switch device.
        /// </summary>
        public double MaxSwitchValue(short id)
        {
            return switches[id].MaximumValue;
        }

        /// <summary>
        ///     Returns the minimum value for this switch device.
        /// </summary>
        public double MinSwitchValue(short id)
        {
            return switches[id].MinimumValue;
        }

        /// <summary>
        ///     The short name of the driver, for display purposes
        /// </summary>
        public string Name => SharedResources.SwitchDriverName;

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

        /// <summary>
        ///     Set the value for this device as a double.
        /// </summary>
        [MustBeConnected]
        public void SetSwitchValue(short id, double value)
        {
            switches[id].SetValue(value);
        }

        public void SetupDialog()
        {
            SharedResources.DoSetupDialog(clientId);
        }

        /// <summary>
        ///     Returns the list of action names supported by this driver (currently none supported).
        /// </summary>
        public ArrayList SupportedActions => new ArrayList();

        /// <summary>
        ///     Returns the step size that this device supports (the difference between successive values of the device).
        /// </summary>
        public double SwitchStep(short id)
        {
            return switches[id].Precision;
        }

        /// <summary>
        ///     Connects to the device.
        /// </summary>
        /// <exception cref="ASCOM.DriverException">
        ///     Failed to connect. Open apparently succeeded but then the device reported that
        ///     is was offline.
        /// </exception>
        private void Connect()
        {
            device = SharedResources.ConnectionManager.GoOnline(clientId);
            if (!device.IsOnline)
            {
                log.Error("Connect failed - device reported offline");
                throw new DriverException(
                    "Failed to connect. Open apparently succeeded but then the device reported that is was offline.");
            }
            device.PerformOnConnectTasks().Wait();
            switches.Clear();
            switches.Add(0, new SnapCapOpenClose("Open/Close", "Controls the SnapCap cover position", device));
            switches.Add(1, new SnapCapFlatPanel("Brightness", "Controls the electroluminescent panel brightness", device));
        }

        private void CreateSwitchNames()
        {
            Settings.Default.SwitchNames = new StringCollection();
            for (var i = 0; i < MaxSwitch; i++)
            {
                Settings.Default.SwitchNames.Add($"Relay {i}");
            }
        }

        /// <summary>
        ///     Disconnects from the device.
        /// </summary>
        private void Disconnect()
        {
            SharedResources.ConnectionManager.GoOffline(clientId);
            switches.Clear();
            device = null; //[Sentinel]
        }

        protected virtual void Dispose(bool fromUserCode)
        {
            if (!disposed)
            {
                if (fromUserCode)
                {
                    SharedResources.ConnectionManager.UnregisterClient(clientId);
                }
            }
            disposed = true;
        }


        /// <summary>
        ///     Finalizes this instance (called prior to garbage collection by the CLR)
        /// </summary>
        ~Switch()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Installs a custom assembly resolver into the AppDomain so that the driver can find its
        ///     referenced assemblies. This avoids the need for strong-naming
        /// </summary>
        private void HandleAssemblyResolveEvents()
        {
            AppDomain.CurrentDomain.AssemblyResolve += AscomDriverAssemblyResolver.ResolveSupportAssemblies;
        }
    }
}