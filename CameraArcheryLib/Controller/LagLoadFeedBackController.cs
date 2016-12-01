using CameraArcheryLib.Factories;
using CameraArcheryLib.Utils;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CameraArcheryLib.Controller
{
    /// <summary>
    /// Controller to make feedback lag's loaded
    /// <para>control the progressBar</para>
    /// <para>control the pop up to inform the quit key</para>
    /// </summary>
    public class LagLoadFeedBackController
    {
        public delegate void OnPropertyChangedDouble(double property);
        public delegate void OnPropertyChangedVisibility(Visibility property);

        /// <summary>
        /// event when progress change
        /// </summary>
        public OnPropertyChangedDouble OnProgressChange;

        /// <summary>
        /// event when visibility change
        /// </summary>
        public OnPropertyChangedVisibility OnVisibilityChange;

        /// <summary>
        /// value of the progress
        /// </summary>
        public double Progress
        {
            get
            {
                Monitor.Enter(progressLocker);
                var res = progress;
                Monitor.Exit(progressLocker);
                return res;
            }
            private set
            {
                Monitor.Enter(progressLocker);
                progress = value;
                Monitor.Exit(progressLocker);

                if (OnProgressChange != null)
                    OnProgressChange(value);
            }
        }
        private double progress;
        private object progressLocker = new object();

        /// <summary>
        /// inform if the lag is loaded
        /// </summary>
        public bool IsLoad
        {
            get
            {
                Monitor.Enter(loadLocker);
                var res = isLoad;
                Monitor.Exit(loadLocker);
                return res;
            }
            private set
            {
                Monitor.Enter(loadLocker);
                isLoad = value;
                Monitor.Exit(loadLocker);
            }
        }
        private object loadLocker = new object();
        private bool isLoad;

        /// <summary>
        /// value of the visibility of the progress bar
        /// </summary>
        public System.Windows.Visibility Visibility
        {
            get
            {
                Monitor.Enter(visibilityLocker);
                var res = visibility;
                Monitor.Exit(visibilityLocker);
                return res;
            }
            private set
            {
                Monitor.Enter(visibilityLocker);
                visibility = value;
                Monitor.Exit(visibilityLocker);
                

                if (OnVisibilityChange != null)
                    OnVisibilityChange(value);
            }
        }
        private System.Windows.Visibility visibility;
        private object visibilityLocker = new object();

        /// <summary>
        /// ctor
        /// <para>visibility of the feedback set visible</para>
        /// <para><code>IsLoad</code> if false</para>
        /// <para><code>progress</code> is set to zero</para>
        /// <para> start a task to start the load</para>
        /// </summary>
        public LagLoadFeedBackController()
        {
            Visibility = System.Windows.Visibility.Visible;
            IsLoad = false;
            Progress = 0;
            new Task(() => StartLoad()).Start();
        }

        /// <summary>
        /// function to start the load
        /// <para>start a timer to inform the progress</para>
        /// <para>wait the time in the config</para>
        /// <para>set <code>IsLoad</code> to true</para>
        /// <para>set visibility of the feedbacks to collapsed</para>
        /// </summary>
        public void StartLoad()
        {
            LogHelper.Write("start video load for : "+SettingFactory.CurrentSetting.Time+ " seconds");
            
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += timer_Elapsed;
            timer.Start();
            Thread.Sleep(SettingFactory.CurrentSetting.Time *1000);
            
            LogHelper.Write("end of video load");
            
            timer.Stop();
            IsLoad = true;
            Visibility = Visibility.Collapsed;
            
            LogHelper.Write("start the video");
            
        }

        /// <summary>
        /// event to update the progress with the timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Progress++;
        }
    }
}
