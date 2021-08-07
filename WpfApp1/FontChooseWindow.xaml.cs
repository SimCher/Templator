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
        private FontService _font;

        public FontService SelectedFont =>
            new(sampleTextBox.FontFamily,
                sampleTextBox.FontStyle,
                sampleTextBox.FontStretch,
                sampleTextBox.FontSize,
                sampleTextBox.FontWeight);

        public FontChooseWindow()
        {
            InitializeComponent();
            sampleTextBox.IsReadOnly = true;
        }



        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
