using CameraArchery.DataBinding;
using CameraArcheryLib.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CameraArcheryLib.Controller
{
    /// <summary>
    ///  controller to manage the list of recorded video file
    /// </summary>
    public static class ListRecordController
    {
        /// <summary>
        /// extension of the video files
        /// </summary>
        public const string EXTENSION_FILE = ".avi";

        /// <summary>
        /// get the list of video
        /// <para> check all the file in the directory</para>
        /// <para> check if the file is already existing in the param list</para>
        /// <para> add the file in the new list</para>
        /// </summary>
        /// <param name="list">current list</param>
        /// <returns>list with all the file</returns>
        public static IList<VideoFile> GetList(string videoFolder)
        {
            var res = new List<VideoFile>();

            // get all the existing file
            var videoNames = GetVideoFileNames(videoFolder);

            foreach (var name in videoNames)
            {
                try
                {
                    VideoFile file = new VideoFile()
                    {
                        Name = name.Replace(videoFolder + "\\", ""),
                        Uri = name
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
        /// get all the file names in the video directory
        /// </summary>
        /// <returns>list of the names</returns>
        public static IEnumerable<string> GetVideoFileNames(string videoFolder)
        {
            if (!Directory.Exists(videoFolder))
                return new List<string>();

            return Directory.EnumerateFiles(videoFolder).Where(
                                        (file) => file.EndsWith(ListRecordController.EXTENSION_FILE));
        }
    }
}
