using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    /// <summary>
    /// Вспомогательный класс для работы с изображениями
    /// </summary>
    static class ImageService
    {
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

        /// <summary>
        /// Открывает диалоговое окно для выбора изображения и возвращает изображение, выбранное пользователем
        /// </summary>
        /// <returns>Изображение, выбранное в диалоговом окне</returns>
        public static Image GetPictureWithOpenFileDialog()
        {
            Image image = null;
            var dialog = ShowOpenFileDialog();

            if ((bool)dialog.ShowDialog())
            {
                var bitmap = new BitmapImage(new Uri(dialog.FileName));
                image = new Image { Source = bitmap };
            }

            return image;
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
    }
}
