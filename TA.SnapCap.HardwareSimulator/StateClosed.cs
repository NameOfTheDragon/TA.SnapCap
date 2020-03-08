// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: StateClosed.cs  Last modified: 2020-02-27@21:41 by Tim Long

using System.Threading.Tasks;

namespace TA.DigitalDomeworks.HardwareSimulator {
    class StateClosed : SimulatorState {
        /// <inheritdoc />
        internal StateClosed(SimulatorStateMachine machine) : base(machine) { }

        /// <inheritdoc />
        public override Task OnEnter()
            {
            base.OnEnter();
            Machine.SignalStopped();
            return Task.CompletedTask;
            }
        }
    }