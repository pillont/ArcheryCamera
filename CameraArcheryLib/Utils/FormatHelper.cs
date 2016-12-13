using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CameraArcheryLib.Utils
{
    public class FormatHelper
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
        public static BitmapSource loadBitmap(Bitmap source)
        {
            if (source == null)
                throw new ArgumentNullException();

            BitmapSource bs = null;
            IntPtr ip = source.GetHbitmap();
            bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
                                                                   IntPtr.Zero, Int32Rect.Empty,
            
            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            return bs;
        }
    }
}
