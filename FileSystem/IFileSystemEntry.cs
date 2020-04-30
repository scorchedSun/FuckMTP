namespace FileSystem
{
    public interface IFileSystemEntry
    {
        string Name { get; }

        string GetPath();
    }
}
