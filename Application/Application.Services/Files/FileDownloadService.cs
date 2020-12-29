using System.IO;
using System.Net;

namespace Application.Services.Files
{
    public class FileDownloadService : IFileDownloadService
    {
        public string FilePath { get; set; }
        public string FileNameUponDownload { get; set; }
        public string FolderPath { get; set; }

        public void Download()
        {
            var fileExtension = Path.GetExtension(FilePath);
            FileNameUponDownload = FileNameUponDownload.Replace(" ", "_");

            if (!FileNameUponDownload.EndsWith(fileExtension))
            {
                if (FileNameUponDownload.Contains("."))
                {
                    FileNameUponDownload = FileNameUponDownload.Replace(".", "_");
                }

                FileNameUponDownload = FileNameUponDownload + fileExtension;
            }

            var client = new WebClient();
            var path = FolderPath + FilePath;
            client.DownloadFile(path, FileNameUponDownload);
        }
    }
}
