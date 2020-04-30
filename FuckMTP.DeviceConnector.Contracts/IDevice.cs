using FileSystem;
using System;
using System.Collections.Generic;

namespace FuckMTP.DeviceConnector.Contracts
{
    public interface IDevice : IDisposable
    {
        string Name { get; }

        Lazy<Directory> Root { get; }

        IEnumerable<Directory> GetSubdirectories(Directory directory);
        IEnumerable<File> GetFiles(Directory directory);
    }
}
