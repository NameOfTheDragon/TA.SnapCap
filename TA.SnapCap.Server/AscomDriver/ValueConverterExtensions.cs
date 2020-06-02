using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.SnapCap.Server.AscomDriver
    {
    internal static class ValueConverterExtensions
        {
        public const int MaxDeviceBrightness = 255;
        public const int MinDeviceBrightness = 25;
        public const int AscomMaxBrightness = MaxDeviceBrightness - MinDeviceBrightness + 1;

        public static int ToDeviceBrightness(this int ascomBrightness)
            {
            return ascomBrightness + MinDeviceBrightness - 1;
            }

        public static int ToAscomBrightness(this int deviceBrightness)
            {
            return deviceBrightness - MinDeviceBrightness + 1;
            }
        }
    }
