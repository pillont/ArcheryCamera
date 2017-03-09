using Accord.Video.DirectShow;
using System;
using System.Drawing;

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

        VideoCaptureDevice VideoSource { get; }
    }
}
