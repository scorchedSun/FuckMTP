using System.Collections.Generic;
using System.IO;

namespace FuckMTP.Core.Contracts
{

    public enum BehaviorRegardingDuplicates
    {
        Ignore = 0,
        CopyWithSuffix,
        Overwrite
    }
}
