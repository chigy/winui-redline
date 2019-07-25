using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace RedlinesProject
{
    class RedLineInfo
    {
        public Point TargetTopLeft { get; set; }
        public double TargetWidth { get; set; }
        public double TargetHeight { get; set; }

        public string Content { get; set; }

        public RedLineInfo(Point topleft, FrameworkElement element, string content)
        {
            TargetTopLeft = topleft;
            Content = content;
            TargetWidth = element.ActualWidth;
            TargetHeight = element.ActualHeight;
        }

        public Point TargetHorizontalCenter()
        {
            return new Point(TargetTopLeft.X + TargetWidth / 2, TargetTopLeft.Y);
        }
    }
}
