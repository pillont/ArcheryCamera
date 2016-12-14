using Accord.Video.DirectShow;
using CameraArchery.DataBinding;
using CameraArchery.UsersControl;
using CameraArcheryLib.Controller;
using CameraArcheryLib.Factories;
using CameraArcheryLib.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Threading;

namespace CameraArchery.Behaviors
{
    /// <summary>
    /// controller to manage the video replay
    /// </summary>
    public class ReplayBehavior : Behavior<CustomReplay>
    {
        #region element of the associated object
        /// <summary>
        /// media element to manage
        /// </summary>
        public MediaElement MediaElement
        {
            get
            {
                return AssociatedObject.MediaElementVideo;
            }
        }

        /// <summary>
        /// slider to manage the time
        /// </summary>
        public Slider TimeSlider 
        { 
            get 
            {
                return AssociatedObject.TimeSlider;
            } 
        }

        /// <summary>
        /// label to inform the time
        /// </summary>
        public Label LabelTime 
        {
            get
            {
                return AssociatedObject.SpeedLabel;
            }
        }
        
        /// <summary>
        /// speed ratio of the mediaElement
        /// </summary>
        public double SpeedRatio
        {
            get
            {
                return MediaElement.SpeedRatio;
            }
        }
        #endregion

        /// <summary>
        /// timer to update the time value
        /// </summary>
        private DispatcherTimer timer { get; set; }

        /// <summary>
        /// locker to acces at the time
        /// </summary>
        private object lockerTimer = new object();

        

        /// <summary>
        /// on attached associated object
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            
            AssociatedObject.IsFrameByFrame = false;
            AssociatedObject.IsStart = false;
            AssociatedObject.PropertyChanged += AssociatedObject_PropertyChanged;


            AssociatedObject.FrameByFrameSetup = FrameByFrameSetup;
            AssociatedObject.RefreshSpeedLabel = RefreshSpeedLabel;
            AssociatedObject.SpeedUp = SpeedUp;
            AssociatedObject.SpeedDown = SpeedDown ;
            AssociatedObject.StartTimer = StartTimer;
            AssociatedObject.StopTimer = StopTimer;
            AssociatedObject.LoadVideoFile = LoadVideoFile;
            AssociatedObject.Stop = Stop;
            AssociatedObject.Pause = Pause;
            AssociatedObject.Play = Play;
            AssociatedObject.Start = Start;
        }

        void AssociatedObject_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "IsFrameByFrame"
                && !AssociatedObject.IsFrameByFrame && AssociatedObject.IsStart)
                    Play();
        }

        /// <summary>
        /// load the file in the media element
        /// </summary>
        public void LoadVideoFile()
        {
            var stringUri = ((VideoFile) AssociatedObject.VideoList.SelectedValue).FullName;

            MediaElement.Stop();
            MediaElement.Source = new Uri(stringUri, UriKind.Relative);
            MediaElement.Position = new TimeSpan(0);
        }

        /// <summary>
        /// stop the video 
        /// </summary>
        public void Stop()
        {
            MediaElement.Stop();
            MediaElement.Position = new TimeSpan(0);
            AssociatedObject.IsPause = false;
            AssociatedObject.IsStart = false;
            AssociatedObject.IsFrameByFrame = false;
        }

        /// <summary>
        /// pause the video
        /// </summary>
        public void Pause()
        {
            AssociatedObject.IsPause = true;

            MediaElement.Pause();
        }

        /// <summary>
        /// start the video
        /// </summary>
        public void Start()
        {
            if (MediaElement.Source == null)
                return;
            MediaElement.Play();

            AssociatedObject.IsPause = false;
            AssociatedObject.IsStart = true;
        }

        /// <summary>
        ///  start the timer
        ///  <para>timer update each second the value on the label timer</para>
        /// </summary>
        public void StartTimer()
        {
            Monitor.Enter(lockerTimer);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            Monitor.Exit(lockerTimer);
        }

        /// <summary>
        /// function of the timer to update the timer
        /// <para>set the timer to zero</para>
        /// <para>get the value if possible</para>
        /// <para>change the value</para>
        /// <para> if no file, inform the user</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            // set to zero
            TimeSpan position = new TimeSpan(0);
            TimeSpan duration = new TimeSpan(0);
            TimeSlider.Value = 0;

            if (MediaElement.Source != null)
            {
                // get time
                if (MediaElement.NaturalDuration.HasTimeSpan)
                {
                    position = MediaElement.Position;
                    TimeSlider.Maximum = MediaElement.NaturalDuration.TimeSpan.Seconds;
                    duration = MediaElement.NaturalDuration.TimeSpan;
                }
                // change the values
                TimeSlider.Value = position.Seconds;
                LabelTime.Content = String.Format("{0} / {1}", new TimeSpan(0, 0, position.Seconds).ToString(@"mm\:ss"), duration.ToString(@"mm\:ss"));
            }
            // no file selected
            else
            {
                LabelTime.Content = "No file selected...";
                TimeSlider.Value = 0;
            }
        }

        /// <summary>
        /// stop the timer
        /// </summary>
        public void StopTimer()
        {
            if (timer != null)
            {
                Monitor.Enter(lockerTimer);
                timer.Stop();
                Monitor.Exit(lockerTimer);
            }
        }

        /// <summary>
        /// play the video
        /// </summary>
        public void Play()
        {
            AssociatedObject.IsPause = false;
            MediaElement.Play();
        }

        /// <summary>
        /// speed down the video
        /// </summary>
        public void SpeedDown()
        {
            if (MediaElement.SpeedRatio > 0)
                MediaElement.SpeedRatio -= 0.1;
        }

        /// <summary>
        /// speed up the video
        /// </summary>
        public void SpeedUp()
        {
            if (MediaElement.SpeedRatio < 2)
                MediaElement.SpeedRatio += 0.1;
        }

        /// <summary>
        /// set the frame by frame of the video
        /// </summary>
        public bool FrameByFrameSetup()
        {
            var res = false;
            if (AssociatedObject.IsStart)
            {
                if (AssociatedObject.IsFrameByFrame)
                {
                    MediaElement.Pause();
                    MediaElement.MouseLeftButtonDown += MediaElementVideo_MouseLeftButtonDown;
                    res = true;
                }
                else
                {
                    MediaElement.MouseLeftButtonDown -= MediaElementVideo_MouseLeftButtonDown;

                    if (!AssociatedObject.IsPause)
                        MediaElement.Play();
                }
            }
            return res;
        }

        /// <summary>
        /// event on the mouse left down in the media element if frame by frame is done
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaElementVideo_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var current = MediaElement.Position;
            var nextValue = new TimeSpan(current.Days,
                                                current.Hours,
                                                current.Minutes,
                                                current.Seconds,
                                                current.Milliseconds + (1000 / SettingFactory.CurrentSetting.Frame));

            if (MediaElement.NaturalDuration.TimeSpan.Ticks > nextValue.Ticks)
                MediaElement.Position = nextValue;
            else
                MediaElement.Position = new TimeSpan(0);
        }

        /// <summary>
        /// get the current time of the mediaElement and show it
        /// </summary>
        public string RefreshSpeedLabel()
        {
            var time = Convert.ToInt32(SpeedRatio * 100);

            // more slowly
            if (time == 0)
                time++;
            // if stop
            if (time == -10)
                time = 0;

            return time.ToString() + "%";
        }
    }
}
