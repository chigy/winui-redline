using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace RedlinesProject
{
    public sealed partial class RedlineViewer : UserControl
    {
        public RedlineViewer(Type controlType)
        {
            this.InitializeComponent();

            Control c = Activator.CreateInstance(controlType) as Control;
            c.Loaded += Control_Loaded;

            ControlStateViewer.MakeControlPretty(c, controlType);

            LayoutRoot.Children.Insert(0, c);
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            Control c = sender as Control;
            Draw(LayoutRoot, c, RedlineCanvas);
        }

        private const double _redlineSpace = 20;
        private const double _redlineMinSize = 9;

        private List<string> _unwantedNames = new List<string>() { "HorizontalDecreaseRect" };

        public void Draw(UIElement container, FrameworkElement fe, Canvas target)
        {
            Draw(container, fe, target, 0, new int[4]);
        }

        private void DebugSpaces(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Debug.Write("  ");
            }
        }

        private void AddRedline(double x, double y, RedlineSide side, double value, Canvas target, int depth, FrameworkElement fe, string propertyName)
        {
            DebugSpaces(depth);
            Debug.WriteLine(fe.Name + "." + propertyName + " = " + value);

            Redline redline = new Redline(side, _redlineMinSize, value);
            Canvas.SetLeft(redline, x);
            Canvas.SetTop(redline, y);
            target.Children.Add(redline);
        }

        private void Draw(UIElement container, FrameworkElement fe, Canvas target, int depth, int[] redlineCount)
        {
            // If a tree is collapsed, don't measure it.
            if (fe.Visibility == Visibility.Collapsed || fe.ActualWidth <= 0 || fe.ActualHeight <= 0)
            {
                return;
            }

            // To make results more attractive, some elements don't need redlines drawn
            if (_unwantedNames.Contains(fe.Name))
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

            GeneralTransform t = fe.TransformToVisual(container);
            Windows.Foundation.Point pos = t.TransformPoint(new Windows.Foundation.Point(0, 0));

            // Redlines should only be reported underneath the control level
            if (depth == 0)
            {
                Debug.WriteLine(fe.Name);

                // Draw bounding rectangle
                Rectangle rc = new Rectangle();
                rc.Width = fe.ActualWidth;
                rc.Height = fe.ActualHeight;
                rc.Stroke = new SolidColorBrush(Windows.UI.Color.FromArgb(150, 100, 150, 200));
                rc.StrokeThickness = 1;
                rc.StrokeDashArray = new DoubleCollection() { 3 };
                Canvas.SetLeft(rc, pos.X);
                Canvas.SetTop(rc, pos.Y);
                target.Children.Add(rc);
            }
            else
            {
                var width = fe.Width;
                var height = fe.Height;

                if (!Double.IsNaN(width) && width > 0)
                {
                    double x = pos.X;
                    double y = (pos.Y - _redlineSpace) - redlineCount[(int)RedlineSide.Top] * _redlineSpace;

                    AddRedline(x, y, RedlineSide.Top, width, target, depth, fe, "Width");
                    redlineCount[(int)RedlineSide.Top]++;
                }

                if (!Double.IsNaN(height) && height > 0)
                {
                    double x = (pos.X - _redlineSpace) - redlineCount[(int)RedlineSide.Left] * _redlineSpace;
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
                    double y = pos.Y + fe.ActualHeight + 1 + redlineCount[(int)RedlineSide.Bottom] * _redlineSpace;

                    AddRedline(x, y, RedlineSide.Bottom, padding.Left, target, depth, fe, "Padding.Left");
                }

                if (padding.Right > 0)
                {
                    double x = pos.X + fe.ActualWidth - padding.Right;
                    double y = pos.Y + fe.ActualHeight + 1 + redlineCount[(int)RedlineSide.Bottom] * _redlineSpace;

                    AddRedline(x, y, RedlineSide.Bottom, padding.Right, target, depth, fe, "Padding.Right");
                }

                if (padding.Top > 0)
                {
                    double x = pos.X + fe.ActualWidth + 1 + redlineCount[(int)RedlineSide.Right] * _redlineSpace;
                    double y = pos.Y;

                    AddRedline(x, y, RedlineSide.Right, padding.Top, target, depth, fe, "Padding.Top");
                }

                if (padding.Bottom > 0)
                {
                    double x = pos.X + fe.ActualWidth + 1 + redlineCount[(int)RedlineSide.Right] * _redlineSpace;
                    double y = pos.Y + fe.ActualHeight - padding.Bottom;

                    AddRedline(x, y, RedlineSide.Right, padding.Bottom, target, depth, fe, "Padding.Bottom");
                }

                if (margin.Left > 0)
                {
                    double x = pos.X - margin.Left;
                    double y = pos.Y + fe.ActualHeight + 1 + redlineCount[(int)RedlineSide.Bottom] * _redlineSpace;

                    AddRedline(x, y, RedlineSide.Bottom, margin.Left, target, depth, fe, "Margin.Left");
                }

                if (margin.Right > 0)
                {
                    double x = pos.X + fe.ActualWidth;
                    double y = pos.Y + fe.ActualHeight + 1 + redlineCount[(int)RedlineSide.Bottom] * _redlineSpace;

                    AddRedline(x, y, RedlineSide.Bottom, margin.Right, target, depth, fe, "Margin.Right");
                }

                if (margin.Top > 0)
                {
                    double x = pos.X + fe.ActualWidth + 1 + redlineCount[(int)RedlineSide.Right] * _redlineSpace;
                    double y = pos.Y - margin.Top;

                    AddRedline(x, y, RedlineSide.Right, margin.Top, target, depth, fe, "Margin.Top");
                }

                if (margin.Bottom > 0)
                {
                    double x = pos.X + fe.ActualWidth + 1 + redlineCount[(int)RedlineSide.Right] * _redlineSpace;
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
