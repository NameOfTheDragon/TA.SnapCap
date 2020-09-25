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
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.MotorDirection = MotorDirection.Closing;
            Machine.MotorEnergized = true;
            Task.Run(() => Machine.SimulatedDelay(TimeSpan.FromSeconds(2.5))) // Todo: move magic number to configuration
                .ContinueWith(task => Machine.Transition(new StateClosed(Machine)),
                    TaskContinuationOptions.NotOnCanceled)
                .ContinueOnAnyThread();
            }

        /// <inheritdoc />
        public override void HaltRequested()
            {
            base.HaltRequested();
            Machine.CancelSimulatedDelay();
            Machine.Transition(new StateUserAbort(Machine));
            }
        }
    }