using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Templator.TransformElements
{
    /// <summary>
    /// Обслуживающий класс, представляющий полный функционал по реализации трансформируемых элементов
    /// </summary>
    static class ElementTransformService
    {
        private static ElementTransform _element;

        /// <summary>
        /// Текущий трансформируемый UI-элемент
        /// </summary>
        public static ElementTransform Element { get; }

        static ElementTransformService()
        {
            _element = new ElementTransform();
        }

        /// <summary>
        /// Состояние перечисления, информирующее о том, в какую сторону растягивается элемент
        /// или StretchDirections.None, если элемент не растягивается
        /// </summary>
        public static StretchDirections StretchDirection { get; private set; }

        /// <summary>
        /// Привязывает трансформируемый элемент к курсору мыши
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        public static void Grab(double mouseX, double mouseY)
        {
            _element.X = mouseX;
            _element.Y = mouseY;

            _element.InputElement?.CaptureMouse();

            if (!_element.IsStretching)
            {
                _element.IsDragging = true;
            }
        }

        /// <summary>
        /// Отвязывает трансформируемый элемент от курсора мыши
        /// </summary>
        public static void Drop()
        {
            _element.InputElement?.ReleaseMouseCapture();
        }

        /// <summary>
        /// Перемещает или скалирует элемент, в зависимости от состояния трансформируемого элемента
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        public static void DragOrStretch(double mouseX, double mouseY)
        {
            BringToFront();

            if (_element.IsDragging && !_element.IsStretching)
            {
                Drag(mouseX, mouseY);
            }

            if (_element.IsStretching && !_element.IsDragging)
            {
                Stretch(mouseX, mouseY);
            }
        }

        /// <summary>
        /// Устанавливает новый трансформируемый элемент и выводит его на передний план
        /// </summary>
        /// <param name="element"></param>
        public static void SetNewElement(object element)
        {
            SetNewZIndex(element);
            _element.InputElement = (IInputElement)element;
        }

        /// <summary>
        /// Сброс всех состояний трансформируемого элемента
        /// </summary>
        public static void ResetStates()
        {
            _element.IsDragging = false;
            _element.IsStretching = false;
        }

        /// <summary>
        /// Устанавливает новое направление скалирования в зависимости от текущего положения курсора мыши
        /// и координат границ трансформируемого элемента.
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        /// <param name="element"></param>
        /// <returns>Новое направление скалирования</returns>
        public static StretchDirections GetStretchDirection(double mouseX, double mouseY, object element)
        {
            StretchDirection = StretchDirections.None;
            var border = (Border)element;
            double rightBorder = border.ActualWidth - border.Padding.Right;
            double bottomBorder = border.ActualHeight - border.Padding.Bottom;

            if ((mouseX >= rightBorder && mouseX < border.ActualWidth) &&
                (mouseY >= bottomBorder && mouseY < border.ActualHeight))
            {
                StretchDirection = StretchDirections.Both;
            }
            else if (mouseX >= rightBorder && mouseX < border.ActualWidth)
            {
                StretchDirection = StretchDirections.Right;
            }
            else if (mouseY >= bottomBorder && mouseY < border.ActualHeight)
            {
                StretchDirection = StretchDirections.Bottom;
            }

            _element.InputElement = (IInputElement)element;
            _element.X = mouseX;
            _element.Y = mouseY;
            _element.IsStretching = true;

            return StretchDirection;
        }

        /// <summary>
        /// Устанавливает новый индекс по Z-координате
        /// </summary>
        /// <param name="element"></param>
        private static void SetNewZIndex(object element)
        {
            int newZIndex = (int)((Border)element).GetValue(Panel.ZIndexProperty);
            _element.ZIndex = newZIndex > _element.ZIndex ? newZIndex : _element.ZIndex;
        }

        /// <summary>
        /// Выводит трансформируемый элемент на передний план
        /// </summary>
        private static void BringToFront()
        {
            ((Border)_element.InputElement)?.SetValue(Panel.ZIndexProperty, _element.ZIndex++);
        }

        /// <summary>
        /// Перетаскивает элемент в зависимости от курсора мыши
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        private static void Drag(double mouseX, double mouseY)
        {
            TextBlock block = new TextBlock();
            if (((UIElement)_element.InputElement)?.RenderTransform is TransformGroup transformGroup)
            {
                var translateTransforms = transformGroup.Children
                    .Where(t => t.GetType().Name == "TranslateTransform").Select(t => t);

                foreach (var transform in translateTransforms)
                {
                    var translateTransform = (TranslateTransform)transform;
                    translateTransform.X += mouseX - _element.X;
                    translateTransform.Y += mouseY - _element.Y;
                }

                _element.X = mouseX;
                _element.Y = mouseY;
            }
        }

        /// <summary>
        /// Скалирует элемент в зависимости от направления скалирования
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        private static void Stretch(double mouseX, double mouseY)
        {

            Border border = (Border)_element.InputElement;
            var xDifferent = mouseX - _element.X;
            var yDifferent = mouseY - _element.Y;

            xDifferent = (border.Width + xDifferent) > border.MinWidth ? xDifferent : border.MinWidth;
            yDifferent = (border.Height + yDifferent) > border.MinHeight ? yDifferent : border.MinHeight;

            if (StretchDirection == StretchDirections.Both)
            {
                ((Border)_element.InputElement).Width += xDifferent;
                ((Border)_element.InputElement).Height += yDifferent;
            }
            else if (StretchDirection == StretchDirections.Right)
            {
                ((Border)_element.InputElement).Width += xDifferent;
            }
            else if (StretchDirection == StretchDirections.Bottom)
            {
                ((Border)_element.InputElement).Height += yDifferent;
            }
            else
            {
                StretchDirection = StretchDirections.None;
                _element.IsStretching = false;
            }


            _element.X = mouseX;
            _element.Y = mouseY;
        }
    }
}
