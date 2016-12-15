using Accord.Video.FFMPEG;
using CameraArchery.UsersControl;
using CameraArcheryLib.Factories;
using CameraArcheryLib.Interface;
using CameraArcheryLib.Models;
using CameraArcheryLib.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace CameraArchery.Behaviors
{
    /// <summary>
    /// controller to recorder
    /// </summary>
    public class RecorderBehavior : Behavior<CustomVideoElement>
    {
        /// <summary>
        /// extension of the video files
        /// </summary>
        public const string EXTENSION_FILE = ".avi";

        /// <summary>
        /// width of the video files
        /// </summary>
        public const int WIDTH = 320;

        /// <summary>
        /// height of the video files
        /// </summary>
        public const int HEIGHT = 240;

        /// <summary>
        /// behavior of the video element
        /// </summary>
        private IVideoBehavior VideoBehavior { get; set; }
        
        /// <summary>
        /// directory of the videos
        /// </summary>
        private string VideoDirectory
        {
            get
            {
                return SettingFactory.CurrentSetting.VideoFolder;
            }
        }

        /// <summary>
        /// writer of video file
        /// </summary>
        private VideoFileWriter Writer 
        {
            get
            {
                return writer;
            }
            set
            {
                writer = value;

                AssociatedObject.IsRecording = Writer != null; 
            }
        }
        private VideoFileWriter writer;
        private object writerLocker = new object();


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="videoBehavior">video behavior associated</param>
        public RecorderBehavior(IVideoBehavior videoBehavior)
        {
            this.VideoBehavior = videoBehavior;
        }

        #region event
        /// <summary>
        /// on attach event
        /// <para>subscribe event to stop recording when videoElement is close</para>
        /// <para>subscribe event to stop recording when videoElement is unload</para>
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            VideoBehavior.OnVideoClose += () => StopRecording();
            AssociatedObject.Unloaded += (t,e) => StopRecording();
        }

        /// <summary>
        /// function to write each frame in the video file
        /// <para>clone the image</para>
        /// <para>write the frame in the file</para>
        /// </summary>
        /// <param name="refBm">if null, done nothing</param>
        private void VideoController_OnNewFrame(ref Bitmap refBm)
        {
            if (refBm == null)
                return;

            var bm = refBm.Clone() as Bitmap;

            Monitor.Enter(writerLocker);

            if (Writer != null)
                Writer.WriteVideoFrame(bm);

            Monitor.Exit(writerLocker);
        }
        #endregion event

        #region function public
        /// <summary>
        /// start the recording if not already start
        /// </summary>
        /// <returns>in form the action : true => record / false => stop</returns>
        public bool Recording()
        {
            // start recording
            if (!AssociatedObject.IsRecording)
            {
                StartRecording();
                return true;
            }
            //stop recording
            StopRecording();
            return false;
        }
        #endregion function public

        #region private function
        /// <summary>
        /// function to start the recording
        ///<para>get the name of the file</para>
        ///<para>update the video name in the setting</para>
        ///<para>create directory if is not existing</para>
        ///<para>if file exist, restart with upper name</para>
        ///<para>start the writer</para>
        /// </summary>
        private void StartRecording()
        {
           // get name
            var name = VideoDirectory +"\\"+SettingFactory.CurrentSetting.VideoNumber + EXTENSION_FILE;

            //save new value
            SettingFactory.CurrentSetting.VideoNumber++;
            SerializeHelper.Serialization<Setting>(SettingFactory.CurrentSetting, SettingFactory.FilePath);
        
            //create dir
            if (!System.IO.Directory.Exists(VideoDirectory))
                System.IO.Directory.CreateDirectory(VideoDirectory);

            // check if file exist
            // if exist create with number +1
            if (File.Exists(name))
            {
                StartRecording();
                return;
            }
            
            StartWriter(name);

            VideoBehavior.OnNewFrame += VideoController_OnNewFrame;
        }

        /// <summary>
        /// start the writer to recorder video
        /// <para>create instance of video writer</para>
        /// <para>create new video file</para>
        /// </summary>
        /// <param name="uri">full uri of the video file -> must not already exist</param>
        private void StartWriter(string uri)
        {
            Contract.Assert(!File.Exists(uri), "video file already exist");

            Monitor.Enter(writerLocker);
            
            Writer = new VideoFileWriter();
            Writer.Open(uri, WIDTH, HEIGHT, SettingFactory.CurrentSetting.Frame);

            Monitor.Exit(writerLocker);
        }
        
        
        /// <summary>
        /// function to stop the recording
        /// <para>close the <code>Writer</code> and set to null</para>
        /// <para>unsubscribe the new frame event to save frame</para>
        /// </summary>
        private void StopRecording()
        {
            Monitor.Enter(writerLocker);
            if (Writer != null)
            {
                Writer.Close();
                Writer = null;
            }
            Monitor.Exit(writerLocker);

            VideoBehavior.OnNewFrame -= VideoController_OnNewFrame;
        }
        #endregion
    }
}
