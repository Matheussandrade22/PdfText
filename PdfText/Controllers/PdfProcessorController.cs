using Microsoft.AspNetCore.Mvc;
using PdfText.Helpers;
using System.Drawing;
using System.Text;
using Tesseract;

namespace PdfText.Controllers
{
    public class PdfProcessorController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files["File"];

            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Selecione um arquivo PDF válido.";
                return View("Index");
            }

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Position = 0;

            var bitmaps = new List<Bitmap>();
            var ocrTexts = new List<string>();

            using var document = PdfiumViewer.PdfDocument.Load(ms);
            using var engine = new TesseractEngine(@"./tessdata", "por", EngineMode.Default);

            for (int i = 0; i < document.PageCount; i++)
            {
                using (var image = document.Render(i, 300, 300, true)) // System.Drawing.Image
                {
                    var bitmap = new Bitmap(image);
                    bitmaps.Add(bitmap);

                    using var pix = PixConverter.ToPix(bitmap);
                    using var page = engine.Process(pix);
                    ocrTexts.Add(page.GetText());
                }
            }

            var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "OcrOutput");
            Directory.CreateDirectory(outputDir);
            var outputPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(file.FileName) + "_searchable.pdf");

            // Use o helper que mantém o layout original e insere texto invisível
            PdfText.Helpers.OcrPdfSearchableHelper.GenerateSearchablePdf(bitmaps, engine, outputPath);

            ViewBag.DownloadFileName = Path.GetFileName(outputPath);
            ViewBag.ExtractedText = string.Join(Environment.NewLine, ocrTexts);
            return View("Index");
        }

        public IActionResult Download(string fileName)
        {
            var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "OcrOutput");
            var filePath = Path.Combine(outputDir, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", fileName);
        }

        // Método opcional para extração de texto, caso queira usar na view
        public string ExtractTextFromImagePdf(Stream pdfStream)
        {
            var extractedText = new StringBuilder();

            using var document = PdfiumViewer.PdfDocument.Load(pdfStream);
            using var engine = new TesseractEngine(@"./tessdata", "por", EngineMode.Default);

            for (int i = 0; i < document.PageCount; i++)
            {
                using (var image = document.Render(i, 300, 300, true))
                {
                    using var pix = PixConverter.ToPix((Bitmap)image);
                    using var page = engine.Process(pix);
                    extractedText.AppendLine(page.GetText());
                }
            }

            return extractedText.ToString();
        }
    }
}