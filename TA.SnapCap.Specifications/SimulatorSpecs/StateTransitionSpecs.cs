
using System.Collections.Generic;
using System.ComponentModel;
using Machine.Specifications;
using TA.SnapCap.HardwareSimulator;
using TA.SnapCap.Specifications.Contexts;

namespace TA.SnapCap.Specifications
    {
    [Subject(typeof(SimulatorStateMachine), "state transitions")]
    internal class when_first_connected : with_simulator_context
        {
        Establish context = () => Context=ContextBuilder.Build();
        Because of = () => Context.Channel.Open();
        It should_pass_through_closing_then_closed = () => Context.StateChanges.ShouldEqual(ExpectedStateChanges);
        static List<string> ExpectedStateChanges = new List<string> {nameof(StateClosing), nameof(StateClosed)};
        }

    [Subject(typeof(SimulatorStateMachine), "state transitions")]
    internal class when_closed_and_open_is_requested : with_simulator_context
        {
        Establish context = () => Context = ContextBuilder.WithOpenChannel().InClosedState()
            .WithPropertyChangeNotifications(args => PropertyChanges.Add(args))
            .Build();
        Because of = () => Context.Simulator.OpenRequested();
        static List<PropertyChangedEventArgs> PropertyChanges = new List<PropertyChangedEventArgs>();
        }
    }