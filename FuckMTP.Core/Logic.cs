using CommonExtensions;
using FuckMTP.Core.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FuckMTP.Core
{
    public class Logic
    {
        private readonly IInteractor interactor;
        private readonly IFileSource fileSource;
        private readonly IFileHandler fileHandler;

        public Logic(IInteractor interactor, IFileSource fileSource, IFileHandler fileHandler)
        {
            this.interactor = interactor ?? throw new ArgumentNullException(nameof(interactor));
            this.fileSource = fileSource ?? throw new ArgumentNullException(nameof(fileSource));
            this.fileHandler = fileHandler ?? throw new ArgumentNullException(nameof(fileHandler));
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
            Action<string, string, bool> fileOperation;
            if (configuration.Mode == Mode.Copy)
                fileOperation = fileHandler.Copy;
            else
                fileOperation = fileHandler.Move;

            List<string> uniqueDirectoryPaths = GetUniqueDirectoryPathsFrom(files);
            string commonBasePath = uniqueDirectoryPaths.GetCommonPrefix();
            commonBasePath = commonBasePath.TrimEnd(Path.DirectorySeparatorChar);
            targetPath = targetPath.Trim(Path.DirectorySeparatorChar);

            EnsureLocalDirectoriesExist(uniqueDirectoryPaths.ConvertAll(p => p.Replace(commonBasePath, targetPath)));

            foreach (IFile file in files)
            {
                string localPath = file.Path.Replace(commonBasePath, targetPath);

                if (File.Exists(localPath) && configuration.BehaviorRegardingDuplicates == BehaviorRegardingDuplicates.Ignore)
                    continue;

                if (configuration.BehaviorRegardingDuplicates != BehaviorRegardingDuplicates.CopyWithSuffix)
                {
                    fileOperation(file.Path, localPath, configuration.BehaviorRegardingDuplicates == BehaviorRegardingDuplicates.Overwrite);
                }
                else
                {
                    string newFileName = DetermineNewNameForPotentialDuplicateBasedOn(file.Name, targetPath);
                    fileOperation(file.Path, localPath.Replace(file.Name, newFileName), false);
                }
            }
        }

        private string DetermineNewNameForPotentialDuplicateBasedOn(string fileName, string targetPath)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            Regex regex = new Regex($@"{fileNameWithoutExtension}(?: \(\d+\))?\.{extension}", RegexOptions.Compiled);

            int numberOfPotentialDuplicates = Directory.GetFiles(targetPath).Count(filePath => regex.IsMatch(Path.GetFileName(filePath)));

            return $"{fileNameWithoutExtension} ({++numberOfPotentialDuplicates}).{extension}";
        }

        private List<string> GetUniqueDirectoryPathsFrom(IReadOnlyList<IFile> files)
        {
            return files
                .Select(file => Path.GetDirectoryName(file.Path))
                .Distinct()
                .OrderBy(path => path.Length)
                .ToList();
        }

        private void EnsureLocalDirectoriesExist(IReadOnlyList<string> localPaths)
        {
            foreach (string subdirectoryPath in localPaths)
            {
                if (!Directory.Exists(subdirectoryPath))
                    Directory.CreateDirectory(subdirectoryPath);
            }
        }
    }
}
