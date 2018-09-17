// This file is part of the TA.SnapCap project
// 
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: SnapCapTransaction.cs  Last modified: 2017-05-07@04:57 by Tim Long

using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using JetBrains.Annotations;
using TA.Ascom.ReactiveCommunications;
using TA.Ascom.ReactiveCommunications.Diagnostics;

namespace TA.SnapCap.DeviceInterface
    {
    internal class SnapCapTransaction : DeviceTransaction
        {
        private const char CommandInitiator = '>';
        private static readonly StringBuilder builder = new StringBuilder();
        protected char SnapCapCommand;

        internal SnapCapTransaction([NotNull] string command) : base(EnsureEncapsulation(command))
            {
            Contract.Requires(command != null);
            Timeout = TimeSpan.FromSeconds(2);
            SnapCapCommand = Command[1];
            }

        [CanBeNull]
        public string ResponsePayload { get; private set; }

        private static string DeEncapsulate(string encapsulated)
            {
            return encapsulated.TrimStart('*').TrimEnd('\r');
            }

        private static string EnsureEncapsulation(string rawCommand)
            {
            builder.Clear();
            if (rawCommand[0] != CommandInitiator) builder.Append(CommandInitiator);
            builder.Append(rawCommand);
            if (!rawCommand.EndsWith("\r\n"))
                builder.Append("\r\n");
            return builder.ToString();
            }

        public override void ObserveResponse(IObservable<char> source)
            {
            var query = from response in source.DelimitedMessageStrings('*', '\r')
                        where response[1] == SnapCapCommand
                        let deEncapsulatedResponse = DeEncapsulate(response)
                        select deEncapsulatedResponse;
            query.Trace("SnapCapTransaction")
                .Take(1)
                .Subscribe(OnNext, OnError, OnCompleted);
            }

        protected override void OnCompleted()
            {
            ResponsePayload = new string(Response.Single().Skip(1).ToArray());
            base.OnCompleted();
            }
        }
    }