using Application.Services.Filtering;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Application.Services.Files
{
    public interface IFileFilterBuilder
    {
        IFileFilterBuilder AddFileExtensionFilter(IFileExtensions allowedExtensions);

        IFileFilterBuilder AddFileSizeInMegaBytesFilter(int maximumSizeAllowed);

        List<IFilter<IFormFile>> Build();
    }
}
