using Application.Services.Files.Filters;
using Application.Services.Filtering;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Application.Services.Files
{
    public class FileFilterBuilder : IFileFilterBuilder
    {
        private List<IFilter<IFormFile>> _filters = new List<IFilter<IFormFile>>();

        public IFileFilterBuilder AddFileExtensionFilter(IFileExtensions allowedExtensions)
        {
            _filters.Add(new FileExtensionFilter(allowedExtensions));
            return this;
        }

        public IFileFilterBuilder AddFileSizeInMegaBytesFilter(int maximumSizeAllowed)
        {
            _filters.Add(new FileSizeInMegaBytesFilter(maximumSizeAllowed));
            return this;
        }

        public List<IFilter<IFormFile>> Build()
        {
            return _filters;
        }
    }
}
