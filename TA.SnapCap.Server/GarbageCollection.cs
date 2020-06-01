// This file is part of the TA.SnapCap project
// 
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
// 
// File: GarbageCollection.cs  Last modified: 2020-06-01@02:05 by Tim Long

using System;
using System.Threading;
using System.Threading.Tasks;
using TA.SnapCap.HardwareSimulator;

namespace TA.SnapCap.Server
    {
    /// <summary>Summary description for GarbageCollection.</summary>
    internal static class GarbageCollection
        {
        static readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        internal static async Task CollectPeriodically(TimeSpan interval)
            {
            var cancellationToken = cancellationTokenSource.Token;
            while (!cancellationToken.IsCancellationRequested)
                {
                await Task.Delay(interval, cancellationToken).ContinueOnAnyThread();
                GC.Collect();
                }
            }

        public static void Stop()
            {
            cancellationTokenSource.Cancel();
            }
        }
    }