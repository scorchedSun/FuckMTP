using FileSystem;
using FluentAdb.Enums;
using FluentAdb.Interfaces;
using FuckMTP.DeviceConnector.Contracts;
using System;
using System.Threading.Tasks;

namespace FuckMTP.ADB
{
    internal sealed class Device : IDevice
    {
        private const string RootPath = "/sdcard/";
        private static readonly object syncRoot = new object();

        private readonly IAdb adb;
        private readonly IDeviceInfo deviceInfo;
        private readonly FileSystemEntryFactory fileSystemEntryFactory;
        private readonly Lazy<IAdbTargeted> target;

        private bool disposed;

        public string Name => deviceInfo.SerialNumber;

        public string SerialNumber => deviceInfo.SerialNumber;

        public Lazy<Directory> Root { get; }

        public IAdbTargeted Target => target.Value;

        private Device(IAdb adb, IDeviceInfo deviceInfo, FileSystemEntryFactory fileSystemEntryFactory)
        {
            this.adb = adb;
            this.deviceInfo = deviceInfo;
            this.fileSystemEntryFactory = fileSystemEntryFactory;
            target = new Lazy<IAdbTargeted>(() => adb.Target(SerialNumber));
            Root = new Lazy<Directory>(() => fileSystemEntryFactory.CreateDirectory(RootPath));
        }

        public static Device Create(IAdb adb, IDeviceInfo deviceInfo, FileSystemEntryFactory fileSystemEntryFactory) => new Device(adb, deviceInfo, fileSystemEntryFactory);

        public void Dispose()
        {
            if (disposed) return;

            lock (syncRoot)
            {
                if (adb.GetState().GetAwaiter().GetResult() != AdbState.Offline)
                    adb.StopServer();
            }

            disposed = true;
        }

        public async Task FillFiles(Directory directory) => await FileSystemManagement.FillFiles(Target, directory).ConfigureAwait(false);

        public async Task FillSubdirectories(Directory directory) => await FileSystemManagement.FillSubdirectories(Target, directory).ConfigureAwait(false);
    }
}
