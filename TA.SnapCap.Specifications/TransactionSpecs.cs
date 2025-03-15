// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System;
using Machine.Specifications;
using TA.SnapCap.DeviceInterface;
using TA.SnapCap.SharedTypes;
using TA.SnapCap.Specifications.Contexts;

namespace TA.SnapCap.Specifications
    {
    [Subject(typeof(SnapCapTransaction))]
    internal class when_creating_a_transaction
        {
        private Because of = () => Transaction = new UnitTestTransaction("Z123");
        private It should_add_encapsulation_at_the_end = () => Transaction.Command.ShouldEndWith("3\r\n");
        private It should_add_encapsulation_at_the_start = () => Transaction.Command.ShouldStartWith(">Z");
        private static SnapCapTransaction Transaction;
        }

    [Subject(typeof(SnapCapTransaction), "response parsing")]
    internal class when_receiving_a_valid_response : with_device_context
        {
        private Establish context = () => Controller = ContextBuilder
            .WithResponses("S123")
            .Build();
        private Because of = () =>
            {
            Transaction = new UnitTestTransaction("S000");
            Processor.CommitTransaction(Transaction);
            Transaction.WaitForCompletionOrTimeout();
            };
        private It should_extract_the_response_payload = () => Transaction.ResponsePayload.ShouldEqual("123");
        private static UnitTestTransaction Transaction;
        }

    [Subject(typeof(TransactionFactory), "creation")]
    public class when_creating_a_new_transaction
        {
        private Because of = () => Transaction = TransactionFactory.Create(Protocol.GetStatus);
        private It should_build_a_valid_command = () => Transaction.Command.ShouldEqual(">S000\r\n");
        private It should_set_the_timeout = () => Transaction.Timeout.ShouldEqual(TimeSpan.FromSeconds(2));
        private static SnapCapTransaction Transaction;
        }
    }