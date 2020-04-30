using MediaDevices;
using System.Collections.Generic;
using System.IO;

namespace FuckMTP.MTPDeviceConnector
{
    internal static class FileSystemManagement
    {
        public static IEnumerable<FileSystem.Directory> GetSubdirectories(MediaDevice device, FileSystem.Directory directory)
        {
            string path = directory.GetPath();

            foreach (string subdirectory in device.EnumerateDirectories(path))
                yield return new FileSystem.Directory(subdirectory.Replace(path, string.Empty), directory);
        }

        public static IEnumerable<FileSystem.File> GetFiles(MediaDevice device, FileSystem.Directory directory)
        {
            string path = directory.GetPath();

            foreach (string file in device.EnumerateFiles(path))
                yield return new FileSystem.File(file.Replace(path, string.Empty), directory);
        }
    }
}
