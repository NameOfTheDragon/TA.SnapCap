// This file is part of the TA.SnapCap project
// 
// Copyright © 2007-2017 Tigra Astronomy, all rights reserved.
// 
// File: SnapCapFlatPanel.cs  Created: 2017-05-07@16:01
// Last modified: 2017-05-07@16:01 by Tim Long

using System;

namespace TA.SnapCap.DeviceInterface
{
    public class SnapCapFlatPanel : ISnapCapSwitch
    {
        private readonly DeviceController controller;

        public SnapCapFlatPanel(string name, DeviceController controller)
        {
            this.controller = controller;
            Name = name;
        }
        /// <summary>
        /// Gets the ELP brightness in the range 0 to 100%
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// SnapCap accepts brightness values in the range 25..255.
        /// We assume that 24 or less is the same as 'off' or 0% illumination.
        /// Therefore a value of 25 should not cause a return value of 0%.
        /// </remarks>
        public double GetValue()
        {
            var disposition = controller.GetState();
            if (!disposition.Illuminated)
                return 0.0;
            var brightness = controller.GetBrightness();
            var trueRange = brightness - 24;
            var fractionOfUnity = trueRange / (255.0 - 24.0);
            return fractionOfUnity * 100.0;
        }

        public string Name { get; set; }

        public void SetValue(double percentBrightness)
        {
            /*
             * percentBrightness is in the range 0.0 - 100.0
             * If it is 0, then that's the same as Off so we turn off the lamp.
             * Otherwise we convert it into the range 25..255 (rounding up)
             * turn the lamp on and set its brightness.
             */

            if (percentBrightness == 0.0)
            {
                controller.ElectroluminescentPanelOff();
                return;
            }

            var range = 255.0 - 24.0;
            var fractionOfUnity = percentBrightness / 100.0;
            var brightness = (int)Math.Ceiling(range * fractionOfUnity);
            controller.ElectroluminescentPanelOn();
            controller.SetBrightness((byte) (brightness + 24));
        }
    }
}