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
using CameraArcheryLib.Utils;

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


        #region event

        /// <summary>
        /// frame by frame change
        /// </summary>
        public event Func<bool> FrameByFrameSetup;
 
        /// <summary>
        /// event on refresh speed label
        /// </summary>
        public event Func<string> RefreshSpeedLabel;

        /// <summary>
        /// event on speed up
        /// </summary>
        public event Action SpeedUp;

        /// <summary>
        /// event on speed down
        /// </summary>
        public event Action SpeedDown;

        /// <summary>
        /// event on start timer
        /// </summary>
        public event Action StartTimer;

        /// <summary>
        /// event on stop timer
        /// </summary>
        public event Action StopTimer;

        /// <summary>
        /// event on load video file
        /// </summary>
        public event Action LoadVideoFile;

        /// <summary>
        /// event on stop
        /// </summary>
        public event Action Stop;

        /// <summary>
        /// event on pause
        /// </summary>
        public event Action Pause;

        /// <summary>
        /// event on start
        /// </summary>
        public event Action Start;

        /// <summary>
        /// event on play
        /// </summary>
        public event Action Play;

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
        public event Action OnFrameClick;

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
        /// update the mode of frameByFrame
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

                if (FrameByFrameSetup !=  null)
                    FrameByFrameSetup();
            }
        }
        private bool isFrameByFrame;

        #endregion

        /// <summary>
        /// list of video file in the listview
        /// binded with the visual
        /// </summary>
        public ObservableCollection<VideoFile> VideoFileList { get; set; }

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

            BehaviorHelper.AddSingleBehavior(new ReplayBehavior(), this);
            BehaviorHelper.AddSingleBehavior(new VideoBrowserBehavior(), BrowserControl);

            Refresh();

            BrowserControl.PropertyChanged += BrowserControl_PropertyChanged;
        }

        #region event
        private void BrowserControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FileName")
                RefreshList();
        }
    
        
        /// <summary>
        /// event to start the file selected
        /// <para>is file not start </para>
        /// <para>if on start click return null do nothing</para>
        /// <para>if IsFrameByFrame is true do nothing</para>
        /// <para>if is not start => start the video</para>
        /// <para>if is pause => is started</para>
        /// <para>else stop the video</para>
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
                LogHelper.Write("start click");
            
                if(Start != null)
                    Start();
                StartPauseUri = new Uri(URI_PAUSE);
            }
            //reply
            else if (IsPause)
            {
                LogHelper.Write("replay click");
            
                if(Play != null)
                    Play();
                StartPauseUri = new Uri(URI_PAUSE);
            }
            //pause
            else
            {
                LogHelper.Write("Pause click");

                if (Pause != null)
                    Pause();
                StartPauseUri = new Uri(URI_PLAY);
            }
        }

        /// <summary>
        /// stop the current replay
        /// <para>if OnStopClick() return false -> do nothing</para>
        /// <para> init the uri</para>
        /// <para>stop the video</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StopReplay();
        }

        /// <summary>
        /// event when file selected change
        ///<para>stop the replay</para>
        ///<para>if selected file is not null -> load the video file</para>
        ///<para>call the event OnListSelectionChange</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StopReplay();

            if (VideoList.SelectedValue != null
            && LoadVideoFile != null)
                LoadVideoFile();

            LogHelper.Write("selected file change : " + (VideoList.SelectedValue as VideoFile));
            if (OnListSelectionChange != null)
                OnListSelectionChange(VideoList.SelectedValue as VideoFile);
        }

        /// <summary>
        /// event when replay is finished
        /// <para>if IsFramByFrame if false -> stop the replay</para>
        /// <para>call the event OnMediaEnded</para>
        /// </summary>
        private void MediaElementVideo_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsFrameByFrame)
                StopReplay();

            LogHelper.Write("media is ended");
            if (OnMediaEnded != null)
                OnMediaEnded();
        }

        /// <summary>
        /// event when the value change
        /// </summary>
        private void TimeSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (TimeSlider.IsMouseCaptured)
                MediaElementVideo.Position = new TimeSpan(0, 0, Convert.ToInt32(e.NewValue));
        }

        /// <summary>
        /// event when the slider got mouse capture
        /// <para>set the value <code>MouseCaptureValue</code></para>
        /// <para>call event OnSliderCapture</para>
        /// pause the media element
        /// </summary>
        private void TimeSlider_GotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MouseCaptureValue = TimeSlider.Value;

            LogHelper.Write("slider is capture at " + MouseCaptureValue + " sec");
            
            if (OnSliderCapture != null)
                OnSliderCapture(MouseCaptureValue);

            if (TimeSlider.IsFocused && MediaElementVideo.Source != null)
                MediaElementVideo.Pause();
        }

        /// <summary>
        /// event when the slider lost mouse capture
        /// <para>play the media element is there is source and is not in pause</para>
        /// <para>call OnSliderChange</para>
        /// <para>is OnSliderChange is true => change the value of the timeSlider</para>
        /// </summary>
        private void TimeSlider_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!IsPause && MediaElementVideo.Source != null)
                MediaElementVideo.Play();

            LogHelper.Write("slider is change to " + TimeSlider.Value + " sec");

            if (OnSliderChange != null &&
                OnSliderChange(TimeSlider.Value))
                TimeSlider.Value = MouseCaptureValue;
        }

        /// <summary>
        /// event when the list of files is loaded
        /// </summary>
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
            if (StopTimer != null)
                StopTimer();
        }

        /// <summary>
        /// event when the component is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (StartTimer != null)
                StartTimer();
        }

        /// <summary>
        /// event to speed down the video
        /// <para>speed down</para>
        /// <para>call OnSpeedDownClick</para>
        /// <para>update speedLabel content</para>
        /// </summary>
        private void SpeedDown_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var initValue = MediaElementVideo.SpeedRatio;

            if (SpeedDown != null)
                SpeedDown();

            LogHelper.Write("speed down : " + MediaElementVideo.SpeedRatio);

            if (OnSpeedDownClick != null
            && !OnSpeedDownClick(MediaElementVideo.SpeedRatio))
                MediaElementVideo.SpeedRatio = initValue;

            if (RefreshSpeedLabel != null)
                SpeedLabel.Content = RefreshSpeedLabel();
        }

        /// <summary>
        /// event to speed up the video
        /// <para>speedUp</para>
        /// <para>call OnSpeedUpClick</para>
        /// <para>refresh the speed label</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeedUp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var initValue = MediaElementVideo.SpeedRatio;

            if (SpeedUp != null)
                SpeedUp();

            LogHelper.Write("speed up : " + MediaElementVideo.SpeedRatio);

            if (OnSpeedUpClick != null
            && !OnSpeedUpClick(MediaElementVideo.SpeedRatio))
                MediaElementVideo.SpeedRatio = initValue;

            if (RefreshSpeedLabel != null)
                SpeedLabel.Content = RefreshSpeedLabel();
        }

        /// <summary>
        /// event to start the frame by frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frame_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LogHelper.Write("on frame click");

            if (OnFrameClick != null)
                OnFrameClick();
        }

        /// <summary>
        /// event on the rename of a video file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //get current file
            var file = (VideoList.SelectedValue as VideoFile);
            //pass to editing
            file.IsEditing = true;

            //update the IsEditing value
            //TODO event onPropertyChange in the list and not the item! and delete this
            var index = VideoFileList.IndexOf(file);
            VideoFileList.Remove(file);
            VideoFileList.Insert(index, file);
            VideoList.SelectedItem = file;
        }

        /// <summary>
        /// function to delete by the contextual menu
        /// <para>call event ondeleteFile</para>
        /// <para>remove the video</para>
        /// </summary>
        private void DeleteItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LogHelper.Write("delete file " + (VideoList.SelectedValue as VideoFile).Uri);

            if (OnDeleteFile != null
            && !OnDeleteFile(VideoList.SelectedValue as VideoFile))
                return;

            ListRecordController.RemoveVideo(MediaElementVideo, VideoList);
        }

        /// <summary>
        /// function to dataBinding
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// <para>element change position</para>
        /// <para>element set speedRatio to zero</para>
        /// </summary>
        private void thumbnail_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var element = (sender as MediaElement);
            element.Position = new TimeSpan(0, 0, 0);
            element.SpeedRatio = 0;
        }
        #endregion event


        /// <summary>
        /// refresh the replay view
        /// </summary>
        private void Refresh()
        {
            if(Stop != null)
                Stop();
            RefreshList();
        }

        /// <summary>
        /// refresh the list of files
        /// <para>init selected file to the first</para>
        /// </summary>
        /// <param name="list">current file in the list</param>
        private void RefreshList()
        {
            try
            {
                VideoFileList = new ObservableCollection<VideoFile>(
                                                    ListRecordController.GetList());
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
        /// stop the replay
        /// </summary>
        private void StopReplay()
        {

            LogHelper.Write("stop click");
            if (OnStopClick != null
            && !OnStopClick())
                return;

            StartPauseUri = new Uri(URI_PLAY);

            if (Stop != null)
                Stop();
        }
    }
}
