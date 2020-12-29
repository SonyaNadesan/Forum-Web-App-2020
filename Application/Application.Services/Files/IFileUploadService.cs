using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Application.Services.Files
{
    public interface IFileUploadService
    {
        Task<ServiceResponse<FileInfo>> Upload(IFormFile file, string fileNameToBeUsedOnUpload, string SavePath);
    }
}
