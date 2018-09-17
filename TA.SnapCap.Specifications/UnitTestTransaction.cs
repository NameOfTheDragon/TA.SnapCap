using TA.SnapCap.DeviceInterface;

namespace TA.SnapCap.Specifications {
    internal class UnitTestTransaction : SnapCapTransaction
        {
        public UnitTestTransaction(string rawCommand) : base(rawCommand) { }
        }
    }