// This file is part of the TA.SnapCap project
// 
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
// 
// File: ISimulatorStateTriggers.cs  Last modified: 2020-05-24@13:22 by Tim Long

namespace TA.SnapCap.HardwareSimulator
    {
    /// <summary>
    /// Defined the input triggers that can be presented to any state.
    /// </summary>
    public interface ISimulatorStateTriggers
        {
        void OpenRequested();

        void CloseRequested();

        void QueryStatusRequested();

        void LampOnRequested();

        void LampOffRequested();

        void SetLampBrightness(uint brightness);
        }
    }