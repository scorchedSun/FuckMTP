using System.Threading.Tasks;

namespace FuckMTP.Core.Contracts
{
    public interface IFileHandler
    {
        Task CopyAsync(string filePath, string targetPath);

        Task MoveAsync(string filePath, string targetPath);
    }
}
