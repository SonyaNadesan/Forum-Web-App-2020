using System.IO;
using System.Linq;

namespace Application.Services.Files
{
    public class FileValidationService : IFileValidationService
    {
        public IFileExtensions AllowedExtensions { get; set; }

        public bool IsValid(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            var result = AllowedExtensions.Extensions.Where(e => e.ToLower() == extension);

            return result != null && result.Any();
        }
    }
}
