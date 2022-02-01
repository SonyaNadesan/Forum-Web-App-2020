using System;
using System.IO;
using System.Net;

namespace Application.Services.Files
{
    public class FileDownloadService : IFileDownloadService
    {
        private string FilePath;
        private string FolderPath;
        private string FileNameUponDownload;

        public ServiceResponse<IDownloadedFile> Download(string filePath, string desiredFileNameUponDownload, string folderPath)
        {
            SetProperties(filePath, desiredFileNameUponDownload, folderPath);

            ValidateFileNameToBeUsedUponDownload();

            var client = new WebClient();
            var path = FolderPath + FilePath;

            var response = new ServiceResponse<IDownloadedFile>();

            try
            {
                client.DownloadFile(path, FileNameUponDownload);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }

            response.Result = new DownloadedFile()
            {
                FileNameUponDownload = FileNameUponDownload,
                FilePath = FilePath,
                FolderPath = FolderPath
            };

            return response;
        }

        public void DownloadWithoutResponse(string filePath, string desiredFileNameUponDownload, string folderPath)
        {
            SetProperties(filePath, desiredFileNameUponDownload, folderPath);

            ValidateFileNameToBeUsedUponDownload();

            var client = new WebClient();
            var path = FolderPath + FilePath;

            try
            {
                client.DownloadFile(path, FileNameUponDownload);
            }
            catch (Exception ex)
            {
                
            }
        }

        private void SetProperties(string filePath, string desiredFileNameUponDownload, string folderPath)
        {
            FilePath = filePath;
            FileNameUponDownload = desiredFileNameUponDownload ?? filePath;
            FolderPath = folderPath;
        }

        private void ValidateFileNameToBeUsedUponDownload()
        {
            var fileExtension = Path.GetExtension(FilePath);
            FileNameUponDownload = FileNameUponDownload.Replace(" ", "_");

            if (!FileNameUponDownload.EndsWith(fileExtension))
            {
                if (FileNameUponDownload.Contains("."))
                {
                    FileNameUponDownload = FileNameUponDownload.Replace(".", "_");
                }

                FileNameUponDownload += fileExtension;
            }
        }
    }
}
