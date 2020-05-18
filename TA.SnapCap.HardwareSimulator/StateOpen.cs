using System.Threading.Tasks;

namespace TA.SnapCap.HardwareSimulator {
    class StateOpen : SimulatorState
        {
        public StateOpen(SimulatorStateMachine machine) :base(machine){ }

        /// <inheritdoc />
        public override Task OnEnter()
            {
            base.OnEnter();
            Machine.SetSystemState(SystemStatus.Open);
            Machine.MotorEnergized = false;
            Machine.SignalStopped();
            return Task.CompletedTask;
            }
        }
    }