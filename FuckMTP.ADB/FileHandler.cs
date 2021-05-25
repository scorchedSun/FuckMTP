using FuckMTP.Core;
using System;
using System.Threading.Tasks;
using System.IO;
using FuckMTP.DeviceConnector.Contracts;

namespace FuckMTP.ADB
{
    public sealed class FileHandler : IFileHandler
    {
        private readonly Device device;

        private FileHandler(IDevice device)
        {
            if (!(device is Device adbDevice))
                throw new ArgumentException("The given device needs to be an ADB device.");

            this.device = adbDevice;
        }

        public static FileHandler For(IDevice device) => new FileHandler(device);

        public async Task CopyAsync(string filePath, string targetPath)
        {
            await device.Target.Pull(filePath, targetPath).ConfigureAwait(false);
        }

        public async Task MoveAsync(string filePath, string targetPath)
        {
            await CopyAsync(filePath, targetPath).ConfigureAwait(false);

            if (File.Exists(targetPath))
            {
                await device.Target.RunCommand($"rm {filePath}").ConfigureAwait(false);
            }
        }
    }
}