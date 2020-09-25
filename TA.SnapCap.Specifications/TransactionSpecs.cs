// This file is part of the TA.SnapCap project
//
// Copyright © 2017-2017 Tigra Astronomy, all rights reserved.
//
// File: TransactionSpecs.cs  Last modified: 2017-05-07@04:58 by Tim Long

using System;
using Machine.Specifications;
using TA.SnapCap.DeviceInterface;
using TA.SnapCap.SharedTypes;
using TA.SnapCap.Specifications.Contexts;

namespace TA.SnapCap.Specifications
    {
    [Subject(typeof(SnapCapTransaction))]
    class when_creating_a_transaction
        {
        static SnapCapTransaction Transaction;
        Because of = () => Transaction = new UnitTestTransaction("Z123");
        It should_add_encapsulation_at_the_end = () => Transaction.Command.ShouldEndWith("3\r\n");
        It should_add_encapsulation_at_the_start = () => Transaction.Command.ShouldStartWith(">Z");
        }

    [Subject(typeof(SnapCapTransaction), "response parsing")]
    class when_receiving_a_valid_response : with_device_context
        {
        static UnitTestTransaction Transaction;
        Establish context = () => Controller = ContextBuilder
            .WithResponses("S123")
            .Build();
        Because of = () =>
            {
            Transaction = new UnitTestTransaction("S000");
            Processor.CommitTransaction(Transaction);
            Transaction.WaitForCompletionOrTimeout();
            };
        It should_extract_the_response_payload = () => Transaction.ResponsePayload.ShouldEqual("123");
        }

        [Subject(typeof(TransactionFactory), "creation")]
        public class when_creating_a_new_transaction
        {
            Because of = () => Transaction = TransactionFactory.Create(Protocol.GetStatus);
            It should_build_a_valid_command = () => Transaction.Command.ShouldEqual(">S000\r\n");
            It should_set_the_timeout = () => Transaction.Timeout.ShouldEqual(TimeSpan.FromSeconds(2));
            static SnapCapTransaction Transaction;
        }
     }