using FileSystem;

namespace FuckMTP.ADB
{
    public class PathHandler : IPathHandler
    {
        public char DirectorySeparator { get; } = '/';

        public string Combine(params string[] paths)
        {
            if (paths.Length == 0) return string.Empty;

            string result = paths[0].TrimEnd(DirectorySeparator);

            for (int i = 1; i < paths.Length; ++i)
                result += DirectorySeparator + paths[i].Trim(DirectorySeparator);

            return result.Replace(" ", "\\ ").Replace('\\', DirectorySeparator);
        }

        public string GetDirectoryName(string path)
        {
            int lastIndexOfSlash = path.LastIndexOf(DirectorySeparator);

            if (lastIndexOfSlash == -1) return path;

            return path.Substring(0, lastIndexOfSlash);
        }
    }
}
