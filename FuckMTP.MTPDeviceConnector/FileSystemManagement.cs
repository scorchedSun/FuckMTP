using MediaDevices;
using System.Collections.Generic;
using System.IO;

namespace FuckMTP.MTPDeviceConnector
{
    internal static class FileSystemManagement
    {
        public static FileSystem.Directory LoadFrom(MediaDevice device)
        {
            if (!device.IsConnected)
                device.Connect();

            try
            {
                MediaDirectoryInfo rootInfo = device.GetRootDirectory();
                FileSystem.Directory root = new FileSystem.Directory(rootInfo.Name);

                var queue = new Queue<string>();
                queue.Enqueue(rootInfo.FullName);
                Dictionary<string, FileSystem.Directory> pathToDirectory = new Dictionary<string, FileSystem.Directory>();
                pathToDirectory[rootInfo.FullName] = root;

                do
                {
                    string path = queue.Dequeue();
                    FileSystem.Directory directory = pathToDirectory[path];

                    foreach (string subpath in device.EnumerateDirectories(path))
                    {
                        string subDirectoryPath = Path.Combine(path, subpath);
                        FileSystem.Directory subDirectory = new FileSystem.Directory(subpath);
                        pathToDirectory[subDirectoryPath] = subDirectory;
                        directory.Children.Add(subDirectory);
                        queue.Enqueue(subDirectoryPath);
                    }

                    foreach (string name in device.EnumerateFiles(path))
                        directory.AddFile(name);

                } while (queue.Count > 0);

                return root;
            }
            finally
            {
                device.Disconnect();
            }
        }
    }
}
