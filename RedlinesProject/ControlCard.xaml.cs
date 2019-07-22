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
        private List<string> unselectedStates = new List<string>
        {
            "Normal",
            "PointerOver",
            "Pressed",
            "Disabled",
            "UncheckedNormal",
            "UncheckedPointerOver",
            "UncheckedPressed",
            "UncheckedDisabled",
        };

        private List<string> selectedStates = new List<string>
        {
            "CheckedNormal",
            "CheckedPointerOver",
            "CheckedPressed",
            "CheckedDisabled",
            "FocusEngagedHorizontal",
            "FocusEngagedVertical",
            "FocusDisengaged",
        };

        private List<string> indeterminateStates = new List<string>
        {
            "IndeterminateNormal",
            "IndeterminatePointerOver",
            "IndeterminatePressed",
            "IndeterminateDisabled",
        };

        public ControlCard(Type controlType)
        {
            this.InitializeComponent();
            
            CreateControlStates(controlType);
        }

        private ControlStateViewer CreateControlStateViewer(Type cType, List<string> states)
        {
            ControlStateViewer ctrlStateViewer = new ControlStateViewer();
            ctrlStateViewer.ControlType = cType;
            ctrlStateViewer.States = states;

            return ctrlStateViewer;
        }

        private void CreateControlStates(Type ctrlType)
        {
            LightControlContainer_UnselectedGrid.Children.Add(CreateControlStateViewer(ctrlType, unselectedStates));

            if (ctrlType == typeof(CheckBox))
            {
                LightSelectedLabel.Visibility = Visibility.Visible;
                LightIndeterminateLabel.Visibility = Visibility.Visible;

                LightControlContainer_SelectedGrid.Children.Add(CreateControlStateViewer(ctrlType, selectedStates));
                LightControlContainer_IndeterminateGrid.Children.Add(CreateControlStateViewer(ctrlType, indeterminateStates));
            }
        }
    }
}
