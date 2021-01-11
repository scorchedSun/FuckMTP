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
    internal sealed class Interactor : IInteractor
    {
        internal IReadOnlyList<IFile> SelectFilesFrom(IDevice device)
        {
            FileBrowser fileBrowser = new FileBrowser(device);
            fileBrowser.ShowDialog();

            return fileBrowser.GetFiles();
        }

        internal IDevice SelectDeviceFrom(IDeviceSource source)
        {
            IList<IDevice> availableDevices = source.GetAvailableDevices().ToList();

            if (availableDevices.Count == 0)
                throw new NoDeviceConnectedException();
            else if (availableDevices.Count == 1)
                return availableDevices[0];

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

        public string SelectTargetPath()
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

        internal void NotifyNoDeviceConnected()
            => MessageBox.Show("Es wurde kein Android-Gerät gefunden. Bitte stellen Sie sicher, dass ihr Gerät mit dem Computer verbunden ist.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

        internal void NotifyNoDeviceSelected()
            => MessageBox.Show("Es wurde kein Gerät ausgewählt. Bitte wählen Sie ein Gerät aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public void NotifyConfigurationAborted()
            => Debug.WriteLine("The configuration process was aborted. Ending process.");

        public void NotifyNoFilesSelected()
            => MessageBox.Show("Es wurden keine Dateien ausgewählt. Der Vorgang wird abgebrochen.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public void NotifyNoTargetPathSelected()
            => MessageBox.Show("Es wurde kein Zielordner ausgewählt. Der Vorgang wird abgebrochen.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
