using System.Collections.Generic;
using System.Drawing;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;

namespace PdfText.Helpers
{
    public static class OcrPdfGenerator
    {
        public static string GenerateSearchablePdf(List<Bitmap> bitmaps, List<string> ocrTexts, string outputPath)
        {
            using (var pdfWriter = new PdfWriter(outputPath))
            using (var pdfDoc = new PdfDocument(pdfWriter))
            using (var doc = new Document(pdfDoc))
            {
                for (int i = 0; i < bitmaps.Count; i++)
                {
                    var img = ImageDataFactory.Create(BitmapToBytes(bitmaps[i]));
                    var image = new iText.Layout.Element.Image(img); // Corrigido: nome completo
                    doc.Add(image);

                    // Adiciona o texto OCR extraído
                    doc.Add(new Paragraph(ocrTexts[i]));

                    if (i < bitmaps.Count - 1)
                        pdfDoc.AddNewPage();
                }
            }
            return outputPath;
        }

        private static byte[] BitmapToBytes(Bitmap bitmap)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}