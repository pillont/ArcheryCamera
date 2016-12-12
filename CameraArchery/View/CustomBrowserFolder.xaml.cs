using CameraArcheryLib;
using CameraArcheryLib.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CameraArchery.View
{
    /// <summary>
    /// Interaction logic for CustomBrowserFolder.xaml
    /// </summary>
    public partial class CustomBrowserFolder : Window
    {
        private Uri RootUri { get; set; }
        public CustomBrowserFolder(Uri uri)
        {
            InitializeComponent();

            RootUri = uri;
            RefreshTreeView();
        }

        private MenuItem GetFolder(Uri uri)
        {
            MenuItem root = new MenuItem(uri);

            foreach(var dir in Directory.EnumerateDirectories(uri.OriginalString))
                root.Items.Add(GetFolder(new Uri(dir, UriKind.Relative)));

            return root;
        }


        private string DirectoryName(string dest)
        {
            return dest.Split('\\').Last();
        }

        private void RefreshTreeView()
        {
            if (!Directory.Exists(RootUri.OriginalString))
                Directory.CreateDirectory(RootUri.OriginalString);
            var root = GetFolder(RootUri);

            TreeControl.Items.Refresh();
            TreeControl.Items.Add(root);
            root.IsSelected = true;
            root.IsExpanded = true;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var item = (TreeControl.SelectedItem as MenuItem);
            var InitUri = new Uri(item.Uri.OriginalString + "\\"+ LanguageController.Get("newDirectoryName"), UriKind.Relative);
            var finalUri = new Uri(InitUri.OriginalString, UriKind.Relative);
            

            int index = 0;
            while (Directory.Exists(finalUri.OriginalString))
            {
                index ++;
                finalUri = new Uri(@""+InitUri.OriginalString + "("+index+")", UriKind.Relative);
            }

            Directory.CreateDirectory(finalUri.OriginalString);
            item.Items.Add(new MenuItem(finalUri));
        }
    }


    public class MenuItem
    {
        public Func<string, bool> OnStopEditing { get; set; }
        
        public Uri Uri { get; private set; }

        public bool IsExpanded { get; set; }
        
        public bool IsSelected { get; set; }
        
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
                            MessageBox.Show(LanguageController.Get("fileExisting"),LanguageController.Get("fileExistingCaption"),  MessageBoxButton.OK, MessageBoxImage.Error);
                            return ;
                        }
                        Directory.Move(Uri.OriginalString, newUri.OriginalString);
                        name = value;
                        Uri = newUri;
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error(e);
                        return;
                    }
                }
                
            }
        }
        public string name;
        
        public ObservableCollection<MenuItem> Items { get; set; }
        
        public MenuItem(Uri uri, bool newFolder = false)
        {
            Uri = uri;

            this.Items = new ObservableCollection<MenuItem>();
            IsSelected = false;
            IsExpanded = false;

            // use field to not update the uri
            name = uri.OriginalString.Split('\\').Last();
            OnStopEditing = (arg) => true;            
        }

        
    }
}
