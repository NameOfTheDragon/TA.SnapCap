// This file is part of the TA.SnapCap project
// 
// Copyright © 2007-2017 Tigra Astronomy, all rights reserved.
// 
// File: SnapCapOpenClose.cs  Created: 2017-05-07@21:19
// Last modified: 2017-05-07@22:20 by Tim Long

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