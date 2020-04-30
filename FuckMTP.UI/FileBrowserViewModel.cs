using FileSystem;
using FuckMTP.DeviceConnector.Contracts;
using System.Windows;

namespace FuckMTP.UI
{
    internal sealed class FileBrowserViewModel : PropertyChangedNotifier
    {
        private ObservableEntry hierarchy;

        public ObservableEntry Hierarchy
        {
            get => hierarchy;
            set
            {
                hierarchy = value;
                NotifyPropertyChanged();
            }
        }

        public FileBrowserViewModel(IDevice device)
        {
            Hierarchy = new ObservableEntry(device.Root.Value);
            foreach (Directory directory in device.GetSubdirectories(device.Root.Value))
                Hierarchy.Entries.Add(new ObservableEntry(directory));
            foreach (File file in device.GetFiles(device.Root.Value))
                Hierarchy.Entries.Add(new ObservableEntry(file));
        }
    }
}
