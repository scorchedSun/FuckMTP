using System;

namespace FuckMTP.Core
{
    public interface IInteractor
    {
        void NotifyNoFilesSelected();

        string SelectTargetPath();
        void NotifyNoTargetPathSelected();

        IOperationConfiguration GetOperationConfiguration();
        void NotifyNoOperationConfigurationProvided();

        void RunWithProgressReport(int numberOfElements, Action<ProgressReporter> action);

        void ReportSuccess();
    }
}
