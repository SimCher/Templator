using System.Windows;

namespace Templator
{
    /// <summary>
    /// Логика взаимодействия для TextDialog.xaml
    /// </summary>
    public partial class TextDialog
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
