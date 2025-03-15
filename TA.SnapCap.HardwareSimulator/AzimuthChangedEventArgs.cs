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
    /// <summary>
    ///     Defines the event arguments passed to the <see cref="SimulatorStateMachine.AzimuthChanged" />
    ///     event handler.
    /// </summary>
    public class AzimuthChangedEventArgs : EventArgs
        {
        /// <summary>Gets or sets the new azimuth.</summary>
        /// <value>The new azimuth.</value>
        public int NewAzimuth { get; internal set; }
        }
    }