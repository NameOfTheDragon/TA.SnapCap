// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: StateClosing.cs  Last modified: 2020-02-23@16:48 by Tim Long

using System;
using System.Threading.Tasks;

namespace TA.SnapCap.HardwareSimulator {
    class StateClosing : SimulatorState {
        /// <inheritdoc />
        internal StateClosing(SimulatorStateMachine machine) : base(machine) { }

        /// <inheritdoc />
        public override async Task OnEnter()
            {
            base.OnEnter();
            await Machine.SimulatedDelay(TimeSpan.FromSeconds(10)) // Todo: move magic number to configuration
                .ContinueOnAnyThread();
            // ToDo: trigger closed endstop
            Machine.Transition(new StateClosed(Machine));
            }
        }
    }