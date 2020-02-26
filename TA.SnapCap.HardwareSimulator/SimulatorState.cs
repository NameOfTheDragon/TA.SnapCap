﻿// This file is part of the TI.DigitalDomeWorks project
//
// Copyright © 2016 TiGra Astronomy, all rights reserved.
//
// File: SimulatorState.cs  Created: 2016-06-20@18:14
// Last modified: 2016-06-21@10:01 by Tim

using System;
using System.Diagnostics.Contracts;
using NLog;

namespace TA.DigitalDomeworks.HardwareSimulator
    {
    /// <summary>
    ///     This is the base class for all simulator state-machine states. It defines the behaviours common to all states,
    ///     as well as static data (shared state) and methods that perform the logic required to make the state machine work.
    /// </summary>
    public class SimulatorState
        {
        /// <summary>
        ///     Provides logging services
        /// </summary>
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// A reference to the state machine that created the state.
        /// </summary>
        protected readonly SimulatorStateMachine Machine;

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
        }
    }