using System.Collections.Generic;

namespace FuckMTP.Core
{
    public interface IFileSource
    {
        IReadOnlyList<IFile> SelectFiles();
    }
}
