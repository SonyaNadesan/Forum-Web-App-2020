using Microsoft.AspNetCore.Http;

namespace Application.Services.Files
{
    public interface IFileSizeRestrictionService
    {
        bool IsValidInBytes(IFormFile file, int bytesLimit);
        bool IsValidInMegabytes(IFormFile file, int megabytesLimit);
        bool IsValidInKilobytes(IFormFile file, int kilobytesLimit);
    }
}
