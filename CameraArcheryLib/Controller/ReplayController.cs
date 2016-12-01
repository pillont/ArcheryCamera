using Accord.Video.DirectShow;
using CameraArchery.DataBinding;
using CameraArcheryLib.Factories;
using CameraArcheryLib.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CameraArcheryLib.Controller
{

    public class ReplayController
    {
        public delegate void IsStateChangeDel(bool b);
        
        /*
         * event of the change of state
         */
        public event IsStateChangeDel IsPauseChange;
        public event IsStateChangeDel IsStartChange;
        public event IsStateChangeDel IsFrameByFrameChange;


        /// <summary>
        /// media element to manage
        /// </summary>
        public MediaElement MediaElement { get; set; }
        
        /// <summary>
        /// slider to manage the time
        /// </summary>
        public Slider TimeSlider { get; set; }
        
        /// <summary>
        /// label to inform the time
        /// </summary>
        public Label LabelTime { get; set; }

        /// <summary>
        /// inform if the media is in pause or not
        /// </summary>
        public bool IsPause 
        { 
            get
            {
                return isPause;
            }
            private set
            {
                isPause = value;

                if (IsPauseChange != null)
                    IsPauseChange(value);
            }
        }
        public bool isPause;
        
        /// <summary>
        /// inform if the media is in start or not
        /// </summary>
        public bool IsStart
        {
            get
            {
                return isStart;
            }
            private set
            {
                isStart = value;

                if (IsStartChange != null)
                    IsStartChange(value);
            }
        }
        public bool isStart;

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

                if (IsFrameByFrameChange != null)
                    IsFrameByFrameChange(value);
            }
        }
        private bool isFrameByFrame;

        /// <summary>
        /// timer to update the time value
        /// </summary>
        private DispatcherTimer timer { get; set; }

        /// <summary>
        /// locker to acces at the time
        /// </summary>
        private object lockerTimer = new object();

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

        /// <summary>
        /// controller to manage the list of file
        /// </summary>
        public ListRecordController ListRecordController { get; set; } 
        
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="mediaElement">media element associate</param>
        /// <param name="timeSlider">slider to inform the time</param>
        /// <param name="labelTime">label to inform the time</param>
        public ReplayController(MediaElement mediaElement, Slider timeSlider, Label labelTime, ListBox videoList)
        {
            this.MediaElement = mediaElement;
            this.TimeSlider = timeSlider;
            this.LabelTime = labelTime;

            ListRecordController = new ListRecordController(videoList);
            IsFrameByFrame = false;
            IsStart = false;

        }


        /// <summary>
        /// load the file in the media element
        /// </summary>
        public void LoadVideoFile()
        {
            var stringUri = ((VideoFile)ListRecordController.VideoList.SelectedValue).FullName;

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
            IsPause = false;
            IsStart = false;
        }

        /// <summary>
        /// pause the video
        /// </summary>
        public void Pause()
        {
            IsPause = true;

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

            IsPause = false;
            IsStart = true;
        }

        /// <summary>
        ///  start the timer
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
                // change the position
                if (MediaElement.NaturalDuration.HasTimeSpan)
                {
                    position = MediaElement.Position;
                    TimeSlider.Maximum = MediaElement.NaturalDuration.TimeSpan.Seconds;
                    duration = MediaElement.NaturalDuration.TimeSpan;

                    if (MediaElement.Position == MediaElement.NaturalDuration.TimeSpan)
                    {
                        MediaElement.Source = null;
                        timer = null;
                        ((DispatcherTimer)sender).Stop();
                    }
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
            IsPause = false;
            MediaElement.Play();
        }

        /// <summary>
        /// speed down the video
        /// </summary>
        public void SpeedDown()
        {
            if(MediaElement.SpeedRatio > 0)
                MediaElement.SpeedRatio -= 0.1;     
        }

        /// <summary>
        /// speed up the video
        /// </summary>
        public void SpeedUp()
        {
            if(MediaElement.SpeedRatio <2)
                MediaElement.SpeedRatio += 0.1;     
        }

        /// <summary>
        /// set the frame by frame of the video
        /// </summary>
        public bool FrameByFrameSetup()
        {
            var res = false;
            if (IsStart)
            {
                if (IsFrameByFrame)
                {
                    MediaElement.Pause();
                    MediaElement.MouseLeftButtonDown += MediaElementVideo_MouseLeftButtonDown;
                    res = true;
                }
                else
                {
                    MediaElement.MouseLeftButtonDown -= MediaElementVideo_MouseLeftButtonDown;
                    
                    if(!isPause)
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
    }
}
