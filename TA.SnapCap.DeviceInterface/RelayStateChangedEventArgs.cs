// This file is part of the TA.SnapCap project
// 
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: RelayStateChangedEventArgs.cs  Last modified: 2017-05-06@19:38 by Tim Long

using System;

namespace TA.SnapCap.DeviceInterface
    {
    public class RelayStateChangedEventArgs : EventArgs
        {
        public RelayStateChangedEventArgs(int relayNumber, bool newState)
            {
            RelayNumber = relayNumber;
            NewState = newState;
            }

        public bool NewState { get; }

        public int RelayNumber { get; }
        }
    }