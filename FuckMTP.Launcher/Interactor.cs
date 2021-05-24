using FuckMTP.Core;
using FuckMTP.DeviceConnector.Contracts;
using FuckMTP.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FuckMTP
{
    internal sealed class Interactor : IInteractor
    {
        internal IReadOnlyList<IFile> SelectFilesFrom(IDevice device)
        {
            FileBrowser fileBrowser = new FileBrowser(device);
            fileBrowser.ShowDialog();

            if (fileBrowser.DialogResult.HasValue && fileBrowser.DialogResult.Value)
            {
                return fileBrowser.GetFiles();
            }
            return Enumerable.Empty<IFile>().ToList().AsReadOnly();
        }

        internal async Task<IDevice> SelectDeviceFrom(IDeviceSource source)
        {
            IList<IDevice> availableDevices = (await source.GetAvailableDevices().ConfigureAwait(false)).ToList();

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
            return new AbortedOperationConfiguration();
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
            return null;
        }

        public void RunWithProgressReport(Action<ProgressReporter> action)
        {
            ProgressWindow progressWindow = new ProgressWindow("Verarbeite Dateien...", action);
            progressWindow.ShowDialog();
        }

        public void ReportSuccess()
        {
            MessageBox.Show("Der Vorgang wurde erfolgreich abgeschlossen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        internal void NotifyNoDeviceConnected()
            => MessageBox.Show("Es wurde kein Android-Gerät gefunden. Bitte stellen Sie sicher, dass ihr Gerät mit dem Computer verbunden ist.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

        internal void NotifyNoDeviceSelected()
            => MessageBox.Show("Es wurde kein Gerät ausgewählt. Bitte wählen Sie ein Gerät aus.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public void NotifyNoFilesSelected()
            => MessageBox.Show("Es wurden keine Dateien ausgewählt. Der Vorgang wird abgebrochen.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public void NotifyNoTargetPathSelected()
            => MessageBox.Show("Es wurde kein Zielordner ausgewählt. Der Vorgang wird abgebrochen.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public void NotifyNoOperationConfigurationProvided()
            => MessageBox.Show("Es wurden keine Einstellungen für den Vorgang gewählt. Der Vorgang wird abgebrochen.", "Feher", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    internal class AbortedOperationConfiguration : IOperationConfiguration
    {
        public Mode Mode => Mode.Abort;
        public BehaviorRegardingDuplicates BehaviorRegardingDuplicates => BehaviorRegardingDuplicates.Ignore;
    }
}
