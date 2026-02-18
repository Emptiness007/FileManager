using FileManager.Models;

namespace FileManager.Services
{
    public class FileService : IFileService
    {
        public List<string> GetDrives()
        {
            return Directory.GetLogicalDrives().ToList();
        }

        public List<FileItemDTO> GetDirectoryFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Путь не указан");
            }

            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"По пути - {path} ничего не найдено");
            }

            List<FileItemDTO> items = new List<FileItemDTO>();

            foreach(var directory in Directory.GetDirectories(path))
            {
                var dir = new DirectoryInfo(directory);
                items.Add(new FileItemDTO
                {
                    Name = dir.Name,
                    Path = dir.FullName,
                    IsDirectory = true,
                    Size = 0,
                    LastUpdate = dir.LastWriteTime
                });
            }

            foreach(var file in Directory.GetFiles(path))
            {
                var f = new FileInfo(file);
                items.Add(new FileItemDTO
                {
                    Name = f.Name,
                    Path = f.FullName,
                    IsDirectory = false,
                    Size = f.Length,
                    LastUpdate = f.LastWriteTime
                });
            }
            return items.OrderByDescending(x => x.IsDirectory).ToList();
        }

        public string? GetParentPath(string currentPath)
        {
            if (string.IsNullOrEmpty(currentPath))
                return null;

            var dirInfo = new DirectoryInfo(currentPath);
            var parent = dirInfo.Parent;

            return parent?.FullName;
        }

        public void Copy(string sourcePath, string targetPath)
        {
            if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(targetPath))
            {
                throw new ArgumentException("Не указан текущий или новый путь");
            }

            if (Directory.Exists(sourcePath))
            {
                string target = Path.Combine(targetPath, Path.GetFileName(sourcePath));
                CopyDirectory(sourcePath, target);
            }
            else if (File.Exists(sourcePath))
            {
                string targetFile = Path.Combine(targetPath, Path.GetFileName(sourcePath));
                File.Copy(sourcePath, targetFile, overwrite: true);
            }
            else
            {
                throw new FileNotFoundException($"Путь - {sourcePath} не найден");
            }
        }

        private static void CopyDirectory(string sourcePath, string targetPath)
        {
            Directory.CreateDirectory(targetPath);

            foreach (string file in Directory.GetFiles(sourcePath))
            {
                string targetFile = Path.Combine(targetPath, Path.GetFileName(file));
                File.Copy(file, targetFile, true);
            }

            foreach (string subDir in Directory.GetDirectories(sourcePath))
            {
                string targetDir = Path.Combine(targetPath, Path.GetFileName(subDir));
                CopyDirectory(subDir, targetDir);
            }
        }

        public void Move(string sourcePath, string targetPath)
        {
            if(string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(targetPath))
            {
                throw new ArgumentException("Не указан текущий или новый путь");
            }

            if (Directory.Exists(sourcePath))
            {
                string targetDir = Path.Combine(targetPath, Path.GetFileName(sourcePath));
                Directory.Move(sourcePath, targetDir);
            }
            else if (File.Exists(sourcePath))
            {
                string targetFile = Path.Combine(targetPath, Path.GetFileName(sourcePath));
                File.Move(sourcePath, targetFile, true);
            }
            else
            {
                throw new FileNotFoundException($"Путь - {sourcePath} не найден");
            }

        }

        public void Delete(string sourcePath)
        {
            if (string.IsNullOrEmpty(sourcePath))
            {
                throw new ArgumentException("Не указан путь");
            }

            if (Directory.Exists(sourcePath))
            {
                Directory.Delete(sourcePath, true);
            }
            else if (File.Exists(sourcePath))
            {
                File.Delete(sourcePath);
            }
            else
            {
                throw new FileNotFoundException("Файл или директория не найдены", sourcePath);
            }
        }
    }
}
