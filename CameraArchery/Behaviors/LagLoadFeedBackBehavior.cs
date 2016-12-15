using CameraArchery.UsersControl;
using CameraArcheryLib.Factories;
using CameraArcheryLib.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace CameraArchery.Behaviors
{
    /// <summary>
    /// Controller to make feedback lag's loaded
    /// <para>control the progressBar</para>
    /// <para>control the pop up to inform the quit key</para>
    /// </summary>
    public class LagLoadFeedBackBehavior : Behavior<CustomVideoElement>
    {
        //private TimeLagBehavior TimeLagBehavior;

        ///// <summary>
        ///// value of the progress
        ///// </summary>-
        //public double Progress
        //{
        //    get
        //    {
        //        Monitor.Enter(progressLocker);
        //        var res = progress;
        //        Monitor.Exit(progressLocker);
        //        return res;
        //    }
        //    private set
        //    {
        //        Monitor.Enter(progressLocker);
        //        progress = value;
        //        Monitor.Exit(progressLocker);
                
        //        Dispatcher.Invoke(() => AssociatedObject.ProgressBar.Value = value);
        //    }
        //}
        //private double progress;
        //private object progressLocker = new object();


        ///// <summary>
        ///// value of the visibility of the progress bar
        ///// </summary>
        //public System.Windows.Visibility Visibility
        //{
        //    get
        //    {
        //        Monitor.Enter(visibilityLocker);
        //        var res = visibility;
        //        Monitor.Exit(visibilityLocker);
        //        return res;
        //    }
        //    private set
        //    {
        //        Monitor.Enter(visibilityLocker);
        //        visibility = value;
        //        Monitor.Exit(visibilityLocker);
                
        //        OnVisibilityChange(value);
        //    }
        //}
        //private System.Windows.Visibility visibility;
        //private object visibilityLocker = new object();

        ///// <summary>
        ///// ctor
        ///// <para>visibility of the feedback set visible</para>
        ///// <para><code>IsLoad</code> if false</para>
        ///// <para><code>progress</code> is set to zero</para>
        ///// <para> start a task to start the load</para>
        ///// </summary>
        //public LagLoadFeedBackBehavior(TimeLagBehavior timeLagBehavior)
        //{
        //    this.TimeLagBehavior = timeLagBehavior;
        //}


        //protected override void OnAttached()
        //{
        //    base.OnAttached();

        //    Visibility = System.Windows.Visibility.Visible;
        //    TimeLagBehavior.IsLoad = false;
        //    Progress = 0;
        //    new Task(() => StartLoad()).Start();

        //}
        ///// <summary>
        ///// function to start the load
        ///// <para>start a timer to inform the progress</para>
        ///// <para>wait the time in the config</para>
        ///// <para>set <code>IsLoad</code> to true</para>
        ///// <para>set visibility of the feedbacks to collapsed</para>
        ///// </summary>
        //public void StartLoad()
        //{
        //    LogHelper.Write("start video load for : "+SettingFactory.CurrentSetting.Time+ " seconds");
            
        //    System.Timers.Timer timer = new System.Timers.Timer(1000);
        //    timer.Elapsed += timer_Elapsed;
        //    timer.Start();
        //    Thread.Sleep(SettingFactory.CurrentSetting.Time *1000);
            
        //    LogHelper.Write("end of video load");
            
        //    timer.Stop();
        //    TimeLagBehavior.IsLoad = true;
        //    Visibility = Visibility.Collapsed;
            
        //    LogHelper.Write("start the video");
            
        //}

        ///// <summary>
        ///// event to update the progress with the timer
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    Progress++;
        //}

       

        ///// <summary>
        ///// event when visibility change
        ///// </summary>
        ///// <param name="vs"></param>
        //private void OnVisibilityChange(Visibility vs)
        //{
        //    Dispatcher.Invoke(() =>
        //    {
        //        AssociatedObject.ProgressBar.Visibility = vs;

        //        if (vs == Visibility.Visible)
        //        {
        //            AssociatedObject.myPopup.IsOpen = true;
        //            AssociatedObject.ButtonPanel.Visibility = Visibility.Collapsed;
        //        }
        //        else
        //        {
        //            AssociatedObject.myPopup.IsOpen = false;
        //            AssociatedObject.ButtonPanel.Visibility = Visibility.Visible;
        //        }
        //    });
        //}
        
    }
}
