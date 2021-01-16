using FileSystem;
using System;

namespace FuckMTP.DeviceConnector.Contracts
{
    public interface IDevice : IDisposable
    {
        string Name { get; }
        string SerialNumber { get; }

        Lazy<Directory> Root { get; }

        void FillSubdirectories(Directory directory);
        void FillFiles(Directory directory);
    }
}
