// This file is part of the AWR Drive System ASCOM Driver project
// 
// Copyright © 2007-2017 Tigra Astronomy, all rights reserved.
// 
// File: FakeEndpoint.cs  Created: 2017-03-11@15:23
// Last modified: 2017-03-11@15:23 by Tim Long

using TA.Ascom.ReactiveCommunications;

namespace TA.SnapCap.Specifications.Fakes
    {
    class FakeEndpoint : DeviceEndpoint
        {
        public override string ToString()
            {
            return "fake device";
            }
        }
    }