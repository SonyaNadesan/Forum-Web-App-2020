namespace Application.Services.Files
{
    public interface IDownloadedFile
    {
        string FilePath { get; set; }
        string FileNameUponDownload { get; set; }
        string FolderPath { get; set; }
    }
}
