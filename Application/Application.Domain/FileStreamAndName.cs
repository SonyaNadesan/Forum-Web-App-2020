using System.IO;

namespace Application.Domain
{
    public class FileStreamAndName
    {
        public MemoryStream AttachmentStream { get; set; }

        public string FileName { get; set; }
    }
}
