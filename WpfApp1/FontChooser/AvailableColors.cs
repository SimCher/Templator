using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace Templator.FontChooser
{
    public class AvailableColors : List<FontColor>
    {
        public AvailableColors() : base()
        { 
            Initialize();
        }

        public FontColor GetFontColorByName(string name)
        {
            FontColor found = null;
            foreach (var b in this)
            {
                if (b.Name == name)
                {
                    found = b;
                    break;
                }
            }

            return found;
        }

        public FontColor GetFontColorByBrush(SolidColorBrush brush)
        {
            FontColor found = null;
            foreach (FontColor color in this)
            {
                if (color.Brush.Color.Equals(brush.Color))
                {
                    found = color;
                    break;
                }
            }

            return found;
        }

        public static FontColor GetFontColor(SolidColorBrush brush)
        {
            var brushList = new AvailableColors();
            return brushList.GetFontColorByBrush(brush);
        }

        public static FontColor GetFontColor(string name)
        {
            var brushList = new AvailableColors();
            return brushList.GetFontColorByName(name);
        }

        public static FontColor GetFontColor(Color color)
            => AvailableColors.GetFontColor(new SolidColorBrush(color));

        public static int GetFontColorIndex(FontColor color)
        {
            var brushList = new AvailableColors();
            SolidColorBrush colorBrush = color.Brush;

            return brushList.TakeWhile(brush => !brush.Brush.Color.Equals(colorBrush.Color)).Count();
        }

        private void Initialize()
        {
            Type brushesType = typeof(Colors);
            var properties = brushesType.GetProperties(BindingFlags.Static | BindingFlags.Public);

            foreach (var property in properties)
            {
                string name = property.Name;
                var brush = new SolidColorBrush((Color)(property.GetValue(null, null)));
                this.Add(new FontColor(name, brush));
            }
        }
    }
}