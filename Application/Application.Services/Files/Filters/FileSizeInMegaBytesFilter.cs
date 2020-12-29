using Microsoft.AspNetCore.Http;

namespace Application.Services.Files.Filters
{
    public class FileSizeInMegaBytesFilter : FileAndSizeRestriction
    {
        public FileSizeInMegaBytesFilter(int maximumSizeAllowed) : base(maximumSizeAllowed)
        {
            MaximumSizeAllowed = maximumSizeAllowed;
        }

        public override bool IsValid(IFormFile file)
        {
            return file.Length <= (MaximumSizeAllowed * 1000000);
        }
    }
}
