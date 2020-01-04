using System.Collections.Generic;
using System.IO;

namespace FuckMTP.Core.Contracts
{
    public interface IFileOperation
    {
        IList<IFile> Files { get; }
        Mode Mode { get; }
        BehaviorRegardingDuplicates BehaviorRegardingDuplicates { get; }
        Path Target { get; }
    }
}
