using Accord.Video.DirectShow;
using CameraArchery.DataBinding;
using CameraArcheryLib.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CameraArcheryLib.Controller
{
    /// <summary>
    ///  controller to manage the list of recorded video file
    /// </summary>
    public class ListRecordController
    {
        /// <summary>
        /// list of the files
        /// </summary>
        public ListBox VideoList { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="videoList"></param>
        public ListRecordController(ListBox videoList)
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
        public IList<VideoFile> GetList(IList<VideoFile> list = null)
        {
            var res = new List<VideoFile>();

            // get all the existing file
            var videoNames = getFileNames();
            foreach (var name in videoNames)
            {
                try
                {
                    VideoFile file = null;

                    // check if file is already existing
                    if (list != null)
                        file = list.FirstOrDefault((video) => video.FullName == name);

                    // get the file
                    if (file == default(VideoFile))
                        file = StringToVideoFile(name);

                    // add the file in the list
                    if (file != null)
                        res.Add(file);
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            // order by the number
            res.Reverse();
            return res;
        }

        /// <summary>
        /// generate the video file by the string url
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public VideoFile StringToVideoFile(string name)
        {
            // get the names
            VideoFile val = new VideoFile()
            {
                Name = name.Replace(RecorderController.VideoDirectory, ""),
                FullName = name,
            };

            // get the image
            Bitmap tmp = GetFirstImage(name);
            val.Image = FormatHelper.loadBitmap(tmp);

            return val;
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
            if (!Directory.Exists(RecorderController.VideoDirectory))
                return new List<string>();

            return Directory.EnumerateFiles(RecorderController.VideoDirectory).Where(
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

            List<VideoFile> list = new List<VideoFile>((VideoList.ItemsSource as IList<VideoFile>));
            list.Remove(file);

            VideoList.ItemsSource = list;

            File.Delete(file.FullName);
            VideoList.SelectedIndex = 0;
        }
    }
}
