// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: SimulatorContextBuilder.cs  Last modified: 2020-02-23@17:56 by Tim Long

using TA.Ascom.ReactiveCommunications;
using TA.DigitalDomeworks.HardwareSimulator;

namespace TA.SnapCap.Specifications.Contexts {
    class SimulatorContextBuilder
        {
        string connectionString = "invalid";

        public SimulatorContext Build()
            {
            var factory = new ChannelFactory();
            factory.RegisterChannelType(
                SimulatorEndpoint.IsConnectionStringValid,
                SimulatorEndpoint.FromConnectionString,
                endpoint => new SimulatorCommunicationsChannel((SimulatorEndpoint)endpoint)
            );

            var context = new SimulatorContext();
            context.Channel = factory.FromConnectionString(connectionString);

            return context;
            }

        public SimulatorContextBuilder WithFastSimulator()
            {
            connectionString = "Simulator:Fast";
            return this;
            }
        public SimulatorContextBuilder WithRealtimeSimulator()
            {
            connectionString = "Simulator:Realtime";
            return this;
            }
        }
    }