using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Transformer = Templator.TransformElements.TransformElementService;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Border _logo;

        public MainWindow()
        {
            InitializeComponent();
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

            _logo = ImageService.GetPictureWithOpenFileDialog();

            _logo.MouseLeftButtonDown += Border_MouseLeftButtonDown;
            _logo.MouseLeave += Border_MouseLeave;
            _logo.MouseEnter += Border_MouseEnter;

            Canvas.SetLeft(_logo, 0);
            Canvas.SetTop(_logo, 20);
            CanvasSpace.Children.Add(_logo);
        }

       

        private void SetBackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            var background = ImageService.GetBackgroundWithOpenFileDialog();

            double oldSize = ColumnSpace.Width.Value;

            if (background.ImageSource.Width > oldSize)
            {
                Width += background.ImageSource.Width - oldSize;
            }

            ColumnSpace.Width = new GridLength(background.ImageSource.Width);

            CanvasSpace.Background = background;
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
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            Transformer.Drop();

            Cursor = Cursors.Arrow;
        }

        private void CanvasSpace_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mousePosition = GetMousePosition((IInputElement) sender);
                Transformer.DragOrStretch(mousePosition.Item1, mousePosition.Item2, Cursor);

            }
            
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Transformer.SetNewElement(sender);
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
                    break;
                default:
                    Cursor = Cursors.Arrow;
                    Transformer.ResetStates();
                    break;
            }
        }
        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Border border = (Border)sender;

            var rightLimit = border.ActualWidth - border.Padding.Right;
            var bottomLimit = border.ActualHeight - border.Padding.Bottom;

            var x = Mouse.GetPosition((IInputElement)sender).X;
            var y = Mouse.GetPosition((IInputElement)sender).Y;

            if (x < rightLimit && y < bottomLimit)
                Cursor = Cursors.Arrow;
        }

       

        private (double, double) GetMousePosition(IInputElement element)
        {
            return (Mouse.GetPosition(element).X, Mouse.GetPosition(element).Y);
        }
    }
}
