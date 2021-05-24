namespace FileSystem
{
    public interface IPathHandler
    {
        char DirectorySeparator { get; }

        string Combine(params string[] paths);

        string GetDirectoryName(string path);
    }
}
