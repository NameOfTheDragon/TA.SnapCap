// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System;

namespace TA.SnapCap.HardwareSimulator
    {
    /// <summary>Event arguments used by state machine events.</summary>
    public class StateEventArgs : EventArgs
        {
        /// <summary>Initializes a new instance of the <see cref="StateEventArgs" /> class.</summary>
        /// <param name="stateName">Name of the new state.</param>
        public StateEventArgs(string stateName)
            {
            StateName = stateName;
            }

        /// <summary>Gets or sets the name of the new state.</summary>
        /// <value>The name of the state.</value>
        public string StateName { get; }
        }
    }