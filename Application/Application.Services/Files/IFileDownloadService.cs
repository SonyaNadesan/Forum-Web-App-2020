namespace Application.Services.Files
{
    public interface IFileDownloadService
    {
        ServiceResponse<IDownloadedFile> Download(string filePath, string desiredFileNameUponDownload, string folderPath);
        void DownloadWithoutResponse(string filePath, string desiredFileNameUponDownload, string folderPath);
    }
}