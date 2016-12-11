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
        public CustomBrowserFolder(string uri)
        {
            InitializeComponent();

            if (!Directory.Exists(uri))
                Directory.CreateDirectory(uri);
            var root = GetFolder(uri);
            TreeControl.Items.Add(root);
            root.IsSelected = true;
            root.IsExpanded = true;
            
        }

        private MenuItem GetFolder(string uri)
        {
            MenuItem root = new MenuItem(){ Title = uri.Split('\\').Last() };

            foreach(var dir in Directory.EnumerateDirectories(uri))
                root.Items.Add(GetFolder(dir));

            return root;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }


    public class MenuItem
    {
        public bool IsExpanded { get; set; }
        public bool IsSelected { get; set; }
        public string Title { get; set; }
        public ObservableCollection<MenuItem> Items { get; set; }
        public MenuItem()
        {
            this.Items = new ObservableCollection<MenuItem>();
            IsSelected = false;
            IsExpanded = false;
        }

    }
}
