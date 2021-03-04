namespace FuckMTP.Core
{
    public interface IOperationConfiguration
    {
        Mode Mode { get; }
        BehaviorRegardingDuplicates BehaviorRegardingDuplicates { get; }
    }
}
