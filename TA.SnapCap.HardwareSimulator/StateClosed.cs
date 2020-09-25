// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: StateClosed.cs  Last modified: 2020-02-27@21:41 by Tim Long

using System.Threading.Tasks;

namespace TA.SnapCap.HardwareSimulator {
    class StateClosed : SimulatorState {
        /// <inheritdoc />
        internal StateClosed(SimulatorStateMachine machine) : base(machine) { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.MotorEnergized = false;
            Machine.SetSystemState(SystemStatus.Closed);
            Machine.SignalStopped();
            }

        /// <inheritdoc />
        public override void OpenRequested()
            {
            base.OpenRequested();
            Machine.Transition(new StateOpening(Machine));
            }
        }
    }