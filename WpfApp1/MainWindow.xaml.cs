using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Templator.TransformElements;

namespace Templator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private TemplateGenerator.TemplateGenerator Generator { get; set; }
        private Canvas _mainWindowCanvas;
        private IList<Border> _borderElements;

        /// <summary>
        /// Коллекция трансформируемых текстовых элементов (нужны для автозаполнения)
        /// </summary>
        ObservableCollection<TextElementTransform> TextElements { get; }

        public MainWindow()
        {
            InitializeComponent();
            TextElements = new ObservableCollection<TextElementTransform>();
            DataContext = TextElements;
        }

        public void Generate()
        {
            var thread = new Thread(new ParameterizedThreadStart(Generator.Generate));
            _borderElements = new List<Border>();
            if (_borderElements.Count == 0)
            {
                FillBordersList();
            }
            RemoveBorders();
            Generator.Generate(CanvasSpace);
            RestoreBorders();
            MessageBox.Show("Готово!", "Генерация завершена", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void SetGenerator(TemplateGenerator.TemplateGenerator generator)
        {
            Generator = new TemplateGenerator.TemplateGenerator(generator);
        }

        /// <summary>
        /// Если возможно, добавить текстовый элемент в коллекцию трансформируемых текст. элементов
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="sampleText"></param>
        public void TryAddToCollection(string elementName, string sampleText)
        {
            if (TextElements.Any(t => t.Name == elementName))
            {
                MessageBox.Show("Элемент с таким названием уже существует!", "Невозможно создать элемент",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            var textElement = ComponentService.GetInteractionTextBlock(elementName, sampleText);

            textElement.InputElement.MouseLeftButtonDown += Border_MouseLeftButtonDown;
            textElement.InputElement.MouseLeave += Border_MouseLeave;
            textElement.InputElement.MouseEnter += Border_MouseEnter;

            Canvas.SetLeft((UIElement)textElement.InputElement, 0);
            Canvas.SetTop((UIElement)textElement.InputElement, 20);
            CanvasSpace.Children.Add((UIElement)textElement.InputElement);

            TextElements.Add(new TextElementTransform(elementName, textElement.InputElement));

            OpenGenerateSettingsButton.IsEnabled = true;

            MessageBox.Show(TextElements.Last() + " " + TextElements.Count);
        }

        /// <summary>
        /// По нажатию ЛКМ в области окна захватить элемент, если курсор входит область элемента
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            var mousePosition = GetMousePosition(CanvasSpace);

            ElementTransformService.Grab(mousePosition.Item1, mousePosition.Item2);

            StateLabel.Content = "Grab";
        }

        /// <summary>
        /// По отпусканию ЛКМ освободить элемент от курсора
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            ElementTransformService.Drop();

            StateLabel.Content = "Drop";

            Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Кнопка добавления изображения (логотипа и пр.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLogoButton_Click(object sender, RoutedEventArgs e)
        {
            var imageTransform = ComponentService.GetInteractionPictureWithOpenFileDialog().InputElement;

            imageTransform.MouseLeftButtonDown += Border_MouseLeftButtonDown;
            imageTransform.MouseLeave += Border_MouseLeave;
            imageTransform.MouseEnter += Border_MouseEnter;

            Canvas.SetLeft((UIElement)imageTransform, 0);
            Canvas.SetTop((UIElement)imageTransform, 20);
            CanvasSpace.Children.Add((UIElement)imageTransform);
        }

        /// <summary>
        /// Кнопка добавления фонового изображения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetBackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            var background = ComponentService.GetBackgroundWithOpenFileDialog();

            CanvasSpace.Background = background;
            AddLogoButton.IsEnabled = true;
            AddTextButton.IsEnabled = true;

        }

        private void CanvasSpace_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mousePosition = GetMousePosition((IInputElement) sender);
                ElementTransformService.DragOrStretch(mousePosition.Item1, mousePosition.Item2);
                StateLabel.Content = "DragOrStretch";
            }
            
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ElementTransformService.SetNewElement(sender);
            StateLabel.Content = "SetNewELement";
        }
        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                return;

            var mousePosition = GetMousePosition((IInputElement)sender);

            var stretchDirection =
                ElementTransformService.GetStretchDirection(mousePosition.Item1, mousePosition.Item2, sender);

            switch (stretchDirection)
            {
                //Установить корректный курсор
                case StretchDirections.Both:
                    Cursor = Cursors.SizeNWSE;
                    return;
                case StretchDirections.Right:
                    Cursor = Cursors.SizeWE;
                    return;
                case StretchDirections.Bottom:
                    Cursor = Cursors.SizeNS;
                    return;
                case StretchDirections.None:
                    Cursor = Cursors.Arrow;
                    break;
            }
            Cursor = Cursors.Arrow;
            ElementTransformService.ResetStates();
            StateLabel.Content = "ResetStates";
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Border border = (Border)sender;

            var rightLimit = border.ActualWidth - border.Padding.Right;
            var bottomLimit = border.ActualHeight - border.Padding.Bottom;

            var x = Mouse.GetPosition((IInputElement)sender).X;
            var y = Mouse.GetPosition((IInputElement)sender).Y;

            if (x < rightLimit && y < bottomLimit)
            {
                Cursor = Cursors.Arrow;
                StateLabel.Content = "Reset";
            }
                
        }

        private (double, double) GetMousePosition(IInputElement element)
        {
            return (Mouse.GetPosition(element).X, Mouse.GetPosition(element).Y);
        }

        private void AddTextButton_OnClick(object sender, RoutedEventArgs e)
        {
            new TextDialog(this).ShowDialog();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (TextElements.Count == 0)
            {
                OpenGenerateSettingsButton.IsEnabled = false;
            }
            else
            {
                OpenGenerateSettingsButton.IsEnabled = true;
            }

            var callButton = (Button)sender;

            if (callButton.DataContext is TextElementTransform element)
            {
                CanvasSpace.Children.Remove((UIElement)element.InputElement);
                TextElements.Remove(element);
            }
        }

        private void OpenGenerateSettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            new ElementConfigWindow(this, TextElements).ShowDialog();
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
            foreach (IInputElement element in CanvasSpace.Children)
            {
                if (element is Border border)
                {
                    _borderElements.Add(border);
                }
            }
        }

        private void GenerateButton_OnClick(object sender, RoutedEventArgs e)
        {
            Generate();
        }
    }
}
