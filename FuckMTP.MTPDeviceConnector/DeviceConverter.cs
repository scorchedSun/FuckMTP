using FuckMTP.DeviceConnector.Contracts;
using MediaDevices;

namespace FuckMTP.MTPDeviceConnector
{
    internal static class DeviceConverter
    {
        public static IDevice Convert(MediaDevice device)
            => new Device
            {
                Name = device.FriendlyName
            };
    }
}
