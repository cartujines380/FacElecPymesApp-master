using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Infraestructura.Transversal.eComp.PDF.Model
{
    public class ImagenLogo
    {
        public static Image ImageResize(Image sourceImage, int newHeight, int newWidth)
        {
            if (
                sourceImage.PixelFormat == PixelFormat.Undefined
                | sourceImage.PixelFormat == PixelFormat.DontCare
                | sourceImage.PixelFormat == PixelFormat.Format16bppArgb1555
                | sourceImage.PixelFormat == PixelFormat.Format16bppGrayScale
            )
            {
                throw new NotSupportedException("Pixel format of the image is not supported.");
            }

            Bitmap bitmap = (
                sourceImage.PixelFormat == PixelFormat.Format1bppIndexed
                | sourceImage.PixelFormat == PixelFormat.Format4bppIndexed
                | sourceImage.PixelFormat == PixelFormat.Format8bppIndexed
            )
            ? new Bitmap(newWidth, newHeight)
            : new Bitmap(newWidth, newHeight, sourceImage.PixelFormat);

            using (Graphics graphicsImage = Graphics.FromImage(bitmap))
            {
                graphicsImage.SmoothingMode = SmoothingMode.HighQuality;
                graphicsImage.DrawImage(sourceImage, 0, 0, bitmap.Width, bitmap.Height);
                graphicsImage.Dispose();
            }

            return bitmap;
        }
    }
}
