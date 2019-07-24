using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace RedlinesProject
{
    public sealed partial class ColorViewer : UserControl
    {
        public HashSet<string> UsedBrushes { get { return m_usedBrushes; } }

        public HashSet<string> m_usedBrushes = new HashSet<string>();
        private BrushNameDictionary _brushNameDict = new BrushNameDictionary();
        private List<RedLineInfo> redlineInfos = new List<RedLineInfo>();

        public ColorViewer(Type type)
        {
            this.InitializeComponent();

            Control control = Activator.CreateInstance(type) as Control;
            control.HorizontalAlignment = HorizontalAlignment.Left;
            control.Loaded += Control_Loaded;

            ControlStateViewer.MakeControlPretty(control, type);

            LayoutRoot.Children.Add(control);
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            TraverseVisualToPrepareRedline(sender as DependencyObject);
            DrawColorLabels();
        }

        private void DrawBoundingRectangle(FrameworkElement frameworkElement)
        {
            Rectangle rc = new Rectangle();
            rc.Width = frameworkElement.ActualWidth;
            rc.Height = frameworkElement.ActualHeight;
            rc.Stroke = new SolidColorBrush(Windows.UI.Color.FromArgb(150, 100, 150, 200));
            rc.StrokeThickness = 1;
            rc.StrokeDashArray = new DoubleCollection() { 3 };
            var transform = frameworkElement.TransformToVisual(LayoutRoot);
            var position = transform.TransformPoint(new Point(0, 0));
            Canvas.SetLeft(rc, position.X);
            Canvas.SetTop(rc, position.Y);
            RedlineCanvas.Children.Add(rc);
        }

        private void TraverseVisualToPrepareRedline(DependencyObject obj)
        {
            int count = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                FrameworkElement child = VisualTreeHelper.GetChild(obj, i) as FrameworkElement;
                if (child != null && child.Visibility == Visibility.Visible)
                {
                    PrepareRedlineInfo(child);
                    TraverseVisualToPrepareRedline(child);
                }
            }
        }

        private void PrepareRedlineInfo(FrameworkElement frameworkElement)
        {
            List<string> brushes = new List<string>();
            foreach (var propertyInfo in frameworkElement.GetType().GetProperties())
            {
                var propertyValue = propertyInfo.GetValue(frameworkElement);
                // Ignore hight contract brushes
                if (!propertyInfo.Name.StartsWith("FocusVisual") && propertyValue is Brush)
                {
                    var brush = propertyValue as Brush;
                    var colorProperty = brush.GetType().GetProperty("Color");
                    if (colorProperty != null)
                    {
                        if (_brushNameDict.ContainsKey(brush))
                        {
                            string text = propertyInfo.Name + ": " + BrushNameDictionary.GetShortName(_brushNameDict[brush]);
                            brushes.Add(text);
                            m_usedBrushes.Add(_brushNameDict[brush]);
                        }
                    }
                }
            }

            if (brushes.Count > 0)
            {
                DrawBoundingRectangle(frameworkElement);
                var t = frameworkElement.TransformToVisual(LayoutRoot);
                var transform = t.TransformPoint(new Point(0, 0));
                var content = String.Join(", ", brushes);
                Point position = new Point(transform.X, transform.Y);
                RedLineInfo redlineInfo = new RedLineInfo(position, frameworkElement, content);
                redlineInfos.Add(redlineInfo);
            }
        }

        private void DrawColorLabels()
        {
            RedlineSide redlineSide = RedlineSide.Top;
            double redlineHeight = 20;
            List<double> xPositions = new List<double>();
            // Reverse sort by x, draw the lines from right to left to avoid line corssing
            redlineInfos.Sort((a, b) => b.TargetHorizontalCenter().X.CompareTo(a.TargetHorizontalCenter().X));
            for (int i = 0; i < redlineInfos.Count; i++)
            {
                var redline = redlineInfos[i];
                var label = new ColorLabel(redline.Content, redlineHeight, redlineSide);
                var position = redline.TargetHorizontalCenter();
                var x = position.X;

                // Make sure labels are not too close to each other
                if(i >= 2)
                {
                    // We switch side(top/bottom) for each draw, [i-2] is the previous redline thats on the same side.
                    var previousX = xPositions[i - 2];
                    if (x > previousX || Math.Abs(x - previousX) < 5)
                    {
                        x = previousX - 5;
                    }
                }

                xPositions.Add(x);
                Canvas.SetLeft(label, x);
                Canvas.SetTop(label, redlineSide == RedlineSide.Bottom ? position.Y + redline.TargetHeight : position.Y);
                RedlineCanvas.Children.Add(label);

                // Flip drawing position and grow redline height to avoid overlapping
                if (redlineSide == RedlineSide.Top)
                {
                    redlineSide = RedlineSide.Bottom;
                }
                else
                {
                    redlineSide = RedlineSide.Top;
                    redlineHeight += 30;
                }
            }
        }
    }
}
