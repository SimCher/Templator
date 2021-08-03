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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Image _logo;
        private Image _draggedImage;
        private Point _mousePosition;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.Source is Image image && CanvasSpace.CaptureMouse())
            {
                _mousePosition = e.GetPosition(CanvasSpace);
                _draggedImage = image;
                Panel.SetZIndex(_draggedImage, 1);
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(_draggedImage != null)
            {
                CanvasSpace.ReleaseMouseCapture();
                Panel.SetZIndex(_draggedImage, 0);
                _draggedImage = null;
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(_draggedImage != null)
            {
                var position = e.GetPosition(CanvasSpace);
                var offset = position - _mousePosition;
                _mousePosition = position;
                Canvas.SetLeft(_draggedImage, Canvas.GetLeft(_draggedImage) + offset.X);
                Canvas.SetTop(_draggedImage, Canvas.GetTop(_draggedImage) + offset.Y);
            }
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
            Canvas.SetLeft(_logo, 0);
            Canvas.SetTop(_logo, 0);
            CanvasSpace.Children.Add(_logo);
        }

        private void SetBackgroundButton_Click(object sender, RoutedEventArgs e)
        {
            this.Background = ImageService.GetBackgroundWithOpenFileDialog();
        }
    }
}
