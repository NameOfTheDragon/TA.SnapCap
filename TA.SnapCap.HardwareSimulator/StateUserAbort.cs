// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: StateUserAbort.cs  Last modified: 2020-06-02@23:45 by Tim Long

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