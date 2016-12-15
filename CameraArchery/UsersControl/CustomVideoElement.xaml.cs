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

        private VideoBehavior VideoBehavior;
        public CustomVideoElement()
        {
            LanguageController.InitLanguage(this.Resources.MergedDictionaries);
            InitializeComponent();
            BehaviorHelper.AddSingleBehavior(new VideoBrowserBehavior(), BrowserControl);

        }

        public void Start(FilterInfo videoDevice)
        {
            this.VideoDevice = videoDevice;
            
            VideoBehavior = new VideoBehavior(VideoDevice);

            BehaviorHelper.AddSingleBehavior(VideoBehavior, this);
            BehaviorHelper.AddSingleBehavior(new TimeLagBehavior(VideoBehavior), this);

            var behaviors = Interaction.GetBehaviors(this);
            if (behaviors.OfType<RecorderBehavior>().ToList().Count == 0)
            {
                RecorderBehavior = new RecorderBehavior(VideoBehavior);
                behaviors.Add(RecorderBehavior);    
            }
        }


        public void Stop()
        {
            this.VideoDevice = null;
            VideoBehavior.CloseVideoSource();
            VideoBehavior = null;
            RecorderBehavior = null;
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
