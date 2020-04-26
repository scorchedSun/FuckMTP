namespace FuckMTP.Core.Contracts
{
    public interface IOperationConfiguration
    {
        Mode Mode { get; }
        BehaviorRegardingDuplicates BehaviorRegardingDuplicates { get; }
    }
}
