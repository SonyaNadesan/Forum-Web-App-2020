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

            HtmlConverter.ConvertToDocument(html, writer);

            msOutput.Flush();

            return msOutput;
        }
    }
}