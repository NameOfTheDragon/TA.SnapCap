// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using System;
using System.Collections.Generic;
using Ninject;
using Ninject.Activation;
using Ninject.Infrastructure.Disposal;
using Ninject.Modules;
using Ninject.Syntax;
using NLog.Fluent;
using NodaTime;
using TA.Ascom.ReactiveCommunications;
using TA.SnapCap.DeviceInterface;
using TA.SnapCap.HardwareSimulator;

namespace TA.SnapCap.Server
    {
    internal static class CompositionRoot
        {
        static CompositionRoot()
            {
            Kernel = new StandardKernel();
            }

        public static void AddBindings(IEnumerable<INinjectModule> bindingModules)
            {
            foreach (var module in bindingModules)
                {
                Kernel.Load(bindingModules);
                }
            }

        private static ScopeObject CurrentScope { get; set; }

        public static IKernel Kernel { get; }

        public static void BeginSessionScope()
            {
            var scope = new ScopeObject();
            Log.Info().Message("Beginning session scope {scope}", scope).Write();
            CurrentScope = scope;
            }

        public static IBindingNamedWithOrOnSyntax<T> InSessionScope<T>(this IBindingInSyntax<T> binding)
            {
            return binding.InScope(ctx => CurrentScope);
            }

        public static void EndSessionScope()
            {
            Log.Info().Message("Ending session scope {scope}", CurrentScope).Write();
            CurrentScope?.Dispose();
            CurrentScope = null;
            }
        }

    internal class ScopeObject : INotifyWhenDisposed
        {
        private static int scopeId;

        public ScopeObject()
            {
            ++scopeId;
            }

        public int ScopeId => scopeId;

        /// <inheritdoc />
        public virtual void Dispose()
            {
            try
                {
                Disposed?.Invoke(this, EventArgs.Empty);
                }
            finally
                {
                IsDisposed = true;
                }
            }

        /// <inheritdoc />
        public bool IsDisposed { get; private set; }

        /// <inheritdoc />
        public event EventHandler Disposed;

        /// <inheritdoc />
        public override string ToString() => $"{nameof(ScopeId)}: {ScopeId}";
        }

    internal class CoreModule : NinjectModule
        {
        public override void Load()
            {
            Bind<DeviceController>().ToSelf().InSessionScope();
            Bind<ICommunicationChannel>().ToMethod(BuildCommunicationsChannel).InSessionScope();
            Bind<ChannelFactory>().ToSelf().InSessionScope();
            Bind<IClock>().ToMethod((context)=>SystemClock.Instance).InSingletonScope();
            Bind<ReactiveTransactionProcessor>().ToSelf().InSessionScope();
            Bind<TransactionObserver>().ToSelf().InSessionScope();
            Bind<ITransactionProcessor>().ToMethod(BuildTransactionProcessor).InSessionScope();
            }

        private ITransactionProcessor BuildTransactionProcessor(IContext arg)
            {
            var observer = Kernel.Get<TransactionObserver>();
            var processor = Kernel.Get<ReactiveTransactionProcessor>();
            processor.SubscribeTransactionObserver(observer);
            return processor;
            }

        private ICommunicationChannel BuildCommunicationsChannel(IContext context)
            {
            var parser = Kernel.Get<InputParser>();
            var machine = Kernel.Get<SimulatorStateMachine>();
            var channelFactory = Kernel.Get<ChannelFactory>();
            channelFactory.RegisterChannelType(
                SimulatorEndpoint.IsConnectionStringValid,
                SimulatorEndpoint.FromConnectionString,
                endpoint => new SimulatorCommunicationsChannel((SimulatorEndpoint)endpoint, machine, parser)
            );

            var channel = channelFactory.FromConnectionString("dummy");
            return channel;
            }

        private DeviceControllerOptions BuildDeviceOptions(IContext arg)
            {
            var options = new DeviceControllerOptions(); // ToDo - populate from AppSettings
            return options;
            }
        }
    }