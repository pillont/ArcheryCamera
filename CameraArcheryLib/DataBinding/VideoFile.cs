using Accord.Video.VFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CameraArchery.DataBinding
{
    public class VideoFile 
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public BitmapSource Image { get; set; }
    }
}
