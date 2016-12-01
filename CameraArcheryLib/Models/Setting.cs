﻿using CameraArcheryLib.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CameraArcheryLib.Models
{
    public class Setting
    {
        /// <summary>
        /// the number of the future video
        /// </summary>
        [XmlElement(ElementName = "RateFrame")]
        public int Frame { get; set; }
        
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
        /// default ctor to serializations
        /// </summary>
        public Setting()
        {
            Time = 0;
            Frame = 1;
            this.Language = LanguageController.Languages.English;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="time">time of lag in second</param>
        /// <param name="language">language</param>
        public Setting(int time, LanguageController.Languages language, int frame)
            :base()
        {
            this.Time = time;
            this.Language = language;
            this.Frame = frame;
        }

        public override string ToString()
        {
            return typeof(Setting).Name + " : Time = " + Time + ", Language = " + Language; 
        }

        public static bool operator ==(Setting x, Setting y)
        {
            if (x as object == null && y as object == null)
                return true;
            if((x as object == null && y as object != null)
                || (x as object != null && y as object == null))
                return false;
            if (x.Time != y.Time)
                return false;
            if (x.Language != y.Language)
                return false;
            if (x.Frame!= y.Frame)
                return false;

            return true;
        }

        public static bool operator !=(Setting x, Setting y)
        {
            return !(x == y);
        }
    }
}