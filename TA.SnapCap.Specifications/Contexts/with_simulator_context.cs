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
            ContextBuilder = new SimulatorTestContextBuilder();
            };
        protected static ChannelFactory factory;
        protected static SimulatorTestContextBuilder ContextBuilder;

        protected static void OpenChannelAndWaitUntilStopped()
            {
            Context.Channel.Open();
            Context.Simulator.WhenStopped().Wait();
            }
        }
    }
