using CommonExtensions;
using FileSystem;
using FuckMTP.Core;
using FuckMTP.DeviceConnector.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

            EnsureContentsAreLoaded(directory);

            CurrentPath = System.IO.Path.Combine(device.Name, directory.GetPath());
            foreach (Directory subdirectory in directory.Children)
                FileSystemControls.Add(new DirectoryControl(subdirectory));
            foreach (File file in directory.Files)
                FileSystemControls.Add(new FileControl(file));
        }

        public IReadOnlyCollection<File> GetFiles()
        {
            EnsureContentsAreLoadedRecursively(CurrentDirectory);
            return CurrentDirectory.Files
                .Union(CurrentDirectory.Children.GetFromHierarchy(directory => directory.Children, directory => directory.Files))
                .ToList().AsReadOnly();
        }       
        
        private void EnsureContentsAreLoaded(Directory directory)
        {
            if (directory.Children.Count == 0)
                device.FillSubdirectories(directory).GetAwaiter().GetResult();
            if (directory.Files.Count == 0)
                device.FillFiles(directory).GetAwaiter().GetResult();
        }

        private void EnsureContentsAreLoadedRecursively(Directory directory)
        {
            Queue<Directory> queue = new Queue<Directory>();
            queue.Enqueue(directory);

            while (queue.Count > 0)
            {
                Directory directoryToLoadContentsOf = queue.Dequeue();

                if (directoryToLoadContentsOf.Children.Count == 0)
                    device.FillSubdirectories(directoryToLoadContentsOf);
                if (directoryToLoadContentsOf.Files.Count == 0)
                    device.FillFiles(directoryToLoadContentsOf);

                queue.EnqueueRange(directoryToLoadContentsOf.Children);
            }
        }
    }
}
