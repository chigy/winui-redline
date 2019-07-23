using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace RedlinesProject
{
    class SizeRedline
    {
        private const double _redlineSize = 20;

        public static void Draw(UIElement container, FrameworkElement fe, Canvas target)
        {
            Draw(container, fe, target, 0, new int[4]);
        }

        private static void DebugSpaces(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Debug.Write("  ");
            }
        }

        private static void AddRedline(double x, double y, RedlineSide side, double value, Canvas target, int depth, FrameworkElement fe, string propertyName)
        {
            DebugSpaces(depth);
            Debug.WriteLine(fe.Name + "." + propertyName + " = " + value);

            Redline redline = new Redline(side, value);
            Canvas.SetLeft(redline, x);
            Canvas.SetTop(redline, y);
            target.Children.Add(redline);
        }

        public static void Draw(UIElement container, FrameworkElement fe, Canvas target, int depth, int[] redlineCount)
        {
            var width = fe.Width;
            var height = fe.Height;

            // If a tree is collapsed, don't measure it.
            if (fe.Visibility == Visibility.Collapsed || fe.ActualWidth <= 0 || fe.ActualHeight <= 0)
            {
                return;
            }

            // Recurse through children first
            var childCount = VisualTreeHelper.GetChildrenCount(fe);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(fe, i) as FrameworkElement;
                if (child != null)
                {
                    Draw(container, child, target, depth + 1, redlineCount);
                }
            }

            // Redlines should only be reported underneath the control level
            if (depth == 0)
            {
                Debug.WriteLine(fe.Name);
            }
            else
            {
                GeneralTransform t = fe.TransformToVisual(container);
                Windows.Foundation.Point pos = t.TransformPoint(new Windows.Foundation.Point(0, 0));

                if (!Double.IsNaN(width) && width > 0)
                {
                    double x = pos.X;
                    double y = (pos.Y - _redlineSize) - redlineCount[(int)RedlineSide.Top] * _redlineSize;

                    AddRedline(x, y, RedlineSide.Top, width, target, depth, fe, "Width");
                    redlineCount[(int)RedlineSide.Top]++;
                }

                if (!Double.IsNaN(height) && height > 0)
                {
                    double x = (pos.X - _redlineSize) - redlineCount[(int)RedlineSide.Left] * _redlineSize;
                    double y = pos.Y;

                    AddRedline(x, y, RedlineSide.Left, height, target, depth, fe, "Height");
                    redlineCount[(int)RedlineSide.Left]++;
                }

                var margin = fe.Margin;

                // Padding isn't on FrameworkElement, but it's on a variety of different things; just look up the property directly
                var padding = new Thickness(0);
                var paddingProp = fe.GetType().GetProperty("Padding");
                if (paddingProp != null)
                {
                    padding = (Thickness)(paddingProp.GetValue(fe, null));
                }

                bool hasBottomRedline = false;
                bool hasRightRedline = false;

                if (padding.Left > 0)
                {
                    double x = pos.X;
                    double y = pos.Y + fe.ActualHeight + 1 + redlineCount[(int)RedlineSide.Bottom] * _redlineSize;

                    AddRedline(x, y, RedlineSide.Bottom, padding.Left, target, depth, fe, "Padding.Left");
                }

                if (padding.Right > 0)
                {
                    double x = pos.X + fe.ActualWidth - padding.Right;
                    double y = pos.Y + fe.ActualHeight + 1 + redlineCount[(int)RedlineSide.Bottom] * _redlineSize;

                    AddRedline(x, y, RedlineSide.Bottom, padding.Right, target, depth, fe, "Padding.Right");
                }

                if (padding.Top > 0)
                {
                    double x = pos.X + fe.ActualWidth + 1 + redlineCount[(int)RedlineSide.Right] * _redlineSize;
                    double y = pos.Y;

                    AddRedline(x, y, RedlineSide.Right, padding.Top, target, depth, fe, "Padding.Top");
                }

                if (padding.Bottom > 0)
                {
                    double x = pos.X + fe.ActualWidth + 1 + redlineCount[(int)RedlineSide.Right] * _redlineSize;
                    double y = pos.Y + fe.ActualHeight - padding.Bottom;

                    AddRedline(x, y, RedlineSide.Right, padding.Bottom, target, depth, fe, "Padding.Bottom");
                }

                if (margin.Left > 0)
                {
                    double x = pos.X - margin.Left;
                    double y = pos.Y + fe.ActualHeight + 1 + redlineCount[(int)RedlineSide.Bottom] * _redlineSize;

                    AddRedline(x, y, RedlineSide.Bottom, margin.Left, target, depth, fe, "Margin.Left");
                }

                if (margin.Right > 0)
                {
                    double x = pos.X + fe.ActualWidth;
                    double y = pos.Y + fe.ActualHeight + 1 + redlineCount[(int)RedlineSide.Bottom] * _redlineSize;

                    AddRedline(x, y, RedlineSide.Bottom, margin.Right, target, depth, fe, "Margin.Right");
                }

                if (margin.Top > 0)
                {
                    double x = pos.X + fe.ActualWidth + 1 + redlineCount[(int)RedlineSide.Right] * _redlineSize;
                    double y = pos.Y - margin.Top;

                    AddRedline(x, y, RedlineSide.Right, margin.Top, target, depth, fe, "Margin.Top");
                }

                if (margin.Bottom > 0)
                {
                    double x = pos.X + fe.ActualWidth + 1 + redlineCount[(int)RedlineSide.Right] * _redlineSize;
                    double y = pos.Y + fe.ActualHeight;

                    AddRedline(x, y, RedlineSide.Right, margin.Bottom, target, depth, fe, "Margin.Bottom");
                }

                if (hasBottomRedline)
                {
                    redlineCount[(int)RedlineSide.Bottom]++;
                }
                if (hasRightRedline)
                {
                    redlineCount[(int)RedlineSide.Right]++;
                }
            }
        }
    }
}
