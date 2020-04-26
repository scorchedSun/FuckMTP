using FuckMTP.Core.Contracts;

namespace FuckMTP.UI.Contracts
{
    public interface IOperationConfiguration
    {
        Mode Mode { get; }
        BehaviorRegardingDuplicates BehaviorRegardingDuplicates { get; }
    }
}
