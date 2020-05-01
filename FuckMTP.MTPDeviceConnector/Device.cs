using FileSystem;
using FuckMTP.DeviceConnector.Contracts;
using MediaDevices;
using System;
using System.Collections.Generic;

namespace FuckMTP.MTPDeviceConnector
{
    internal sealed class Device : IDevice
    {
        private readonly MediaDevice mediaDevice;
        private bool disposed;

        public string Name => mediaDevice.FriendlyName;

        public Lazy<Directory> Root { get; }

        private Device(MediaDevice mediaDevice)
        {
            this.mediaDevice = mediaDevice ?? throw new ArgumentNullException(nameof(mediaDevice));
            Root = new Lazy<Directory>(LoadRoot);
        }

        ~Device() => Dispose();

        public static IDevice Create(MediaDevice mediaDevice) => new Device(mediaDevice);

        public void FillSubdirectories(Directory directory)
        {
            EnsureIsConnected();
            FileSystemManagement.FillSubdirectories(mediaDevice, directory);
        }

        public void FillFiles(Directory directory)
        {
            EnsureIsConnected();
            FileSystemManagement.FillFiles(mediaDevice, directory);
        }

        private Directory LoadRoot()
        {
            EnsureIsConnected();
            return new Directory(mediaDevice.GetRootDirectory().FullName);
        }

        private void EnsureIsConnected()
        {
            if (!mediaDevice.IsConnected)
                mediaDevice.Connect();
        }

        public void Dispose()
        {
            if (disposed || !mediaDevice.IsConnected) return;

            mediaDevice.Disconnect();
            disposed = false;
            GC.SuppressFinalize(this);
        }
    }
}
