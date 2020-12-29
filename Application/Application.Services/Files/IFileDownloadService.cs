namespace Application.Services.Files
{
    public interface IFileDownloadService
    {
        string FilePath { get; set; }
        string FileNameUponDownload { get; set; }
        string FolderPath { get; set; }
        void Download();
    }
}