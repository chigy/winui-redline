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
        private const double _redlineSpace = 20;
        private const double _redlineMinSize = 9;
        private int[] _redlineCount;
        private Control _control;
        Windows.Foundation.Point _controlPos;

        private List<string> _unwantedNames = new List<string>() { "HorizontalDecreaseRect" };

        public RedlineViewer(Type controlType)
        {
            this.InitializeComponent();

            _control = Activator.CreateInstance(controlType) as Control;
            _control.Loaded += Control_Loaded;

            ControlStateViewer.MakeControlPretty(_control, controlType);

            LayoutRoot.Children.Insert(0, _control);
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            if (_control.GetType() == typeof(MenuFlyoutItem))
            {
                // Need to pause before drawing to let the control settle.
                _control.SizeChanged += MenuFlyoutItem_SizeChanged;
                VisualStateManager.GoToState(_control, "IconPlaceholder", false);
            }
            else
            {
                Draw();
            }
        }

        private void MenuFlyoutItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw();
        }

        public void Draw()
        {
            RedlineCanvas.Children.Clear();
            _redlineCount = new int[4];

            GeneralTransform t = _control.TransformToVisual(LayoutRoot);
            _controlPos = t.TransformPoint(new Windows.Foundation.Point(0, 0));

            Debug.WriteLine(_control.Name);

            // Draw bounding rectangle
            Rectangle rc = new Rectangle();
            rc.Width = _control.ActualWidth;
            rc.Height = _control.ActualHeight;
            rc.Stroke = new SolidColorBrush(Windows.UI.Color.FromArgb(150, 100, 150, 200));
            rc.StrokeThickness = 1;
            rc.StrokeDashArray = new DoubleCollection() { 3 };
            Canvas.SetLeft(rc, _controlPos.X);
            Canvas.SetTop(rc, _controlPos.Y);
            RedlineCanvas.Children.Add(rc);

            Draw(LayoutRoot, _control, RedlineCanvas, 0);
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

            double addedSpace = _redlineCount[(int)side] * _redlineSpace;
            double size = _redlineMinSize + addedSpace;

            GeneralTransform t = fe.TransformToVisual(target);
            Windows.Foundation.Point pos = t.TransformPoint(new Windows.Foundation.Point(0, 0));

            switch (side)
            {
                case RedlineSide.Left:
                    x -= addedSpace;
                    size += pos.X - _controlPos.X;
                    break;

                case RedlineSide.Top:
                    y -= addedSpace;
                    size += pos.Y - _controlPos.Y;
                    break;

                case RedlineSide.Right:
                    x += addedSpace;
                    double extendX = (_controlPos.X + _control.ActualWidth) - (pos.X + fe.ActualWidth);
                    x -= extendX;
                    size += extendX;
                    break;

                case RedlineSide.Bottom:
                    y += addedSpace;
                    double extendY = (_controlPos.Y + _control.ActualHeight) - (pos.Y + fe.ActualHeight);
                    y -= extendY;
                    size += extendY;
                    break;
            }

            Redline redline = new Redline(side, size, value);
            Canvas.SetLeft(redline, x);
            Canvas.SetTop(redline, y);

            target.Children.Add(redline);
        }

        private void Draw(UIElement container, FrameworkElement fe, Canvas target, int depth)
        {
            // If a tree is collapsed, don't measure it.
            if (fe.Visibility == Visibility.Collapsed || fe.ActualWidth <= 0 || fe.ActualHeight <= 0 || fe.Opacity == 0)
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
                    Draw(container, child, target, depth + 1);
                }
            }

            GeneralTransform t = fe.TransformToVisual(container);
            Windows.Foundation.Point pos = t.TransformPoint(new Windows.Foundation.Point(0, 0));

            bool hasBottomRedline = false;
            bool hasRightRedline = false;

            // Size and margins should only be reported underneath the control level
            if (depth > 0)
            {
                var width = fe.Width;
                var height = fe.Height;

                if (!Double.IsNaN(width) && width > 0)
                {
                    double x = pos.X;
                    double y = (_controlPos.Y - _redlineSpace) - 1;

                    AddRedline(x, y, RedlineSide.Top, width, target, depth, fe, "Width");
                    _redlineCount[(int)RedlineSide.Top]++;
                }

                if (!Double.IsNaN(height) && height > 0)
                {
                    double x = (_controlPos.X - _redlineSpace) - 1;
                    double y = pos.Y;

                    AddRedline(x, y, RedlineSide.Left, height, target, depth, fe, "Height");
                    _redlineCount[(int)RedlineSide.Left]++;
                }

                var margin = fe.Margin;

                if (margin.Left > 0)
                {
                    double x = pos.X - margin.Left;
                    double y = _controlPos.Y + _control.ActualHeight + 1;

                    AddRedline(x, y, RedlineSide.Bottom, margin.Left, target, depth, fe, "Margin.Left");
                }

                if (margin.Right > 0)
                {
                    double x = pos.X + fe.ActualWidth;
                    double y = _controlPos.Y + _control.ActualHeight + 1;

                    AddRedline(x, y, RedlineSide.Bottom, margin.Right, target, depth, fe, "Margin.Right");
                }

                if (margin.Top > 0)
                {
                    double x = _controlPos.X + _control.ActualWidth + 1;
                    double y = pos.Y - margin.Top;

                    AddRedline(x, y, RedlineSide.Right, margin.Top, target, depth, fe, "Margin.Top");
                }

                if (margin.Bottom > 0)
                {
                    double x = _controlPos.X + _control.ActualWidth + 1;
                    double y = pos.Y + fe.ActualHeight;

                    AddRedline(x, y, RedlineSide.Right, margin.Bottom, target, depth, fe, "Margin.Bottom");
                }
            }

            // Padding isn't on FrameworkElement, but it's on a variety of different things; just look up the property directly
            var padding = new Thickness(0);
            var paddingProp = fe.GetType().GetProperty("Padding");
            if (paddingProp != null)
            {
                padding = (Thickness)(paddingProp.GetValue(fe, null));
            }

            if (padding.Left > 0)
            {
                double x = pos.X;
                double y = _controlPos.Y + _control.ActualHeight + 1;

                AddRedline(x, y, RedlineSide.Bottom, padding.Left, target, depth, fe, "Padding.Left");
            }

            if (padding.Right > 0)
            {
                double x = pos.X + fe.ActualWidth - padding.Right;
                double y = _controlPos.Y + _control.ActualHeight + 1;

                AddRedline(x, y, RedlineSide.Bottom, padding.Right, target, depth, fe, "Padding.Right");
            }

            if (padding.Top > 0)
            {
                double x = _controlPos.X + _control.ActualWidth + 1;
                double y = pos.Y;

                AddRedline(x, y, RedlineSide.Right, padding.Top, target, depth, fe, "Padding.Top");
            }

            if (padding.Bottom > 0)
            {
                double x = _controlPos.X + _control.ActualWidth + 1;
                double y = pos.Y + fe.ActualHeight - padding.Bottom;

                AddRedline(x, y, RedlineSide.Right, padding.Bottom, target, depth, fe, "Padding.Bottom");
            }

            if (hasBottomRedline)
            {
                _redlineCount[(int)RedlineSide.Bottom]++;
            }
            if (hasRightRedline)
            {
                _redlineCount[(int)RedlineSide.Right]++;
            }
        }
    }
}
