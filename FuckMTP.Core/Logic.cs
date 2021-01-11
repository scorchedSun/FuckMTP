using FuckMTP.Core.Contracts;
using System;
using System.Collections.Generic;

namespace FuckMTP.Core
{
    public class Logic
    {
        private readonly IInteractor interactor;
        private readonly IFileSource fileSource;

        public Logic(IInteractor interactor, IFileSource fileSource)
        {
            this.interactor = interactor ?? throw new ArgumentNullException(nameof(interactor));
            this.fileSource = fileSource ?? throw new ArgumentNullException(nameof(fileSource));
        }

        public void Run()
        {
            IReadOnlyList<IFile> files = fileSource.SelectFiles();

            if (files.Count == 0)
            {
                interactor.NotifyNoFilesSelected();
                return;
            }

            string target = interactor.SelectTargetPath();

            if (string.IsNullOrWhiteSpace(target))
            {
                interactor.NotifyNoTargetPathSelected();
                return;
            }

            IOperationConfiguration configuration = interactor.GetOperationConfiguration();

            //IDirectory rootDirectory;
            //using (IBusyIndicator busyIndicator = interactor.SetBusy())
            //{
            //    rootDirectory = deviceConnector.ReadMetadataOfAllFiles();
            //}

            //IFileOperation operation = interactor.CreateFileOperation(rootDirectory);

            //using (IBusyIndicator busyIndicator = interactor.SetBusy())
            //{
            //    Execute(operation);
            //}

            //interactor.NotifySuccess(operation);
        }

        private bool DetermineIsLocalCopy()
        {
            return false;
        }

        //private void Execute(IFileOperation operation)
        //{
        //    switch (operation.Configuration.Mode)
        //    {
        //        case Mode.Copy:
        //            deviceConnector.CopyFiles(operation.Files, operation.TargetPath, operation.Configuration.BehaviorRegardingDuplicates);
        //            break;
        //        case Mode.Move:
        //            deviceConnector.MoveFiles(operation.Files, operation.TargetPath, operation.Configuration.BehaviorRegardingDuplicates);
        //            break;
        //    }
        //}

        //private IDevice SelectDevice()
        //{
        //    IList<IDevice> connectedDevices = deviceConnector.GetConnectedDevices();

        //    if (connectedDevices.Count == 1)
        //        return connectedDevices[0];
        //    else if (connectedDevices.Count > 1)
        //    {
        //        IDevice selectedDevice = interactor.SelectOneDevice(connectedDevices);
        //        if (selectedDevice is null)
        //            throw new NoDeviceSelectedException();
        //    }

        //    throw new NoDeviceConnectedException();
        //}
    }
}
