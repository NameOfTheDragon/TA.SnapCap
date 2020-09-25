using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.SnapCap.SharedTypes
    {
    public static class ValueConverterExtensions
        {
        public const int MaxDeviceBrightness = 255;
        public const int MinDeviceBrightness = 25;
        public const int AscomMaxBrightness = MaxDeviceBrightness - MinDeviceBrightness + 1;

        public static int ToDeviceBrightness(this int ascomBrightness)
            {
            return ascomBrightness + MinDeviceBrightness - 1;
            }

        /// <summary>
        /// Accepts a device brightness percentage [0.0..100.0]
        /// and returns the equivalent ASCOM value [0..231]
        /// </summary>
        /// <param name="percentBrightness"></param>
        /// <returns></returns>
        public static int ToAscomBrightness(this int percentBrightness)
            {
            var brightness = (double)percentBrightness;
            var ascomBrightness = brightness.MapToRange(0, 100, 0, AscomMaxBrightness);
            return (int)ascomBrightness;
            }
        }
    }
