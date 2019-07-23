using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace RedlinesProject
{
    public sealed partial class ColorViewer : UserControl
    {
        public HashSet<string> UsedBrushes { get { return m_usedBrushes; } }

        public HashSet<string> m_usedBrushes = new HashSet<string>();
        private RedlineSide _redlineSide = RedlineSide.Top;
        private double _redlineHeight = 20;
        private BrushNameDictionary _brushNameDict = new BrushNameDictionary();

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
            TraverseVisualTree(sender as DependencyObject);
        }

        private void TraverseVisualTree(DependencyObject obj)
        {
            int count = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                FrameworkElement child = VisualTreeHelper.GetChild(obj, i) as FrameworkElement;
                if (child != null && child.Visibility == Visibility.Visible)
                {
                    DrawColorBrushRedLine(child);
                    TraverseVisualTree(child);
                }
            }
        }

        private void DrawColorBrushRedLine(FrameworkElement frameworkElement)
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
                var t = frameworkElement.TransformToVisual(LayoutRoot);
                var transform = t.TransformPoint(new Point(0, 0));
                ColorLabel label = new ColorLabel(String.Join(", ", brushes), _redlineHeight, _redlineSide);
                Canvas.SetLeft(label, transform.X + frameworkElement.ActualWidth/2);
                Canvas.SetTop(label, _redlineSide == RedlineSide.Bottom ? transform.Y + frameworkElement.ActualHeight : transform.Y);
                RedlineCanvas.Children.Add(label);

                // Flip drawing position and grow redline height to avoid overlapping
                if (_redlineSide == RedlineSide.Top)
                {
                    _redlineSide = RedlineSide.Bottom;
                }
                else
                {
                    _redlineSide = RedlineSide.Top;
                    _redlineHeight += 30;
                }
            }

        }
    }
}
