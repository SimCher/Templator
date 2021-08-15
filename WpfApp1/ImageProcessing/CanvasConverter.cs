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
        private static Brush _background;

        /// <summary>
        /// Экспортирует передаваемый элемент фреймворка в изображение по переданному пути сохранения
        /// </summary>
        /// <param name="element"></param>
        /// <param name="savePath"></param>
        public static void ExportImage(FrameworkElement element, string savePath)
        {
            if (savePath == null)
            {
                return;
            }

            var container = element as Panel;
            var image = new Image { Source = RenderElement(element) };

            if (container != null)
            {
                SetChildrenVisibility(container);

                _background ??= container.Background;

                container.Background = new ImageBrush(image.Source);
            }

            //TODO: если .png, то... если jpg, то... и т.п.
            PngExport(RenderElement(element, ComponentService.BackgroundInitialSize),
                savePath);

            ModifyPositionBack(element);

            if (container != null)
            {
                SetChildrenVisibility(container, true);
                container.Background = _background;
            }

        }

        /// <summary>
        /// Выравнивает элемент, а также его потомков
        /// </summary>
        /// <param name="element"></param>
        private static void ModifyPosition(FrameworkElement element)
        {
            Size size = new Size(element.ActualWidth + element.Margin.Left + element.Margin.Right,
                element.ActualHeight + element.Margin.Top + element.Margin.Bottom);

            element.Measure(size);
            element.Arrange(new Rect(size));
        }

        /// <summary>
        /// Выравнивает элемент, а также его потомков относительно указанного размера
        /// </summary>
        /// <param name="element"></param>
        /// <param name="needSize"></param>
        private static void ModifyPosition(FrameworkElement element, Size needSize)
        {
            element.Measure(needSize);
            element.Arrange(new Rect(needSize));
        }

        /// <summary>
        /// Преобразовывает элемент в объект типа DrawingVisual, предназначенный
        /// для рендеринга
        /// </summary>
        /// <param name="visual">Объект, поддерживающий рендеринг</param>
        /// <param name="bound">Границы объекта, определяющие область для будущего рендеринга</param>
        /// <returns></returns>
        private static DrawingVisual ModifyToDrawingVisual(Visual visual, Rect bound)
        {
            var drawingVisual = new DrawingVisual();
            var drawingContext = drawingVisual.RenderOpen();
            var brush = new VisualBrush(visual);
            drawingContext.DrawRectangle(brush, null, bound);
            drawingContext.Close();

            return drawingVisual;
        }

        /// <summary>
        /// Устанавливает всем потомком переданного элемента значение видимости
        /// </summary>
        /// <param name="container">Контейнер, потомкам которого нужно изменить видимость</param>
        /// <param name="visibility">true, если нужно включить видимость и false, наоборот (изначально false)</param>
        private static void SetChildrenVisibility(Panel container, bool visibility = false)
        {
            foreach (var child in container.Children)
            {
                if (child is not FrameworkElement element)
                {
                    continue;
                }

                element.Visibility = visibility ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Рендерит элемент и возвращает результат рендеринга
        /// </summary>
        /// <param name="element">Элемент, который нужно отрендерить</param>
        /// <param name="size"><i>(Необазятально)Размер, который установится элементу и относительно которого
        /// будут выровнены все его дочерние элементы</i></param>
        /// <returns></returns>
        private static RenderTargetBitmap RenderElement(FrameworkElement element, Size size = new())
        {
            if (size == default)
            {
                //Настройка позиции элемента для корректного рендеринга
                ModifyPosition(element);
            }
            else
            {
                ModifyPosition(element, size);
            }
            
            //Получение изначальных границ для корректного рендеринга
            Rect bound = VisualTreeHelper.GetDescendantBounds(element);

            //Преобразование элемента в объект типа DrawingVisual для возможности рендеринга
            var visual = ModifyToDrawingVisual(element, bound);

            //Инициализация рендера
            var render = new RenderTargetBitmap((int)bound.Width, (int)bound.Height,
                300, 300, PixelFormats.Pbgra32);

            //Рендер элемента
            render.Render(visual);

            return render;
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
        /// Экспортирует результат переданного рендера по указанному пути сохранения
        /// в формате PNG
        /// </summary>
        /// <param name="render"></param>
        /// <param name="savePath"></param>
        private static void PngExport(RenderTargetBitmap render, string savePath)
        {
            using FileStream stream = new(savePath, FileMode.Create);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(render));
            encoder.Save(stream);
        }
    }
}
