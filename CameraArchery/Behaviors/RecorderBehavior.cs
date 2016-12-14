using Accord.Video.FFMPEG;
using CameraArchery.UsersControl;
using CameraArcheryLib.Factories;
using CameraArcheryLib.Interface;
using CameraArcheryLib.Models;
using CameraArcheryLib.Utils;
using System;
using System.Collections.Generic;
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
    public class RecorderBehavior : Behavior<CustomVideoImage>
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

        private IVideoBehavior VideoBehavior;
        
        /// <summary>
        /// directory of the videos
        /// </summary>
        public string VideoDirectory
        {
            get
            {
                return SettingFactory.CurrentSetting.VideoFolder;
            }
        }

        /// <summary>
        /// writer of video file
        /// </summary>
        internal VideoFileWriter Writer { get; set; }
        internal object writerLocker = new object();

        /// <summary>
        /// inform if the controller is recodring or not
        /// </summary>
        public bool IsRecording
        {
            get
            {
                Monitor.Enter(writerLocker);
                var res = Writer != null;
                Monitor.Exit(writerLocker);

                return res;
            }
        }


        protected override void OnAttached()
        {
            base.OnAttached();
            
            AssociatedObject.Unloaded += (t,e) => StopRecording();
        }

        public RecorderBehavior(IVideoBehavior videoBehavior)
        {
            this.VideoBehavior = videoBehavior;
            VideoBehavior.OnVideoClose += () => StopRecording();
        
        }
        /// <summary>
        /// function to start the recording
        /// </summary>
        public void StartRecording()
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

         
            Monitor.Enter(writerLocker);
            
            // create instance of video writer
            Writer = new VideoFileWriter();

            // create new video file
            Writer.Open(name, WIDTH, HEIGHT, SettingFactory.CurrentSetting.Frame);

            Monitor.Exit(writerLocker);
        }
        
        /// <summary>
        /// function to write each frame in the video file
        /// </summary>
        /// <param name="refBm"></param>
        public void VideoController_OnNewFrame(ref Bitmap refBm)
        {
            if (refBm == null)
                return;

            // get the frame
            var bm = refBm.Clone() as Bitmap;

            // write the frame
            Monitor.Enter(writerLocker);

            if (Writer != null)
                Writer.WriteVideoFrame(bm);

            Monitor.Exit(writerLocker);
        }
        
        /// <summary>
        /// function to stop the recording
        /// </summary>
        public void StopRecording()
        {
            Monitor.Enter(writerLocker);
            if (Writer != null)
            {
                Writer.Close();
                Writer = null;
            }
            Monitor.Exit(writerLocker);
        }

        /// <summary>
        /// start the recording if not already start
        /// </summary>
        /// <returns>in form the action : true => record / false => stop</returns>
        public bool Recording()
        {
            // start recording
            if (!IsRecording)
            {
                StartRecording();
                VideoBehavior.OnNewFrame += VideoController_OnNewFrame;
                return true;
            }
            //stop recording
            StopRecording();
            return false;
        }
    }
}
