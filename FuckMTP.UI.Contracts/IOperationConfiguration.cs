using FuckMTP.Core;

namespace FuckMTP.UI.Contracts
{
    public interface IOperationConfiguration
    {
        Mode Mode { get; }
        BehaviorRegardingDuplicates BehaviorRegardingDuplicates { get; }
    }
}
