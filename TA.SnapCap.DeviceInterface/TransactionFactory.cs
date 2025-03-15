// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System;

namespace TA.SnapCap.DeviceInterface
    {
    internal static class TransactionFactory
        {
        public static SnapCapTransaction Create(char action, byte? payload = null)
            {
            var command = $"{action}{payload ?? 0:000}";
            return new SnapCapTransaction(command) { Timeout = TimeSpan.FromSeconds(2) };
            }
        }
    }