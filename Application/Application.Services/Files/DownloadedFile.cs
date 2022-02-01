namespace Application.Services.Files
{
    class DownloadedFile : IDownloadedFile
    {
        public string FilePath { get; set; }
        public string FileNameUponDownload { get; set; }
        public string FolderPath { get; set; }
    }
}
