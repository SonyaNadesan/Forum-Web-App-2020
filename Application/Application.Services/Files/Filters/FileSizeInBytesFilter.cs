using Application.Services.Filtering;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Files.Filters
{
    public class FileSizeInBytesFilter : FileAndSizeRestriction, IFilter<IFormFile>
    {
        public FileSizeInBytesFilter(int maximumSizeAllowed) : base(maximumSizeAllowed)
        {
            MaximumSizeAllowed = maximumSizeAllowed;
        }

        public override bool IsValid(IFormFile file)
        {
            return file.Length <= MaximumSizeAllowed;
        }
    }
}
