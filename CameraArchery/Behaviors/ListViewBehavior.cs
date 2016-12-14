using Accord.Video.DirectShow;
using CameraArchery.DataBinding;
using CameraArcheryLib.Controller;
using CameraArcheryLib.Factories;
using CameraArcheryLib.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace CameraArchery.Behaviors
{
    /// <summary>
    ///  controller to manage the list of recorded video file
    /// </summary>
    public class ListRecordBehavior : Behavior<ListView>
    {
        /// <summary>
        /// list of the files
        /// </summary>
        public ListBox VideoList { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="videoList"></param>
        public ListRecordBehavior(ListBox videoList)
        {
            this.VideoList = videoList;
        }

        /// <summary>
        /// get the list of video
        /// <para> check all the file in the directory</para>
        /// <para> check if the file is already existing in the param list</para>
        /// <para> add the file in the new list</para>
        /// </summary>
        /// <param name="list">current list</param>
        /// <returns>list with all the file</returns>
        public IList<VideoFile> GetList()
        {
            var res = new List<VideoFile>();

            // get all the existing file
            var videoNames = getFileNames();


            foreach (var name in videoNames)
            {
                try
                {
                    VideoFile file = new VideoFile()
                    {
                        Name = name.Replace(SettingFactory.CurrentSetting.VideoFolder + "\\", ""),
                        FullName = name
                    };

                    // add the file in the list
                    if (file != null)
                        res.Add(file);
                }
                catch (Exception e)
                {
                    LogHelper.Write("video not load");
                    LogHelper.Error(e);
                }
            }
            // order by the number
            res.Reverse();
            return res;
        }




        /// <summary>
        /// function to get the first image of the video
        /// <para> set a reader on the file</para>
        /// <para>get the first image</para>
        /// </summary>
        /// <param name="name">full url string of the file</param>
        /// <returns>the first image of the video file</returns>
        private static Bitmap GetFirstImage(string name)
        {
            FileVideoSource reader = new FileVideoSource(name);

            object locker = new object();

            Bitmap tmp = null;
            reader.NewFrame += (t, e) =>
            // get the first image 
            //close the reader when reading
            {
                tmp = e.Frame;
                Monitor.Enter(locker);
                Monitor.Pulse(locker);
                Monitor.Exit(locker);
                reader.Stop();

            };

            // wait the first image
            Monitor.Enter(locker);
            reader.Start();
            Monitor.Wait(locker, 1000);
            Monitor.Exit(locker);

            return tmp;
        }

        /// <summary>
        /// get all the file names in the video directory
        /// </summary>
        /// <returns>list of the names</returns>
        public IEnumerable<string> getFileNames()
        {
            if (!Directory.Exists(SettingFactory.CurrentSetting.VideoFolder))
                return new List<string>();

            return Directory.EnumerateFiles(SettingFactory.CurrentSetting.VideoFolder).Where(
                                        (file) => file.EndsWith(RecorderController.ExtensionFile));
        }

        /// <summary>
        ///  remove a video file
        ///  <para>stop the media element</para>
        ///  <para> set the source of the mediaElement to null</para>
        ///  <para>remove the file of the list file</para>
        ///  <para>delete the file</para>
        /// </summary>
        /// <param name="MediaElementVideo">mediaElement to view the file</param>
        /// <param name="VideoList">list of the video file</param>
        public void RemoveVideo(MediaElement MediaElementVideo, ListBox VideoList)
        {
            MediaElementVideo.Stop();
            MediaElementVideo.Source = null;
            var file = VideoList.SelectedItem as VideoFile;

            ObservableCollection<VideoFile> list = new ObservableCollection<VideoFile>((VideoList.ItemsSource as IList<VideoFile>));
            list.Remove(file);

            VideoList.ItemsSource = list;

            File.Delete(file.FullName);
            VideoList.SelectedIndex = 0;
        }
    }
    
}