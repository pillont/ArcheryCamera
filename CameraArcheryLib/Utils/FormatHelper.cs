using System;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows;

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

        //Somewhere in the class
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// transform the image to adapt in the image content
        /// </summary>
        /// <param name="source">bitmap source</param>
        /// <returns></returns>
        public static BitmapSource loadBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                return null;
            var hBitmap = bitmap.GetHbitmap();
            var res = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(hBitmap); //delete gdi object

            bitmap.Dispose();
            return res;
        }
    }
}
