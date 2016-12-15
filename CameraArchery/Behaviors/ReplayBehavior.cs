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
        private MediaElement MediaElement
        {
            get
            {
                return AssociatedObject.MediaElementVideo;
            }
        }

        /// <summary>
        /// slider to manage the time
        /// </summary>
        private Slider TimeSlider 
        { 
            get 
            {
                return AssociatedObject.TimeSlider;
            } 
        }

        /// <summary>
        /// label to inform the time
        /// </summary>
        private Label LabelTime 
        {
            get
            {
                return AssociatedObject.lblStatus;
            }
        }
        
        /// <summary>
        /// speed ratio of the mediaElement
        /// </summary>
        private double SpeedRatio
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

        #region event
        /// <summary>
        /// on attached associated object
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            
            AssociatedObject.IsFrameByFrame = false;
            AssociatedObject.IsStart = false;
        
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

        /// <summary>
        /// event on the mouse left down in the media element if frame by frame is done
        /// <para>get current position</para>
        /// <para>get the next image</para>
        /// <para>if position is out the video => go to the begin</para>
        /// </summary>
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
        #endregion event 

        #region private function
        /// <summary>
        /// load the file in the media element
        /// <para>get the uri of the file</para>
        /// <para>stop the media element</para>
        /// <para>change the mediaelement source</para>
        /// <para>change the mediaelement position to the begin</para>
        /// </summary>
        private void LoadVideoFile()
        {
            var stringUri = ((VideoFile) AssociatedObject.VideoList.SelectedValue).FullName;

            MediaElement.Stop();
            MediaElement.Source = new Uri(stringUri, UriKind.Relative);
            MediaElement.Position = new TimeSpan(0);
        }

        /// <summary>
        /// stop the video 
        /// <para> stop the media element</para>
        /// <para> change the position to the begin</para>
        /// <para> IsPause is set to false</para>
        /// <para> IsFrameByFrame is set to false</para>
        /// <para> IsStart is set to false</para>
        /// </summary>
        private void Stop()
        {
            MediaElement.Stop();
            MediaElement.Position = new TimeSpan(0);
            AssociatedObject.IsPause = false;
            AssociatedObject.IsStart = false;
            AssociatedObject.IsFrameByFrame = false;
        }

        /// <summary>
        /// pause the video
        /// <para>IsPause is set to true</para>
        /// <para>mediaElement is pause</para>
        /// </summary>
        private void Pause()
        {
            AssociatedObject.IsPause = true;

            MediaElement.Pause();
        }

        /// <summary>
        /// start the video
        /// <para>if source of the mediaElement is null, do nothing</para>
        /// <para>play the mediaElement</para>
        /// IsPause is set to false
        /// IsStart is set to true
        /// </summary>
        private void Start()
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
        private void StartTimer()
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
        private void StopTimer()
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
        /// <para>IsPause is set to false</para>
        /// </summary>
        private void Play()
        {
            AssociatedObject.IsPause = false;
            MediaElement.Play();
        }

        /// <summary>
        /// speed down the video
        /// <para>if SpeedRatio <= 0  do nothing</para>
        /// <para>SpeedRatio -= 0.1</para>
        /// </summary>
        private void SpeedDown()
        {
            if (MediaElement.SpeedRatio > 0)
                MediaElement.SpeedRatio -= 0.1;
        }

        /// <summary>
        /// speed up the video
        /// <para>if SpeedRatio >= 2  do nothing</para>
        /// <para>SpeedRatio += 0.1</para>
        /// </summary>
        private void SpeedUp()
        {
            if (MediaElement.SpeedRatio < 2)
                MediaElement.SpeedRatio += 0.1;
        }

        /// <summary>
        /// set the frame by frame of the video
        /// <para>if IsStart is false => do nothing</para>
        /// <para>if IsFrameByFrame is true => mediaElement set pause and unsubscribe the click event</para>
        /// <para>else =>unsubscribe the click event and mediaElement set to play if IsPause</para>
        /// </summary>
        private bool FrameByFrameSetup()
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
        /// get the current time of the mediaElement and show it
        /// </summary>
        private string RefreshSpeedLabel()
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
        #endregion private function
    }
}
