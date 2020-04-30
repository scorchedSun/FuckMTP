using FileSystem;
using System.Collections.ObjectModel;

namespace FuckMTP.UI
{
    public class ObservableEntry : IFileSystemEntry
    {
        internal IFileSystemEntry Source { get; }

        public ObservableCollection<IFileSystemEntry> Entries { get; } = new ObservableCollection<IFileSystemEntry>();

        public ObservableEntry(IFileSystemEntry source) => Source = source;

        public string Name => Source.Name;

        public string GetPath() => Source.GetPath();
    }
}
