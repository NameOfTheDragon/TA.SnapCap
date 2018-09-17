// This file is part of the TA.SnapCap project
// 
// Copyright © 2007-2017 Tigra Astronomy, all rights reserved.
// 
// File: SnapCapFlatPanel.cs  Created: 2017-05-07@16:01
// Last modified: 2017-05-11@03:24 by Tim Long

using System.Diagnostics.Contracts;
using PostSharp.Patterns.Contracts;

namespace TA.SnapCap.DeviceInterface
{
    public class SnapCapFlatPanel : ISnapCapSwitch
    {
        [NotNull] private readonly DeviceController controller;
        private int lastKnownBrightness;

        public SnapCapFlatPanel([NotNull] string name, [NotNull] string description, [NotNull] DeviceController controller)
        {
            Contract.Requires(name != null);
            Contract.Requires(controller != null);
            Contract.Requires(description != null);
            this.controller = controller;
            Description = description;
            Name = name;
            lastKnownBrightness = controller.Brightness;
        }

        public string Description { get; }

        public bool GetState()
        {
            var disposition = controller.GetState();
            return disposition.Illuminated;
        }

        /// <summary>
        ///     Gets the ELP brightness in the range 0 to 100%
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///     SnapCap accepts brightness values in the range 25..255.
        ///     We assume that 24 or less is the same as 'off' or 0% illumination.
        ///     Therefore a value of 25 should not cause a return value of 0%.
        /// </remarks>
        public double GetValue()
        {
            var disposition = controller.GetState();
            if (!disposition.Illuminated)
                return 0.0;
            var brightness = controller.GetBrightness();
            return DeviceController.BrightnessToPercent((byte) brightness);
        }

        public double MaximumValue => 100.0;

        public double MinimumValue => 0.0;

        public string Name { get; set; }

        public double Precision => double.Epsilon;

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

            controller.ElectroluminescentPanelOn();
            SetPercentBrightness(percentBrightness);
            lastKnownBrightness = (int) percentBrightness;
        }

        /// <summary>
        ///     Turns on the electroluminescent panel on or off.
        ///     When turning on, the brightness setting is also set to a nonzero value.
        ///     If known, the last set brightness will be used, otherwise 50% is assumed.
        /// </summary>
        /// <param name="turnOn"></param>
        public void SetValue(bool turnOn)
        {
            var brightness = lastKnownBrightness > 0 ? lastKnownBrightness : 50;
            SetValue(turnOn ? brightness : 0.0);
        }

        private void SetPercentBrightness(double percentBrightness)
        {
            var brightness = DeviceController.BrightnessFromPercent(percentBrightness);
            controller.SetBrightness(brightness);
        }
    }
}