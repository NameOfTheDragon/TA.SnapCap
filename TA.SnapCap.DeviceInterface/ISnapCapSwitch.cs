// This file is part of the TA.SnapCap project
// 
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: SwitchBase.cs  Last modified: 2017-05-07@15:26 by Tim Long

namespace TA.SnapCap.DeviceInterface
{
    public interface ISnapCapSwitch
    {
        string Name { get; set; }
        double GetValue();
        void SetValue(double newValue);
    }
}