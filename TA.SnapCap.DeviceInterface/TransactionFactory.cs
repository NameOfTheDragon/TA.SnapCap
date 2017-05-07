using System;

namespace TA.SnapCap.DeviceInterface
{
 static class TransactionFactory
{
    public static SnapCapTransaction Create(char action, byte? payload = null)
    {
        var command = $"{action}{payload ?? 0:000}";
        return new SnapCapTransaction(command) {Timeout = TimeSpan.FromSeconds(2)};
    }
}
}