using Accord.Video.VFW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CameraArchery.DataBinding
{
    /// <summary>
    /// element to show a video file in a listView
    /// </summary>
    public class VideoFile 
    {
        /// <summary>
        /// name of the file
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// uri of the file
        /// </summary>
        public string Uri { get; set; }
        
        public override string ToString()
        {
            return this.Name;
        }
    }
}
