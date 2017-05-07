// This file is part of the TA.SnapCap project
// 
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: SnapCapDisposition.cs  Last modified: 2017-05-07@15:21 by Tim Long

namespace TA.SnapCap.DeviceInterface
{
    public enum SnapCapDisposition
    {
        Open = 1,
        Closed = 2,
        Timeout = 3,
        OpenCircuit = 4,
        Overcurrent = 5,
        UserAbort = 6
    }
}