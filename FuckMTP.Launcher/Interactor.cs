using FuckMTP.Core.Contracts;
using FuckMTP.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace FuckMTP
{
    internal sealed class Interactor : IInteractor
    {
        public IList<IFile> GetFiles()
        {
            FileDrop fileDrop = new FileDrop();

            if (fileDrop.ShowDialog().Value && fileDrop.Files.Any())
                return fileDrop.Files.Select(path => new File(path)).Cast<IFile>().ToList();
            throw new FileSelectionAbortedException();
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
    }
}
