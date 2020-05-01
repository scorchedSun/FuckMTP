using FuckMTP.DeviceConnector.Contracts;
using System.Windows;

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

        private void btnAbort_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = false;
        }

        private void lbFiles_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
            => viewModel.NavigateIntoSelectedDirectory();

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
            => viewModel.NavigateIntoParentDirectory();

        private void btnNext_Click(object sender, RoutedEventArgs e)
            => viewModel.NavigateIntoSelectedDirectory();
    }
}
