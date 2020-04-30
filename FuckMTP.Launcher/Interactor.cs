using FileSystem;
using FuckMTP.Core.Contracts;
using FuckMTP.DeviceConnector.Contracts;
using FuckMTP.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace FuckMTP
{
    internal sealed class Interactor : IInteractor, IDisposable
    {
        private readonly IDeviceSource deviceSource;
        private IDevice selectedDevice;
        private bool disposed;

        public Interactor(IDeviceSource deviceSource)
            => this.deviceSource = deviceSource ?? throw new ArgumentNullException(nameof(deviceSource));

        ~Interactor() => Dispose();

        public IList<IFile> GetFiles()
        {
            selectedDevice?.Dispose();

            selectedDevice = SelectDevice();

            FileBrowser fileBrowser = new FileBrowser(selectedDevice);
            fileBrowser.ShowDialog();

            return null;
        }

        private IDevice SelectDevice()
        {
            IList<IDevice> availableDevices = deviceSource.GetAvailableDevices().ToList();

            if (availableDevices.Count == 0)
                throw new NoDeviceConnectedException();
            else if (availableDevices.Count == 1)
                return availableDevices.First();

            DeviceSelector deviceSelector = new DeviceSelector(availableDevices);

            if (deviceSelector.ShowDialog().Value && deviceSelector.SelectedDevice != null)
                return deviceSelector.SelectedDevice;
            throw new NoDeviceSelectedException();
        }


        public IOperationConfiguration GetOperationConfiguration()
        {
            ModeSelection window = new ModeSelection();

            if (window.ShowDialog().Value)
                return window.Configuration;
            throw new ConfigurationAbortedException();
        }

        public string GetTargetPath()
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog
            {
                Description = "Zielordner auswählen"
            };
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                return folderBrowser.SelectedPath;
            }
            throw new NoFolderSelectedException();
        }

        public void NotifyConfigurationAborted()
            => Debug.WriteLine("The configuration process was aborted. Ending process.");

        public void NotifyFileSelectionAborted()
            => MessageBox.Show("Es wurden keine Dateien ausgewählt. Der Vorgang wird abgebrochen.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public void NotifyNoFolderSelected()
            => MessageBox.Show("Es wurde kein Zielordner ausgewählt. Der Vorgang wird abgebrochen.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public void Dispose()
        {
            if (disposed) return;

            selectedDevice.Dispose();
            disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
