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
        /// <summary>
        /// uri of the play image
        /// </summary>
        public const string URI_PLAY = "pack://application:,,,/Ressources/Images/play.png";

        /// <summary>
        /// uri of the pause image
        /// </summary>
        public const string URI_PAUSE = "pack://application:,,,/Ressources/Images/pause.png";

        /// <summary>
        /// event to dataBinding
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Uri of the start/pause image
        /// </summary>
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
        private Uri startPauseUri;
        
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
            
            StartPauseUri = new Uri(URI_PLAY);

            ReplayController = new ReplayController(MediaElementVideo, TimeSlider, lblStatus, VideoList);
            ReplayController.IsStartChange += ReplayController_IsStartChange; 
            
            Refresh();
        }
        
        /// <summary>
        /// event when the IsStarProperty change
        /// </summary>
        /// <param name="b"></param>
        void ReplayController_IsStartChange(bool b)
        {
            if(b)
                CheckButton.IsEnabled = true;
            else
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
                StartPauseUri = new Uri(URI_PAUSE);
            }
            //continue
            else if (ReplayController.IsPause)
            {
                ReplayController.Play();
                StartPauseUri = new Uri(URI_PAUSE);
            }
            //pause
            else
            {
                ReplayController.Pause();
                StartPauseUri = new Uri(URI_PLAY);
            }
        }

        /// <summary>
        /// stop the current replay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StartPauseUri = new Uri(URI_PLAY);

            ReplayController.Stop();
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
            SpeedLabel.Content = ReplayController.RefreshSpeedLabel();
        }

        /// <summary>
        /// eve,t to speed up the video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeedUp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ReplayController.SpeedUp();
            SpeedLabel.Content = ReplayController.RefreshSpeedLabel();
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
        
        /// <summary>
        /// function to delete by the contextual menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ReplayController.ListRecordController.RemoveVideo(MediaElementVideo, VideoList);
        }

       
        /// <summary>
        /// function to dataBinding
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
     }
}
