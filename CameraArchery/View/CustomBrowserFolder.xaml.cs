using CameraArchery.VisualModel;
using CameraArcheryLib;
using CameraArcheryLib.Controller;
using CameraArcheryLib.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
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

        private bool IsToSave { get; set; }
        public Uri InitUri { get; set; }
        public Uri SelectedUri { get; set; }
        private Uri RootUri { get; set; }

        public CustomBrowserFolder(Uri rootUri, Uri initUri)
        {
            InitializeComponent();

            IsToSave = false;
            RootUri = rootUri;
            InitUri = initUri;
            SelectedUri = initUri;
            RefreshTreeView();
        }

        private CustomMenuItem GetFolder(Uri uri)
        {
            CustomMenuItem root = new CustomMenuItem(uri) 
            {   
                IsSelected = (uri == SelectedUri) , 
                isExpanded = (SelectedUri.OriginalString.StartsWith(uri.OriginalString)) 
            };

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
            root.IsExpanded = true;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var treeItem = (TreeControl.SelectedItem as CustomMenuItem);
            var InitUri = new Uri(treeItem.Uri.OriginalString + "\\"+ LanguageController.Get("newDirectoryName"), UriKind.Relative);
            var finalUri = new Uri(InitUri.OriginalString, UriKind.Relative);
            

            int index = 0;
            while (Directory.Exists(finalUri.OriginalString))
            {
                index ++;
                finalUri = new Uri(@""+InitUri.OriginalString + "("+index+")", UriKind.Relative);
            }

            Directory.CreateDirectory(finalUri.OriginalString);
            SelectedUri = finalUri;
         
            // change the current item
            var newItem = new CustomMenuItem(finalUri) { IsSelected = true };
            treeItem.Items.Add(newItem);
            treeItem.IsSelected = false;
            treeItem.IsExpanded = true;            
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            IsToSave = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (TreeControl.SelectedItem == null)
            {
                MessageBox.Show("URI NULL ");
                e.Cancel = true;
            }
            else if (IsToSave)
                this.SelectedUri = (TreeControl.SelectedItem as CustomMenuItem).Uri;
        }
    }
}
