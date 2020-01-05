using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FuckMTP.Core.Contracts
{
    public interface IDirectory
    {
        string Name { get; }

        string Path { get; }

        IEnumerable<IDirectory> Directories { get; }
        IEnumerable<IFile> Files { get; }
    }
}
