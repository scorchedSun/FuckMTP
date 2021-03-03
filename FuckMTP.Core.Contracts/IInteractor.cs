using System;

namespace FuckMTP.Core.Contracts
{
    public interface IInteractor
    {
        void NotifyNoFilesSelected();

        string SelectTargetPath();
        void NotifyNoTargetPathSelected();

        IOperationConfiguration GetOperationConfiguration();
        void NotifyNoOperationConfigurationProvided();

        void RunWithProgressReport(Action<ProgressReporter> action);

        void ReportSuccess();
    }
}
