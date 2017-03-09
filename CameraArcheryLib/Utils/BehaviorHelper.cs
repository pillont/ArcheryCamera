using System.Linq;
using System.Windows;
using System.Windows.Interactivity;

namespace CameraArcheryLib.Utils
{
    public static class BehaviorHelper
    {
        /// <summary>
        /// function to add behavior on associated object if not associated to other behavior of same type
        /// </summary>
        /// <typeparam name="T">type of the behavior</typeparam>
        /// <param name="behavior">behavior to add</param>
        /// <param name="obj">associated object</param>
        /// <returns>behavior -> default is behavior not attach</returns>
        public static Behavior<T> AddSingleBehavior<T>(Behavior<T> behavior, T obj) where T : DependencyObject
        {
            var behaviors = Interaction.GetBehaviors(obj);
            if (!behaviors.Any((b) => b.GetType() == behavior.GetType()))
            {
                behaviors.Add(behavior);
                return behavior;
            }
            return default(Behavior<T>);
        }
    }
}
