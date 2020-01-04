using FuckMTP.Core.Contracts;
using System;
using System.Collections.Generic;

namespace FuckMTP.Core
{
    public class Logic
    {
        private readonly IDeviceConnector deviceConnector;
        private readonly IInteractor interactor;

        public Logic(IDeviceConnector deviceConnector, IInteractor interactor)
        {
            this.deviceConnector = deviceConnector ?? throw new ArgumentNullException(nameof(deviceConnector));
            this.interactor = interactor ?? throw new ArgumentNullException(nameof(interactor));
        }

        public void Run()
        {
            try
            {
                IDevice device = SelectDevice();

                deviceConnector.UseDevice(device);

                IDirectory rootDirectory;
                using (IBusyIndicator busyIndicator = interactor.SetBusy())
                {
                    rootDirectory = deviceConnector.GetFileMetadata();
                }

                IFileOperation operation = interactor.CreateFileOperation(rootDirectory);

                using (IBusyIndicator busyIndicator = interactor.SetBusy())
                {
                    Execute(operation);
                }

                interactor.NotifySuccess(operation);
            }
            catch (NoDeviceConnectedException)
            {
                interactor.NotifyNoDeviceConnected();
            }
            catch (ExecutionFailedException ex)
            {
                interactor.NotifyFileOperationFailed(ex.Message);
            }
        }

        private void Execute(IFileOperation operation)
        {
            switch (operation.Mode)
            {
                case Mode.Copy:
                    deviceConnector.CopyFiles(operation.Files, operation.Target, operation.BehaviorRegardingDuplicates);
                    break;
                case Mode.Move:
                    deviceConnector.MoveFiles(operation.Files, operation.Target, operation.BehaviorRegardingDuplicates);
                    break;
            }
        }

        private IDevice SelectDevice()
        {
            IList<IDevice> connectedDevices = deviceConnector.GetConnectedDevices();

            if (connectedDevices.Count == 1)
                return connectedDevices[0];
            else if (connectedDevices.Count > 1)
                return interactor.SelectOneDevice(connectedDevices);

            throw new NoDeviceConnectedException();
        }
    }
}
