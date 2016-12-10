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



            TreeControl.Items.Add(GetFolder(uri));

        }

        private MenuItem GetFolder(string uri)
        {
            MenuItem root = new MenuItem(){ Title = uri.Split('\\').Last() };

            foreach(var dir in Directory.EnumerateDirectories(uri))
                root.Items.Add(GetFolder(dir));

            return root;
        }
    }


    public class MenuItem
    {
        public string Title { get; set; }
        public ObservableCollection<MenuItem> Items { get; set; }
        public MenuItem()
        {
            this.Items = new ObservableCollection<MenuItem>();
        }

    }
}
