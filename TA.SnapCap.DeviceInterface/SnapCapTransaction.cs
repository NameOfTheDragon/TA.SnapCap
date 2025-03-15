// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using JetBrains.Annotations;
using Timtek.ReactiveCommunications;
using Timtek.ReactiveCommunications.Diagnostics;

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