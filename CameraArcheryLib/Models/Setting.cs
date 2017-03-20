using System.IO;
using System.Xml.Serialization;

namespace CameraArcheryLib.Models
{
    public class Setting
    {
        private const string VIDEO_FOLDER_NAME = "\\Video";

        /// <summary>
        /// the number of the future video
        /// </summary>
        [XmlElement(ElementName = "VideoNumber")]
        public int VideoNumber { get; set; }

        /// <summary>
        /// time of the lag in second
        /// </summary>
        [XmlElement(ElementName = "Time")]
        public int Time { get; set; }

        /// <summary>
        /// language of the application
        /// </summary>
        [XmlElement(ElementName = "Language")]
        public LanguageController.Languages Language { get; set; }

        /// <summary>
        /// language of the application
        /// </summary>
        [XmlElement(ElementName = "VideoFolder")]
        public string VideoFolder { get; set; }

        /// <summary>
        /// default ctor to serializations
        /// </summary>
        public Setting()
        {
            Time = 0;
            this.Language = LanguageController.Languages.English;
            VideoFolder = @"" + Directory.GetCurrentDirectory() + VIDEO_FOLDER_NAME;

            // create default folder if not existing
            if (!Directory.Exists(VideoFolder))
                Directory.CreateDirectory(VideoFolder);
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="time">time of lag in second</param>
        /// <param name="language">language</param>
        public Setting(int time, LanguageController.Languages language)
            : this()
        {
            this.Time = time;
            this.Language = language;
        }

        public override string ToString()
        {
            return typeof(Setting).Name + " : Time = " + Time + ", Language = " + Language;
        }

        public static bool operator ==(Setting x, Setting y)
        {
            if (x as object == null && y as object == null)
                return true;
            if ((x as object == null && y as object != null)
                || (x as object != null && y as object == null))
                return false;
            if (x.Time != y.Time)
                return false;
            if (x.Language != y.Language)
                return false;

            return true;
        }

        public static bool operator !=(Setting x, Setting y)
        {
            return !(x == y);
        }
    }
}
