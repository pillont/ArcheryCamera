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
        private TimeLagController timeLagController;

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
 
            var behaviors =Interaction.GetBehaviors(this);
            if(behaviors.OfType<RecorderBehavior>().ToList().Count == 0)
            {
                // new recorder Controller
                RecorderBehavior = new RecorderBehavior(videoBehavior);
                behaviors.Add(RecorderBehavior);
                // add lag on the video Controller
                videoBehavior.OnNewFrame += (ref Bitmap img) => img = timeLagController.OnNewFrame(img);
                videoBehavior.OnVideoClose += () => timeLagController.Clear();

//TODO use behaviorHelper

                // init the time lag Controller
                timeLagController = new TimeLagController();
                timeLagController.lagLoadFeedBackController.OnProgressChange = (db) => Dispatcher.Invoke(() => ProgressBar.Value = db);
                timeLagController.lagLoadFeedBackController.OnVisibilityChange = OnVisibilityChange;

                // update the view with the time lag Controller
                ProgressBar.Minimum = 0;
                ProgressBar.Maximum = SettingFactory.CurrentSetting.Time;
                ProgressBar.DataContext = timeLagController;
            }
        }

        /// <summary>
        /// event when visibility change
        /// </summary>
        /// <param name="vs"></param>
        private void OnVisibilityChange(Visibility vs)
        {
            Dispatcher.Invoke(() =>
            {
                ProgressBar.Visibility = vs;

                if (vs == Visibility.Visible)
                {
                    myPopup.IsOpen = true;
                    ButtonPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    myPopup.IsOpen = false;
                    ButtonPanel.Visibility = Visibility.Visible;
                }
            });
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
