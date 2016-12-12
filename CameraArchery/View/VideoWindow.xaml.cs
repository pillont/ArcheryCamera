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
<<<<<<< HEAD
        
            CustomReplayComponent.OnStopClick += CustomReplayComponent_OnStopClick;
            CustomReplayComponent.OnStartClick += CustomReplayComponent_OnStartClick;
            CustomReplayComponent.OnSpeedUpClick += CustomReplayComponent_OnSpeedUpClick;
            CustomReplayComponent.OnSpeedDownClick += CustomReplayComponent_OnSpeedDownClick;
            CustomReplayComponent.OnSliderChange += CustomReplayComponent_OnSliderChange;
            CustomReplayComponent.OnSliderCapture += CustomReplayComponent_OnSliderCapture;
            CustomReplayComponent.OnMediaEnded += CustomReplayComponent_OnMediaEnded;
            CustomReplayComponent.OnListSelectionChange += CustomReplayComponent_OnListSelectionChange;
            CustomReplayComponent.OnFrameClick += CustomReplayComponent_OnFrameClick;
            CustomReplayComponent.OnDeleteFile += CustomReplayComponent_OnDeleteFile;
        }

        private bool CustomReplayComponent_OnDeleteFile(DataBinding.VideoFile arg)
        {
            LogHelper.Write("delete file "+ arg.FullName);
            return true;
        }

        private bool CustomReplayComponent_OnFrameClick()
        {
            LogHelper.Write("on frame click");
            return true;
        }

        private void CustomReplayComponent_OnListSelectionChange(DataBinding.VideoFile obj)
        {
            LogHelper.Write("selected file change : " + obj.FullName);
        }

        private void CustomReplayComponent_OnMediaEnded()
        {
            LogHelper.Write("media is ended");
        }

        private void CustomReplayComponent_OnSliderCapture(double obj)
        {
            LogHelper.Write("slider is capture at " + obj +" sec");
        }

        private bool CustomReplayComponent_OnSliderChange(double arg)
        {
            LogHelper.Write("slider is change to " + arg + " sec");
            return true;
        }

        private bool CustomReplayComponent_OnSpeedDownClick(double arg)
        {
            LogHelper.Write("speed down : " + arg);
            return true;
        }

        private bool CustomReplayComponent_OnSpeedUpClick(double arg)
        {
            LogHelper.Write("speed up : " + arg);
            return true;    
        }

        private bool CustomReplayComponent_OnStartClick(bool isStart, bool isPause)
        {
            if(!isStart)
            LogHelper.Write("start click");
            else if (isPause)
                    LogHelper.Write("reply click");
            else
                LogHelper.Write("Pause click");

            return true;
        }

        private bool CustomReplayComponent_OnStopClick()
        {
            LogHelper.Write("stop click");
            return true;
=======

<<<<<<< HEAD
            BrowserControl.DefaultFile = SettingFactory.CurrentSetting.VideoFolder; 
>>>>>>> selected and return value on the browser ok
=======
>>>>>>> same value in the two browsers
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
            LogHelper.Write("click recording");

            var isRecording = videoController.Recording();

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
<<<<<<< HEAD
<<<<<<< HEAD
            LogHelper.Write("tab control change to : "+MainTabControl.SelectedValue+ ". Is recording : " + videoController.recorderController.IsRecording);

            if (e.Source is TabControl && videoController.recorderController.IsRecording)
=======
            if (e.Source is System.Windows.Controls.TabControl && videoController.recorderController.IsRedording)
>>>>>>> create folders ok
                MainTabControl.SelectedIndex = 0;
=======
            if (e.Source is System.Windows.Controls.TabControl)
            {
                // no change
                if (videoController.recorderController.IsRedording)
                    MainTabControl.SelectedIndex = 0;
                //change
                else
                {
                    // update the value in the folder
                    BrowserControl.SelectedUri = SettingFactory.CurrentSetting.VideoFolder;
                    Replay.BrowserControl.SelectedUri = SettingFactory.CurrentSetting.VideoFolder;
                }
            }
>>>>>>> same value in the two browsers
        }
    }
}
