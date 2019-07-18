using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace RedlinesProject
{
    class SizeRedline
    {
        public static void Draw(UIElement container, Control control, Canvas target)
        {
            /*var size = control.ActualSize;
            var width = control.Width;
            var height = control.Height;
            var minWidth = control.MinWidth;
            var minHeight = control.MinHeight;
            var maxWidth = control.MaxWidth;
            var maxHeight = control.MaxHeight;*/

            var padding = control.Padding;

            GeneralTransform t = control.TransformToVisual(container);
            Windows.Foundation.Point pos = t.TransformPoint(new Windows.Foundation.Point(0, 0));

            if (padding.Left > 0)
            {
                // ### this stuff needs to go into the redline code.
                double x = pos.X;
                double y = pos.Y + control.ActualHeight + 1;

                Redline redline = new Redline(RedlineSide.Bottom, padding.Left);
                Canvas.SetLeft(redline, x);
                Canvas.SetTop(redline, y);
                target.Children.Add(redline);
            }

            if (padding.Right > 0)
            {
                double x = pos.X + control.ActualWidth - padding.Right;
                double y = pos.Y + control.ActualHeight + 1;

                Redline redline = new Redline(RedlineSide.Bottom, padding.Right);
                Canvas.SetLeft(redline, x);
                Canvas.SetTop(redline, y);
                target.Children.Add(redline);
            }

            if (padding.Top > 0)
            {
                double x = pos.X + control.ActualWidth + 1;
                double y = pos.Y;

                Redline redline = new Redline(RedlineSide.Right, padding.Top);
                Canvas.SetLeft(redline, x);
                Canvas.SetTop(redline, y);
                target.Children.Add(redline);
            }

            if (padding.Bottom > 0)
            {
                double x = pos.X + control.ActualWidth + 1;
                double y = pos.Y + control.ActualHeight - padding.Bottom;

                Redline redline = new Redline(RedlineSide.Right, padding.Bottom);
                Canvas.SetLeft(redline, x);
                Canvas.SetTop(redline, y);
                target.Children.Add(redline);
            }
        }
    }
}
