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
    public sealed partial class ControlCard : Page
    {
        public ControlCard(Type controlType)
        {
            this.InitializeComponent();

            CreateLightThemeControlStates(controlType);
            CreateDarkThemeControlStates(controlType);
        }

        private void CreateDarkThemeControlStates(Type ctrlType)
        {
            StateViewerDarkTheme.ControlType = ctrlType;
            StateViewerDarkTheme.States = new List<string>
            {
                "Normal",
                "PointerOver",
                "Pressed",
                "Disabled",
                "UncheckedNormal",
                "UncheckedPointerOver",
                "UncheckedPressed",
                "CheckedNormal",
                "CheckedPointerOver",
                "CheckedPressed",
                "CheckedDisabled",
                "IndeterminateNormal",
                "IndeterminatePointerOver",
                "IndeterminatePressed",
                "IndeterminateDisabled",
                "FocusEngagedHorizontal",
                "FocusEngagedVertical",
                "FocusDisengaged",
            };
        }

        private void CreateLightThemeControlStates(Type ctrlType)
        {
            StateViewerLightTheme.ControlType = ctrlType;
            StateViewerLightTheme.States = new List<string>
            {
                "Normal",
                "PointerOver",
                "Pressed",
                "Disabled",
                "UncheckedNormal",
                "UncheckedPointerOver",
                "UncheckedPressed",
                "CheckedNormal",
                "CheckedPointerOver",
                "CheckedPressed",
                "CheckedDisabled",
                "IndeterminateNormal",
                "IndeterminatePointerOver",
                "IndeterminatePressed",
                "IndeterminateDisabled",
                "FocusEngagedHorizontal",
                "FocusEngagedVertical",
                "FocusDisengaged",
            };
        }
    }
}
