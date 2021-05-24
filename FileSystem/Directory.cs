using System;
using System.Collections.Generic;

namespace FileSystem
{
    public class Directory : IFileSystemEntry
    {
        private readonly IPathHandler pathHandler;

        public string Name { get; }

        public Directory Parent { get; }
        public IList<Directory> Children { get; } = new List<Directory>();
        public IList<File> Files { get; } = new List<File>();

        public bool IsRoot => Parent is null;

        internal Directory(IPathHandler pathHandler, string name, Directory parent = null)
        {
            if (pathHandler is null) throw new ArgumentNullException(nameof(pathHandler));
            if (name is null) throw new ArgumentNullException(nameof(name));

            this.pathHandler = pathHandler;
            Name = name;
            Parent = parent;
        }

        public string GetPath() => Parent is null ? Name : pathHandler.Combine(Parent.GetPath(), Name);

        public void AddSubdirectory(string name) => Children.Add(new Directory(pathHandler, name, this));
        public void AddFile(string name) => Files.Add(new File(pathHandler, name, this));
    }
}
