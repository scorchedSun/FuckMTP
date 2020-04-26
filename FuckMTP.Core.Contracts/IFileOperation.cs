using System.Collections.Generic;

namespace FuckMTP.Core.Contracts
{
    public interface IFileOperation
    {
        IList<IFile> Files { get; }
        IOperationConfiguration Configuration { get; }
        string TargetPath { get; }
    }
}
