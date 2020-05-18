using System;
using System.Text;
using NLog.Fluent;
using TA.Ascom.ReactiveCommunications.Diagnostics;
using TA.SnapCap.SharedTypes;

namespace TA.SnapCap.HardwareSimulator
    {
    public class InputParser
        {
        private readonly ISimulatorStateTriggers stateMachine;

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

        private StringBuilder inputBuffer = new StringBuilder();

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
            switch (command)
                {
                case 'O':
                    stateMachine.OpenRequested();
                    break;
                case 'S':   // query status
                    stateMachine.QueryStatusRequested();
                    break;
                default:
                    Log.Warn().Message("Unsupported command {command}", command).Write();
                    break;
                }
            }
        }
    }
