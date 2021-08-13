using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Templator.FontChooser
{
    public class FontSetting
    {
        private FontFamily _family;
        private FontStyle _style;
        private FontStretch _stretch;
        private double _size;
        private FontWeight _weight;
        private SolidColorBrush _brushColor;

        public FontFamily Family
        {
            get => _family;
            private set => _family = value;
        }

        public FontStyle Style
        {
            get => _style;
            private set => _style = value;
        }

        public FontStretch Stretch
        {
            get => _stretch;
            private set => _stretch = value;
        }

        public double Size
        {
            get => _size;
            private set => _size = value;
        }

        public FontWeight Weight
        {
            get => _weight;
            private set => _weight = value;
        }

        public SolidColorBrush BrushColor
        {
            get => _brushColor;
            private set => _brushColor = value;
        }

        public FontColor Color
        {
            get => AvailableColors.GetFontColor(BrushColor);
        }

        public FamilyTypeface Typeface
        {
            get
            {
                FamilyTypeface typeface = new();
                typeface.Stretch = Stretch;
                typeface.Weight = Weight;
                typeface.Style = Style;

                return typeface;
            }
        }

        public FontSetting()
        {
            
        }
        
        
        public FontSetting(FontFamily family, FontStyle style, FontStretch stretch,
            double size, FontWeight weight, SolidColorBrush brushColor)
        {
            _family = family;
            _style = style;
            _stretch = stretch;
            _size = size;
            _weight = weight;
            _brushColor = brushColor;
        }

        public static FontSetting GetControlFont(TextBlock control)
        {
            FontSetting font = new()
            {
                Family = control.FontFamily,
                Size = control.FontSize,
                Style = control.FontStyle,
                Stretch = control.FontStretch,
                Weight = control.FontWeight,
                BrushColor = (SolidColorBrush)control.Foreground
            };

            return font;
        }

        public static string GetTypefaceAsString(FamilyTypeface typeface)
        {
            var builder = new StringBuilder(typeface.Stretch.ToString());
            builder.Append(" ");
            builder.Append(typeface.Weight.ToString());
            builder.Append(" ");
            builder.Append(typeface.Style.ToString());

            return builder.ToString();
        }

        public static void ApplyFontTo(TextBlock control, FontSetting font)
        {
            control.FontFamily = font._family;
            control.FontStyle = font._style;
            control.FontStretch = font._stretch;
            control.FontSize = font._size;
            control.FontWeight = font._weight;
            control.Foreground = font._brushColor;
        }

    }
}
