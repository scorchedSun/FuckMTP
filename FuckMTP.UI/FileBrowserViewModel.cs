using FileSystem;
using FuckMTP.DeviceConnector.Contracts;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace FuckMTP.UI
{
    internal sealed class FileBrowserViewModel : PropertyChangedNotifier
    {
        private readonly IDevice device;

        private bool previousEnabled;
        private bool nextEnabled;
        private DirectoryControl selectedDirectory;
        private Directory currentDirectory;
        private string currentPath;

        public ObservableCollection<Control> FileSystemControls { get; set; } = new ObservableCollection<Control>();

        public DirectoryControl SelectedDirectory
        {
            get => selectedDirectory;
            set
            {
                selectedDirectory = value;
                NotifyPropertyChanged();
                NextEnabled = SelectedDirectory?.Directory != null;
            }
        }

        public Directory CurrentDirectory
        {
            get => currentDirectory;
            set
            {
                currentDirectory = value;
                NotifyPropertyChanged();
                PreviousEnabled = currentDirectory?.Parent != null;
            }
        }

        public string CurrentPath
        {
            get => currentPath;
            set
            {
                currentPath = value;
                NotifyPropertyChanged();
            }
        }

        public bool PreviousEnabled
        {
            get => previousEnabled;
            set
            {
                previousEnabled = value;
                NotifyPropertyChanged();
            }
        }

        public bool NextEnabled
        {
            get => nextEnabled;
            set
            {
                nextEnabled = value;
                NotifyPropertyChanged();
            }
        }

        public FileBrowserViewModel(IDevice device)
            => this.device = device ?? throw new ArgumentNullException(nameof(device));

        public void NavigateIntoSelectedDirectory()
        {
            if (SelectedDirectory?.Directory is null) return;

            DisplayContentsOf(SelectedDirectory.Directory);
        }

        public void NavigateIntoParentDirectory()
        {
            if (CurrentDirectory?.Parent is null) return;

            DisplayContentsOf(CurrentDirectory.Parent);
        }

        public void DisplayContentsOf(Directory directory)
        {
            FileSystemControls.Clear();
            CurrentDirectory = directory;

            if (directory.Children.Count == 0)
                device.FillSubdirectories(directory);
            if (directory.Files.Count == 0)
                device.FillFiles(directory);

            CurrentPath = System.IO.Path.Combine(device.Name, directory.GetPath());
            foreach (Directory subdirectory in directory.Children)
                FileSystemControls.Add(new DirectoryControl(subdirectory));
            foreach (File file in directory.Files)
                FileSystemControls.Add(new FileControl(file));
        }
    }
}
