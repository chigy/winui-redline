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
    public sealed partial class LandingPage : Page
    {
        RedlineViewer redlineViewer;

        public LandingPage()
        {
            this.InitializeComponent();

            Loaded += LandingPage_Loaded;
        }

        private void LandingPage_Loaded(object sender, RoutedEventArgs e)
        {
            redlineViewer = new RedlineViewer(typeof(MenuFlyoutItem));
            LayoutRoot.Children.Add(redlineViewer);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ControlPage), typeof(Button));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ControlPage), typeof(CheckBox));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ControlPage), typeof(Slider));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ControlPage), typeof(MenuFlyoutItem));
        }
    }
}
