// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TA.DigitalDomeworks.HardwareSimulator
    {
    public static class AsyncExtensions
        {
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
            {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
                if (task != await Task.WhenAny(task, tcs.Task))
                    throw new OperationCanceledException(cancellationToken);

            return task.Result;
            }

        public static ConfiguredTaskAwaitable<TResult> ContinueOnAnyThread<TResult>(this Task<TResult> task)
            {
            return task.ConfigureAwait(continueOnCapturedContext: false);
            }

        public static ConfiguredTaskAwaitable ContinueOnAnyThread(this Task task)
            {
            return task.ConfigureAwait(continueOnCapturedContext: false);
            }

        public static ConfiguredTaskAwaitable ContinueOnCurrentThread(this Task task)
            {
            return task.ConfigureAwait(continueOnCapturedContext: true);
            }

        public static Task AsTask(this WaitHandle handle)
            {
            return AsTask(handle, Timeout.InfiniteTimeSpan);
            }

        public static Task AsTask(this WaitHandle handle, TimeSpan timeout)
            {
            var tcs = new TaskCompletionSource<object>();
            var registration = ThreadPool.RegisterWaitForSingleObject(handle, (state, timedOut) =>
                {
                var localTcs = (TaskCompletionSource<object>)state;
                if (timedOut)
                    localTcs.TrySetCanceled();
                else
                    localTcs.TrySetResult(null);
                }, tcs, timeout, executeOnlyOnce: true);
            tcs.Task.ContinueWith((_, state) => ((RegisteredWaitHandle)state).Unregister(null), registration, TaskScheduler.Default);
            return tcs.Task;
            }
        }
    }