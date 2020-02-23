using Machine.Specifications;
using TA.Ascom.ReactiveCommunications;
using TA.DigitalDomeworks.HardwareSimulator;

namespace TA.SnapCap.Specifications.SimulatorSpecs
    {
    // ToDo: there's a builder pattern struggling to get out here
    [Subject(typeof(SimulatorCommunicationsChannel), "create from connection string")]
    internal class when_creating_a_realtime_simulator
        {
        Establish context = () =>
            {
            factory = new ChannelFactory();
            factory.RegisterChannelType(
                SimulatorEndpoint.IsConnectionStringValid,
                SimulatorEndpoint.FromConnectionString,
                endpoint => new SimulatorCommunicationsChannel((SimulatorEndpoint) endpoint)
                );
            };
        Because of = () => channel = factory.FromConnectionString("Simulator:Realtime");
        It should_be_realtime = () => endpoint.Realtime.ShouldBeTrue();

        static SimulatorCommunicationsChannel simulator => channel as SimulatorCommunicationsChannel;
        static SimulatorEndpoint endpoint => simulator.Endpoint as SimulatorEndpoint;
        static ICommunicationChannel channel;
        static ChannelFactory factory;
        }
    [Subject(typeof(SimulatorEndpoint), "create from connection string")]
    internal class when_creating_a_test_simulator
        {
        Because of = () => endpoint=SimulatorEndpoint.FromConnectionString("Simulator:Fast");
        It should_be_realtime = () => endpoint.Realtime.ShouldBeFalse();
        static SimulatorEndpoint endpoint;
        }

    }