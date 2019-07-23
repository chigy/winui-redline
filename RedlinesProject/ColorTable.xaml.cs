using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace RedlinesProject
{
    public sealed partial class ColorTable : UserControl
    {
        public ColorTable(IEnumerable<string> brushNames)
        {
            this.InitializeComponent();

            List<string> brushes = new List<string>();
            foreach (var brushName in brushNames)
            {
                var brush = App.Current.Resources[brushName] as SolidColorBrush;
                brushes.Add(brushName + ": " + brush.Color.ToString());
            }
            BrushTextBlock.Text = String.Join("\n", brushes);
        }
    }
}
