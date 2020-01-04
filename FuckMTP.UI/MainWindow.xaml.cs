using FuckMTP.ADB;
using FuckMTP.Core;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            logic = new Logic(new DeviceConnector(), this);
            logic.Run();
        }

        public IFileOperation CreateFileOperation(IDirectory rootDirectory)
        {
            throw new NotImplementedException();
        }

        public void NotifyFileOperationFailed(string message)
        {
            throw new NotImplementedException();
        }

        public void NotifyNoDeviceConnected()
        {
            throw new NotImplementedException();
        }

        public void NotifySuccess(IFileOperation operation)
        {
            throw new NotImplementedException();
        }

        public IDevice SelectOneDevice(object devices)
        {
            throw new NotImplementedException();
        }

        public IBusyIndicator SetBusy()
        {
            throw new NotImplementedException();
        }
    }
}
