namespace FuckMTP.Core.Contracts
{
    public interface IFileHandler
    {
        void Copy(string filePath, string targetPath, bool overwriteExisting);

        void Move(string filePath, string targetPath, bool overwriteExisting);
    }
}
