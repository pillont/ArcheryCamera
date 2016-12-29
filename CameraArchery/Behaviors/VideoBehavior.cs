using Accord.Video;
using Accord.Video.DirectShow;
using CameraArchery.View;
using CameraArcheryLib.Controller;
using CameraArcheryLib.Factories;
using CameraArcheryLib.Models;
using CameraArcheryLib.Utils;
using SharpAvi.Output;
using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interactivity;
using CameraArchery.UsersControl;
using CameraArcheryLib.Interface;


namespace CameraArchery.Behaviors
{
    /// <summary>
    ///  Controller to show the video
    /// </summary>
    public class VideoBehavior : Behavior<CustomVideoElement>, IVideoBehavior
    {
        #region event
        /// <summary>
        /// event when the video is close
        /// </summary>
        public event Action OnVideoClose;

        /// <summary>
        /// event when new frame is capture
        /// </summary>
        public event newFrameDelegate OnNewFrame;
        #endregion event

        /// <summary>
        /// camera device
        /// </summary>
        private FilterInfo videoDevice { get;  set; }

        /// <summary>
        ///  source of the video
        /// </summary>
        private VideoCaptureDevice VideoSource
        {
            get
            { return videoSource; }
            set
            { videoSource = value; }
        }
        private VideoCaptureDevice videoSource;


        /// <summary>
        /// ctor
        /// init the Controllers
        /// start the video
        /// </summary>
        /// <param name="showImageDel">function to show each images in the view</param>
        /// <param name="videoDevices">device to take the video</param>
        public VideoBehavior(FilterInfo videoDevices)
        {
            this.videoDevice = videoDevices;
        }


        #region event
        
        /// <summary>
        /// event on the attack of the associated object
        /// <para>VideoSource is not null => start the video</para>
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            if (VideoSource == null)
                StartVideo();

            Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive).Closed += VideoBehavior_Closed;
        }

        /// <summary>
        /// event to show each image
        /// </summary>
        /// <param name="bm">image to show</param>
        private void ShowImage(Bitmap bm)
        {
            try
            {
                Dispatcher.Invoke(() =>
                    AssociatedObject.PictureVideo.Source = FormatHelper.loadBitmap(bm));
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                Dispatcher.Invoke(() =>
                    new CustomMessageBox("Error", "FrameError", e.Message).ShowDialog());

                Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive).Close();
            }
        }
    
        /// <summary>
        /// event when videoBehavior is closed
        /// <para>close the videoSource</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoBehavior_Closed(object sender, EventArgs e)
        {
            CloseVideoSource();
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
            
            VideoSource = new VideoCaptureDevice(videoDevice.MonikerString);
            VideoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            CloseVideoSource();
            VideoSource.Start();
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
            eventArgs.Frame.Dispose();

            NewFraming(ref img);
            if( img != null)
                ShowImage(img);
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
        #endregion

        #region public functions
        /// <summary>
        /// close the device safely
        /// </summary>
        public void CloseVideoSource()
        {
            if (!(VideoSource == null))
                if (VideoSource.IsRunning)
                {
                    if (OnVideoClose != null)
                        OnVideoClose();
        
                    
                    LogHelper.Write("stop the video");
            
                    VideoSource.SignalToStop();
                    VideoSource.Source = null;
                }
        }
        #endregion public functions

        #region private functions
        /// <summary>
        /// close the video
        /// </summary>
        /// <returns></returns>
        private bool CloseVideo()
        {
            if (VideoSource.IsRunning)
            {
                CloseVideoSource();
                return true;
            }
            return false;
        }
        #endregion private functions
    }
}
