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
    [ProgId(SharedResources.SwitchDriverId)]
    [Guid("dd351fb1-ad95-4901-9672-777b93d0fe24")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [UsedImplicitly]
    [ServedClassName(SharedResources.SwitchDriverName)]
    [NLogTraceWithArguments]

    class CoverCalibrator : AscomDriverBase, ICoverCalibratorV1
        {
        /// <inheritdoc />
        public void SetupDialog() { }

        /// <inheritdoc />
        public string Action(string ActionName, string ActionParameters)
            {
            return null;
            }

        /// <inheritdoc />
        public void CommandBlind(string Command, bool Raw = false) { }

        /// <inheritdoc />
        public bool CommandBool(string Command, bool Raw = false)
            {
            return false;
            }

        /// <inheritdoc />
        public string CommandString(string Command, bool Raw = false)
            {
            return null;
            }

        /// <inheritdoc />
        void ICoverCalibratorV1.Dispose() { }

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
        public bool Connected { get; set; }

        /// <inheritdoc />
        public string Description { get; }

        /// <inheritdoc />
        public string DriverInfo { get; }

        /// <inheritdoc />
        public string DriverVersion { get; }

        /// <inheritdoc />
        public short InterfaceVersion { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public ArrayList SupportedActions { get; }

        /// <inheritdoc />
        public CoverStatus CoverState { get; }

        /// <inheritdoc />
        public CalibratorStatus CalibratorState { get; }

        /// <inheritdoc />
        public int Brightness { get; }

        /// <inheritdoc />
        public int MaxBrightness { get; }

        /// <inheritdoc />
        void IDisposable.Dispose() { }

        /// <inheritdoc />
        bool IAscomDriver.Connected { get; }
        }
    }
