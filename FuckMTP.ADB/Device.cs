using FileSystem;
using FluentAdb.Interfaces;
using FuckMTP.DeviceConnector.Contracts;
using System;
using System.Threading.Tasks;

namespace FuckMTP.ADB
{
    internal sealed class Device : IDevice
    {
        private const string RootPath = "/sdcard/";

        private readonly IDeviceInfo deviceInfo;
        private readonly Lazy<IAdbTargeted> target;

        public string Name => deviceInfo.SerialNumber;

        public string SerialNumber => deviceInfo.SerialNumber;

        public Lazy<Directory> Root { get; }

        public IAdbTargeted Target => target.Value;

        private Device(IAdb adb, IDeviceInfo deviceInfo, FileSystemEntryFactory fileSystemEntryFactory)
        {
            this.deviceInfo = deviceInfo;
            target = new Lazy<IAdbTargeted>(() => adb.Target(SerialNumber));
            Root = new Lazy<Directory>(() => fileSystemEntryFactory.CreateDirectory(RootPath));
        }

        public static Device Create(IAdb adb, IDeviceInfo deviceInfo, FileSystemEntryFactory fileSystemEntryFactory) => new Device(adb, deviceInfo, fileSystemEntryFactory);

        public void Dispose()
        {
        }

        public async Task FillFiles(Directory directory) => await FileSystemManagement.FillFiles(Target, directory).ConfigureAwait(false);

        public async Task FillSubdirectories(Directory directory) => await FileSystemManagement.FillSubdirectories(Target, directory).ConfigureAwait(false);
    }
}
