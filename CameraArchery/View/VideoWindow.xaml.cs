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
using System.Windows.Interactivity;
using CameraArchery.Behaviors;

namespace CameraArchery.View
{
    /// <summary>
    /// Interaction logic for VideoWindow.xaml
    /// </summary>
    public partial class VideoWindow : Window
    {
        /// <summary>
        /// Controller of the time lag
        /// </summary>
        private TimeLagController timeLagController;

        private RecorderBehavior RecorderBehavior;

        /// <summary>
        /// ctor
        /// <para>init the language</para>
        /// <para>init the conponent</para>
        /// <para>init the video Controller</para>
        /// <para>init the time lag Controller</para>
        /// <para>connect the VideoBrowserBehavior and LogReplayBehavior</para>
        /// </summary>
        /// <param name="videoDevices">video device</param>
        public VideoWindow(FilterInfo videoDevice)
        {
            LogHelper.Write("--------------- open video view -----------------");
            
            LanguageController.InitLanguage(this.Resources.MergedDictionaries);
            InitializeComponent();

            // new view Controller
            var videoBehavior = new VideoBehavior(videoDevice);
            Interaction.GetBehaviors(PictureVideo).Add(videoBehavior);

            // new recorder Controller
            RecorderBehavior = new RecorderBehavior(videoBehavior);
            Interaction.GetBehaviors(PictureVideo).Add(RecorderBehavior);

            // init the time lag Controller
            timeLagController = new TimeLagController();
            timeLagController.lagLoadFeedBackController.OnProgressChange = (db) => Dispatcher.Invoke(() =>  ProgressBar.Value = db);
            timeLagController.lagLoadFeedBackController.OnVisibilityChange = OnVisibilityChange;
            
            // update the view with the time lag Controller
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = SettingFactory.CurrentSetting.Time;
            ProgressBar.DataContext = timeLagController;

            // add lag on the video Controller
            videoBehavior.OnNewFrame += (ref Bitmap img) => img = timeLagController.OnNewFrame(img);
            videoBehavior.OnVideoClose += () => timeLagController.Clear();
        
            Interaction.GetBehaviors(BrowserControl).Add(new VideoBrowserBehavior());
            Interaction.GetBehaviors(CustomReplayComponent).Add(new LogReplayBehavior());

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
        
        /// <summary>
        /// event when the tabControler selected replay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LogHelper.Write("tab control change to : "+MainTabControl.SelectedValue+ ". Is recording : " + RecorderBehavior.IsRecording);

            if (e.Source is TabControl && RecorderBehavior.IsRecording)
                MainTabControl.SelectedIndex = 0;
       
            if (e.Source is System.Windows.Controls.TabControl)
            {
                // no change
                if (RecorderBehavior.IsRecording)
                    MainTabControl.SelectedIndex = 0;
                //change
                else
                {
                    // update the value in the folder
                    BrowserControl.SelectedUri = new Uri(SettingFactory.CurrentSetting.VideoFolder, UriKind.Absolute);
                    CustomReplayComponent.BrowserControl.SelectedUri = new Uri(SettingFactory.CurrentSetting.VideoFolder, UriKind.Absolute);
                }
            }
        }
    }
}
