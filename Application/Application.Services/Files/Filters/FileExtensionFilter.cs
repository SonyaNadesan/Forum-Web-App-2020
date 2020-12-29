using Application.Services.Filtering;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace Application.Services.Files.Filters
{
    public class FileExtensionFilter : IFilter<IFormFile>
    {
        public IFormFile File { get; set; }
        public IFileExtensions AllowedExtensions { get; set; }

        public string Description { get; private set; }

        public FileExtensionFilter(IFileExtensions allowedExtensions)
        {
            File = File;
            AllowedExtensions = allowedExtensions;
            Description = "Extension";
        }

        public bool IsValid(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            var result = AllowedExtensions.Extensions.Where(e => e.ToLower() == extension);
            return result != null && result.Any();
        }
    }
}
