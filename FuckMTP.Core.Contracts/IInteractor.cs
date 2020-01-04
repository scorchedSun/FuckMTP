using System;
using System.Collections.Generic;
using System.Text;

namespace FuckMTP.Core.Contracts
{
    public interface IInteractor
    {
        IDevice SelectOneDevice(object devices);
        void NotifyNoDeviceConnected();
        IFileOperation CreateFileOperation(IDirectory rootDirectory);
        IBusyIndicator SetBusy();
        void NotifyFileOperationFailed(string message);
        void NotifySuccess(IFileOperation operation);
    }
}
