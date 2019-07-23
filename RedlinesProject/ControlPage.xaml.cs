using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RedlinesProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ControlPage : Page
    {
        public ControlPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                LightContainer.Children.Add(new ControlCard(e.Parameter as Type));

                RedlineViewer redlineViewer = new RedlineViewer(e.Parameter as Type);
                Grid.SetRow(redlineViewer, 1);
                LightContainer.Children.Add(redlineViewer);

                DarkContainer.Children.Add(new ControlCard(e.Parameter as Type));
            }

            base.OnNavigatedTo(e);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LandingPage));
        }
    }
}
