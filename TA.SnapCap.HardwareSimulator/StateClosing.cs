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
    internal class StateClosing : SimulatorState
        {
        /// <inheritdoc />
        internal StateClosing(SimulatorStateMachine machine) : base(machine) { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.MotorDirection = MotorDirection.Closing;
            Machine.MotorEnergized = true;
            Task
                .Run(() => Machine.SimulatedDelay(
                    TimeSpan.FromSeconds(2.5))) // Todo: move magic number to configuration
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