using System.Collections.Generic;

namespace FuckMTP.UI
{
    internal sealed class FileDropViewModel : PropertyChangedNotifier
    {
        public ObservableSet<string> Files { get; } = new ObservableSet<string>();

        public void AddFiles(IEnumerable<string> paths)
        {
            int countBefore = Files.Count;
            foreach (string path in paths)
                Files.Add(path);
        }
    }
}
