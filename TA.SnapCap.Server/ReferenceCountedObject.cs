// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System.Runtime.InteropServices;

namespace TA.SnapCap.Server
    {
    [ComVisible(false)]
    public class ReferenceCountedObjectBase
        {
        public ReferenceCountedObjectBase()
            {
            // We increment the global count of objects.
            Server.CountObject();
            }

        ~ReferenceCountedObjectBase()
            {
            // We decrement the global count of objects.
            Server.UncountObject();
            // We then immediately test to see if we the conditions
            // are right to attempt to terminate this server application.
            Server.ExitIf();
            }
        }
    }