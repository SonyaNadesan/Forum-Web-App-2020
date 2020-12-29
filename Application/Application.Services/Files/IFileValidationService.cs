namespace Application.Services.Files
{
    public interface IFileValidationService
    {
        IFileExtensions AllowedExtensions { get; set; }

        bool IsValid(string fileName);
    }
}
