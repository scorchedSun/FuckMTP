using FuckMTP.Core.Contracts;

namespace FuckMTP
{
    internal sealed class File : IFile
    {
        public string Name { get; }

        public string Path { get; }

        public File(string path)
        {
            Name = System.IO.Path.GetFileName(path);
            Path = path;
        }
    }
}
