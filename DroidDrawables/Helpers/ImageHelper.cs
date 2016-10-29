using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

namespace DroidDrawables.Helpers
{
    public class ImageHelper
    {
        public ImageHelper()
        {

        }

        public void Resize(string imageFile, string outputFile, double scaleFactor, ref string message)
        {
            using (var srcImage = Image.FromFile(imageFile))
            {
                var newWidth = (int)(Math.Round(
                                                srcImage.Width * scaleFactor, 
                                                0));
                var newHeight = (int)(Math.Round(
                                                srcImage.Height * scaleFactor, 
                                                0));
                using (var newImage = new Bitmap(newWidth, newHeight))
                using (var graphics = Graphics.FromImage(newImage))
                {
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));
                    newImage.Save(outputFile, ImageFormat.Png);
                }
                Thread.Sleep(5);
                message = string.Format("\t{0} - {1:d}x{2:d} => {3:d}x{4:d}",
                                    Path.GetFileName(imageFile),
                                    srcImage.Width,
                                    srcImage.Height,
                                    newWidth,
                                    newHeight
                                    );
            }
        }
    }
}
