using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace RedlinesProject
{
    class BrushNameDictionary:IReadOnlyDictionary<Brush, string>
    {
        private static List<string> s_knownBrushNames;
        private static Dictionary<Brush, string> s_innerDictionary;

        static BrushNameDictionary()
        {
            s_knownBrushNames = new List<string>()
            {
                "SystemControlBackgroundAccentBrush",
                "SystemControlBackgroundAltHighBrush",
                "SystemControlBackgroundAltMediumHighBrush",
                "SystemControlBackgroundAltMediumBrush",
                "SystemControlBackgroundAltMediumLowBrush",
                "SystemControlBackgroundBaseHighBrush",
                "SystemControlBackgroundBaseLowBrush",
                "SystemControlBackgroundBaseMediumBrush",
                "SystemControlBackgroundBaseMediumHighBrush",
                "SystemControlBackgroundBaseMediumLowBrush",
                "SystemControlBackgroundChromeBlackHighBrush",
                "SystemControlBackgroundChromeBlackMediumBrush",
                "SystemControlBackgroundChromeBlackLowBrush",
                "SystemControlBackgroundChromeBlackMediumLowBrush",
                "SystemControlBackgroundChromeMediumBrush",
                "SystemControlBackgroundChromeMediumLowBrush",
                "SystemControlBackgroundChromeWhiteBrush",
                "SystemControlBackgroundListLowBrush",
                "SystemControlBackgroundListMediumBrush",
                "SystemControlDisabledAccentBrush",
                "SystemControlDisabledBaseHighBrush",
                "SystemControlDisabledBaseLowBrush",
                "SystemControlDisabledBaseMediumLowBrush",
                "SystemControlDisabledChromeDisabledHighBrush",
                "SystemControlDisabledChromeDisabledLowBrush",
                "SystemControlDisabledChromeHighBrush",
                "SystemControlDisabledChromeMediumLowBrush",
                "SystemControlDisabledListMediumBrush",
                "SystemControlDisabledTransparentBrush",
                "SystemControlFocusVisualPrimaryBrush",
                "SystemControlFocusVisualSecondaryBrush",
                "SystemControlRevealFocusVisualBrush",
                "SystemControlForegroundAccentBrush",
                "SystemControlForegroundAltHighBrush",
                "SystemControlForegroundAltMediumHighBrush",
                "SystemControlForegroundBaseHighBrush",
                "SystemControlForegroundBaseLowBrush",
                "SystemControlForegroundBaseMediumBrush",
                "SystemControlForegroundBaseMediumHighBrush",
                "SystemControlForegroundBaseMediumLowBrush",
                "SystemControlForegroundChromeBlackHighBrush",
                "SystemControlForegroundChromeHighBrush",
                "SystemControlForegroundChromeMediumBrush",
                "SystemControlForegroundChromeWhiteBrush",
                "SystemControlForegroundChromeDisabledLowBrush",
                "SystemControlForegroundChromeGrayBrush",
                "SystemControlForegroundListLowBrush",
                "SystemControlForegroundListMediumBrush",
                "SystemControlForegroundTransparentBrush",
                "SystemControlForegroundChromeBlackMediumBrush",
                "SystemControlForegroundChromeBlackMediumLowBrush",
                "SystemControlHighlightAccentBrush",
                "SystemControlHighlightAltAccentBrush",
                "SystemControlHighlightAltAltHighBrush",
                "SystemControlHighlightAltBaseHighBrush",
                "SystemControlHighlightAltBaseLowBrush",
                "SystemControlHighlightAltBaseMediumBrush",
                "SystemControlHighlightAltBaseMediumHighBrush",
                "SystemControlHighlightAltAltMediumHighBrush",
                "SystemControlHighlightAltBaseMediumLowBrush",
                "SystemControlHighlightAltListAccentHighBrush",
                "SystemControlHighlightAltListAccentLowBrush",
                "SystemControlHighlightAltListAccentMediumBrush",
                "SystemControlHighlightAltChromeWhiteBrush",
                "SystemControlHighlightAltTransparentBrush",
                "SystemControlHighlightBaseHighBrush",
                "SystemControlHighlightBaseLowBrush",
                "SystemControlHighlightBaseMediumBrush",
                "SystemControlHighlightBaseMediumHighBrush",
                "SystemControlHighlightBaseMediumLowBrush",
                "SystemControlHighlightChromeAltLowBrush",
                "SystemControlHighlightChromeHighBrush",
                "SystemControlHighlightListAccentHighBrush",
                "SystemControlHighlightListAccentLowBrush",
                "SystemControlHighlightListAccentMediumBrush",
                "SystemControlHighlightListMediumBrush",
                "SystemControlHighlightListLowBrush",
                "SystemControlHighlightChromeWhiteBrush",
                "SystemControlHighlightTransparentBrush",
                "SystemControlHyperlinkTextBrush",
                "SystemControlHyperlinkBaseHighBrush",
                "SystemControlHyperlinkBaseMediumBrush",
                "SystemControlHyperlinkBaseMediumHighBrush",
                "SystemControlPageBackgroundAltMediumBrush",
                "SystemControlPageBackgroundAltHighBrush",
                "SystemControlPageBackgroundMediumAltMediumBrush",
                "SystemControlPageBackgroundBaseLowBrush",
                "SystemControlPageBackgroundBaseMediumBrush",
                "SystemControlPageBackgroundListLowBrush",
                "SystemControlPageBackgroundChromeLowBrush",
                "SystemControlPageBackgroundChromeMediumLowBrush",
                "SystemControlPageBackgroundTransparentBrush",
                "SystemControlPageTextBaseHighBrush",
                "SystemControlPageTextBaseMediumBrush",
                "SystemControlPageTextChromeBlackMediumLowBrush",
                "SystemControlTransparentBrush",
                "SystemControlErrorTextForegroundBrush",
                "SystemControlTransientBorderBrush",
                "SystemControlTransientBackgroundBrush",
                "SystemControlDescriptionTextForegroundBrush"
            };

            s_innerDictionary = new Dictionary<Brush, string>();

            var resource = Application.Current.Resources as ResourceDictionary;
            s_knownBrushNames.ForEach(brushName => s_innerDictionary[resource[brushName] as Brush] = brushName);
        }

        public string this[Brush key]
        {
            get { return s_innerDictionary[key]; }
        }

        public IEnumerable<Brush> Keys
        {
            get { return s_innerDictionary.Keys; }
        }

        public IEnumerable<string> Values
        {
            get { return s_innerDictionary.Values; }
        }

        public int Count
        {
            get { return s_innerDictionary.Count; }
        }

        public bool ContainsKey(Brush key)
        {
            return s_innerDictionary.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<Brush, string>> GetEnumerator()
        {
            return s_innerDictionary.GetEnumerator();
        }

        public bool TryGetValue(Brush key, out string value)
        {
            return s_innerDictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return s_innerDictionary.GetEnumerator();
        }

        public static string GetShortName(string brushName)
        {
            brushName = brushName.Replace("SystemControl", "");
            brushName = brushName.Replace("Brush", "");
            return brushName;
        }
    }
}
