// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

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