using FuckMTP.ADB;
using FuckMTP.Core;
using FuckMTP.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace FuckMTP.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IInteractor
    {
        Logic logic;

        public MainWindow()
        {
            InitializeComponent();
            logic = new Logic(new DeviceConnector(@"C:\adb\adb.exe"), this);
            logic.Run();
        }

        public IFileOperation CreateFileOperation(IDirectory rootDirectory)
        {
            throw new NotImplementedException();
        }

        public void NotifyFileOperationFailed(string message)
        {
            MessageBox.Show(message, "Operation failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void NotifyNoDeviceConnected()
        {
            MessageBox.Show("No device connected!");
        }

        public void NotifyNoDeviceSelected()
        {
            MessageBox.Show("No device selected!");
        }

        public void NotifySuccess(IFileOperation operation)
        {
            MessageBox.Show("Operation finished successfully!");
        }

        public IDevice SelectOneDevice(IList<IDevice> devices)
        {
            DeviceSelector deviceSelector = new DeviceSelector(devices);
            deviceSelector.ShowDialog();

            return deviceSelector.SelectedDevice;
        }

        public IBusyIndicator SetBusy()
        {
            return new BusyIndicator(this);
        }

        private class BusyIndicator : IBusyIndicator
        {
            private readonly Window window;
            private readonly Cursor previousCursor;

            public BusyIndicator(Window window)
            {
                this.window = window ?? throw new ArgumentNullException(nameof(window));
                previousCursor = this.window.Cursor;
                this.window.Cursor = Cursors.Wait;
            }

            public void Dispose()
            {
                window.Cursor = previousCursor;
            }
        }
    }
}
