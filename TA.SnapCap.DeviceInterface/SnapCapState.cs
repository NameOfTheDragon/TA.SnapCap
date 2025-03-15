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
    public struct SnapCapState
        {
        public bool MotorRunning { get; private set; }

        public bool Illuminated { get; private set; }

        public SnapCapDisposition Disposition { get; private set; }

        public static SnapCapState FromResponsePayload(string payload)
            {
            var payloadLength = payload.Length;
            if (payloadLength != 3)
                throw new ArgumentException($"Ivalid payload; expected 3 characters but got {payloadLength}");
            var motorRunning = payload[0] == '1';
            var lampOn = payload[1] == '1';
            var dispositionCode = int.Parse(payload[2].ToString());
            var disposition = (SnapCapDisposition)dispositionCode;
            return new SnapCapState { Disposition = disposition, Illuminated = lampOn, MotorRunning = motorRunning };
            }
        }
    }