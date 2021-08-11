using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Templator.ImageProcessing
{
    /// <summary>
    /// Содержит операции для преобразования Canvas в графическое изображение
    /// </summary>
    static class CanvasConverter
    {
        /// <summary>
        /// Модифицирует позицию элемента для корректного отображения после рендеринга
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private static Size ModifyPosition(FrameworkElement element)
        {
            Size size = new Size(element.ActualWidth + element.Margin.Left + element.Margin.Right,
                element.ActualHeight + element.Margin.Top + element.Margin.Bottom);
            element.Measure(size);
            element.Arrange(new Rect(-element.Margin.Left, -element.Margin.Top,
                size.Width, size.Height));

            return size;
        }

        /// <summary>
        /// Устанавливает позицию элемента в позицию по умолчанию
        /// </summary>
        /// <param name="element"></param>
        private static void ModifyPositionBack(FrameworkElement element)
        {
            element.Measure(new Size());
        }

        /// <summary>
        /// Экспортирует элемент как файл формата .png по переданному пути
        /// </summary>
        /// <param name="element"></param>
        /// <param name="savePath"></param>
        public static void PngExport(FrameworkElement element, string savePath)
        {
            if (savePath == null)
            {
                return;
            }
            
            Size size = ModifyPosition(element);

            var renderBitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96,
                PixelFormats.Pbgra32);
            renderBitmap.Render(element);

            using (FileStream stream = new(savePath, FileMode.Create))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(stream);
            }

            ModifyPositionBack(element);
        }
    }
}
