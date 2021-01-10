using Application.Services.Filtering;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Application.Services.Files
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly IFilterService<IFormFile> _filterService;
        private readonly IFileFilterBuilder _fileFilterBuilder;

        private readonly List<IFilter<IFormFile>> _filters;

        public ImageUploadService(IFileUploadService fileUploadService, IFilterService<IFormFile> filterService, IFileFilterBuilder fileFilterBuilder)
        {
            _fileUploadService = fileUploadService;
            _filterService = filterService;
            _fileFilterBuilder = fileFilterBuilder;

            _filters = _fileFilterBuilder.AddFileExtensionFilter(new ImageFileExtensions())
                                         .AddFileSizeInMegaBytesFilter(30)
                                         .Build();
        }

        public async Task<ServiceResponse<FileInfo>> Upload(IFormFile file, string savePath, string baseNameToBeUsedOnUpload)
        {
            var fileNameOnUpload = baseNameToBeUsedOnUpload + Path.GetExtension(file.FileName);

            var fileInfo = new FileInfo()
            {
                File = file,
                FileName = fileNameOnUpload,
                FilePath = Path.Combine(savePath, fileNameOnUpload)
            };

            var response = new ServiceResponse<FileInfo>(fileInfo);

            var isValid = _filterService.IsValidAgainstAllFilters(file, _filters);

            if (!isValid)
            {
                response.ErrorMessage = "Sorry, something went wrong. Please try again.";
                return response;
            }

            return await _fileUploadService.Upload(file, fileNameOnUpload, savePath);
        }
    }
}
