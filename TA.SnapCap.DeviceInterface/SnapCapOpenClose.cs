// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using JetBrains.Annotations;

namespace TA.SnapCap.DeviceInterface
    {
    public class SnapCapOpenClose : ISnapCapSwitch
        {
        private readonly DeviceController controller;

        public SnapCapOpenClose(string name, string description, DeviceController controller)
            {
            this.controller = controller;
            Description = description;
            Name = name;
            }

        public string Description { get; }

        public bool GetState()
            {
            var disposition = controller.GetState();
            return disposition.Disposition == SnapCapDisposition.Open;
            }

        public double GetValue()
            {
            return GetState() ? 1.0 : 0.0;
            }

        public double MaximumValue => 1.0;

        public double MinimumValue => 0.0;

        [NotNull]
        public string Name { get; set; }

        public double Precision => 1.0;

        public void SetValue(double newValue)
            {
            if (newValue.IsNonZero())
                controller.OpenCap();
            else
                controller.CloseCap();
            }

        public void SetValue(bool turnOn)
            {
            if (turnOn)
                controller.OpenCap();
            else
                controller.CloseCap();
            }
        }
    }