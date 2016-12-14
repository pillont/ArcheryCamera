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
    public class VideoBrowserBehavior : Behavior<CustomBrowser>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PropertyChanged +=AssociatedObject_PropertyChanged; 
        }

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
