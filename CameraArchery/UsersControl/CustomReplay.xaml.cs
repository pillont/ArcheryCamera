using CameraArchery.DataBinding;
using CameraArcheryLib;
using CameraArcheryLib.Controller;
using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using CameraArcheryLib.Factories;
using System.Collections.ObjectModel;
using System.Windows.Interactivity;
using CameraArchery.Behaviors;

namespace CameraArchery.UsersControl
{
    /// <summary>
    /// Interaction logic for CustomReplay.xaml
    /// </summary>
    public partial class CustomReplay : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        #region constant
        /// <summary>
        /// uri of the play image
        /// </summary>
        public const string URI_PLAY = "pack://application:,,,/Ressources/Images/play.png";

        /// <summary>
        /// uri of the pause image
        /// </summary>
        public const string URI_PAUSE = "pack://application:,,,/Ressources/Images/pause.png";
        #endregion




        internal Func<bool> FrameByFrameSetup; 
        internal Func<string> RefreshSpeedLabel;
        internal Action SpeedUp;
        internal Action SpeedDown;
        internal Action StartTimer;
        internal Action StopTimer;
        internal Action LoadVideoFile;
        internal Action Stop;  
        internal Action Pause;
        internal Action Start;
        internal Action Play;



        #region event
        /// <summary>
        /// event to dataBinding
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// event during a start click
        /// arg:
        ///     1 : is Started
        ///     2 : is Paused
        /// return bool inform if can start
        /// </summary>
        public event Func<bool, bool, bool> OnStartClick;

        /// <summary>
        /// event during a stop click
        /// return bool inform if can stop
        /// </summary>
        public event Func<bool> OnStopClick;

        /// <summary>
        /// event during a list video selection change
        /// param : new value
        /// </summary>
        public event Action<VideoFile> OnListSelectionChange;

        /// <summary>
        /// event to inform the end of the video
        /// </summary>
        public event Action OnMediaEnded;

        /// <summary>
        /// event to inform the capture of a slider
        /// </summary>
        public event Action<double> OnSliderCapture;

        /// <summary>
        /// event to inform the change of the value of the slider
        /// arg : new value
        /// return : bool to inform if can change
        /// </summary>
        public event Func<double, bool> OnSliderChange;

        /// <summary>
        /// event to inform the speedDown click
        /// arg : new value
        /// return : bool to inform if can change
        /// </summary>
        public event Func<double, bool> OnSpeedDownClick;

        /// <summary>
        /// event to inform the speedUp click
        /// arg : new value
        /// return : bool to inform if can change
        /// </summary>
        public event Func<double, bool> OnSpeedUpClick;

        /// <summary>
        /// event to inform the frame by frame click
        /// arg : new value
        /// return : bool to inform if can change
        /// </summary>
        public event Func<bool> OnFrameClick;

        /// <summary>
        /// event to inform the deleting of a file
        /// arg : file to delete
        /// return : bool to inform if can delete
        /// </summary>
        public event Func<VideoFile, bool> OnDeleteFile;
        
        #endregion

        #region states
        /// <summary>
        /// inform if the media is in pause or not
        /// </summary>
        public bool IsPause
        {
            get
            {
                return isPause;
            }
            set
            {
                isPause = value;

             }
        }
        private bool isPause;

        /// <summary>
        /// inform if the media is in start or not
        /// </summary>
        public bool IsStart
        {
            get
            {
                return isStart;
            }
            set
            {
                isStart = value;
                CheckButton.IsEnabled = value;
            }
        }
        private bool isStart;


        /// <summary>
        /// inform if the vieo is in frame by frame or not
        /// </summary>
        public bool IsFrameByFrame
        {
            get
            {
                return isFrameByFrame;
            }
            set
            {
                isFrameByFrame = value;
                OnPropertyChanged("IsFrameByFrame");
           }
        }
        private bool isFrameByFrame;

