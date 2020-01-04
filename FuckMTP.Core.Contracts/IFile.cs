using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FuckMTP.Core.Contracts
{
    public interface IFile
    {
        string Name { get; }

        Path Path { get; }
    }
}
