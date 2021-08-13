using System.Windows.Media;

namespace Templator.FontChooser
{
#nullable enable
    public class FontColor
    {
        public string Name { get; set; }
        public SolidColorBrush Brush { get; set; }

        public FontColor(string name, SolidColorBrush brush)
        {
            Name = name;
            Brush = brush;
        }

        public override bool Equals(object? obj)
        {
            FontColor? p = obj as FontColor;
            return (object?)p != null && ((Name == p.Name) && (Brush.Equals(p.Brush)));
        }

        public override int GetHashCode()
            => base.GetHashCode();

        public override string ToString()
        {
            return $"FontColor[Color={Name},{Brush}]";
        }
    }
}