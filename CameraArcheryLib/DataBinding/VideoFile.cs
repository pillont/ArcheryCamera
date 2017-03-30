using CameraArcheryLib;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace CameraArchery.DataBinding
{
    /// <summary>
    /// element to show a video file in a listView
    /// </summary>
    public class VideoFile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// name of the file
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("StartPauseUri");
            }
        }

        private string name;

        /// <summary>
        /// uri of the file
        /// </summary>
        public string Uri { get; set; }

        public bool IsEditing
        {
            get
            {
                return isEditing;
            }
            set
            {
                isEditing = value;
                OnPropertyChanged("StartPauseUri");

                if (Uri == null)
                    return;

                // check if the name is changed
                var packages = Uri.Split('\\');
                if (packages.Last() == Name)
                    return;

                // get new name
                var newUri = "";
                foreach (var dir in packages)
                    if (dir != packages.Last())
                        newUri += dir + "\\";
                newUri += name;

                ChangeName?.Invoke(this, newUri);
            }
        }

        public Action<VideoFile, string> ChangeName { get; set; }

        private bool isEditing;

        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// function to dataBinding
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
