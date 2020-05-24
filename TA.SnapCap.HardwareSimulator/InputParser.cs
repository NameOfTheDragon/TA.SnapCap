// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: InputParser.cs  Last modified: 2020-05-24@15:29 by Tim Long

using System;
using System.Text;
using NLog.Fluent;
using PostSharp.Patterns.Model;
using TA.Ascom.ReactiveCommunications.Diagnostics;
using TA.SnapCap.SharedTypes;

namespace TA.SnapCap.HardwareSimulator
    {
    public class InputParser
        {
        private readonly ISimulatorStateTriggers stateMachine;

        private readonly StringBuilder inputBuffer = new StringBuilder();

        public InputParser(ISimulatorStateTriggers stateMachine)
            {
            this.stateMachine = stateMachine;
            }

        public void SubscribeTo(IObservable<char> input)
            {
            input.Trace("SimulatorIn")
                .Subscribe(OnNext, OnError, OnCompleted);
            }

        private void OnCompleted() => Log.Info().Message("Completed").Write();

        private void OnError(Exception ex) => Log.Error().Exception(ex).Message("Sequence error").Write();

        private void OnNext(char input)
            {
            switch (input)
                {
                    case Protocol.CommandInitiator:
                        inputBuffer.Clear();
                        break;
                    case Protocol.CommandTerminator:
                        TryExecuteCommand();
                        inputBuffer.Clear();
                        break;
                    default:
                        inputBuffer.Append(input);
                        break;
                }
            }

        private void TryExecuteCommand()
            {
            var command = inputBuffer[0];
            var payload = ExtractPayload();
            switch (command)
                {
                    case Protocol.OpenCover:
                        stateMachine.OpenRequested();
                        break;
                    case Protocol.CloseCover:
                        stateMachine.CloseRequested();
                        break;
                    case Protocol.GetStatus: // query status
                        stateMachine.QueryStatusRequested();
                        break;
                    case Protocol.ElpOn:
                        stateMachine.LampOnRequested();
                        break;
                    case Protocol.ElpOff:
                        stateMachine.LampOffRequested();
                        break;
                    case Protocol.SetBrightness:
                        stateMachine.SetLampBrightness(ExtractPayload());
                        break;
                    default:
                        Log.Warn().Message("Unsupported command {command}", command).Write();
                        break;
                }
            }

        /// <summary>Extracts the command payload and returns the unsigned integer value, or zero.</summary>
        /// <returns>System.UInt32.</returns>
        [Pure]
        private uint ExtractPayload()
            {
            if (inputBuffer.Length < 4)
                return 0;
            if (char.IsDigit(inputBuffer[1]) && char.IsDigit(inputBuffer[2]) && char.IsDigit(inputBuffer[3]))
                {
                var payload = inputBuffer.ToString(1, 3);
                try
                    {
                    var value = uint.Parse(payload);
                    return value;
                    }
                catch (FormatException e)
                    {
                    Log.Warn().Exception(e).Message("Unable to parse payload as uint: {payload} (returning 0)", payload)
                        .Write();
                    return 0;
                    }
                }
            return 0;
            }
        }
    }