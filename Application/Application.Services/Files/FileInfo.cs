using Microsoft.AspNetCore.Http;

namespace Application.Services.Files
{
    public class FileInfo : IFileInfo
    {
        public IFormFile File { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string SavePath { get; set; }

        public FileInfo()
        {

        }

        public FileInfo(IFormFile file, string filePath, string fileName, string savePath)
        {
            File = file;
            FilePath = filePath;
            FileName = fileName;
            SavePath = savePath;
        }
    }
}
