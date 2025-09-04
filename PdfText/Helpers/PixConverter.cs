using System.Drawing;
using Tesseract;

namespace PdfText.Helpers  // Coloque o namespace do seu projeto + Helpers
{
    public static class PixConverter
    {
        public static Pix ToPix(Bitmap bitmap)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;
                return Pix.LoadFromMemory(stream.ToArray());
            }
        }
    }
}
