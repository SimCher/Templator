using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Templator.TextCollection;
using WpfApp1;

namespace Templator
{
    /// <summary>
    /// Логика взаимодействия для TextDialog.xaml
    /// </summary>
    public partial class TextDialog : Window
    {
        private MainWindow _callingWindow;
        public TextDialog(MainWindow window)
        {
            InitializeComponent();
            _callingWindow = window;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            _callingWindow.TryAddToCollection(ElementNameTextBox.Text, SampleTextBox.Text);
            Close();
        }
    }
}
