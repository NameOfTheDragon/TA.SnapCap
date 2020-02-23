using Machine.Specifications;
using TA.Ascom.ReactiveCommunications;
using TA.DigitalDomeworks.HardwareSimulator;
using TA.SnapCap.Specifications.Contexts;

namespace TA.SnapCap.Specifications.SimulatorSpecs
    {
    // ToDo: there's a builder pattern struggling to get out here
    [Subject(typeof(SimulatorCommunicationsChannel), "create from connection string")]
    internal class when_creating_a_realtime_simulator : with_simulator_context
        {
        Establish context = () => Context = Builder
            .WithRealtimeSimulator()
            .Build();
        It should_be_realtime = () => Context.Endpoint.Realtime.ShouldBeTrue();
        }

    [Subject(typeof(SimulatorEndpoint), "create from connection string")]
    internal class when_creating_a_test_simulator
        {
        Because of = () => endpoint=SimulatorEndpoint.FromConnectionString("Simulator:Fast");
        It should_be_realtime = () => endpoint.Realtime.ShouldBeFalse();
        static SimulatorEndpoint endpoint;
        }

    }