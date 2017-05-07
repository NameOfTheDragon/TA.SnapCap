// This file is part of the TA.SnapCap project
// 
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: SnapCapOpenClose.cs  Last modified: 2017-05-07@15:26 by Tim Long

namespace TA.SnapCap.DeviceInterface
{
    public class SnapCapOpenClose : ISnapCapSwitch
    {
        private readonly DeviceController controller;

        public SnapCapOpenClose(string name, DeviceController controller)
        {
            this.controller = controller;
            Name = name;
        }

        public double GetValue()
        {
            var state = controller.GetState();
            return state.Disposition == SnapCapDisposition.Open ? 1.0 : 0.0;
        }

        public string Name { get; set; }

        public void SetValue(double newValue)
        {
            if (newValue == 0.0)
                controller.OpenCap();
            else
                controller.CloseCap();
        }
    }
}