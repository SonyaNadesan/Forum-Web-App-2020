using Microsoft.AspNetCore.Http;

namespace Application.Services.Files
{
    public interface IFileInfo
    {
        IFormFile File { get; set; }
        string FilePath { get; set; }
        string FileName { get; set; }
        string SavePath { get; set; }
    }
}