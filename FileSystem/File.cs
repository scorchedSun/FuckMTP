using System;

namespace FileSystem
{

    public class File
    {
        public string Name { get; }
        public string Extension { get; }

        public Directory Location { get; }

        public File(string name, string extension, Directory location)
        {
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (location is null) throw new ArgumentNullException(nameof(location));

            Name = name;
            Extension = extension;
            Location = location;
        }
    }
}
