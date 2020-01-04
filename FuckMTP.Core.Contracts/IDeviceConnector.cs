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
        IDirectory GetFileMetadata();
        void CopyFiles(IList<IFile> files, Path target, BehaviorRegardingDuplicates behaviorRegardingDuplicates);
        void MoveFiles(IList<IFile> files, Path target, BehaviorRegardingDuplicates behaviorRegardingDuplicates);
    }
}
