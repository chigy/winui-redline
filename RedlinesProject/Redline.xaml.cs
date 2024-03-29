﻿using System;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace RedlinesProject
{
    public enum RedlineSide
    {
        Left,
        Top,
        Right,
        Bottom
    }

    public sealed partial class Redline : UserControl
    {
        RedlineSide _side;

        public Redline(RedlineSide side, double size, double value)
        {
            this.InitializeComponent();

            _side = side;

            if (IsHorizontal())
            {
                Width = value;
                if (_side == RedlineSide.Top)
                {
                    HorizontalBottomRow.Height = new GridLength(size);
                }
                else
                {
                    HorizontalTopRow.Height = new GridLength(size);
                }
            }
            else
            {
                Height = value;
                if (_side == RedlineSide.Left)
                {
                    VerticalRightRow.Width = new GridLength(size);
                }
                else
                {
                    VerticalLeftRow.Width = new GridLength(size);
                }
            }

            HorizontalLabel.Text = value.ToString();
            VerticalLabel.Text = value.ToString();

            Loaded += Redline_Loaded;
        }

        private void Redline_Loaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, IsHorizontal() ? "Horizontal" : "Vertical", false);

            VisualStateManager.GoToState(this, _side.ToString(), false);
        }

        private bool IsHorizontal()
        {
            return _side == RedlineSide.Top || _side == RedlineSide.Bottom;
        }
    }
}
