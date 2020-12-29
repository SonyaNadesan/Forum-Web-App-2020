using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.Services.Files
{
    public class FileUploadService : IFileUploadService
    {
        public async Task<ServiceResponse<FileInfo>> Upload(IFormFile file, string fileNameToBeUsedOnUpload, string savePath)
        {
            var fileInfo = new FileInfo()
            {
                File = file,
                FileName = fileNameToBeUsedOnUpload + Path.GetExtension(file.FileName),
                FilePath = Path.Combine(savePath, file.FileName)
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
