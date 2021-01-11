using System;
using System.IO;

namespace FileSystem
{
    public class File : IFileSystemEntry
    {
        public string Name { get; }

        public Directory Location { get; }

        public File(string name, Directory location)
        {
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (location is null) throw new ArgumentNullException(nameof(location));

            Name = name;
            Location = location;
        }

        public string GetPath() => Location is null ? Name : Path.Combine(Location.GetPath(), Name);
    }
}
