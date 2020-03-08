// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: SimulatorContextBuilder.cs  Last modified: 2020-02-23@17:56 by Tim Long

using System;
using TA.Ascom.ReactiveCommunications;
using TA.DigitalDomeworks.HardwareSimulator;

namespace TA.SnapCap.Specifications.Contexts {
    class SimulatorContextBuilder
        {
        string connectionString = "invalid";
        bool openChannel = false;   // Whether the channel should be  opened by the builder.
        Action<SimulatorStateMachine> initializeStateMachine = machine => { }; // called to initialize the state machine. DO nothing by default.

        public SimulatorContext Build()
            {
            var factory = new ChannelFactory();
            factory.ClearRegisteredDevices();
            factory.RegisterChannelType(
                SimulatorEndpoint.IsConnectionStringValid,
                SimulatorEndpoint.FromConnectionString,
                endpoint => new SimulatorCommunicationsChannel((SimulatorEndpoint)endpoint)
            );

            var context = new SimulatorContext();
            context.Channel = factory.FromConnectionString(connectionString);
            context.SimulatorChannel.IsOpen = openChannel;
            // ReSharper disable once EventExceptionNotDocumented - Exception means failed test, so don't catch it.
            initializeStateMachine(context.Simulator);
            context.Simulator.StateChanged += (args) => context.StateChanges.Add(args.StateName);
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

        public SimulatorContextBuilder WithOpenChannel()
            {
            openChannel = true;
            return this;
            }

        public SimulatorContextBuilder InStoppedState()
            {
            initializeStateMachine = machine => machine.Initialize(new StateClosed(machine));
            return this;
            }
        }
    }