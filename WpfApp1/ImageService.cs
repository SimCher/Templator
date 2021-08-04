using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    /// <summary>
    /// Вспомогательный класс для работы с изображениями
    /// </summary>
    static class ImageService
    {
        /// <summary>
        /// Открывает диалоговое окно для выбора изображения и возвращает изображение, выбранное пользователем
        /// </summary>
        /// <returns>Изображение, выбранное в диалоговом окне</returns>
        public static Border GetPictureWithOpenFileDialog()
        {
            Image image = null;
            var dialog = ShowOpenFileDialog();

            if ((bool)dialog.ShowDialog())
            {
                var bitmap = new BitmapImage(new Uri(dialog.FileName));
                image = new Image { Source = bitmap, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center};
            }

            var border = GetBorderElement();
            border.Child = image;

            //border.Width = image.Width;
            //border.Height = image.Height;

            return border;
        }

        /// <summary>
        /// Открывает диалоговое окно для выбора фонового изображения и возвращает объект типа ImageBrush
        /// </summary>
        /// <returns>Изображение как объект типа ImageBrush</returns>
        public static ImageBrush GetBackgroundWithOpenFileDialog()
        {
            var dialog = ShowOpenFileDialog();
            ImageBrush imageBrush = null;

            if ((bool) dialog.ShowDialog())
            {
                var bitmap = new BitmapImage(new Uri(dialog.FileName));
                imageBrush = new ImageBrush {ImageSource = bitmap};
            }

            return imageBrush;
        }

        /// <summary>
        /// Возвращает объект типа Border который будет являться контейнером для интерактивного объекта
        /// </summary>
        /// <returns>Объект типа Border</returns>
        private static Border GetBorderElement()
        {
            var border = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 100, 100, 255)),
                BorderThickness = new Thickness(1),
                Padding = new Thickness(3),
                Height = 68,
                Width = 68,
                MinHeight = 10,
                MinWidth = 10
            };
            var transformCollection = new TransformCollection
            {
                new ScaleTransform(),
                new TranslateTransform()
            };

            var transformGroup = new TransformGroup();
            transformGroup.Children = transformCollection;

            border.Effect = new DropShadowEffect { Color = Color.FromArgb(255, 100, 100, 100) };
            border.RenderTransform = transformGroup;

            return border;
        }

        /// <summary>
        /// Вызывает OpenFileDialog и возвращает данные выбранного файла
        /// </summary>
        /// <returns></returns>
        private static Microsoft.Win32.OpenFileDialog ShowOpenFileDialog()
        {
            return new()
            {
                Filter = "Графические файлы (*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png"
            };
        }

    }
}
