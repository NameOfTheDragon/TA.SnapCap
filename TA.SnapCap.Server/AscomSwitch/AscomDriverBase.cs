// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: AscomDriverBase.cs  Last modified: 2020-02-15@19:33 by Tim Long

using System;
using System.Collections;
using System.Reflection;
using ASCOM;
using JetBrains.Annotations;
using NLog;
using TA.SnapCap.Aspects;
using TA.SnapCap.DeviceInterface;
using NotImplementedException = System.NotImplementedException;

namespace TA.SnapCap.Server.AscomSwitch
    {
    public abstract class AscomDriverBase : ReferenceCountedObjectBase, IDisposable, IAscomDriver
        {
        protected readonly ILogger log = LogManager.GetCurrentClassLogger();
        protected Guid clientId;
        protected DeviceController device;
        protected bool disposed;

        internal bool IsOnline => device?.IsOnline ?? false;

        /// <summary>
        ///     Returns a description of the device, such as manufacturer and modelnumber. Any ASCII characters
        ///     may be used.
        /// </summary>
        public string Description => "SnapCap Controller";

        /// <summary>Descriptive and version information about this ASCOM driver.</summary>
        public string DriverInfo => @"ASCOM Switch driver for GTD SnapCap
Professionally developed by Tigra Astronomy";

        /// <summary>A string containing only the major and minor version of the driver.</summary>
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

        /// <summary>The ASCOM interface version number that this device supports.</summary>
        public abstract short InterfaceVersion { get; }

        /// <summary>The short name of the driver, for display purposes</summary>
        public string Name => SharedResources.SwitchDriverName;

        /// <summary>Returns the list of action names supported by this driver (currently none supported).</summary>
        public ArrayList SupportedActions => new ArrayList();

        /// <summary>Gets or sets the connection state.</summary>
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

        public void Dispose()
            {
            Dispose(true);
            GC.SuppressFinalize(this);
            }

        [MustBeConnected]
        public string Action(string ActionName, string ActionParameters)
            {
            throw new NotImplementedException();
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

        protected virtual void Dispose(bool fromUserCode)
            {
            if (!disposed)
                {
                if (fromUserCode)
                    {
                    Disconnect();
                    }
                SharedResources.ConnectionManager.UnregisterClient(clientId);
                }
            disposed = true;
            }

        public void SetupDialog()
            {
            SharedResources.DoSetupDialog(clientId);
            }

        /// <summary>Connects to the device.</summary>
        /// <exception cref="ASCOM.DriverException">
        ///     Failed to connect. Open apparently succeeded but then the
        ///     device reported that is was offline.
        /// </exception>
        protected virtual void Connect()
            {
            device = SharedResources.ConnectionManager.GoOnline(clientId);
            if (!device.IsOnline)
                {
                log.Error("Connect failed - device reported offline");
                throw new DriverException(
                    "Failed to connect. Open apparently succeeded but then the device reported that is was offline.");
                }
            device.PerformOnConnectTasks().Wait();
            }

        /// <summary>Disconnects from the device.</summary>
        protected virtual void Disconnect()
            {
            SharedResources.ConnectionManager.GoOffline(clientId);
            device = null; //[Sentinel]
            }
        }
    }