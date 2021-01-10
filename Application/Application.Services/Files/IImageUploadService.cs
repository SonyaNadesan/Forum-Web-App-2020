using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Application.Services.Files
{
    public interface IImageUploadService
    {
        Task<ServiceResponse<FileInfo>> Upload(IFormFile file, string savePath, string baseNameToBeUsedOnUpload);
    }
}
