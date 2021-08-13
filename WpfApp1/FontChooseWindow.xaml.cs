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

namespace Templator
{
    /// <summary>
    /// Логика взаимодействия для FontChooseWindow.xaml
    /// </summary>
    public partial class FontChooseWindow : UserControl
    {
        public FontChooseWindow()
        {
            InitializeComponent();
            SampleTextBox.IsReadOnly = true;
        }

        public FontSetting SelectedFont
        {
            get => new(SampleTextBox.FontFamily,
                SampleTextBox.FontStyle,
                SampleTextBox.FontStretch,
                SampleTextBox.FontSize,
                SampleTextBox.FontWeight,
                ColorPicker.SelectedFontColor.Brush);
        }

        private void ColorPicker_ColorChanged(object sender, RoutedEventArgs e)
        {
            SampleTextBox.Foreground = ColorPicker.SelectedFontColor.Brush;
        }
    }
}
