// This file is part of the TA.SnapCap project
// 
// Copyright © 2007-2017 Tigra Astronomy, all rights reserved.
// 
// File: MathExtensions.cs  Created: 2017-05-07@16:50
// Last modified: 2017-05-07@16:53 by Tim Long

using System;

namespace TA.SnapCap.DeviceInterface
{
    public static class MathExtensions
    {
        public static bool IsCloseToZero(this double value)
        {
            return Math.Abs(value) < double.Epsilon;
        }

        public static bool IsNonZero(this double value)
        {
            return Math.Abs(value) > double.Epsilon;
        }
    }
}