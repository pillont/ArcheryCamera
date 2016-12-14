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
        public FilterInfo VideoDevice { get; set; }

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
            VideoDevice = videoDevice;
            LogHelper.Write("--------------- open video view -----------------");
            
            LanguageController.InitLanguage(this.Resources.MergedDictionaries);
            InitializeComponent();
            CustomVideoComponent.VideoDevice = videoDevice;
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
        /// event when the tabControler selected replay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LogHelper.Write("tab control change to : " + MainTabControl.SelectedValue + ". Is recording : " + CustomVideoComponent.IsRecording);

            if (e.Source is TabControl && CustomVideoComponent.IsRecording)
                MainTabControl.SelectedIndex = 0;
       
            if (e.Source is System.Windows.Controls.TabControl)
            {
                // no change
                if (CustomVideoComponent.IsRecording)
                    MainTabControl.SelectedIndex = 0;
                //change
                else
                {
                    // update the value in the folder
                    CustomVideoComponent.BrowserControl.SelectedUri = new Uri(SettingFactory.CurrentSetting.VideoFolder, UriKind.Absolute);
                    CustomReplayComponent.BrowserControl.SelectedUri = new Uri(SettingFactory.CurrentSetting.VideoFolder, UriKind.Absolute);
                }
            }
        }
    }
}
