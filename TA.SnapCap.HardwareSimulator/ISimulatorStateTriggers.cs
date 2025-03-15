﻿// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

namespace TA.SnapCap.HardwareSimulator
    {
    /// <summary>Defined the input triggers that can be presented to any state.</summary>
    public interface ISimulatorStateTriggers
        {
        void OpenRequested();

        void CloseRequested();

        void HaltRequested();

        void QueryStatusRequested();

        void LampOnRequested();

        void LampOffRequested();

        void SetLampBrightness(uint brightness);

        void GetLampBrightness();
        }
    }