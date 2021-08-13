using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Templator.FontChooser;
using Templator.TransformElements;

namespace Templator
{
    /// <summary>
    /// Логика взаимодействия для ColorFontDialog.xaml
    /// </summary>
    public partial class ColorFontDialog : Window
    {
        private FontSetting _selectedFont;
        private TextElementTransform _element;

        public ColorFontDialog(TextElementTransform element)
        {
            _selectedFont = FontSetting.GetControlFont((TextBlock)element.UIElement);
            InitializeComponent();
            _element = element;
        }

        public FontSetting SelectedFont
        {
            get => _selectedFont;
            set
            {
                FontSetting setting = value;
                _selectedFont = setting;
            }
        }

        private void SyncFontName()
        {
            string fontFamilyName = _selectedFont.Family.Source;
            int index = 0;
            foreach (var item in ColorFontChooser.FamilyListBox.Items)
            {
                string itemName = item.ToString();
                if (fontFamilyName == itemName)
                {
                    break;
                }

                index++;
            }

            ColorFontChooser.FamilyListBox.SelectedIndex = index;
            ColorFontChooser.FamilyListBox.ScrollIntoView(ColorFontChooser.FamilyListBox.Items[index]);
        }

        private void SyncFontSize()
        {
            double fontSize = _selectedFont.Size;
            ColorFontChooser.FontSizeSlider.Value = fontSize;
        }

        private void SyncFontColor()
        {
            int colorIndex = AvailableColors.GetFontColorIndex(SelectedFont.Color);
            ColorFontChooser.ColorPicker.SuperCombo.SelectedIndex = colorIndex;
            ColorFontChooser.SampleTextBox.Foreground = SelectedFont.Color.Brush;
            ColorFontChooser.ColorPicker.SuperCombo.BringIntoView();
        }

        private void SyncFontTypeface()
        {
            string fontTypeFace = FontSetting.GetTypefaceAsString(SelectedFont.Typeface);
            int index = 0;
            foreach (var item in ColorFontChooser.TypefacesListBox.Items)
            {
                FamilyTypeface face = item as FamilyTypeface;
                if (fontTypeFace == FontSetting.GetTypefaceAsString(face))
                {
                    break;
                }

                index++;
            }

            ColorFontChooser.TypefacesListBox.SelectedIndex = index;
        }

        private void ColorFontDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            SyncFontColor();
            SyncFontName();
            SyncFontSize();
            SyncFontTypeface();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedFont = ColorFontChooser.SelectedFont;
            FontSetting.ApplyFontTo((TextBlock)_element.UIElement, SelectedFont);
            DialogResult = true;
        }
    }
}
