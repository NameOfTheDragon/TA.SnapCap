// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System;
using System.Reflection;
using JetBrains.Annotations;
using Machine.Specifications.Annotations;
using TA.Utils.Core;
using Timtek.ReactiveCommunications;

namespace TA.SnapCap.Specifications.TestHelpers
    {
    /// <summary>Extension methods for manipulating non-public members of transaction classes.</summary>
    internal static class TransactionExtensions
        {
        /// <summary>Sets the protected response property.</summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="response">The response string, which can be null or empty.</param>
        /// <seealso cref="SimulateCompletionWithResponse" />
        public static void SetResponse(this DeviceTransaction transaction, [CanBeNull] string response)
            {
            var maybeResponse = Maybe<string>.From(response);
            var transactionType = typeof(DeviceTransaction);
            var responseProperty = transactionType.GetProperty("Response");
            responseProperty.SetValue(transaction, maybeResponse, BindingFlags.Instance | BindingFlags.NonPublic, null,
                null, null);
            }

        /// <summary>
        ///     Marks a transaction as completed with the supplied response string, which will release any
        ///     waiting threads.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="response">The response.</param>
        public static void SimulateCompletionWithResponse(this DeviceTransaction transaction, [NotNull] string response)
            {
            transaction.SetResponse(response);
            var transactionType = transaction.GetType();
            var makeHotMethod = transactionType.GetMethod("MakeHot",
                BindingFlags.Instance | BindingFlags.NonPublic);
            makeHotMethod.Invoke(transaction, BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder,
                new object[] { }, null);
            var onCompletedMethod = transactionType.GetMethod("OnCompleted",
                BindingFlags.Instance | BindingFlags.NonPublic);
            onCompletedMethod.Invoke(transaction, BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder,
                new object[] { }, null);
            }

        /// <summary>
        ///     Marks a transaction as Failed and provides a <see cref="TimeoutException" /> as the source of
        ///     failure. This also completes the transaction and releases any waiting threads.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="message">The message.</param>
        public static void TimedOut(this DeviceTransaction transaction, string message = null)
            {
            var exception = new TimeoutException(message ?? "Timeout");
            var type = transaction.GetType();
            var onErrorMethod = type.GetMethod("OnError", BindingFlags.Instance | BindingFlags.NonPublic);
            onErrorMethod.Invoke(transaction, BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder,
                new object[] { exception }, null);
            }
        }
    }