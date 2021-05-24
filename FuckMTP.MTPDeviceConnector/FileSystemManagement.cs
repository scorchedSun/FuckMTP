//using MediaDevices;

//namespace FuckMTP.MTPDeviceConnector
//{
//    internal static class FileSystemManagement
//    {
//        public static void FillSubdirectories(MediaDevice device, FileSystem.Directory directory)
//        {
//            directory.Children.Clear();
//            string path = directory.GetPath();

//            foreach (string subdirectoryPath in device.EnumerateDirectories(path))
//                directory.Children.Add(new FileSystem.Directory(subdirectoryPath.Replace(path, string.Empty).Trim('\\'), directory));
//        }

//        public static void FillFiles(MediaDevice device, FileSystem.Directory directory)
//        {
//            directory.Files.Clear();
//            string path = directory.GetPath();

//            foreach (string filePath in device.EnumerateFiles(path))
//                directory.Files.Add(new FileSystem.File(filePath.Replace(path, string.Empty).Trim('\\'), directory));
//        }
//    }
//}
