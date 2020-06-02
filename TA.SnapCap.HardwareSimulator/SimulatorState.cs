// This file is part of the TI.DigitalDomeWorks project
//
// Copyright © 2016 TiGra Astronomy, all rights reserved.
//
// File: SimulatorState.cs  Created: 2016-06-20@18:14
// Last modified: 2016-06-21@10:01 by Tim

using System;
using System.Threading.Tasks;
using NLog;
using NLog.Fluent;

namespace TA.SnapCap.HardwareSimulator
    {
    /// <summary>
    ///     This is the base class for all simulator state-machine states. It defines the behaviours common to all states,
    ///     as well as static data (shared state) and methods that perform the logic required to make the state machine work.
    /// </summary>
    public class SimulatorState : ISimulatorStateTriggers
        {
        /// <summary>
        ///     Provides logging services
        /// </summary>
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// A reference to the state machine that created the state.
        /// </summary>
        protected readonly SimulatorStateMachine Machine;
        protected static readonly TimeSpan StallTimeout = TimeSpan.FromSeconds(20);

        /// <summary>
        ///     Initializes the simulator state with a reference to the parent state machine.
        /// </summary>
        /// <param name="machine">The associated state machine.</param>
        internal SimulatorState(SimulatorStateMachine machine)
            {
            this.Machine = machine;
            }

        /// <summary>
        ///     Gets the descriptive name of the current state.
        /// </summary>
        /// <value>The state's descriptive name, as a string.</value>
        public virtual string Name
            {
            get
                {
                Log.Warn("State does not override the Name property.");
                return this.GetType().Name;
                }
            }


        #region IDisposable Members
        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
            {
            }
        #endregion

        /// <summary>
        ///     Called (by the state machine) when exiting from the state
        /// </summary>
        public virtual void OnExit() { }

        /// <summary>
        ///     Called (by the state machine) when entering the state.
        /// </summary>
        public virtual void OnEnter() { }


        #region Events
        #region Delegates
        #endregion

        #endregion

        #region State Triggers
        /// <inheritdoc />
        public virtual void OpenRequested()
            {
            Log.Info().Message("Open requested").Write();
            Machine.SendResponse("*O000");
            }

        /// <inheritdoc />
        public virtual void HaltRequested()
            {
            Log.Info().Message("Halt requested").Write();
            Machine.SendResponse("*A000");
            }

        /// <inheritdoc />
        public virtual void QueryStatusRequested()
            {
            var response = Machine.BuildStatusResponse();
            Log.Debug().Message($"Query status; responding: {response}").Write();
            Machine.SendResponse(response);
            }

        public virtual void CloseRequested()
            {
            Log.Info().Message("Close requested").Write();
            Machine.SendResponse("*C000");
            }

        #region Lamp Control
        /*
         * Lamp control operations are always valid, so are handled here in the abstract base state.
         * Other states are free to override this behaviour if necessary.
         */

        /// <inheritdoc />
        public virtual void LampOnRequested()
            {
            Log.Info().Message("Lamp on requested").Write();
            Machine.LampOn = true;
            Machine.SendResponse("*L000");
            }

        /// <inheritdoc />
        public virtual void LampOffRequested()
            {
            Log.Info().Message("Lamp off requested").Write();
            Machine.LampOn = false;
            Machine.SendResponse("*D000");
            }

        public virtual void GetLampBrightness()
            {
            Log.Info().Message("Get lamp brightness").Write();
            Machine.SendResponse($"*J{Machine.LampBrightness:D3}");
            }

        /// <inheritdoc />
        public virtual void SetLampBrightness(uint brightness)
            {
            Log.Info().Message("Set lamp brightness to {brightness}", brightness).Write();
            Machine.LampBrightness = brightness;
            Machine.SendResponse($"*B{brightness:D3}");
            }
        #endregion Lamp Control
        #endregion State Triggers
        }
    }