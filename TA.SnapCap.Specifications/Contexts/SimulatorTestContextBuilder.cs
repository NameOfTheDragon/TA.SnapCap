// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: SimulatorTestContextBuilder.cs  Last modified: 2020-03-22@16:40 by Tim Long

using System;
using System.Linq;
using Ninject;
using Ninject.Modules;
using NodaTime;
using TA.Ascom.ReactiveCommunications;
using TA.SnapCap.HardwareSimulator;

namespace TA.SnapCap.Specifications.Contexts
    {
    class SimulatorTestContextBuilder : NinjectModule
        {
        private readonly IKernel testKernel = new StandardKernel();
        string connectionString = "Simulator:Fast";
        Action<SimulatorStateMachine>
            initializeStateMachine = machine => { }; // called to initialize the state machine. DO nothing by default.
        bool openChannel;
        Maybe<SimulatorStateMachine> stateMachine = Maybe<SimulatorStateMachine>.Empty;

        /// <inheritdoc />
        public override void Load()
            {
            Bind<SimulatorStateMachine>().ToMethod(ctx => GetStateMachine());
            Bind<ISimulatorStateTriggers>().ToMethod(ctx => GetStateMachine());
            Bind<IClock>().ToMethod(_ => SystemClock.Instance).InSingletonScope();
            Bind<SimulatorEndpoint>()
                .ToMethod(ctx => SimulatorEndpoint.FromConnectionString(connectionString))
                .InSingletonScope();
            Bind<DeviceEndpoint>().To<SimulatorEndpoint>().InSingletonScope();
            Bind<InputParser>().ToSelf().InSingletonScope();
            Bind<SimulatorCommunicationsChannel>().ToSelf().InSingletonScope();
            Bind<ICommunicationChannel>().To<SimulatorCommunicationsChannel>().InSingletonScope();
            Bind<SimulatorContext>().ToSelf().InSingletonScope();
            }

        public SimulatorStateMachine GetStateMachine()
            {
            if (stateMachine.Any()) return stateMachine.Single();
            var machine = new SimulatorStateMachine(realTime: false, SystemClock.Instance);
            stateMachine = new Maybe<SimulatorStateMachine>(machine);
            return stateMachine.Single();
            }

        public SimulatorContext Build()
            {
            testKernel.Load(this);
            var context = testKernel.Get<SimulatorContext>();
            context.SimulatorChannel.IsOpen = openChannel;
            initializeStateMachine(context.Simulator);
            context.Simulator.StateChanged += args => context.StateChanges.Add(args.StateName);
            return context;
            }

        public SimulatorTestContextBuilder WithFastSimulator()
            {
            connectionString = "Simulator:Fast";
            return this;
            }

        public SimulatorTestContextBuilder WithRealtimeSimulator()
            {
            connectionString = "Simulator:Realtime";
            return this;
            }

        public SimulatorTestContextBuilder WithOpenChannel()
            {
            openChannel = true;
            return this;
            }

        public SimulatorTestContextBuilder InClosedState()
            {
            initializeStateMachine = InitializeStateMachineInClosedState;
            return this;
            }

        void InitializeStateMachineInClosedState(SimulatorStateMachine machine)
            {
            var simulatorState = new StateClosed(machine);
            var inputParser = testKernel.Get<InputParser>();
            machine.Initialize(simulatorState, inputParser);
            }
        }
    }