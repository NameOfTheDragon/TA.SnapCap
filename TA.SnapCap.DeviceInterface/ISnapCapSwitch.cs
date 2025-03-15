// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

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