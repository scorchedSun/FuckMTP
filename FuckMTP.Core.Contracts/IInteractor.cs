using System.Collections.Generic;

namespace FuckMTP.Core.Contracts
{

    public interface IInteractor
    {
        void NotifyNoFilesSelected();

        string SelectTargetPath();
        void NotifyNoTargetPathSelected();

        IOperationConfiguration GetOperationConfiguration();
    }

}
