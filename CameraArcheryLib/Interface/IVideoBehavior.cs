using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraArcheryLib.Interface
{
    /// <summary>
    /// delegate to show each frame
    /// </summary>
    /// <param name="bm"></param>
    public delegate void newFrameDelegate(ref Bitmap bm);

    public interface IVideoBehavior
    {
        event Action OnVideoClose;
        event newFrameDelegate OnNewFrame;
             
    }
}
