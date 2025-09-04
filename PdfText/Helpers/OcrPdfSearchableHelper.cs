using System.Collections.Generic;
using System.Drawing;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Kernel.Colors;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Font;
using iText.IO.Image;
using iText.IO.Font.Constants;
using Tesseract;
using iText.Layout.Properties;

namespace PdfText.Helpers
{
    public static class OcrPdfSearchableHelper
    {
        public static string GenerateSearchablePdf(List<Bitmap> bitmaps, TesseractEngine engine, string outputPath)
        {
            using (var pdfWriter = new PdfWriter(outputPath))
            using (var pdfDoc = new PdfDocument(pdfWriter))
            {
                for (int i = 0; i < bitmaps.Count; i++)
                {
                    var bitmap = bitmaps[i];
                    var img = ImageDataFactory.Create(BitmapToBytes(bitmap));
                    var page = pdfDoc.AddNewPage(new PageSize(img.GetWidth(), img.GetHeight()));
                    var canvas = new PdfCanvas(page);

                    // Adiciona a imagem como fundo
                    canvas.AddImageFittedIntoRectangle(img, new iText.Kernel.Geom.Rectangle(0, 0, img.GetWidth(), img.GetHeight()), false);

                    // OCR com bounding boxes
                    using var pix = PixConverter.ToPix(bitmap);
                    using var tesseractPage = engine.Process(pix);
                    var ri = tesseractPage.GetIterator();
                    ri.Begin();

                    var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                    var transparent = new DeviceRgb(255, 255, 255); // Branco

                    do
                    {
                        string word = ri.GetText(PageIteratorLevel.Word);
                        if (!string.IsNullOrEmpty(word))
                        {
                            if (ri.TryGetBoundingBox(PageIteratorLevel.Word, out var rect))
                            {
                                // Ajuste as coordenadas conforme DPI e tamanho da imagem
                                var x = rect.X1;
                                var y = img.GetHeight() - rect.Y2; // Inverte Y
                                var width = rect.X2 - rect.X1;
                                var height = rect.Y2 - rect.Y1;

                                canvas.BeginText();
                                canvas.SetFontAndSize(font, height > 0 ? height : 10);
                                canvas.SetTextRenderingMode(iText.Kernel.Pdf.Canvas.PdfCanvasConstants.TextRenderingMode.INVISIBLE);
                                canvas.MoveText(x, y);
                                canvas.ShowText(word);
                                canvas.EndText();
                            }
                        }
                    } while (ri.Next(PageIteratorLevel.Word));
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