using Accord.Video.FFMPEG;
using CameraArcheryLib.Factories;
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
using System.Windows.Media;

namespace CameraArcheryLib.Controller
{
    /// <summary>
    /// controller to recorder
    /// </summary>
    public class RecorderController
    {
        /// <summary>
        /// writer of video file
        /// </summary>
        private VideoFileWriter Writer { get;set; }
        private object writerLocker = new object();

        /// <summary>
        /// extension of the video files
        /// </summary>
        public const string ExtensionFile = ".avi";

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
        /// width of the video files
        /// </summary>
        public const int Width = 320;
        
        /// <summary>
        /// height of the video files
        /// </summary>
        public const int Height = 240;

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

        /// <summary>
        /// function to start the recording
        /// </summary>
        public void StartRecording()
        {
           // get name
            var name = VideoDirectory +"\\"+SettingFactory.CurrentSetting.VideoNumber + ExtensionFile;

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
            Writer.Open(name, Width, Height, SettingFactory.CurrentSetting.Frame);
        
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
            
            if(Writer != null)
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
    }
}
