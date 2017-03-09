using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Interactivity;
using System.Windows;
using CameraArcheryLib.Utils;

namespace CameraArcheryTest.Utils
{
    [TestClass]
    public class BehaviorHelperTest
    {
        internal class BehaviorTest : Behavior<DependencyObject>
        {
        }

        [TestMethod]
        public void TestAddSingleBehavior()
        {
            var obj = new DependencyObject();
            var behavior = new BehaviorTest();
            var res = BehaviorHelper.AddSingleBehavior(behavior, obj);

            Assert.IsTrue(Interaction.GetBehaviors(obj).Count == 1);
            Assert.IsTrue(res == behavior);

            res = BehaviorHelper.AddSingleBehavior(new BehaviorTest(), obj);

            Assert.IsTrue(Interaction.GetBehaviors(obj).Count == 1);
            Assert.IsTrue(res == default(BehaviorTest));
        }
    }
}
