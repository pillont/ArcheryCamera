using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Collections.Generic;

namespace CameraArchery.Thumbs
{
    public class ResizeThumb : Thumb
    {
        private RotateTransform rotateTransform;
        private double angle;
        private Point transformOrigin;
        private ContentControl designerItem;
        private Canvas canvas;

        public ResizeThumb(ContentControl adornedElement, VerticalAlignment verticalAlign, HorizontalAlignment horizontamAlign)
        {
            VerticalAlignment = verticalAlign;
            HorizontalAlignment = horizontamAlign;

            designerItem = adornedElement;
            DragStarted += new DragStartedEventHandler(this.ResizeThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);
        }

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.canvas = VisualTreeHelper.GetParent(this.designerItem) as Canvas;
            
            if (this.canvas != null)
            {
                this.transformOrigin = this.designerItem.RenderTransformOrigin;

                this.rotateTransform = this.designerItem.RenderTransform as RotateTransform;
                if (this.rotateTransform != null)
                {
                    this.angle = this.rotateTransform.Angle * Math.PI / 180.0;
                }
                else
                {
                    this.angle = 0.0d;
                }

                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.canvas);
            }
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double deltaVertical, deltaHorizontal;

            switch (VerticalAlignment)
            {
                case System.Windows.VerticalAlignment.Bottom:
                    deltaVertical = Math.Min(-e.VerticalChange, this.designerItem.ActualHeight - this.designerItem.MinHeight);
                    Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))));
                    Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) - deltaVertical * this.transformOrigin.Y * Math.Sin(-this.angle));
                    this.designerItem.Height -= deltaVertical;
                    break;
                case System.Windows.VerticalAlignment.Top:
                    deltaVertical = Math.Min(e.VerticalChange, this.designerItem.ActualHeight - this.designerItem.MinHeight);
                    Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + deltaVertical * Math.Cos(-this.angle) + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))));
                    Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + deltaVertical * Math.Sin(-this.angle) - (this.transformOrigin.Y * deltaVertical * Math.Sin(-this.angle)));
                    this.designerItem.Height -= deltaVertical;
                    break;
                default:
                    break;
            }

            switch (HorizontalAlignment)
            {
                case System.Windows.HorizontalAlignment.Left:
                    deltaHorizontal = Math.Min(e.HorizontalChange, this.designerItem.ActualWidth - this.designerItem.MinWidth);
                    Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + deltaHorizontal * Math.Sin(this.angle) - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle));
                    Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + deltaHorizontal * Math.Cos(this.angle) + (this.transformOrigin.X * deltaHorizontal * (1 - Math.Cos(this.angle))));
                    this.designerItem.Width -= deltaHorizontal;
                    break;
                case System.Windows.HorizontalAlignment.Right:
                    deltaHorizontal = Math.Min(-e.HorizontalChange, this.designerItem.ActualWidth - this.designerItem.MinWidth);
                    Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle));
                    Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + (deltaHorizontal * this.transformOrigin.X * (1 - Math.Cos(this.angle))));
                    this.designerItem.Width -= deltaHorizontal;
                    break;
                default:
                    break;
            }
            e.Handled = true;
        }
    }
}
