using FileSystem;
using FluentAdb.Interfaces;
using FuckMTP.DeviceConnector.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuckMTP.ADB
{
    public sealed class DeviceSource : IDeviceSource
    {
        private readonly IAdb adb;

        public DeviceSource(AdbHandler adbHandler)
        {
            adb = adbHandler.AdbInstance;
        }

        public async Task<IEnumerable<IDevice>> GetAvailableDevices()
            => (await adb.GetDevices().ConfigureAwait(false)).Select(deviceInfo => Device.Create(adb, deviceInfo, new FileSystemEntryFactory(new PathHandler())));
    }
}
