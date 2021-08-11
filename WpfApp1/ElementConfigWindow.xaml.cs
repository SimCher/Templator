using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Templator.TransformElements;

namespace Templator
{
    /// <summary>
    /// Логика взаимодействия для ElementConfigWindow.xaml
    /// </summary>
    public partial class ElementConfigWindow : Window
    {
        private TemplateGenerator.TemplateGenerator Generator { get; }
        private Canvas _canvas;
        private IList<Border> _borderElements;

        public ElementConfigWindow(Canvas mainWindowCanvas, IEnumerable<TextElementTransform> uiTextTransforms)
        {
            InitializeComponent();
            _canvas = mainWindowCanvas;
            Generator = new TemplateGenerator.TemplateGenerator(uiTextTransforms);
            DataContext = Generator.TemplateSettings;
            _borderElements = new List<Border>();
            FillBordersList();

            AddHandler(System.Windows.Controls.Validation.ErrorEvent, new RoutedEventHandler(OnValidationRaised));
        }

        private void Generate()
        {
            RemoveBorders();
            Generator.Generate(_canvas);
            RestoreBorders();

            MessageBox.Show("Готово!", "Генерация завершена", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void GenerateButton_OnClick(object sender, RoutedEventArgs e)
        {
            Generate();
            Close();
        }
        
        private void RestoreBorders()
        {
            foreach (var border in _borderElements)
            {
                border.BorderThickness = new Thickness(1);
            }
        }

        private void RemoveBorders()
        {
            foreach (var border in _borderElements)
            {
                border.BorderThickness = new Thickness(0);
            }
        }

        private void FillBordersList()
        {
            foreach (IInputElement element in _canvas.Children)
            {
                if (element is Border border)
                {
                    _borderElements.Add(border);
                }
            }
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
