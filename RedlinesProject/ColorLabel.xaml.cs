using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace RedlinesProject
{
    public sealed partial class ColorLabel : UserControl
    {
        private RedlineSide _side;

        public ColorLabel(string text, double lineHeight, RedlineSide side)
        {
            this.InitializeComponent();

            _side = side;

            if(side == RedlineSide.Bottom)
            {
                BottomTextBlock.Text = text;
                BottomVerticalLine.Y2 = lineHeight;
            }
            else
            {
                TopTextBlock.Text = text;
                TopHorizontalLine.Y1 = -lineHeight;
                TopHorizontalLine.Y2 = TopHorizontalLine.Y1;
                TopTextBlock.RenderTransform = new TranslateTransform() { Y = -lineHeight };
                TopVerticalLine.Y1 = -lineHeight;
            }

            Loaded += ColorLabel_Loaded;
        }

        private void ColorLabel_Loaded(object sender, RoutedEventArgs e)
        {
            if (_side == RedlineSide.Bottom)
            {
                VisualStateManager.GoToState(this, "Bottom", false);
            }
        }
    }
}
