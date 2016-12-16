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
        #region behavior
        /// <summary>
        /// behavior to control the behavior
        /// </summary>
        private RecorderBehavior RecorderBehavior { get; set; }

        /// <summary>
        /// behavior of the behavior
        /// </summary>
        private VideoBehavior VideoBehavior { get; set; }
        #endregion behavior

        /// <summary>
        /// video device associate
        /// </summary>
        private FilterInfo VideoDevice { get; set; }
        
        /// <summary>
        /// inform if is recording
        /// </summary>
        public bool IsRecording { get; set; }

        /// <summary>
        /// ctor
        /// <para>add behavior on the browser</para>
        /// </summary>
        public CustomVideoElement()
        {
            LanguageController.InitLanguage(this.Resources.MergedDictionaries);
            InitializeComponent();
            BehaviorHelper.AddSingleBehavior(new VideoBrowserBehavior(), BrowserControl);
        }


        #region event
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
        #endregion

        #region public function
        /// <summary>
        /// start the video
        /// <para>get the video device</para>
        /// <para>add video behavior</para>
        /// <para>add timeLagBehavior</para>
        /// <para>add recorder behavior</para>
        /// <para>add </para>
        /// </summary>
        /// <param name="videoDevice"></param>
        public void Start(FilterInfo videoDevice)
        {
            this.VideoDevice = videoDevice;
            
            VideoBehavior = new VideoBehavior(VideoDevice);

            BehaviorHelper.AddSingleBehavior(VideoBehavior, this);
            BehaviorHelper.AddSingleBehavior(new TimeLagBehavior(VideoBehavior), this);
            RecorderBehavior = BehaviorHelper.AddSingleBehavior(new RecorderBehavior(VideoBehavior), this) as RecorderBehavior;
        }

        /// <summary>
        /// stop the video
        /// <para>VideoDevice is set to null</para>
        /// <para>VideoBehavior is close</para>
        /// <para>VideoBehavior is set to null</para>
        /// <para>RecorderBehavior is set to null</para>
        /// </summary>
        public void Stop()
        {
            this.VideoDevice = null;
            VideoBehavior.CloseVideoSource();
            VideoBehavior = null;
            RecorderBehavior = null;
        }
        #endregion
    }
}
