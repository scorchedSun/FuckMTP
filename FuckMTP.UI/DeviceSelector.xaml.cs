using FuckMTP.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FuckMTP.UI
{
    /// <summary>
    /// Interaction logic for DeviceSelector1.xaml
    /// </summary>
    public partial class DeviceSelector : Window
    {
        public IList<IDevice> Devices { get; }
        public IDevice SelectedDevice => (IDevice)lbDevices.SelectedItem;

        public DeviceSelector(IList<IDevice> devices)
        {
            InitializeComponent();
            DataContext = this;
            Devices = devices;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAbort_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void lbDevices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
