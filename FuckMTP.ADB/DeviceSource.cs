using FileSystem;
using FluentAdb;
using FluentAdb.Enums;
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

        public DeviceSource(IConfiguration configuration)
        {
            adb = Adb.New(configuration.PathToExecutable);
        }

        public async Task<IEnumerable<IDevice>> GetAvailableDevices()
        {
            if (await adb.GetState().ConfigureAwait(false) == AdbState.Offline)
                await adb.StartServer().ConfigureAwait(false);
            return (await adb.GetDevices().ConfigureAwait(false)).Select(deviceInfo => Device.Create(adb, deviceInfo, new FileSystemEntryFactory(new PathHandler())));
        }
    }
}
