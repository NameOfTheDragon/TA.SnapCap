// This file is part of the TA.SnapCap project
// 
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: SnapCapState.cs  Last modified: 2017-05-07@15:21 by Tim Long

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
            var disposition = (SnapCapDisposition) dispositionCode;
            return new SnapCapState {Disposition = disposition, Illuminated = lampOn, MotorRunning = motorRunning};
        }
    }
}