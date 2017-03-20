using CameraArchery.Manager;
using CameraArchery.UsersControl;
using System.Windows;
using System.Windows.Interactivity;

namespace CameraArchery.Behaviors
{
    /// <summary>
    /// Controller to make a lag
    /// </summary>
    public class TimeLagBehavior : Behavior<CustomVideoElement>
    {
        /// <summary>
        /// Controller to make feedback
        /// </summary>
        private LagLoadFeedBackManager lagLoadFeedBackManager { get; set; }

        /// <summary>
        /// behavior of the video
        /// </summary>
        private int Delay { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="videoBehavior">video behavior associated</param>
        public TimeLagBehavior(int delay)
        {
            this.Delay = delay;
        }

        #region event

        /// <summary>
        /// on attach event
        /// <para>init the lag load feedBack manager</para>
        /// <para>clear</para>
        /// <para>subscribe the event to make lag on the video</para>
        /// <para>update the view with the time lag Controller</para>
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += AssociatedObject_Loaded;

            // update the view with the time lag Controller
            AssociatedObject.ProgressBar.Minimum = 0;
            AssociatedObject.ProgressBar.Maximum = Delay;
            AssociatedObject.ProgressBar.DataContext = this;
        }

        /// <summary>
        /// start the load of the lag after the first load of the associated object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= AssociatedObject_Loaded;

            lagLoadFeedBackManager = new LagLoadFeedBackManager()
            {
                OnProgressChange = (db) => Dispatcher.Invoke(() => AssociatedObject.ProgressBar.Value = db),
                OnVisibilityChange = OnVisibilityChange
            };
        }

        /// <summary>
        /// event when visibility change
        /// <para>if visibility is visible => open the popUp and collapsed the button to record</para>
        /// <para>else => close the popUp and make visible the Option Panel</para>
        /// </summary>
        /// <param name="vs">visibility of the Option Panel</param>
        private void OnVisibilityChange(Visibility vs)
        {
            Dispatcher.Invoke(() =>
            {
                AssociatedObject.ProgressBar.Visibility = vs;

                if (vs == Visibility.Visible)
                    AssociatedObject.OptionPanel.Visibility = Visibility.Collapsed;
                else
                    AssociatedObject.OptionPanel.Visibility = Visibility.Visible;
            });
        }

        #endregion event
    }
}
