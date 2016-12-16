using Accord.Video.VFW;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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
        public string name;

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

                if (Uri != null)
                {
                    // check if the name is changed
                    var packages = Uri.Split('\\');
                    if (packages.Last() == Name)
                        return;
                    
                    // get new name
                    var newUri = "";
                    foreach (var dir in packages)
                        if(dir != packages.Last())
                            newUri += dir + "\\";
                    newUri += name;
                    
                    // change the name
                    File.Move(Uri, newUri);
                    Uri = newUri;
                }
            }
        }
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
