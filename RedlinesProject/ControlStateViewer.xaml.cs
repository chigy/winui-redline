using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace RedlinesProject
{
    public sealed partial class ControlStateViewer : UserControl
    {
        Type _controlType;
        List<string> _states;

        public ControlStateViewer()
        {
            this.InitializeComponent();
        }

        public Type ControlType
        {
            get
            {
                return _controlType;
            }

            set
            {
                _controlType = value;
                UpdateGrid();
            }
        }

        public List<string> States
        {
            get
            {
                return _states;
            }

            set
            {
                _states = value;
                UpdateGrid();
            }
        }

        private void UpdateGrid()
        {
            StateGridView.Items.Clear();

            if (_controlType == null || _states == null)
            {
                return;
            }

            foreach (string state in _states)
            {
                StackPanel sp = new StackPanel();
                sp.Margin = new Thickness(4);

                //TextBlock textBlock = new TextBlock();
                //textBlock.Text = state;
                //textBlock.FontSize = 9;
                //sp.Children.Add(textBlock);

                Control c = Activator.CreateInstance(_controlType) as Control;
                c.Loaded += Control_Loaded;

                c.DataContext = state;

                MakeControlPretty(c, _controlType);

                sp.Children.Add(c);
                StateGridView.Items.Add(sp);
            }
        }

        public static void MakeControlPretty(Control c, Type controlType)
        {
            ContentControl cc = c as ContentControl;
            if (cc != null)
            {
                string[] s = controlType.ToString().Split('.');
                cc.Content = s[s.Length - 1];
            }

            //Make slider pretty to look at
            if (controlType == typeof(Slider))
            {
                c.Width = 200;
                (c as Slider).Value = 35;
                (c as Slider).Maximum = 100;
            }
            if (controlType == typeof(Button))
            {
                c.Width = 150;
            }
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            Control c = sender as Control;
            if (c != null)
            {
                bool foundState = VisualStateManager.GoToState(c, c.DataContext as string, false);

                if(!foundState)
                {
                    StateGridView.Items.Remove(c.Parent);
                }
            }
        }
    }
}
