using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Templator.FontChooser
{
    public class FontService
    {
        private FontFamily _family;
        private FontStyle _style;
        private FontStretch _stretch;
        private double _size;
        private FontWeight _weight;
        private FamilyTypeface _typeface;

        public FontService()
        {
            
        }

        public FontService(FontFamily family, FontStyle style, FontStretch stretch,
            double size, FontWeight weight)
        {
            _family = family;
            _style = style;
            _stretch = stretch;
            _size = size;
            _weight = weight;

            _typeface = new FamilyTypeface
            {
                Stretch = _stretch,
                Weight = _weight,
                Style = _style
            };
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

        public static void ApplyFontTo(Control control, FontService font)
        {
            control.FontFamily = font._family;
            control.FontStyle = font._style;
            control.FontStretch = font._stretch;
            control.FontSize = font._size;
            control.FontWeight = font._weight;
        }

    }
}
