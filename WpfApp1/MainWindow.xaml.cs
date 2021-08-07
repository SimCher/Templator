using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Templator.TextCollection;
using WpfApp1;
using Transformer = Templator.TransformElements.TransformElementService;

namespace Templator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Border _logo;
        private Border[] _generatedTexts;
        private int _textsLimit;

        public ObservableCollection<TextElement> TextElements { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            TextElements = new ObservableCollection<TextElement>();
            this.DataContext = TextElements;
        }
        
        private void AddLogoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_logo != null)
            {
                if (CanvasSpace.Children.Contains(_logo))
                {
                    CanvasSpace.Children.Remove(_logo);
                }
            }

            _logo = ComponentService.GetInteractionPictureWithOpenFileDialog();

            _logo.MouseLeftButtonDown += Border_MouseLeftButtonDown;
            _logo.MouseLeave += Border_MouseLeave;
            _logo.MouseEnter += Border_MouseEnter;

            Canvas.SetLeft(_logo, 0);
            Canvas.SetTop(_logo, 20);
            CanvasSpace.Children.Add(_logo);
        }

       

        private void SetBackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            var background = ComponentService.GetBackgroundWithOpenFileDialog();

            CanvasSpace.Background = background;
            AddLogoButton.IsEnabled = true;
            AddTextButton.IsEnabled = true;
        }
        
        /// <summary>
        /// По нажатию мыши в области окна
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            var mousePosition = GetMousePosition(CanvasSpace);

            Transformer.Grab(mousePosition.Item1, mousePosition.Item2);

            StateLabel.Content = "Grab";
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            Transformer.Drop();

            StateLabel.Content = "Drop";

            Cursor = Cursors.Arrow;
        }

        private void CanvasSpace_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mousePosition = GetMousePosition((IInputElement) sender);
                Transformer.DragOrStretch(mousePosition.Item1, mousePosition.Item2);
                StateLabel.Content = "DragOrStretch";
            }
            
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Transformer.SetNewElement(sender);
            StateLabel.Content = "SetNewELement";
        }
        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                return;

            var mousePosition = GetMousePosition((IInputElement)sender);

            var stretchDirection =
                Transformer.GetStretchDirection(mousePosition.Item1, mousePosition.Item2, sender);

            switch (stretchDirection)
            {
                //Установить корректный курсор
                case Transformer.StretchDirections.Both:
                    Cursor = Cursors.SizeNWSE;
                    return;
                case Transformer.StretchDirections.Right:
                    Cursor = Cursors.SizeWE;
                    return;
                case Transformer.StretchDirections.Bottom:
                    Cursor = Cursors.SizeNS;
                    return;
                case Transformer.StretchDirections.None:
                    Cursor = Cursors.Arrow;
                    break;
                default:
                    break;
            }
            Cursor = Cursors.Arrow;
            Transformer.ResetStates();
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

        public void TryAddToCollection(string elementName, string sampleText)
        {
            if (string.IsNullOrWhiteSpace(elementName))
            {
                MessageBox.Show("Введите корректное название текста!",
                    "Невозможно создать элемент",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (TextElements.Any(t => t.Name == elementName))
            {
                MessageBox.Show("Элемент с таким названием уже существует!", "Невозможно создать элемент",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            var textElement = ComponentService.GetInteractionTextBlock(sampleText);

            textElement.MouseLeftButtonDown += Border_MouseLeftButtonDown;
            textElement.MouseLeave += Border_MouseLeave;
            textElement.MouseEnter += Border_MouseEnter;

            Canvas.SetLeft(textElement, 0);
            Canvas.SetTop(textElement, 20);
            CanvasSpace.Children.Add(textElement);

            TextElements.Add(new TextElement{Name = elementName, TextControl = textElement});

            MessageBox.Show(TextElements.Last().ToString() + " " + TextElements.Count);
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
            var callButton = (Button)sender;

            if (callButton.DataContext is TextElement element)
            {
                CanvasSpace.Children.Remove(element.TextControl);
                TextElements.Remove(element);
            }
        }

        private void OpenGenerateSettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
