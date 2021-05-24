using FileSystem;
using System;
using System.Threading.Tasks;

namespace FuckMTP.DeviceConnector.Contracts
{
    public interface IDevice : IDisposable
    {
        string Name { get; }
        string SerialNumber { get; }

        Lazy<Directory> Root { get; }

        Task FillSubdirectories(Directory directory);
        Task FillFiles(Directory directory);
    }
}
