using System.Collections.Generic;

namespace FuckMTP.Core.Contracts
{
    public interface IFileSource
    {
        IReadOnlyList<IFile> SelectFiles();
    }
}
