using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ASCOM;
using ASCOM.DeviceInterface;
using JetBrains.Annotations;
using TA.SnapCap.Aspects;
using TA.SnapCap.Server.AscomSwitch;

namespace TA.SnapCap.Server.AscomCoverCalibrator
    {
    [ProgId(SharedResources.CoverCalibratorDriverId)]
    [Guid("dd351fb1-ad95-4901-9672-777b93d0fe24")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [UsedImplicitly]
    [ServedClassName(SharedResources.DriverName)]
    [NLogTraceWithArguments]

    class CoverCalibrator : AscomDriverBase, ICoverCalibratorV1
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
