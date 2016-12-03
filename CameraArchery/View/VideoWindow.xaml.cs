using CameraArcheryLib.Factories;
using CameraArcheryLib.Utils;
using CameraArcheryLib.Controller;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CameraArcheryLib;
using Accord.Video.DirectShow;

namespace CameraArchery.View
{
    /// <summary>
    /// Interaction logic for VideoWindow.xaml
    /// </summary>
    public partial class VideoWindow : Window
    {
        /// <summary>
        /// Controller to show the video
        /// </summary>
        private VideoController videoController;

        /// <summary>
        /// Controller of the time lag
        /// </summary>
        private TimeLagController timeLagController;

        /// <summary>
        /// ctor
        /// <para>init the language</para>
        /// <para>init the conponent</para>
        /// <para>init the video Controller</para>
        /// <para>init the time lag Controller</para>
        /// <para>connect the Controller to the view</para>
        /// <para>connect the lag Controller to the video Controller</para>
        /// </summary>
        /// <param name="videoDevices">video device</param>
        public VideoWindow(FilterInfo videoDevice)
        {
            LogHelper.Write("--------------- open video view -----------------");
            LanguageController.InitLanguage(this.Resources.MergedDictionaries);

            InitializeComponent();

            // new view Controller
            videoController = new VideoController(ShowImage, videoDevice);
            

            // init the time lag Controller
            timeLagController = new TimeLagController();
            timeLagController.lagLoadFeedBackController.OnProgressChange = (db) => Dispatcher.Invoke(() =>  ProgressBar.Value = db);
            timeLagController.lagLoadFeedBackController.OnVisibilityChange = OnVisibilityChange;
            
            // update the view with the time lag Controller
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = SettingFactory.CurrentSetting.Time;
            ProgressBar.DataContext = timeLagController;

            // add lag on the video Controller
            videoController.OnNewFrame += (ref Bitmap img) => img = timeLagController.OnNewFrame(img);
            videoController.OnVideoClose += () => timeLagController.Clear();
        }

        /// <summary>
        /// event to show each image
        /// </summary>
        /// <param name="bm">image</param>
        private void ShowImage(Bitmap bm)
        {
            try
            {
                Dispatcher.Invoke(() =>pictureBox1.Source = FormatHelper.loadBitmap(bm));
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
                Dispatcher.Invoke(() =>
                    new CustomMessageBox("Error", "FrameError", e.Message).ShowDialog());
                this.Close();
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
        /// prevent sudden close while device is running
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {

            videoController.recorderController.StopRecording();
            videoController.CloseVideoSource();
            LogHelper.Write("------------- video window close -------------");
        }

        /// <summary>
        /// event when key is press
        /// <para>if the key is escape, close the windows</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var key = e.Key;
            LogHelper.Write("video window key down : "+key);

            if(key == Key.Escape)
                this.Close();
        }

        /// <summary>
        ///  event of the button of click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Recording_Click(object sender, RoutedEventArgs e)
        {
            var isRecording = videoController.Recording();

            (sender as Button).Content = isRecording ?
                LanguageController.Get("StopRecord") :
                LanguageController.Get("StartRecord");

        }    
        
        /// <summary>
        /// event when the tabControler selected replay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl && videoController.recorderController.IsRedording)
                MainTabControl.SelectedIndex = 0;
        }
    }
}
