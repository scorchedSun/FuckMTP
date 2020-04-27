using System.Collections.Generic;

namespace FuckMTP.DeviceConnector.Contracts
{
    public interface IDeviceSource
    {
        IEnumerable<IDevice> GetAvailableDevices();
    }
}
