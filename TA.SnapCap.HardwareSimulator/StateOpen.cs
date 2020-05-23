using System.Threading.Tasks;

namespace TA.SnapCap.HardwareSimulator
    {
    class StateOpen : SimulatorState
        {
        public StateOpen(SimulatorStateMachine machine) : base(machine) { }

        /// <inheritdoc />
        public override Task OnEnter()
            {
            base.OnEnter();
            Machine.SetSystemState(SystemStatus.Open);
            Machine.MotorEnergized = false;
            Machine.SignalStopped();
            return Task.CompletedTask;
            }

        /// <inheritdoc />
        public override void CloseRequested()
            {
            base.CloseRequested();
            Machine.MotorDirection = MotorDirection.Closing;
            Machine.MotorEnergized = true;
            Machine.Transition(new StateClosing(Machine));
            }
        }
    }