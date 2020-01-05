using System.Collections.Generic;
using System.Linq;
using FuckMTP.Core.Contracts;
using Managed.Adb;

namespace FuckMTP.ADB
{
    public class DeviceConnector : IDeviceConnector
    {
        private const string DataRootDirectory = "/sdcard/";

        private readonly AndroidDebugBridge androidDebugBridge;
        private readonly AdbHelper adbHelper = AdbHelper.Instance;
        private IList<Device> connectedDevices;
        private Managed.Adb.IDevice selectedDevice;

        public DeviceConnector(string pathToExecutable)
        {
            androidDebugBridge = AndroidDebugBridge.CreateBridge(pathToExecutable, true);
            androidDebugBridge.Start();
        }

        public IList<Core.Contracts.IDevice> GetConnectedDevices()
        {
            connectedDevices = adbHelper.GetDevices(AndroidDebugBridge.SocketAddress);
            return DeviceConverter.Convert(connectedDevices.Where(device => device.IsOnline)).ToList();
        }

        public void UseDevice(Core.Contracts.IDevice device)
        {
            selectedDevice = connectedDevices.Single(d => d.SerialNumber.Equals(device.SerialNumber));
            typeof(BusyBox).GetProperty(nameof(BusyBox.Available)).SetValue(selectedDevice.BusyBox, false);  // ensure native commands
        }

        public IDirectory ReadMetadataOfAllFiles()
        {
            List<FileEntry> entries = new List<FileEntry>();
            List<string> links = new List<string>();
            ListingServiceReceiver receiver = new ListingServiceReceiver(selectedDevice.FileListingService.Root, entries, links);
            selectedDevice.ExecuteShellCommand("ls -la " + DataRootDirectory, receiver);

            return FileEntryConverter.Convert(receiver.Entries);
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
