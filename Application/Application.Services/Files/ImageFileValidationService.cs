namespace Application.Services.Files
{
    public class ImageFileValidationService : FileValidationService, IImageFileValidationService
    {
        public ImageFileValidationService()
        {
            AllowedExtensions = new ImageFileExtensions();
        }
    }
}
