using CameraArchery.UsersControl;
using CameraArcheryLib.Controller;
using CameraArcheryLib.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;

namespace CameraArchery.Behaviors
{
    /// <summary>
    /// behavior of the video browsrer
    /// </summary>
    public class VideoBrowserBehavior : Behavior<CustomBrowser>
    {
        /// <summary>
        /// on attached event
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PropertyChanged +=AssociatedObject_PropertyChanged; 
        }

        /// <summary>
        /// event when the property change
        /// <para>if uri change => change in the current setting</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObject_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedUri")
            {
                if (SettingFactory.CurrentSetting.VideoFolder != AssociatedObject.SelectedUri.OriginalString)
                    SettingController.UpdateUri(AssociatedObject.SelectedUri);
            }
        }
    }
}
