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
using Syncfusion.CompoundFile.XlsIO.Net;
using Templator.TemplateGenerator;
using Templator.TransformElements;
using Directory = System.IO.Directory;

namespace Templator
{
    /// <summary>
    /// Логика взаимодействия для ElementConfigWindow.xaml
    /// </summary>
    public partial class ElementConfigWindow : Window
    {
        private TemplateGenerator.TemplateGenerator Generator { get; }
        private MainWindow _window;
        private IList<Border> _borderElements;

        public ElementConfigWindow(MainWindow mainWindow, IEnumerable<TextElementTransform> uiTextTransforms)
        {
            InitializeComponent();
            _window = mainWindow;
            Generator = new TemplateGenerator.TemplateGenerator(uiTextTransforms);
            DataContext = Generator.TemplateSettings;
            _borderElements = new List<Border>();

            AddHandler(System.Windows.Controls.Validation.ErrorEvent, new RoutedEventHandler(OnValidationRaised));
        }

        private void GenerateButton_OnClick(object sender, RoutedEventArgs e)
        {
            _window.SetGenerator(Generator);
            Close();
        }

        private void ExcelColumnTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void SetTemplateSettings()
        {
            string message = "";

            for (int i = 0; i < Generator.TemplateSettings.Count; i++)
            {
                message += $"{i}. {Generator.TemplateSettings[i]} ";
            }

            MessageBox.Show(message);
        }

        private void OnValidationRaised(object sender, RoutedEventArgs e)
        {
            var args = (ValidationErrorEventArgs)e;

            if (args.Error.RuleInError is ExceptionValidationRule)
            {
                if (args.Action == ValidationErrorEventAction.Added)
                {

                }
            }
        }
    }
}
