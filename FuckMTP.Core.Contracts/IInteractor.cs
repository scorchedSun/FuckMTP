using System.Collections.Generic;

namespace FuckMTP.Core.Contracts
{

    public interface IInteractor
    {
        string GetTargetPath();
        IOperationConfiguration GetOperationConfiguration();

        void NotifyNoFolderSelected();

        void NotifyConfigurationAborted();
        IList<IFile> GetFiles();
        void NotifyFileSelectionAborted();
    }
}
