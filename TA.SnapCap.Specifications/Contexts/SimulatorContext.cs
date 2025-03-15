using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using TA.SnapCap.HardwareSimulator;
using Timtek.ReactiveCommunications;

namespace TA.SnapCap.Specifications.Contexts
    {
    class SimulatorContext
        {
        [Inject] public ICommunicationChannel Channel { get; set; }

        public SimulatorCommunicationsChannel SimulatorChannel => Channel as SimulatorCommunicationsChannel;

        public SimulatorStateMachine Simulator => SimulatorChannel.Simulator;

        public SimulatorEndpoint Endpoint => Channel.Endpoint as SimulatorEndpoint;

        public List<string> StateChanges = new List<string>();

        public InputParser Parser => SimulatorChannel.inputParser;

        internal StringBuilder ReceiveBuffer { get; } = new StringBuilder();

        public string Response => ReceiveBuffer.ToString();
        }
    }
