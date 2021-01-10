using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.Services.Files
{
    public class FileUploadService : IFileUploadService
    {
        public async Task<ServiceResponse<FileInfo>> Upload(IFormFile file, string fileNameOnUpload, string savePath)
        {
            var fileInfo = new FileInfo()
            {
                File = file,
                FileName = fileNameOnUpload,
                FilePath = Path.Combine("wwwroot/" + savePath, fileNameOnUpload)
            };

            var response = new ServiceResponse<FileInfo>(fileInfo);

            try
            {
                using (var stream = new FileStream(fileInfo.FilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                fileInfo.FileName = string.Empty;
                fileInfo.FilePath = string.Empty;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
