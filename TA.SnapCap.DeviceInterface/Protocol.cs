// This file is part of the TA.SnapCap project
// 
// Copyright © 2007-2017 Tigra Astronomy, all rights reserved.
// 
// File: Protocol.cs  Created: 2017-05-07@14:52
// Last modified: 2017-05-07@16:30 by Tim Long

namespace TA.SnapCap.DeviceInterface
{
    internal static class Protocol
    {
        public const char ElpOff = 'D';
        public const char ElpOn = 'L';
        public const char GetBrightness = 'J';
        public const char GetFirmwareVersion = 'V';
        public const char GetStatus = 'S';
        public const char SetBrightness = 'B';
        public const char OpenCover = 'O';
        public const char CloseCover = 'C';
    }
}