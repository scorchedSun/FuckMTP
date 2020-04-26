using FuckMTP.Core.Contracts;
using System;
using System.Collections.Generic;

namespace FuckMTP.Core
{
    public class Logic
    {
        private readonly IInteractor interactor;

        public Logic(IInteractor interactor)
        {
            this.interactor = interactor ?? throw new ArgumentNullException(nameof(interactor));
        }

        public void Run()
        {
            try
            {
                IList<IFile> files = interactor.GetFiles();

                string target = interactor.GetTargetPath();

                if (string.IsNullOrWhiteSpace(target))
                {
                    interactor.NotifyNoFolderSelected();
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
            catch (FileSelectionAbortedException)
            {
                interactor.NotifyFileSelectionAborted();
            }
            catch (NoFolderSelectedException)
            {
                interactor.NotifyNoFolderSelected();
            }
            catch (ConfigurationAbortedException)
            {
                interactor.NotifyConfigurationAborted();
            }
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
