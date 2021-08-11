using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using Templator.TransformElements;

namespace Templator
{
    /// <summary>
    /// Вспомогательный класс для работы с изображениями
    /// </summary>
    static class ComponentService
    {
        public static TextElementTransform GetInteractionTextBlock(string name)
        {
            var textBlock = new TextBlock
            {
                Text = "Loren Ipsum", 
                TextAlignment = TextAlignment.Center,
                Background = new SolidColorBrush(Colors.Transparent),

            };
            var border = GetBorderElement();
            border.Child = textBlock;

            var textTransform = new TextElementTransform(name, border);

            return textTransform;
        }

        public static TextElementTransform GetInteractionTextBlock(string name, string initialText)
        {
            if (!string.IsNullOrWhiteSpace(initialText))
            {
                var textBlock = new TextBlock
                {
                    Text = initialText,
                    TextAlignment = TextAlignment.Left,
                    Background = new SolidColorBrush(Colors.Transparent),
                };
                var border = GetBorderElement();
                border.Child = textBlock;

                var textTransform = new TextElementTransform(name, border);

                return textTransform;
            }

            return GetInteractionTextBlock(name);
        }
        /// <summary>
        /// Открывает диалоговое окно для выбора изображения и возвращает изображение, выбранное пользователем
        /// </summary>
        /// <returns>Изображение, выбранное в диалоговом окне</returns>
        public static ImageElementTransform GetInteractionPictureWithOpenFileDialog()
        {
            Image image = null;
            var dialog = ShowImageOpenFileDialog();

            if ((bool)dialog.ShowDialog())
            {
                var bitmap = new BitmapImage(new Uri(dialog.FileName));
                image = new Image { Source = bitmap, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center};
            }

            var border = GetBorderElement();
            border.Child = image;

            var imageTransform = new ImageElementTransform(border);

            //border.Width = image.Width;
            //border.Height = image.Height;

            return imageTransform;
        }

        /// <summary>
        /// Открывает диалоговое окно для выбора фонового изображения и возвращает объект типа ImageBrush
        /// </summary>
        /// <returns>Изображение как объект типа ImageBrush</returns>
        public static ImageBrush GetBackgroundWithOpenFileDialog()
        {
            var dialog = ShowImageOpenFileDialog();
            ImageBrush imageBrush = null;

            if ((bool) dialog.ShowDialog())
            {
                var bitmap = new BitmapImage(new Uri(dialog.FileName));
                imageBrush = new ImageBrush {ImageSource = bitmap};
            }

            return imageBrush;
        }

        public static string GetExcelFilename()
        {
            var dialog = ShowExcelOpenFIleDialog();

            return (bool)dialog.ShowDialog() ? dialog.FileName : string.Empty;
        }

        public static string GetSavePath()
        {
            var dialog = ShowSaveFileDialog();

            return (dialog.ShowDialog() == true) ? dialog.FileName : string.Empty;
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

        private static Microsoft.Win32.SaveFileDialog ShowSaveFileDialog()
        {
            return new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".png",
                Filter = "Изображение (.png)|*.png"
            };
        }

        /// <summary>
        /// Вызывает OpenFileDialog и возвращает данные выбранного файла
        /// </summary>
        /// <returns></returns>
        private static Microsoft.Win32.OpenFileDialog ShowImageOpenFileDialog()
        {
            return new()
            {
                Filter = "Графические файлы (*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png"
            };
        }

        private static Microsoft.Win32.OpenFileDialog ShowExcelOpenFIleDialog()
        {
            return new()
            {
                Filter = "Excel (*.xlsx)|*.xlsx"
            };
        }

    }
}
