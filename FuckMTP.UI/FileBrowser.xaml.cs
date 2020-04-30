using FileSystem;
using FuckMTP.DeviceConnector.Contracts;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FuckMTP.UI
{
    /// <summary>
    /// Interaction logic for FileBrowser.xaml
    /// </summary>
    public partial class FileBrowser : Window
    {
        private readonly IDevice device;

        public ObservableEntry Hierarchy { get; }

        public FileBrowser(IDevice device)
        {
            InitializeComponent();
            this.device = device;
            DataContext = this;
            tblLocation.Text = System.IO.Path.Combine(device.Name, device.Root.Value.GetPath());
            foreach (FileSystem.Directory directory in device.GetSubdirectories(device.Root.Value))
                spFiles.Children.Add(new Rectangle() { Width = 30, Height = 30, Name = directory.Name, Fill = Brushes.Gray, Margin = new Thickness(5), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top });
            //foreach (File file in device.GetFiles(device.Root.Value))
            //    Hierarchy.Entries.Add(new ObservableEntry(file));
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
    }
}
