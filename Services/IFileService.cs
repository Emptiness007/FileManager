using FileManager.Models;

namespace FileManager.Services
{
    public interface IFileService
    {
        List<string> GetDrives();
        List<FileItemDTO> GetDirectoryFile(string path);
        string? GetParentPath(string currentPath);

        void Copy(string sourcePath, string targetPath);
        void Move(string sourcePath, string targetPath);
        void Delete(string sourcePath);

    }
}
