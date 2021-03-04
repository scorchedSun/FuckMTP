using System.Collections.Generic;

namespace FuckMTP.Core
{
    public interface IDirectory
    {
        string Name { get; }

        string Path { get; }

        IEnumerable<IDirectory> Directories { get; }
        IEnumerable<IFile> Files { get; }
    }
}
