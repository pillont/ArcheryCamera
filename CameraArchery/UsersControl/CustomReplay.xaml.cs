using CameraArchery.DataBinding;
using CameraArcheryLib;
using CameraArcheryLib.Controller;
using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;

namespace CameraArchery.UsersControl
{
    /// <summary>
    /// Interaction logic for CustomReplay.xaml
    /// </summary>
    public partial class CustomReplay : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {

        private Uri startPauseUri;

        public Uri StartPauseUri
        {
            get
            {
                return startPauseUri;
            }
            set
            {
                startPauseUri = value;
                OnPropertyChanged("StartPauseUri");
            }
        }

        /// <summary>
        /// inform if the vieo is in frame by frame or not
        /// </summary>
        public bool IsFrameByFrame
        {
            get
            {
                return ReplayController.IsFrameByFrame;
            }
            set
            {
                ReplayController.IsFrameByFrame = value;
                OnPropertyChanged("IsFrameByFrame");

                if (!value && ReplayController.isStart)
                    ReplayController.Play();
            }
        }


        /// <summary>
        /// replay Controller to manage the replay
        /// </summary>
        private ReplayController ReplayController { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public CustomReplay()
        {
            InitializeComponent();
            this.DataContext = this;
            StartPauseUri = new Uri("pack://application:,,,/Ressources/Images/play.png");

            ReplayController = new ReplayController(MediaElementVideo, TimeSlider, lblStatus, VideoList); 
            ReplayController.OnStart += OnStartVideo; 
            ReplayController.OnStop += OnStopVideo;

            Refresh();
        }



        /// <summary>
        /// event when the video is start
        /// </summary>
        private void OnStartVideo()
        {
            CheckButton.IsEnabled = true;
        }

        /// <summary>
        /// event when the video is stop
        /// </summary>
        private void OnStopVideo()
        {
            CheckButton.IsEnabled = false;
        }
        /// <summary>
        /// refresh the replay view
        /// </summary>
        public void Refresh()
        {
            ReplayController.Stop();
            RefreshList();
        }

        /// <summary>
        /// refresh the list of files
        /// </summary>
        /// <param name="list">current file in the list</param>
        private void RefreshList(IList<VideoFile> list = null)
        {
            try
            {
                VideoList.ItemsSource = ReplayController.ListRecordController.GetList(list);
            }
            catch(Exception e)
            {
                var res = MessageBox.Show(LanguageController.Get("VideoFileException"), LanguageController.Get("VideoFileNotFound"), MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (res == DialogResult.OK)
                    RefreshList();
                else
                    Environment.Exit(-1);
            }
            VideoList.SelectedIndex = 0;
        }

        /// <summary>
        /// event to start the file selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsFrameByFrame)
                return;

            //start
            if (!ReplayController.isStart)
            {
                ReplayController.Start();
                StartPauseUri = new Uri("pack://application:,,,/Ressources/Images/pause.png");
            }
            //continue
            else if (ReplayController.IsPause)
            {
                ReplayController.Play();
                StartPauseUri = new Uri("pack://application:,,,/Ressources/Images/pause.png");
            }
            //pause
            else
            {
                ReplayController.Pause();
                StartPauseUri = new Uri("pack://application:,,,/Ressources/Images/play.png");
            }
        }

        /// <summary>
        /// stop the current replay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StartPauseUri = new Uri("pack://application:,,,/Ressources/Images/play.png");

            ReplayController.Stop();
        }

        /// <summary>
        /// pause or replay the current video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PauseReplay_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        /// <summary>
        /// event when file selected change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Stop_Click(sender, e);

            if (VideoList.SelectedValue != null)
                ReplayController.LoadVideoFile();
        }

        /// <summary>
        /// event when replay is finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaElementVideo_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsFrameByFrame)
                Stop_Click(sender, e);
        }

        /// <summary>
        /// event when the value change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (TimeSlider.IsMouseCaptured)
                MediaElementVideo.Position = new TimeSpan(0, 0, Convert.ToInt32(e.NewValue));
        }

        /// <summary>
        /// event when the slider got mouse capture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeSlider_GotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (TimeSlider.IsFocused && MediaElementVideo.Source != null)
                MediaElementVideo.Pause();
        }

        /// <summary>
        /// event when the slider lost mouse capture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeSlider_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!ReplayController.IsPause && MediaElementVideo.Source != null)
                MediaElementVideo.Play();
        }

        /// <summary>
        /// event when the list of files is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoList_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            RefreshList((IList<VideoFile>)VideoList.ItemsSource);
        }

        /// <summary>
        /// event when the component is unloaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ReplayController.StopTimer();

        }

        /// <summary>
        /// event when the component is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ReplayController.StartTimer();
        }

        /// <summary>
        /// event to speed down the video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeedDown_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ReplayController.SpeedDown();
            RefreshSpeedLabel();
        }

        /// <summary>
        /// eve,t to speed up the video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeedUp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ReplayController.SpeedUp();
            RefreshSpeedLabel();
        }

        /// <summary>
        /// get the current time of the mediaElement and show it
        /// </summary>
        private void RefreshSpeedLabel()
        {
            var time = Convert.ToInt32(ReplayController.SpeedRatio * 100);
            
            // more slowly
            if(time == 0)
                time ++;
            // if stop
            if (time == -10)
                time = 0;

            SpeedLabel.Content = time.ToString() + "%";
        }

        /// <summary>
        /// event to start the frame by frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frame_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ReplayController.FrameByFrameSetup();
        }

        private void DeleteItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ReplayController.ListRecordController.RemoveVideo(MediaElementVideo, VideoList);
        }

        public event PropertyChangedEventHandler  PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        }
     }
}
