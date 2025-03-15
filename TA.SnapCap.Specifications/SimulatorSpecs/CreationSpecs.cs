// This file is part of the TA.SnapCap project.
// 
// This source code is dedicated to the memory of Andras Dan, late owner of Gemini Telescope Design.
// Licensed under the Tigra/Timtek MIT License. In summary, you may do anything at all with this source code,
// but whatever you do is your own responsibility and not mine, and nothing you do affects my ownership of my intellectual property.
// 
// Tim Long, Timtek Systems, 2025.

using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using TA.SnapCap.DeviceInterface;
using TA.SnapCap.HardwareSimulator;
using TA.SnapCap.SharedTypes;
using TA.SnapCap.Specifications.Contexts;

namespace TA.SnapCap.Specifications.SimulatorSpecs
    {
    [Subject(typeof(SimulatorCommunicationsChannel), "create from connection string")]
    internal class when_creating_a_realtime_simulator : with_simulator_context
        {
        private Establish context = () => Context = ContextBuilder
            .WithRealtimeSimulator()
            .Build();
        private It should_be_real_time = () => Context.Endpoint.Realtime.ShouldBeTrue();
        // We don' test for other states because it would take too long.
        }

    [Subject(typeof(SimulatorCommunicationsChannel), "create from connection string")]
    internal class when_creating_a_fast_simulator : with_simulator_context
        {
        private Establish context = () => Context = ContextBuilder
            .WithFastSimulator()
            .Build();
        private Because of = () => OpenChannelAndWaitUntilStopped();
        private It should_be_fast = () => Context.Endpoint.Realtime.ShouldBeFalse();
        private It should_have_passed_through_closing_then_closed_states =
            () => Context.StateChanges.ShouldBeLike(expectedStates);
        private static readonly IEnumerable<string> expectedStates = new List<string>
                { nameof(StateClosing), nameof(StateClosed) };
        }

    [Subject(typeof(SimulatorStateMachine), "Opening")]
    internal class when_closed_and_an_open_request_is_received : with_simulator_context
        {
        private Establish context = () => Context = ContextBuilder
            .WithFastSimulator()
            .WithOpenChannel()
            .InClosedState()
            .Build();
        private Because of = () => Context.Channel.Send(Protocol.GetCommandString(Protocol.OpenCover));
        private It should_transition_through_opening_state =
            () => Context.StateChanges.First().ShouldEqual(nameof(StateOpening));
        }

    /*
     * Command protocol looks like this...
     * 000 in commands or responses represents 3 decimal digits. Always 3 digits left-padded with zero.
     * All commands begin with '>' and end with "\n\r" (keyboard ^M ^J). Yes, its backwards :-|
     *
     * Cmd  Reply   Notes
     * O    *O000   Open; ignored if the device thinks it is already open.
     * o    *o000   Force Open; tries to open regardless of current state
     * C    *C000   Close; ignored if the device thinks it is already closed
     * c    *c000   Force Close; tries to close regardless of current state
     * L    *L000   Lamp on
     * D    *D000   Lamp off
     * B000 *B000   Set brightness (PWM). Payload is not validated but will probably go wrong if it is > 255
     * J    *J000   Get brightness. Always 3 digits padded with zeroes as necessary.
     * I    none    Causes a stream of diagnostic information to be emitted continuously until >E
     * E    none    Stops the diagnostic output.
     * S    *S000   Report status as follows:
     *              First digit: Motor on (1) or off (0)
     *              Second digit: Lamp on (1) or off (0)
     *              Third digit: system state.
     *                  OPEN 1
     *                  CLOSED 2
     *                  TIMEOUT 3
     *                  OPEN_CIRCUIT 4
     *                  OVERCURRENT 5
     *                  USERABORT 6
     * A    *A000   Abort (stop movement). Sets status to 6 even if there was no movement in progress.
     * V    *V102   Report version string.
     */
    }