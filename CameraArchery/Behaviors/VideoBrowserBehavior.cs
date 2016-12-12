using CameraArchery.UsersControl;
using CameraArcheryLib.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace CameraArcheryLib.Behaviors
{
    public class VideoBrowserBehavior : Behavior<CustomBrowser>
    {
        public VideoBrowserBehavior()
        {
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DefaultFile = SettingFactory.CurrentSetting.VideoFolder;
            AssociatedObject.OnPropertyChanged += testc;
        }
    }
}
