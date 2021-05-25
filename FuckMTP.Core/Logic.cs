using CommonExtensions;
using FileSystem;
using FuckMTP.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FuckMTP.Core
{
    public class Logic
    {
        private readonly IInteractor interactor;
        private readonly IFileSource fileSource;
        private readonly IFileHandler fileHandler;
        private readonly IPathHandler pathHandler;

        public Logic(IInteractor interactor, IFileSource fileSource, IFileHandler fileHandler, IPathHandler pathHandler)
        {
            this.interactor = interactor ?? throw new ArgumentNullException(nameof(interactor));
            this.fileSource = fileSource ?? throw new ArgumentNullException(nameof(fileSource));
            this.fileHandler = fileHandler ?? throw new ArgumentNullException(nameof(fileHandler));
            this.pathHandler = pathHandler ?? throw new ArgumentNullException(nameof(pathHandler));
        }

        public void Run()
        {
            IReadOnlyList<IFile> files = fileSource.SelectFiles();

            if (files.Count == 0)
            {
                interactor.NotifyNoFilesSelected();
                return;
            }

            string target = interactor.SelectTargetPath();

            if (string.IsNullOrWhiteSpace(target))
            {
                interactor.NotifyNoTargetPathSelected();
                return;
            }

            IOperationConfiguration configuration = interactor.GetOperationConfiguration();

            if (configuration is null || configuration.Mode == Mode.Abort)
            {
                interactor.NotifyNoOperationConfigurationProvided();
                return;
            }
            
            Process(files, target, configuration);

            interactor.ReportSuccess();
        }

        private void Process(IReadOnlyList<IFile> files, string targetPath, IOperationConfiguration configuration)
        {
            Func<string, string, Task> fileOperation;
            if (configuration.Mode == Mode.Copy)
                fileOperation = fileHandler.CopyAsync;
            else
                fileOperation = fileHandler.MoveAsync;

            List<string> uniqueDirectoryPaths = GetUniqueDirectoryPathsFrom(files);
            string commonBasePath = uniqueDirectoryPaths.GetCommonPrefix();
            commonBasePath = commonBasePath.TrimEnd(pathHandler.DirectorySeparator);
            targetPath = targetPath.Trim(Path.DirectorySeparatorChar);

            EnsureLocalDirectoriesExist(uniqueDirectoryPaths.ConvertAll(p => p.Replace(commonBasePath, targetPath)));

            interactor.RunWithProgressReport(files.Count, progressReporter =>
            {
                Parallel.ForEach(files, file =>
                {
                    string localPath = file.Path.Replace(commonBasePath, targetPath);

                    if (System.IO.File.Exists(localPath) && configuration.BehaviorRegardingDuplicates == BehaviorRegardingDuplicates.Ignore)
                        return;

                    if (configuration.BehaviorRegardingDuplicates != BehaviorRegardingDuplicates.CopyWithSuffix)
                    {
                        fileOperation(file.Path, localPath).GetAwaiter().GetResult();
                    }
                    else
                    {
                        string newFileName = DetermineNewNameForPotentialDuplicateBasedOn(file.Name, targetPath);
                        fileOperation(file.Path, localPath.Replace(file.Name, newFileName)).GetAwaiter().GetResult();
                    }

                    progressReporter.StepOne();
                });
            });
        }

        private string DetermineNewNameForPotentialDuplicateBasedOn(string fileName, string targetPath)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            Regex regex = new Regex($@"{fileNameWithoutExtension}(?: \(\d+\))?\.{extension}", RegexOptions.Compiled);

            int numberOfPotentialDuplicates = System.IO.Directory.GetFiles(targetPath).Count(filePath => regex.IsMatch(Path.GetFileName(filePath)));

            return $"{fileNameWithoutExtension} ({++numberOfPotentialDuplicates}).{extension}";
        }

        private List<string> GetUniqueDirectoryPathsFrom(IReadOnlyList<IFile> files)
        {
            return files
                .Select(file => pathHandler.GetDirectoryName(file.Path))
                .Distinct()
                .OrderBy(path => path.Length)
                .ToList();
        }

        private void EnsureLocalDirectoriesExist(IReadOnlyList<string> localPaths)
        {
            foreach (string subdirectoryPath in localPaths)
            {
                if (!System.IO.Directory.Exists(subdirectoryPath))
                    System.IO.Directory.CreateDirectory(subdirectoryPath);
            }
        }
    }
}
