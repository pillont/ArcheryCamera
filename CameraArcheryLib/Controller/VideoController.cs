using Accord.Video;
using Accord.Video.DirectShow;
using AviFile;
using CameraArcheryLib.Factories;
using CameraArcheryLib.Models;
using CameraArcheryLib.Utils;
using SharpAvi.Output;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CameraArcheryLib.Controller
{
    /// <summary>
    ///  Controller to show the video
    /// </summary>
    public class VideoController
    {
        /// <summary>
        /// controller to record the video
        /// </summary>
        public RecorderController recorderController { get; set; }

        
        /// <summary>
        /// delegate to show each frame
        /// </summary>
        /// <param name="bm"></param>
        public delegate void newFrameDelegate(ref Bitmap bm);

        /// <summary>
        /// event when the video is close
        /// </summary>
        public event Action OnVideoClose;
        
        /// <summary>
        /// event when new frame is capture
        /// </summary>
        public event newFrameDelegate OnNewFrame;

        /// <summary>
        /// camera device
        /// </summary>
        private FilterInfo videoDevice { get;  set; }

        /// <summary>
        /// function to show each frame
        /// </summary>
        private Action<Bitmap> showImage { get; set; }

        /// <summary>
        ///  source of the video
        /// </summary>
        private VideoCaptureDevice videoSource { get; set; }
        
        /// <summary>
        /// ctor
        /// init the Controllers
        /// start the video
        /// </summary>
        /// <param name="showImageDel">function to show each images in the view</param>
        /// <param name="videoDevices">device to take the video</param>
        public VideoController(Action<Bitmap> showImageDel,  FilterInfo videoDevices)
        {
            this.recorderController = new RecorderController();
            this.videoDevice = videoDevices;
            this.showImage = showImageDel;

            StartVideo();
        }

        /// <summary>
        /// start the recording if not already start
        /// </summary>
        /// <returns>in form the action : true => record / false => stop</returns>
        public bool Recording()
        {
            // start recording
            if (!recorderController.IsRedording)
            {
                recorderController.StartRecording();
                OnNewFrame += recorderController.VideoController_OnNewFrame;
                return true;
            }
            //stop recording
            recorderController.StopRecording();
            return false;
        }

        /// <summary>
        /// start the video
        /// set handler foreach image taking
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <returns></returns>
        private void StartVideo()
        {
            LogHelper.Write("video start");
            
            videoSource = new VideoCaptureDevice(videoDevice.MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            CloseVideoSource();
            //videoSource.DesiredFrameSize = new Size(1600, 1200);
            //videoSource.DesiredFrameRate = SettingFactory.getCurrent.Frame;
            videoSource.Start();
        }

        /// <summary>
        /// eventhandler if new frame is ready
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // save new frame
            Bitmap img = (Bitmap)eventArgs.Frame.Clone();

            NewFraming(ref img);
            if( img != null)
                showImage(img);
        }

        /// <summary>
        /// event during the capture of a new frame
        /// </summary>
        /// <param name="img"></param>
        private void NewFraming(ref Bitmap img)
        {
            if(OnNewFrame != null)
                OnNewFrame(ref img);
        }

        /// <summary>
        /// close the device safely
        /// </summary>
        public void CloseVideoSource()
        {
            if(OnVideoClose != null)
                OnVideoClose();
        
            if (!(videoSource == null))
                if (videoSource.IsRunning)
                {
                    recorderController.StopRecording();
                    LogHelper.Write("stop the video");
            
                    videoSource.SignalToStop();
                    videoSource.Source = null;
                }
        }


        /// <summary>
        /// close the video
        /// </summary>
        /// <returns></returns>
        private bool CloseVideo()
        {
            if (videoSource.IsRunning)
            {
                CloseVideoSource();
                return true;
            }
            return false;
        }
    }
}
