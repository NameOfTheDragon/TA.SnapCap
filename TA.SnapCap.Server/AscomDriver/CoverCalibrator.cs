// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of András Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ASCOM;
using ASCOM.DeviceInterface;
using ASCOM.Utilities.Exceptions;
using JetBrains.Annotations;
using NLog.Fluent;
using TA.SnapCap.DeviceInterface;
using NotImplementedException = ASCOM.NotImplementedException;
using InvalidValueException = ASCOM.InvalidValueException;
using TA.SnapCap.SharedTypes;
using System.Runtime.CompilerServices;

namespace TA.SnapCap.Server.AscomDriver
    {
    [ProgId(SharedResources.CoverCalibratorDriverId)]
    [Guid("2c2a2dfe-497c-4eed-a808-9d038c2b441a")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [UsedImplicitly]
    [ServedClassName(SharedResources.DriverName)]
    public class CoverCalibrator : AscomDriverBase, ICoverCalibratorV1
        {
        public CoverCalibrator()
            {
            clientId = SharedResources.ConnectionManager.RegisterClient(SharedResources.CoverCalibratorDriverId);
            }

        /// <inheritdoc />
        public void OpenCover()
            {
            AssertConnected();
            device.OpenCap();
            }

        /// <inheritdoc />
        public void CloseCover()
            {
            AssertConnected();
            device.CloseCap();
            }

        /// <inheritdoc />
        public void HaltCover()
            {
            AssertConnected();
            device.Halt();
            }

        /// <inheritdoc />
        public void CalibratorOn(int Brightness)
            {
            AssertConnected();
            if (Brightness < 0 || Brightness > ValueConverterExtensions.AscomMaxBrightness)
                throw new InvalidValueException(
                    $"Brightness {Brightness} is outside the allowed range of 0..{ValueConverterExtensions.AscomMaxBrightness}");
            if (Brightness == 0)
                {
                device.SetBrightness(0);
                device.ElectroluminescentPanelOn();
                }
            else
                {
                var deviceBrightness = Brightness.ToDeviceBrightness();
                device.SetBrightness((byte)deviceBrightness);
                device.ElectroluminescentPanelOn();
                }
            }

        /// <inheritdoc />
        public void CalibratorOff()
            {
            AssertConnected();
            device.SetBrightness(0);
            device.ElectroluminescentPanelOff();
            }

        /// <inheritdoc />
        public override short InterfaceVersion => 1;

        /// <inheritdoc />
        public CoverStatus CoverState
            {
            get
                {
                if (device?.IsOnline ?? false)
                    {
                    if (device.MotorRunning)
                        return CoverStatus.Moving;
                    var status = device.Disposition;
                    switch (status)
                        {
                            case SnapCapDisposition.Open:
                                return CoverStatus.Open;
                            case SnapCapDisposition.Closed:
                                return CoverStatus.Closed;
                            case SnapCapDisposition.Timeout:
                            case SnapCapDisposition.OpenCircuit:
                            case SnapCapDisposition.Overcurrent:
                                return CoverStatus.Error;
                            default:
                                return CoverStatus.Unknown;
                        }
                    }
                return CoverStatus.Unknown;
                }
            }

        /// <inheritdoc />
        public CalibratorStatus CalibratorState
            {
            get
                {
                if (!(device?.IsOnline ?? false))
                    return CalibratorStatus.Unknown;
                if (!device.Illuminated)
                    return CalibratorStatus.Off;
                return device.LampWarmingUp ? CalibratorStatus.NotReady : CalibratorStatus.Ready;
                }
            }

        /// <inheritdoc />
        public int Brightness => device.Brightness.ToAscomBrightness(); // 0..231

        /// <inheritdoc />
        /// <remarks>
        ///     SnapCap has a range of valid brightnesses of 25..255, therefore we need to scale this to a
        ///     zero-based contiguous range here in the ASCOM interface.
        /// </remarks>
        public int MaxBrightness => ValueConverterExtensions.AscomMaxBrightness;

        private void AssertConnected([CallerMemberName] string caller = null)
            {
            if (Connected) return;
            var message = $"The device is not connected. Method {caller} requires a connected device.";
            log.Error(message);
            throw new NotConnectedException(message);
            }

        /// <inheritdoc />
        protected override void Connect()
            {
            base.Connect();
            while (CoverState == CoverStatus.Moving) Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
            }
        }
    }