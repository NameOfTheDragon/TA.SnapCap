// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: StateOpening.cs  Last modified: 2020-05-18@18:02 by Tim Long

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TA.SnapCap.HardwareSimulator
    {
    class StateOpening : SimulatorState
        {
        private CancellationTokenSource cancellationSource;

        /// <inheritdoc />
        internal StateOpening(SimulatorStateMachine machine) : base(machine) { }

        /// <inheritdoc />
        public override async Task OnEnter()
            {
            base.OnEnter();
            Machine.MotorDirection = MotorDirection.Opening;
            Machine.MotorEnergized = true;
            cancellationSource = new CancellationTokenSource(StallTimeout);
            var cancellationToken = cancellationSource.Token;
            await SimulateOpening(cancellationToken).ContinueOnAnyThread();
            if (!cancellationToken.IsCancellationRequested)
                Machine.Transition(new StateOpen(Machine));
            else
                {
                Machine.SetSystemState(SystemStatus.Timeout);
                }
            }

        /// <inheritdoc />
        public override void OnExit()
            {
            base.OnExit();
            Machine.MotorEnergized = false;
            }

        private Task SimulateOpening(CancellationToken cancel)
            {
            cancel.ThrowIfCancellationRequested();
            var delayTask = Machine.RealTime
                ? Task.Delay(TimeSpan.FromSeconds(10), cancel)
                : Task.CompletedTask;
            return delayTask;
            }
        }
    }