using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CameraArcheryLib.Utils
{
    public static class FormatHelper
    {
        /// <summary>
        /// inform if the string is numeric of not
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumericText(string str)
        {
            if (str == null)
                throw new ArgumentNullException();
            try
            {
                var val = double.Parse(str);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// inform if the string is numeric of not
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsTextNumericInteger(string str)
        {
            if (str == null)
                throw new ArgumentNullException();
            try
            {
                int.Parse(str);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// transform the image to adapt in the image content
        /// </summary>
        /// <param name="source">bitmap source</param>
        /// <returns></returns>
        public static BitmapSource loadBitmap(Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height, 96, 96, PixelFormats.Bgr24, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }
    }
}
