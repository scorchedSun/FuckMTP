using FuckMTP.DeviceConnector.Contracts;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

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
            DialogResult = true;
            Close();
        }

        private void btnAbort_Click(object sender, RoutedEventArgs e)
            => Close();

        private void lbDevices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
