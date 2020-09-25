// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: StateOpening.cs  Last modified: 2020-05-18@18:02 by Tim Long

using System;
using System.Threading.Tasks;

namespace TA.SnapCap.HardwareSimulator
    {
    class StateOpening : SimulatorState
        {
        /// <inheritdoc />
        internal StateOpening(SimulatorStateMachine machine) : base(machine) { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.MotorDirection = MotorDirection.Opening;
            Machine.MotorEnergized = true;
            Task.Run(()=>Machine.SimulatedDelay(TimeSpan.FromSeconds(10)))
                    .ContinueWith(task => Machine.Transition(new StateOpen(Machine)),
                        TaskContinuationOptions.NotOnCanceled)
                    .ContinueOnAnyThread();
            }

        public override void HaltRequested()
            {
            base.HaltRequested();
            Machine.CancelSimulatedDelay();
            Machine.Transition(new StateUserAbort(Machine));
            }


        /// <inheritdoc />
        public override void OnExit()
            {
            base.OnExit();
            Machine.MotorEnergized = false;
            }
        }
    }