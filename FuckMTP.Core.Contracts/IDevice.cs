using System;
using System.Collections.Generic;
using System.Text;

namespace FuckMTP.Core.Contracts
{
    public interface IDevice
    {
        string SerialNumber { get; }
        string Name { get; }
    }
}
