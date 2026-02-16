namespace FileManager.Models
{
    public class FileItemDTO
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsDirectory { get; set; }
        public long Size { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
