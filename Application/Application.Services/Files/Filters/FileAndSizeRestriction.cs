using Application.Services.Filtering;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Files.Filters
{
    public abstract class FileAndSizeRestriction : IFileAndSizeRestriction, IFilter<IFormFile>
    {
        public string Description { get; private set; }
        public int MaximumSizeAllowed { get; set; }

        protected FileAndSizeRestriction(int maximumSizeAllowed)
        {
            MaximumSizeAllowed = maximumSizeAllowed;
            Description = "Size";
        }

        public abstract bool IsValid(IFormFile file);
    }
}
