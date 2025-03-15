// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System;
using System.Text;
using JetBrains.Annotations;
using NLog.Fluent;
using TA.SnapCap.SharedTypes;
using Timtek.ReactiveCommunications.Diagnostics;

namespace TA.SnapCap.HardwareSimulator
    {
    public class InputParser
        {
        private readonly StringBuilder inputBuffer = new StringBuilder();
        private ISimulatorStateTriggers actions;

        public void SubscribeTo(IObservable<char> input, ISimulatorStateTriggers actions)
            {
            this.actions = actions;
            input.Trace("SimulatorIn")
                .Subscribe(OnNext, OnError, OnCompleted);
            }

        private void OnCompleted()
            {
            Log.Info().Message("Completed").Write();
            }

        private void OnError(Exception ex)
            {
            Log.Error().Exception(ex).Message("Sequence error").Write();
            }

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
                        actions.OpenRequested();
                        break;
                    case Protocol.CloseCover:
                        actions.CloseRequested();
                        break;
                    case Protocol.GetStatus: // query status
                        actions.QueryStatusRequested();
                        break;
                    case Protocol.ElpOn:
                        actions.LampOnRequested();
                        break;
                    case Protocol.ElpOff:
                        actions.LampOffRequested();
                        break;
                    case Protocol.SetBrightness:
                        actions.SetLampBrightness(ExtractPayload());
                        break;
                    case Protocol.GetBrightness:
                        actions.GetLampBrightness();
                        break;
                    case Protocol.Halt:
                        actions.HaltRequested();
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