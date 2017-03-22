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
        private LagLoadFeedBackManager LagLoadFeedBackManager { get; set; }

        /// <summary>
        /// behavior of the video
        /// </summary>
        public int Delay { get; set; }

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
        }

        /// <summary>
        /// start the load of the lag after the first load of the associated object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            //FIXME : use sender because, some time AssociatedObject property is null...
            var element = sender as CustomVideoElement;
            element.ProgressBar.DataContext = this;

            // update the view with the time lag Controller
            element.ProgressBar.Minimum = 0;
            element.ProgressBar.Maximum = Delay;

            LagLoadFeedBackManager = new LagLoadFeedBackManager(
                onProgressChange: (db) => Dispatcher.Invoke(() => element.ProgressBar.Value = db),
                onVisibilityChange: vs => OnVisibilityChange(vs, element)
            );
        }

        /// <summary>
        /// function to stop current load
        /// </summary>
        public void StopLoad()
        {
            LagLoadFeedBackManager.StopLoad();
        }

        /// <summary>
        /// event when visibility change
        /// <para>if visibility is visible => open the popUp and collapsed the button to record</para>
        /// <para>else => close the popUp and make visible the Option Panel</para>
        /// </summary>
        /// <param name="vs">visibility of the Option Panel</param>
        /// <param name="element">FIXME : use sender because, some time AssociatedObject property is null...</param>
        private void OnVisibilityChange(Visibility vs, CustomVideoElement element)
        {
            Dispatcher.Invoke(() =>
            {
                element.ProgressBar.Visibility = vs;

                if (vs == Visibility.Visible)
                {
                    element.PictureVideo.Visibility = Visibility.Hidden;
                    element.OptionPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    element.PictureVideo.Visibility = Visibility.Visible;
                    element.OptionPanel.Visibility = Visibility.Visible;
                }
            });
        }

        #endregion event
    }
}
