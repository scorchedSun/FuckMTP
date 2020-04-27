using FileSystem;
using System;

namespace FuckMTP.DeviceConnector.Contracts
{
    public interface IDevice
    {
        string Name { get; }

        Lazy<Directory> Root { get; }
    }
}
