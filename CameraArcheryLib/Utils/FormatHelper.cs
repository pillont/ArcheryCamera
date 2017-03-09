using System;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;

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
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    // You need to specify the image format to fill the stream.
                    // I'm assuming it is PNG
                    bitmap.Save(memoryStream, ImageFormat.Png);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    BitmapDecoder bitmapDecoder = BitmapDecoder.Create(
                        memoryStream,
                        BitmapCreateOptions.PreservePixelFormat,
                        BitmapCacheOption.OnLoad);

                    // This will disconnect the stream from the image completely...
                    WriteableBitmap writable =
            new WriteableBitmap(bitmapDecoder.Frames.Single());
                    writable.Freeze();

                    return writable;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
