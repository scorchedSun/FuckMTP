using System;

namespace FileSystem
{
    public class File : IFileSystemEntry
    {
        private readonly IPathHandler pathHandler;

        public string Name { get; }

        public Directory Location { get; }

        internal File(IPathHandler pathHandler, string name, Directory location)
        {
            if (pathHandler is null) throw new ArgumentNullException(nameof(pathHandler));
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (location is null) throw new ArgumentNullException(nameof(location));

            this.pathHandler = pathHandler;
            Name = name;
            Location = location;
        }

        public string GetPath() => Location is null ? Name : pathHandler.Combine(Location.GetPath(), Name);
    }
}
