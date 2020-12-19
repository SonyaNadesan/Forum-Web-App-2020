using System.IO;
using iText.Html2pdf;
using iText.Kernel.Pdf;

namespace Application.Services.Documents
{
    public class PdfGeneratorFromHtmlService : IPdfGeneratorService<string>
    {
        public MemoryStream Generate(string html)
        {
            var memoryStream = new MemoryStream();

            using (var pdfWriter = new PdfWriter(memoryStream))
            {
                pdfWriter.SetCloseStream(false);

                using (var document = HtmlConverter.ConvertToDocument(html, pdfWriter))
                {
                }
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}