// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System;
using System.Threading.Tasks;

namespace TA.SnapCap.HardwareSimulator
    {
    internal class StateOpening : SimulatorState
        {
        /// <inheritdoc />
        internal StateOpening(SimulatorStateMachine machine) : base(machine) { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.MotorDirection = MotorDirection.Opening;
            Machine.MotorEnergized = true;
            Task.Run(() => Machine.SimulatedDelay(TimeSpan.FromSeconds(10)))
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