using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TA.Ascom.ReactiveCommunications;
using TA.DigitalDomeworks.HardwareSimulator;

namespace TA.SnapCap.Specifications.Contexts
    {
    class SimulatorContext
        {
        public ICommunicationChannel Channel { get; set; }

        public SimulatorCommunicationsChannel Simulator => Channel as SimulatorCommunicationsChannel;

        public SimulatorEndpoint Endpoint => Channel.Endpoint as SimulatorEndpoint;

        }
    }
