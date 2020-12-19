using System.IO;
using iText.Html2pdf;
using iText.Kernel.Pdf;

namespace Application.Services.Documents
{
    public class PdfGeneratorFromHtmlService : IPdfGeneratorService<string>
    {
        public MemoryStream Generate(string html)
        {
            MemoryStream msOutput = new MemoryStream();

            var writer = new PdfWriter(msOutput);

            var pdf = HtmlConverter.ConvertToDocument(html, writer);

            msOutput.Flush();

            return msOutput;
        }

        public MemoryStream GenerateFromFile(string filepath)
        {
            var html = File.ReadAllText(filepath);

            return Generate(html);
        }
    }
}