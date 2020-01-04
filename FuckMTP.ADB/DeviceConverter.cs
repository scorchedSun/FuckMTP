using System.Collections.Generic;
using System.Linq;
using System.Net;
using FuckMTP.Core.Contracts;
using Managed.Adb;

namespace FuckMTP.ADB
{

    internal static class DeviceConverter
    {
        public static Core.Contracts.IDevice Convert(Managed.Adb.IDevice device)
        {
            return new DeviceDTO
            {
                SerialNumber = device.SerialNumber
            };
        }

        public static IEnumerable<Core.Contracts.IDevice> Convert(IEnumerable<Managed.Adb.IDevice> devices)
        {
            foreach (Managed.Adb.IDevice device in devices)
                yield return Convert(device);
        }

        private class DeviceDTO : Core.Contracts.IDevice
        {
            public string SerialNumber { get; set; }

            public string Name { get; set; }
        }
    }
}
