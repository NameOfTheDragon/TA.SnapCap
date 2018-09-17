// This file is part of the TA.SnapCap project
// 
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
// 
// File: ReferenceCountedObject.cs  Last modified: 2017-05-06@19:59 by Tim Long

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