        #endregion
        
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
        /// variable to save the value on mouse capture on the slider
        /// </summary>
        private double MouseCaptureValue;

        

        
        /// <summary>
        /// ctor
        /// </summary>
        public CustomReplay()
        {
            InitializeComponent();
            this.DataContext = this;
            
            StartPauseUri = new Uri(URI_PLAY);

            Interaction.GetBehaviors(this).Add(new ReplayBehavior());
             
            Refresh();

            BrowserControl.PropertyChanged += BrowserControl_PropertyChanged;
            Interaction.GetBehaviors(BrowserControl).Add(new VideoBrowserBehavior());
        }

        private void BrowserControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FileName")
                RefreshList();
        }
        
        

        /// <summary>
        /// refresh the replay view
        /// </summary>
        public void Refresh()
        {
            Stop();
            RefreshList();
        }


        public ObservableCollection<VideoFile> VideoFileList {get;set;}

        /// <summary>
        /// refresh the list of files
        /// </summary>
        /// <param name="list">current file in the list</param>
        private void RefreshList()
        {
            try
            {
                VideoFileList = new ObservableCollection<VideoFile>();
                foreach(var file in ListRecordController.GetList())
                    VideoFileList.Add(file);

                OnPropertyChanged("VideoFileList"); 
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
            if (OnStartClick != null
            && !OnStartClick(IsStart, IsPause))
                return;
            
            if (IsFrameByFrame)
                return;

            //start
            if (!IsStart)
            {
                Start();
                StartPauseUri = new Uri(URI_PAUSE);
            }
            //reply
            else if (IsPause)
            {
                Play();
                StartPauseUri = new Uri(URI_PAUSE);
            }
            //pause
            else
            {
                Pause();
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
            if (OnStopClick != null
            && !OnStopClick())
                return;
            
            StartPauseUri = new Uri(URI_PLAY);
            Stop();
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
                LoadVideoFile();

            if (OnListSelectionChange != null)
                OnListSelectionChange(VideoList.SelectedValue as VideoFile);
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
        
            if (OnMediaEnded != null)
                    OnMediaEnded();
            
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
            MouseCaptureValue = TimeSlider.Value;
            if (OnSliderCapture != null)
                OnSliderCapture(MouseCaptureValue);
            
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
            if (!IsPause && MediaElementVideo.Source != null)
                MediaElementVideo.Play();

            if (OnSliderChange != null &&
                OnSliderChange(TimeSlider.Value))
                TimeSlider.Value = MouseCaptureValue;
        }

        /// <summary>
        /// event when the list of files is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoList_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            RefreshList();
        }

        /// <summary>
        /// event when the component is unloaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            StopTimer();
        }

        /// <summary>
        /// event when the component is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            StartTimer();
        }

        /// <summary>
        /// event to speed down the video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeedDown_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var initValue = MediaElementVideo.SpeedRatio;
            SpeedDown();
        
            if (OnSpeedDownClick != null
            && !OnSpeedDownClick(MediaElementVideo.SpeedRatio))
                MediaElementVideo.SpeedRatio = initValue;
            
            SpeedLabel.Content = RefreshSpeedLabel();
        }

        /// <summary>
        /// eve,t to speed up the video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeedUp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var initValue = MediaElementVideo.SpeedRatio;
            SpeedUp();

            if (OnSpeedUpClick != null
            && !OnSpeedUpClick(MediaElementVideo.SpeedRatio))
                MediaElementVideo.SpeedRatio = initValue;

            SpeedLabel.Content = RefreshSpeedLabel();
        }

        /// <summary>
        /// event to start the frame by frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frame_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OnFrameClick != null
            && !OnFrameClick())
                return;
            
            FrameByFrameSetup();
        }
        
        /// <summary>
        /// function to delete by the contextual menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (OnDeleteFile != null
            && !OnDeleteFile(VideoList.SelectedValue as VideoFile))
                return;
            
            ListRecordController.RemoveVideo(MediaElementVideo, VideoList);
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

        private void thumbnail_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var me= (sender as MediaElement);
            me.Position = new TimeSpan(0, 0, 0);
            me.SpeedRatio = 0;
        }
     }
}
