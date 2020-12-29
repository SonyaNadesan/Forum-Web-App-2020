using Application.Services.Filtering;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Files.Filters
{
    public class FileSizeInKiloBytesFilter : FileAndSizeRestriction, IFilter<IFormFile>
    {
        public FileSizeInKiloBytesFilter(int maximumSizeAllowed) : base(maximumSizeAllowed)
        {
            MaximumSizeAllowed = maximumSizeAllowed;
        }

        public override bool IsValid(IFormFile file)
        {
            return file.Length <= (MaximumSizeAllowed * 1000);
        }
    }
}
