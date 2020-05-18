// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: SystemStatus.cs  Last modified: 2020-05-13@21:07 by Tim Long
namespace TA.SnapCap.HardwareSimulator
    {
    /*
     * Overall system status. Values are:
     * OPEN 1
     * CLOSED 2
     * TIMEOUT 3
     * OPEN_CIRCUIT 4
     * OVERCURRENT 5
     * USERABORT 6

     */
    public enum SystemStatus
        {
        Undefined = 0,
        Open = 1,
        Closed = 2,
        Timeout = 3,
        OpenCircuit = 4,
        OverCurrent = 5,
        UserAbort = 6
        }
    }