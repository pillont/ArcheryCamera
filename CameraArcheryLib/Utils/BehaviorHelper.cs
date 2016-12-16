using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace CameraArcheryLib.Utils
{
    public static class BehaviorHelper
    {
        public static Behavior<T> AddSingleBehavior<T>(Behavior<T> behavior, T obj) where T : DependencyObject
        {
            var behaviors = Interaction.GetBehaviors(obj);
            if (!behaviors.Any((b) => b.GetType() == behavior.GetType()))
            {
                behaviors.Add(behavior);
                return behavior;
            }
            return null;
        }
    }
}
