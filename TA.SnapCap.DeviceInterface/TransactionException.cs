// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System;
using Timtek.ReactiveCommunications;

namespace TA.SnapCap.DeviceInterface
    {
    [Serializable]
    public sealed class TransactionException : Exception
        {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public TransactionException() { }

        public TransactionException(string message) : base(message) { }

        public TransactionException(string message, Exception inner) : base(message, inner) { }

        public DeviceTransaction Transaction { get; set; }
        }
    }