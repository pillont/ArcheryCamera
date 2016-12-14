using CameraArcheryLib;
using CameraArcheryLib.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CameraArchery.VisualModel
{
    public class CustomMenuItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Func<string, bool> OnStopEditing { get; set; }

        public Uri Uri { get; private set; }

        public bool isExpanded;
        public bool IsExpanded 
        {
            get
            {
                return isExpanded;
            }
            set 
            {
                isExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        public bool isSelected;
        public bool IsSelected 
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");

            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    try
                    {
                        var newUri = new Uri(Directory.GetParent(Uri.OriginalString).FullName + "\\" + value);


                        if (Directory.Exists(newUri.OriginalString))
                        {
                            MessageBox.Show(LanguageController.Get("fileExisting"), LanguageController.Get("fileExistingCaption"), MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }


                        Directory.Move(Uri.OriginalString, newUri.OriginalString);
                        name = value;
                        Uri = newUri;
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error(e);
                        throw e;
                    }
                }

            }
        }
        public string name;

        public ObservableCollection<CustomMenuItem> Items { get; set; }

        public CustomMenuItem(Uri uri)
        {
            Uri = uri;

            this.Items = new ObservableCollection<CustomMenuItem>();
            IsSelected = false;
            IsExpanded = false;

            // use field to not update the uri
            name = uri.OriginalString.Split('\\').Last();
            OnStopEditing = (arg) => true;
        }




        /// <summary>
        /// function to dataBinding
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        /// <summary>
        /// delete item in his child
        /// <para>if contains -> delete and return true</para>
        /// <para>check in all the items and return true if found in the child</para>
        /// </summary>
        /// <param name="folderToDelete"></param>
        /// <returns>return true if found, return false if not found</returns>
        internal bool Delete(CustomMenuItem folderToDelete)
        {
            if (this.Items.Remove(folderToDelete))
                return true;
            
            foreach (var item in Items)
                if (item.Delete(folderToDelete))
                    return true;
                
            return false;
        }
    }
}
