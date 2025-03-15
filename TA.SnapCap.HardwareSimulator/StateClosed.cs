// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System.Threading.Tasks;

namespace TA.SnapCap.HardwareSimulator
    {
    internal class StateClosed : SimulatorState
        {
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