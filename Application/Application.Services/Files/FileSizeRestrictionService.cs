using Microsoft.AspNetCore.Http;

namespace Application.Services.Files
{
    public class FileSizeRestrictionService : IFileSizeRestrictionService
    {
        public bool IsValidInBytes(IFormFile file, int bytesLimit)
        {
            return file.Length <= bytesLimit;
        }

        public bool IsValidInKilobytes(IFormFile file, int kilobytesLimit)
        {
            return file.Length <= (kilobytesLimit * 1000);
        }

        public bool IsValidInMegabytes(IFormFile file, int megabytesLimit)
        {
            return file.Length <= (megabytesLimit * 1000000);

        }
    }
}
