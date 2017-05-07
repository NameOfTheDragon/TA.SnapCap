// This file is part of the TA.SnapCap project
// 
// Copyright © 2007-2017 Tigra Astronomy, all rights reserved.
// 
// File: SnapCapFlatPanel.cs  Created: 2017-05-07@21:19
// Last modified: 2017-05-07@22:20 by Tim Long

using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace TA.SnapCap.DeviceInterface
    {
    public class SnapCapFlatPanel : ISnapCapSwitch
        {
        private readonly DeviceController controller;
        private double lastKnownBrightness = 50.0;

        public SnapCapFlatPanel([NotNull] string name, [NotNull] string description, [NotNull] DeviceController controller)
            {
            Contract.Requires(name != null);
            Contract.Requires(controller != null);
            Contract.Requires(description != null);
            this.controller = controller;
            Description = description;
            Name = name;
            }

        [NotNull]
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
            var trueRange = brightness - 24;
            var fractionOfUnity = trueRange / (255.0 - 24.0);
            return fractionOfUnity * 100.0;
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
                lastKnownBrightness = percentBrightness;
                return;
                }

            controller.ElectroluminescentPanelOn();
            SetPercentBrightness(percentBrightness);
            }

        public void SetValue(bool turnOn)
            {
            controller.ElectroluminescentPanelOn();
            SetPercentBrightness(lastKnownBrightness);
            }

        private void SetPercentBrightness(double percentBrightness)
            {
            var range = 255.0 - 24.0;
            var fractionOfUnity = percentBrightness / 100.0;
            var brightness = (int) Math.Ceiling(range * fractionOfUnity);
            controller.SetBrightness((byte) (brightness + 24));
            lastKnownBrightness = percentBrightness;
            }
        }
    }