using CameraArchery.UsersControl;
using CameraArcheryLib.Controller;
using CameraArcheryLib.Factories;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
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
        public LagLoadFeedBackManager lagLoadFeedBackManager { get; set; }

        /// <summary>
        /// list of frame save during the difference of time
        /// </summary>
        private List<Bitmap> TempsImagesStock
        {
            get
            {
                Monitor.Enter(tempsImagesStockLocker);
                var res = tempsImagesStock;
                Monitor.Exit(tempsImagesStockLocker);
                return res;

            }
            set
            {
                Monitor.Enter(tempsImagesStockLocker);
                tempsImagesStock = value;
                Monitor.Exit(tempsImagesStockLocker);
            }
        }
        private List<Bitmap> tempsImagesStock;
        private object tempsImagesStockLocker = new object();

        private VideoBehavior VideoBehavior;
        /// <summary>
        /// ctor
        /// set the Controller of the feedBack
        /// </summary>
        public TimeLagBehavior(VideoBehavior videoBehavior)
        {
            this.VideoBehavior = videoBehavior;
        }


        protected override void OnAttached()
        {
            base.OnAttached();

            lagLoadFeedBackManager = new LagLoadFeedBackManager()
            {
                OnProgressChange = (db) => Dispatcher.Invoke(() => AssociatedObject.ProgressBar.Value = db),
                OnVisibilityChange = OnVisibilityChange
            };

            Clear();

            // add lag on the video Controller
            VideoBehavior.OnNewFrame += (ref Bitmap img) => img = OnNewFrame(img);
            VideoBehavior.OnVideoClose += () => Clear();


            // update the view with the time lag Controller
            AssociatedObject.ProgressBar.Minimum = 0;
            AssociatedObject.ProgressBar.Maximum = SettingFactory.CurrentSetting.Time;
            AssociatedObject.ProgressBar.DataContext = this;
        }

        /// <summary>
        /// event when visibility change
        /// </summary>
        /// <param name="vs"></param>
        private void OnVisibilityChange(Visibility vs)
        {
            Dispatcher.Invoke(() =>
            {
                AssociatedObject.ProgressBar.Visibility = vs;

                if (vs == Visibility.Visible)
                {
                    AssociatedObject.myPopup.IsOpen = true;
                    AssociatedObject.ButtonPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    AssociatedObject.myPopup.IsOpen = false;
                    AssociatedObject.ButtonPanel.Visibility = Visibility.Visible;
                }
            });
        }
        /// <summary>
        /// clear the memory
        /// </summary>
        public void Clear()
        {
            TempsImagesStock = new List<Bitmap>();
        }

        /// <summary>
        /// event to each image enter by the camera
        /// <para>stock the picture</para>
        /// <para>if the load is finish, show the last picture in the stock</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public Bitmap OnNewFrame(Bitmap img)
        {
            TempsImagesStock.Add(img);
            Bitmap res = null;
            if (lagLoadFeedBackManager.IsLoad)
            {
                res = TempsImagesStock[0];
                TempsImagesStock.RemoveAt(0);
            }
            return res;
        }
    }
}
