using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuckMTP.DeviceConnector.Contracts
{
    public interface IDeviceSource
    {
        Task<IEnumerable<IDevice>> GetAvailableDevices();
    }
}
