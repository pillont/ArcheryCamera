using CameraArchery.UsersControl;
using CameraArcheryLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;

namespace CameraArchery.Behaviors
{
    /// <summary>
    /// behavior to make log in replay component
    /// </summary>
    public class LogReplayBehavior : Behavior<CustomReplay>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.OnStopClick += CustomReplayComponent_OnStopClick;
            AssociatedObject.OnStartClick += CustomReplayComponent_OnStartClick;
            AssociatedObject.OnSpeedUpClick += CustomReplayComponent_OnSpeedUpClick;
            AssociatedObject.OnSpeedDownClick += CustomReplayComponent_OnSpeedDownClick;
            AssociatedObject.OnSliderChange += CustomReplayComponent_OnSliderChange;
            AssociatedObject.OnSliderCapture += CustomReplayComponent_OnSliderCapture;
            AssociatedObject.OnMediaEnded += CustomReplayComponent_OnMediaEnded;
            AssociatedObject.OnListSelectionChange += CustomReplayComponent_OnListSelectionChange;
            AssociatedObject.OnFrameClick += CustomReplayComponent_OnFrameClick;
            AssociatedObject.OnDeleteFile += CustomReplayComponent_OnDeleteFile;
        }

        /// <summary>
        /// log on the start click
        /// <para>log on start</para>
        /// <para>log on replay</para>
        /// <para>log on pause</para>
        /// </summary>
        /// <param name="isStart"></param>
        /// <param name="isPause"></param>
        /// <returns></returns>
        private bool CustomReplayComponent_OnStartClick(bool isStart, bool isPause)
        {
            if (!isStart)
                LogHelper.Write("start click");
            else if (isPause)
                LogHelper.Write("replay click");
            else
                LogHelper.Write("Pause click");
          
            return true;
        }

        /// <summary>
        /// log on the stop click
        /// </summary>
        private bool CustomReplayComponent_OnStopClick()
        {
            LogHelper.Write("stop click");

            return true;
        }

        /// <summary>
        /// log on the deleting of file
        /// </summary>
        /// <param name="arg"></param>
        private bool CustomReplayComponent_OnDeleteFile(DataBinding.VideoFile arg)
        {
            LogHelper.Write("delete file " + arg.FullName);
            return true;
        }

        /// <summary>
        /// log on frameByFrame click
        /// </summary>
        private void CustomReplayComponent_OnFrameClick()
        {
            LogHelper.Write("on frame click");
        }

        /// <summary>
        /// log on change of selected file
        /// </summary>
        /// <param name="obj"></param>
        private void CustomReplayComponent_OnListSelectionChange(DataBinding.VideoFile obj)
        {
            LogHelper.Write("selected file change : " + obj);
        }

        /// <summary>
        /// log on media end
        /// </summary>
        private void CustomReplayComponent_OnMediaEnded()
        {
            LogHelper.Write("media is ended");
        }

        /// <summary>
        /// log on capture of the time slider
        /// </summary>
        /// <param name="obj"></param>
        private void CustomReplayComponent_OnSliderCapture(double obj)
        {
            LogHelper.Write("slider is capture at " + obj + " sec");
        }

        /// <summary>
        /// log on the change of value in the time slider
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CustomReplayComponent_OnSliderChange(double arg)
        {
            LogHelper.Write("slider is change to " + arg + " sec");
            return true;
        }

        /// <summary>
        /// log on speedDownClick
        /// </summary>
        /// <param name="arg">new value</param>
        private bool CustomReplayComponent_OnSpeedDownClick(double arg)
        {
            LogHelper.Write("speed down : " + arg);
            return true;
        }

        /// <summary>
        /// log on speed up Click
        /// </summary>
        /// <param name="arg"></param>
        private bool CustomReplayComponent_OnSpeedUpClick(double arg)
        {
            LogHelper.Write("speed up : " + arg);
            return true;
        }
    }
}
