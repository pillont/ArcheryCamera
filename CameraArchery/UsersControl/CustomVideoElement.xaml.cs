using Accord.Video.DirectShow;
using CameraArchery.Behaviors;
using CameraArcheryLib;
using CameraArcheryLib.Controller;
using CameraArcheryLib.Factories;
using CameraArcheryLib.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CameraArchery.UsersControl
{
    /// <summary>
    /// Interaction logic for CustomVideoElement.xaml
    /// </summary>
    public partial class CustomVideoElement : UserControl
    {

        /// <summary>
        /// behavior to control the behavior
        /// </summary>
        public RecorderBehavior RecorderBehavior;
        public FilterInfo VideoDevice;
        private TimeLagBehavior timeLagController;

        public bool IsRecording { get; set; }


        public CustomVideoElement()
        {
            LanguageController.InitLanguage(this.Resources.MergedDictionaries);
            InitializeComponent();
            this.Loaded +=CustomVideoElement_Loaded;

            BehaviorHelper.AddSingleBehavior(new VideoBrowserBehavior(), BrowserControl);

        }

        private void CustomVideoElement_Loaded(object sender, RoutedEventArgs e)
        {
            var videoBehavior = new VideoBehavior(VideoDevice);

            BehaviorHelper.AddSingleBehavior(videoBehavior, this);
            BehaviorHelper.AddSingleBehavior(new TimeLagBehavior(videoBehavior), this);
            BehaviorHelper.AddSingleBehavior(new RecorderBehavior(videoBehavior), this);
        }

        

 

        /// <summary>
        ///  event of the button of click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Recording_Click(object sender, RoutedEventArgs e)
        {
            LogHelper.Write("click recording");


            var isRecording = RecorderBehavior.Recording();

            if (isRecording)
            {
                LogHelper.Write("start recording");
                (sender as Button).Content = LanguageController.Get("StopRecord");
            }
            else
            {
                LogHelper.Write("stop recording");
                (sender as Button).Content = LanguageController.Get("StartRecord");
            }
        }
    }
}
