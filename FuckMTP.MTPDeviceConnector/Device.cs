using FileSystem;
using FuckMTP.DeviceConnector.Contracts;
using MediaDevices;
using System;

namespace FuckMTP.MTPDeviceConnector
{
    internal sealed class Device : IDevice
    {
        private readonly MediaDevice mediaDevice;

        public string Name => mediaDevice.FriendlyName;

        public Lazy<Directory> Root { get; }

        private Device(MediaDevice mediaDevice)
        {
            this.mediaDevice = mediaDevice ?? throw new ArgumentNullException(nameof(mediaDevice));
            Root = new Lazy<Directory>(LoadFiles);
        }

        public static IDevice Create(MediaDevice mediaDevice) => new Device(mediaDevice);

        private Directory LoadFiles() => FileSystemManagement.LoadFrom(mediaDevice);
    }
}
