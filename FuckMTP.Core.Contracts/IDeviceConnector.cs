using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FuckMTP.Core.Contracts
{
    public interface IDeviceConnector
    {
        IList<IDevice> GetConnectedDevices();
        void UseDevice(IDevice device);
        IDirectory ReadMetadataOfAllFiles();
        void CopyFiles(IList<IFile> files, string targetPath, BehaviorRegardingDuplicates behaviorRegardingDuplicates);
        void MoveFiles(IList<IFile> files, string targetPath, BehaviorRegardingDuplicates behaviorRegardingDuplicates);
    }
}
