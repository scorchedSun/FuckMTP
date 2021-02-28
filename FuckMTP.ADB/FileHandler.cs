using FuckMTP.Core.Contracts;
using System;
using IDevice = FuckMTP.DeviceConnector.Contracts.IDevice;
using FluentAdb;
using FluentAdb.Interfaces;
using System.Threading.Tasks;
using System.IO;

namespace FuckMTP.ADB
{
    public sealed class FileHandler : IFileHandler, IDisposable
    {
        private const string Root = "/sdcard";

        private readonly IAdb adb;
        private readonly IAdbTargeted target;

        private bool disposed;

        private FileHandler(IDevice device, IConfiguration configuration)
        {
            adb = Adb.New(configuration.PathToExecutable);
            target = adb.Target(device.SerialNumber);
        }

        public static RequiresConfiguration For(IDevice device) => new RequiresConfiguration(device);

        public async Task CopyAsync(string filePath, string targetPath)
        {
            string sourcePath = filePath.Replace(@"\Phone", Root).Replace(@"\", "/");
            await target.Pull(sourcePath, targetPath).ConfigureAwait(false);
        }

        public async Task MoveAsync(string filePath, string targetPath)
        {
            await CopyAsync(filePath, targetPath).ConfigureAwait(false);

            if (File.Exists(targetPath))
            {
                string sourcePath = filePath.Replace(@"\Phone", Root).Replace(@"\", "/");
                await target.RunCommand($"rm {sourcePath}").ConfigureAwait(false);
            }
        }

        public void Dispose()
        {
            if (disposed) return;

            adb?.StopServer();

            disposed = true;
        }

        public sealed class RequiresConfiguration
        {
            private readonly IDevice device;

            internal RequiresConfiguration(IDevice device) => this.device = device;

            public FileHandler With(IConfiguration configuration) => new FileHandler(device, configuration);
        }
    }
}