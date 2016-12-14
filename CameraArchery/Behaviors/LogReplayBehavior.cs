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

        private bool CustomReplayComponent_OnStartClick(bool isStart, bool isPause)
        {
            if (!isStart)
                LogHelper.Write("start click");
            else if (isPause)
                LogHelper.Write("reply click");
            else
                LogHelper.Write("Pause click");

            return true;
        }

        private bool CustomReplayComponent_OnStopClick()
        {
            LogHelper.Write("stop click");
            return true;

        }

        private bool CustomReplayComponent_OnDeleteFile(DataBinding.VideoFile arg)
        {
            LogHelper.Write("delete file " + arg.FullName);
            return true;
        }

        private bool CustomReplayComponent_OnFrameClick()
        {
            LogHelper.Write("on frame click");
            return true;
        }

        private void CustomReplayComponent_OnListSelectionChange(DataBinding.VideoFile obj)
        {
            LogHelper.Write("selected file change : " + obj);
        }

        private void CustomReplayComponent_OnMediaEnded()
        {
            LogHelper.Write("media is ended");
        }

        private void CustomReplayComponent_OnSliderCapture(double obj)
        {
            LogHelper.Write("slider is capture at " + obj + " sec");
        }

        private bool CustomReplayComponent_OnSliderChange(double arg)
        {
            LogHelper.Write("slider is change to " + arg + " sec");
            return true;
        }

        private bool CustomReplayComponent_OnSpeedDownClick(double arg)
        {
            LogHelper.Write("speed down : " + arg);
            return true;
        }

        private bool CustomReplayComponent_OnSpeedUpClick(double arg)
        {
            LogHelper.Write("speed up : " + arg);
            return true;
        }
    }
}
