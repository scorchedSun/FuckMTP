using FileSystem;
using FluentAdb.Interfaces;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FuckMTP.ADB
{

    internal static class FileSystemManagement
    {
        public async static Task FillSubdirectories(IAdbTargeted target, Directory directory)
        {
            directory.Children.Clear();
            string result = await target.Shell.RunCommand($"ls -F {GetPathOf(directory)}").ConfigureAwait(false);
            foreach (string name in SplitOutputOfLS(result).Where(entry => entry.EndsWith("/")).Select(TrimSlashes))
                directory.AddSubdirectory(name);
        }

        public async static Task FillFiles(IAdbTargeted target, Directory directory)
        {
            directory.Files.Clear();
            string result = await target.Shell.RunCommand($"ls -F {GetPathOf(directory)}").ConfigureAwait(false);
            foreach (string name in SplitOutputOfLS(result).Where(entry => !entry.EndsWith("/")).Select(TrimSlashes))
                directory.AddFile(name);
        }

        private static string GetPathOf(Directory directory) => (directory.GetPath().EndsWith("/") ? directory.GetPath() : directory.GetPath() + "/").Replace("\\", "/").Replace(" ", "\\ ");

        private static string[] SplitOutputOfLS(string output) => Regex.Split(output, $@"{Environment.NewLine}|(?<!\\)\s+", RegexOptions.Compiled);

        private static string TrimSlashes(string value) => value.Trim('/');
    }
}
