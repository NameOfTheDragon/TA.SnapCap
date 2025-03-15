// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System;
using System.ComponentModel;
using System.Linq;
using Ninject;
using Ninject.Modules;
using NodaTime;
using TA.SnapCap.HardwareSimulator;
using TA.Utils.Core;
using Timtek.ReactiveCommunications;

namespace TA.SnapCap.Specifications.Contexts
    {
    internal class SimulatorTestContextBuilder : NinjectModule
        {
        private readonly IKernel testKernel = new StandardKernel();
        private string connectionString = "Simulator:Fast";
        private Action<SimulatorContext>
            initializeStateMachine = machine => { }; // called to initialize the state machine. DO nothing by default.
        private bool openChannel;
        private Maybe<SimulatorStateMachine> stateMachine = Maybe<SimulatorStateMachine>.Empty;
        private PropertyChangedEventHandler propertyChangedAction = (sender, args) => { };
        private bool initialLampState;
        private uint lampBrightness;

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
            var parser = Kernel.Get<InputParser>();
            var machine = new SimulatorStateMachine(false, SystemClock.Instance, parser);
            stateMachine = Maybe<SimulatorStateMachine>.From(machine);
            return stateMachine.Single();
            }

        public SimulatorContext Build()
            {
            testKernel.Load(this);
            var context = testKernel.Get<SimulatorContext>();
            context.SimulatorChannel.IsOpen = openChannel;
            initializeStateMachine(context);
            context.Simulator.LampOn = initialLampState;
            context.Simulator.LampBrightness = lampBrightness;
            context.Simulator.StateChanged += args => context.StateChanges.Add(args.StateName);
            context.SimulatorChannel.ObservableReceivedCharacters.Subscribe(ch => context.ReceiveBuffer.Append(ch));
            context.Simulator.PropertyChanged += propertyChangedAction;
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
            initializeStateMachine = ctx => ctx.Simulator.Initialize(new StateClosed(ctx.Simulator), ctx.Parser);
            return this;
            }

        public SimulatorTestContextBuilder InOpenState()
            {
            initializeStateMachine = ctx => ctx.Simulator.Initialize(new StateOpen(ctx.Simulator), ctx.Parser);
            return this;
            }

        public SimulatorTestContextBuilder InClosingState()
            {
            initializeStateMachine = ctx => ctx.Simulator.Initialize(new StateClosing(ctx.Simulator), ctx.Parser);
            return this;
            }

        public SimulatorTestContextBuilder WithPropertyChangeNotifications(PropertyChangedEventHandler handler)
            {
            propertyChangedAction = handler;
            return this;
            }

        public SimulatorTestContextBuilder WithLampOn()
            {
            initialLampState = true;
            return this;
            }

        public SimulatorTestContextBuilder WithLampBrightness(uint brightness)
            {
            lampBrightness = brightness;
            return this;
            }
        }
    }