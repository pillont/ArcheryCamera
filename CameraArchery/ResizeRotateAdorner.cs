using CameraArchery.Thumbs;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CameraArchery.Adorners

{
    public class ResizeRotateAdorner : Adorner
    {
        private Timer timer;
        // Resizing adorner uses Thumbs for visual elements.  
        // The Thumbs have built-in mouse input handling.
        ResizeThumb topLeft, topRight, bottomLeft, bottomRight;
        RotateThumb rotation;

        // To store and manage the adorner's visual children.
        VisualCollection visualChildren;

        // Initialize the ResizingAdorner.
        public ResizeRotateAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            Visibility = Visibility.Collapsed;
            adornedElement.GotMouseCapture += adornedElement_GotMouseCapture;
        
            visualChildren = new VisualCollection(this);

            // Call a helper method to initialize the Thumbs
            // with a customized cursors.
            BuildAdornerCorner(ref topLeft, Cursors.SizeNWSE, HorizontalAlignment.Left, VerticalAlignment.Top);
            BuildAdornerCorner(ref topRight, Cursors.SizeNESW, HorizontalAlignment.Right, VerticalAlignment.Top);
            BuildAdornerCorner(ref bottomLeft, Cursors.SizeNESW, HorizontalAlignment.Left, VerticalAlignment.Bottom);
            BuildAdornerCorner(ref bottomRight, Cursors.SizeNWSE, HorizontalAlignment.Right, VerticalAlignment.Bottom);


            rotation = new RotateThumb(AdornedElement as ContentControl);
            rotation.DragCompleted += (t, e) => RestartTimer();
            rotation.DragStarted += (t, e) => StopTimer();

            // Set some arbitrary visual characteristics.
            rotation.Height = rotation.Width = 10;
            rotation.Opacity = 0.40;
            rotation.Background = new SolidColorBrush(Colors.Green);

            visualChildren.Add(rotation);
     
        }

        void adornedElement_GotMouseCapture(object sender, MouseEventArgs e)
        {
            
            if (timer != null)
                timer.Stop();

            RestartTimer();   
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(()=>Visibility = Visibility.Collapsed);
        }

        void StopTimer()
        {
            timer.Stop();
        }

        
        void RestartTimer()
        {
            Visibility = Visibility.Visible;

            timer = new Timer(5000) { AutoReset = false };
            timer.Start();
            timer.Elapsed += timer_Elapsed;
        }

        // Arrange the Adorners.
        protected override Size ArrangeOverride(Size finalSize)
        {
            // desiredWidth and desiredHeight are the width and height of the element that's being adorned.  
            // These will be used to place the ResizingAdorner at the corners of the adorned element.  
            double desiredWidth = AdornedElement.DesiredSize.Width;
            double desiredHeight = AdornedElement.DesiredSize.Height;
            // adornerWidth & adornerHeight are used for placement as well.
            double adornerWidth = this.DesiredSize.Width;
            double adornerHeight = this.DesiredSize.Height;

            topLeft.Arrange(new Rect(0, 0, adornerWidth, adornerHeight));
            topRight.Arrange(new Rect(0, 0, adornerWidth, adornerHeight));
            bottomLeft.Arrange(new Rect(0, 0, adornerWidth, adornerHeight));
            bottomRight.Arrange(new Rect(0, 0, adornerWidth, adornerHeight));
            rotation.Arrange(new Rect(30, 0, adornerWidth, adornerHeight));

            // Return the final size.
            return finalSize;
        }

        // Helper method to instantiate the corner Thumbs, set the Cursor property, 
        // set some appearance properties, and add the elements to the visual tree.
        void BuildAdornerCorner(ref ResizeThumb cornerThumb, Cursor customizedCursor, HorizontalAlignment horizontalAlign, VerticalAlignment verticalAlign)
        {
            if (cornerThumb != null) return;

            cornerThumb = new ResizeThumb(AdornedElement as ContentControl, verticalAlign, horizontalAlign);
            cornerThumb.DragCompleted += (t, e) => RestartTimer();
            cornerThumb.DragStarted += (t, e) => StopTimer();

            // Set some arbitrary visual characteristics.
            cornerThumb.Cursor = customizedCursor;
            cornerThumb.Height = cornerThumb.Width = 10;
            cornerThumb.Opacity = 0.40;
            cornerThumb.Background = new SolidColorBrush(Colors.MediumBlue);

            visualChildren.Add(cornerThumb);
        }

        // This method ensures that the Widths and Heights are initialized.  Sizing to content produces
        // Width and Height values of Double.NaN.  Because this Adorner explicitly resizes, the Width and Height
        // need to be set first.  It also sets the maximum size of the adorned element.
        void EnforceSize(FrameworkElement adornedElement)
        {
            if (adornedElement.Width.Equals(Double.NaN))
                adornedElement.Width = adornedElement.DesiredSize.Width;
            if (adornedElement.Height.Equals(Double.NaN))
                adornedElement.Height = adornedElement.DesiredSize.Height;

            FrameworkElement parent = adornedElement.Parent as FrameworkElement;
            if (parent != null)
            {
                adornedElement.MaxHeight = parent.ActualHeight;
                adornedElement.MaxWidth = parent.ActualWidth;
            }
        }
        // Override the VisualChildrenCount and GetVisualChild properties to interface with 
        // the adorner's visual collection.
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
    }
}