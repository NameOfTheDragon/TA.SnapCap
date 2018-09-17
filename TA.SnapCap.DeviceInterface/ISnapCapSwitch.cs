// This file is part of the TA.SnapCap project
// 
// Copyright © 2007-2017 Tigra Astronomy, all rights reserved.
// 
// File: ISnapCapSwitch.cs  Created: 2017-05-07@14:38
// Last modified: 2017-05-11@02:34 by Tim Long

namespace TA.SnapCap.DeviceInterface
{
    public interface ISnapCapSwitch
    {
        string Description { get; }

        double MaximumValue { get; }

        double MinimumValue { get; }

        string Name { get; set; }

        double Precision { get; }

        bool GetState();

        double GetValue();

        void SetValue(double newValue);

        void SetValue(bool turnOn);
    }
}