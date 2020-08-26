// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: CoverCalibrator.cs  Last modified: 2020-05-28@00:28 by Tim Long

using System;
using System.Runtime.InteropServices;
using ASCOM;
using ASCOM.DeviceInterface;
using ASCOM.Utilities.Exceptions;
using JetBrains.Annotations;
using NLog.Fluent;
using TA.SnapCap.Aspects;
using TA.SnapCap.DeviceInterface;
using NotImplementedException = ASCOM.NotImplementedException;
using InvalidValueException = ASCOM.InvalidValueException;
using TA.SnapCap.SharedTypes;

namespace TA.SnapCap.Server.AscomDriver
    {
    [ProgId(SharedResources.CoverCalibratorDriverId)]
    [Guid("2c2a2dfe-497c-4eed-a808-9d038c2b441a")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [UsedImplicitly]
    [ServedClassName(SharedResources.DriverName)]
    [NLogTraceWithArguments]
    public class CoverCalibrator : AscomDriverBase, ICoverCalibratorV1
        {

        public CoverCalibrator()
            {
            clientId = SharedResources.ConnectionManager.RegisterClient(SharedResources.CoverCalibratorDriverId);
            }

        /// <inheritdoc />
        [MustBeConnected] public void OpenCover() => device.OpenCap();

        /// <inheritdoc />
        [MustBeConnected] public void CloseCover() => device.CloseCap();

        /// <inheritdoc />
        [MustBeConnected] public void HaltCover() => device.Halt();

        /// <inheritdoc />
        [MustBeConnected]
        public void CalibratorOn(int Brightness)
            {
            if (Brightness < 1 || Brightness > ValueConverterExtensions.AscomMaxBrightness)
                {
                throw new InvalidValueException(
                    $"Brightness {Brightness} is outside the allowed range of 1..{ValueConverterExtensions.AscomMaxBrightness}");
                }
            var deviceBrightness = Brightness.ToDeviceBrightness();
            device.SetBrightness((byte)deviceBrightness);
            device.ElectroluminescentPanelOn();
            }

        /// <inheritdoc />
        [MustBeConnected]
        public void CalibratorOff()
            {
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
                if (device?.IsOnline ?? false)
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
        /// SnapCap has a range of valid brightnesses of 25..255, therefore we need to scale this
        /// to a zero-based contiguous range here in the ASCOM interface.
        /// </remarks>
        public int MaxBrightness => ValueConverterExtensions.AscomMaxBrightness;
        }
    }