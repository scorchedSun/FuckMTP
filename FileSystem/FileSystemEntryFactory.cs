using System;

namespace FileSystem
{

    public class FileSystemEntryFactory
    {
        private readonly IPathHandler pathHandler;

        public FileSystemEntryFactory(IPathHandler pathHandler) => this.pathHandler = pathHandler ?? throw new ArgumentNullException(nameof(pathHandler));

        public Directory CreateDirectory(string name) => new Directory(pathHandler, name);
        public Directory CreateDirectory(string name, Directory parent) => new Directory(pathHandler, name, parent);
        public File CreateFile(string name, Directory location) => new File(pathHandler, name, location);
    }
}
