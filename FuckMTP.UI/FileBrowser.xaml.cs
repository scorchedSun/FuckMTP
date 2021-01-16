using FileSystem;
using FuckMTP.Core.Contracts;
using FuckMTP.DeviceConnector.Contracts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FuckMTP.UI
{
    /// <summary>
    /// Interaction logic for FileBrowser.xaml
    /// </summary>
    public partial class FileBrowser : Window
    {
        private readonly FileBrowserViewModel viewModel;

        public FileBrowser(IDevice device)
        {
            InitializeComponent();
            DataContext = viewModel = new FileBrowserViewModel(device);
            viewModel.DisplayContentsOf(device.Root.Value);
        }

        public IReadOnlyList<IFile> GetFiles()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            IEnumerable<IFile> files = viewModel.GetFiles().Select(FileAdapter.Adapt);
            Mouse.OverrideCursor = null;

            return files.ToList().AsReadOnly();
        }
        private void btnAbort_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void lbFiles_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
            => viewModel.NavigateIntoSelectedDirectory();

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
            => viewModel.NavigateIntoParentDirectory();

        private void btnNext_Click(object sender, RoutedEventArgs e)
            => viewModel.NavigateIntoSelectedDirectory();

        private class FileAdapter : IFile
        {
            private readonly File file;

            private FileAdapter(File file) => this.file = file;

            public static FileAdapter Adapt(File file) => new FileAdapter(file);

            public string Name => file.Name;

            public string Path => file.GetPath();
        }
    }
}
