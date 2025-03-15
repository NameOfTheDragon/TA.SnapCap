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
    internal class StateUserAbort : SimulatorState
        {
        public StateUserAbort(SimulatorStateMachine machine) : base(machine) { }

        public override void OnEnter()
            {
            base.OnEnter();
            Machine.SetSystemState(SystemStatus.UserAbort);
            Machine.MotorEnergized = false;
            Machine.SignalStopped();
            }

        /// <inheritdoc />
        public override void CloseRequested()
            {
            base.CloseRequested();
            Machine.MotorDirection = MotorDirection.Closing;
            Machine.MotorEnergized = true;
            Machine.Transition(new StateClosing(Machine));
            }

        public override void OpenRequested()
            {
            base.OpenRequested();
            Machine.MotorDirection = MotorDirection.Opening;
            Machine.MotorEnergized = true;
            Machine.Transition(new StateOpening(Machine));
            }
        }
    }