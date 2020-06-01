// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: CoverCalibrator.cs  Last modified: 2020-05-28@00:28 by Tim Long

using System;
using System.Runtime.InteropServices;
using ASCOM;
using ASCOM.DeviceInterface;
using JetBrains.Annotations;
using NLog.Fluent;
using TA.SnapCap.Aspects;
using TA.SnapCap.DeviceInterface;
using NotImplementedException = ASCOM.NotImplementedException;

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
        public void OpenCover() => device.OpenCap();

        /// <inheritdoc />
        public void CloseCover() => device.CloseCap();

        /// <inheritdoc />
        public void HaltCover()
            {
            Log.Warn().Message("This needs to be implemented").Write();
            throw new NotImplementedException();
            }

        /// <inheritdoc />
        public void CalibratorOn(int Brightness)
            {
            device.SetBrightness((byte) Brightness);
            device.ElectroluminescentPanelOn();
            }

        /// <inheritdoc />
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
            }

        /// <inheritdoc />
        public CalibratorStatus CalibratorState
            {
            //ToDo: must be implemented
            get { return CalibratorStatus.Error; }
            }

        /// <inheritdoc />
        public int Brightness => device.Brightness;

        /// <inheritdoc />
        public int MaxBrightness => 255;
        }
    }