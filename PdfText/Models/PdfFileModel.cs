using Microsoft.AspNetCore.Http;

namespace PdfText.Models
{
    public class PdfFileModel
    {
        public IFormFile File { get; set; }
        public string ExtractedText { get; set; }
    }
}
