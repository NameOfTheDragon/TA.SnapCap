// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: CoverCalibrator.cs  Last modified: 2020-05-27@19:35 by Tim Long

using System.Runtime.InteropServices;
using ASCOM;
using ASCOM.DeviceInterface;
using JetBrains.Annotations;
using TA.SnapCap.Aspects;

namespace TA.SnapCap.Server.AscomDriver
    {
    [ProgId(SharedResources.CoverCalibratorDriverId)]
    [Guid("2c2a2dfe-497c-4eed-a808-9d038c2b441a")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    //[UsedImplicitly]
    [ServedClassName(SharedResources.DriverName)]
    //[NLogTraceWithArguments]
    public class CoverCalibrator : AscomDriverBase, ICoverCalibratorV1
        {
        /// <inheritdoc />
        public void OpenCover() { }

        /// <inheritdoc />
        public void CloseCover() { }

        /// <inheritdoc />
        public void HaltCover() { }

        /// <inheritdoc />
        public void CalibratorOn(int Brightness) { }

        /// <inheritdoc />
        public void CalibratorOff() { }

        /// <inheritdoc />
        public override short InterfaceVersion => 1;

        /// <inheritdoc />
        public CoverStatus CoverState { get; }

        /// <inheritdoc />
        public CalibratorStatus CalibratorState { get; }

        /// <inheritdoc />
        public int Brightness { get; }

        /// <inheritdoc />
        public int MaxBrightness { get; }
        }
    }