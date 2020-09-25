using System.Threading.Tasks;

namespace TA.SnapCap.HardwareSimulator
    {
    class StateOpen : SimulatorState
        {
        public StateOpen(SimulatorStateMachine machine) : base(machine) { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.SetSystemState(SystemStatus.Open);
            Machine.MotorEnergized = false;
            Machine.SignalStopped();
            }

        /// <inheritdoc />
        public override void CloseRequested()
            {
            base.CloseRequested();
            Machine.Transition(new StateClosing(Machine));
            }
        }
    }