// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System;
using System.Threading;
using System.Threading.Tasks;
using TA.SnapCap.HardwareSimulator;

namespace TA.SnapCap.Server
    {
    /// <summary>Summary description for GarbageCollection.</summary>
    internal static class GarbageCollection
        {
        private static readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

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