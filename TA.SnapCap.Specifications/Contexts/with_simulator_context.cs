using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using TA.Ascom.ReactiveCommunications;

namespace TA.SnapCap.Specifications.Contexts
    {
    class with_simulator_context
        {
        protected static SimulatorContext Context { get; set; }

        Establish context = () =>
            {
            Builder = new SimulatorContextBuilder();
            };
        protected static ChannelFactory factory;
        protected static SimulatorContextBuilder Builder;
        }
    }
