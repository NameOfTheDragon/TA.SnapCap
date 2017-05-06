// This file is part of the TA.SnapCap project
// 
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: ArduinoSwitchTransaction.cs  Last modified: 2017-05-06@19:40 by Tim Long

using System;
using PostSharp.Patterns.Contracts;
using TA.Ascom.ReactiveCommunications.Transactions;

namespace TA.SnapCap.DeviceInterface
    {
    internal abstract class ArduinoSwitchTransaction : TerminatedStringTransaction
        {
        protected ArduinoSwitchTransaction([Required] string command) : base(command)
            {
            Timeout = TimeSpan.FromSeconds(2);
            }
        }
    }