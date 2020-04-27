using System;
using System.Collections.Generic;
using System.IO;

namespace FileSystem
{
    public class Directory
    {
        public string Name { get; }

        public Directory Parent { get; }
        public IList<Directory> Children { get; } = new List<Directory>();
        public IList<File> Files { get; } = new List<File>();

        public bool IsRoot => Parent is null;

        public Directory(string name, Directory parent = null)
        {
            if (name is null) throw new ArgumentNullException(nameof(name));

            Name = name;
            Parent = parent;
        }

        public void AddFile(string name)
            => Files.Add(new File(Path.GetFileName(name), Path.GetExtension(name), this));
    }
}
