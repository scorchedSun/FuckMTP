using System.Collections.Generic;
using System.Linq;
using System.Net;
using FuckMTP.Core.Contracts;
using Managed.Adb;

namespace FuckMTP.ADB
{
    public class DeviceConnector : IDeviceConnector
    {
        private readonly AdbHelper adbHelper = AdbHelper.Instance;
        private IList<Device> connectedDevices;
        private Managed.Adb.IDevice selectedDevice;

        public IList<Core.Contracts.IDevice> GetConnectedDevices()
        {
            connectedDevices = adbHelper.GetDevices(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 0));
            return DeviceConverter.Convert(connectedDevices.Where(device => device.IsOnline)).ToList();
        }

        public void UseDevice(Core.Contracts.IDevice device)
            => selectedDevice = connectedDevices.Single(d => d.SerialNumber.Equals(device.SerialNumber));

        public IDirectory ReadMetadataOfAllFiles()
        {
            FileEntry root = selectedDevice.FileListingService.Root;
            return FileRootConverter.Convert(root);
        }

        public void CopyFiles(IList<IFile> files, string targetPath, BehaviorRegardingDuplicates behaviorRegardingDuplicates)
        {
            throw new System.NotImplementedException();
        }        

        public void MoveFiles(IList<IFile> files, string targetPath, BehaviorRegardingDuplicates behaviorRegardingDuplicates)
        {
            throw new System.NotImplementedException();
        }
    }
}
