using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace CameraArcheryLib.Controller
{
    /// <summary>
    /// Controller to make a lag 
    /// </summary>
    public class TimeLagController
    {
        /// <summary>
        /// Controller to make feedback
        /// </summary>
        public LagLoadFeedBackController lagLoadFeedBackController { get; set; }



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

        /// <summary>
        /// ctor
        /// set the Controller of the feedBack
        /// </summary>
        public TimeLagController()
        {
            lagLoadFeedBackController = new LagLoadFeedBackController();
            Clear();
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
            if (lagLoadFeedBackController.IsLoad)
            {
                res = TempsImagesStock[0];
                TempsImagesStock.RemoveAt(0);
            }
            return res;
        }
    }
}
