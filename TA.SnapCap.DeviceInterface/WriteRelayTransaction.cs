// This file is part of the TA.SnapCap project
// 
// Copyright � 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: WriteRelayTransaction.cs  Last modified: 2017-05-06@19:39 by Tim Long

namespace TA.SnapCap.DeviceInterface
    {
    internal class WriteRelayTransaction : ArduinoSwitchTransaction
        {
        private readonly ushort relay;
        private readonly bool value;

        public WriteRelayTransaction(ushort relay, bool value) : base(CreateWriteCommand(relay, value))
            {
            this.relay = relay;
            this.value = value;
            }

        private static string CreateWriteCommand(ushort relay, bool value)
            {
            var onOrOff = value ? '1' : '0';
            return $":S{relay}{onOrOff}#";
            }

        protected override void OnNext(string response)
            {
            // Expecting :Srv#
            var relayNumber = int.Parse(response.Substring(2, 1));
            var relayValue = response[3] == '1';
            if (response[0] != 'S')
                {
                OnError(
                    new TransactionException(
                        $"Response appears to be of the wrong type. Expected 'S' but got '{response[0]}'"));
                return;
                }
            if (relayNumber != relay)
                {
                OnError(new TransactionException(
                    $"Response contained an unexpected relay number. Expected={relay}, Actual={relayNumber}"));
                return;
                }
            if (relayValue != value)
                {
                OnError(
                    new TransactionException(
                        $"Response contained an unexpected relay value. Expected={value}, Actual={relayValue}"));
                return;
                }
            base.OnNext(response);
            }
        }
    }