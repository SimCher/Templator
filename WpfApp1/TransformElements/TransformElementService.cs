using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Templator.TransformElements
{
    /// <summary>
    /// Обслуживающий класс для работы с интерактивными элементами управления
    /// </summary>
    internal static class TransformElementService
    {
        #region NestedTypes
        /// <summary>
        /// Содержит названия направлений при растягивании элемента
        /// </summary>
        public enum StretchDirections
        {
            None, Right, Bottom, Both
        }

        /// <summary>
        /// Класс, представляющий элемент интерфейса, доступный для трансформирования и скалирования
        /// </summary>
        class TransformElement
        {
            /// <summary>
            /// Структура, представляющая точку на трёхмерной оси координат, но z здесь является индексом (int).
            /// Хранит координаты в double, в отличии от стандартного Point (там int)
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="_zIndex"></param>
            private struct PointXYZ
            {
                private double _x;
                private double _y;
                private int _zIndex;

                public double X { get => _x; set => _x = value; }
                public double Y { get => _y; set => _y = value; }
                public int ZIndex
                {
                    get => _zIndex;
                    set => _zIndex = value;
                }


                public PointXYZ(double x, double y, int zIndex)
                {
                    _x = x;
                    _y = y;
                    _zIndex = zIndex;
                }

                public override string ToString()
                {
                    return $"[X = {_x}, Y = {_y}, ZIndex = {_zIndex}]";
                }
            }
            #region Fields
            private bool _isDragging;
            private bool _isStretching;
            private bool _isStretchLeft;
            private bool _isStretchRight;
            private IInputElement _inputElement;
            private PointXYZ _position;
            #endregion
            #region Properties

            /// <summary>
            /// Элемент UI WPF который необходимо изменить в рантайме
            /// </summary>
            public IInputElement InputElement
            {
                get => _inputElement;
                set
                {
                    _inputElement = value;
                    _isDragging = false;
                    _isStretching = false;
                }
            }

            /// <summary>
            /// X-координата элемента
            /// </summary>
            public double X
            {
                get => _position.X;
                set => _position.X = value;
            }

            /// <summary>
            /// Y-координата элемента
            /// </summary>
            public double Y
            {
                get => _position.Y;
                set => _position.Y = value;
            }
            /// <summary>
            /// Индекс элемента по Z
            /// </summary>
            public int ZIndex
            {
                get => _position.ZIndex;
                set => _position.ZIndex = value;
            }
            /// <summary>
            /// Перетаскивается ли элемент
            /// </summary>
            public bool IsDragging
            {
                get => _isDragging;
                set
                {
                    _isDragging = value;
                    _isStretching = !_isDragging;
                }
            }
            /// <summary>
            /// Растягивается ли элемент
            /// </summary>
            public bool IsStretching
            {
                get => _isStretching;
                set
                {
                    _isStretching = value;
                    _isDragging = !_isStretching;
                }
            }
            /// <summary>
            /// Растягивается ли элемент влево
            /// </summary>
            public bool IsStretchLeft
            {
                get => _isStretchLeft;
                set
                {
                    _isStretchLeft = value;
                    _isStretchRight = !_isStretchLeft;
                }
            }
            /// <summary>
            /// Растягивается ли элемент вправо
            /// </summary>
            public bool IsStretchRight
            {
                get => _isStretchRight;
                set
                {
                    _isStretchRight = value;
                    _isStretchLeft = !_isStretchRight;
                }
            }
            #endregion

            #region Methods

            #endregion
        }
        #endregion NestedClasses

        #region Fields

        private static TransformElement Element { get; }

        #endregion

        #region Constructor
        static TransformElementService()
        {
            Element = new TransformElement();
        }
#endregion

        #region PublicMethods
        /// <summary>
        /// "Захватывает" объект, находящийся под курсором. Следует вызывать в обработчике нажатия ЛКМ
        /// </summary>
        /// <param name="mouseX">X-координата курсора</param>
        /// <param name="mouseY">Y-координата курсора</param>
        public static void Grab(double mouseX, double mouseY)
        {
            Element.X = mouseX;
            Element.Y = mouseY;

            Element.InputElement?.CaptureMouse();

            if (!Element.IsStretching)
            {
                Element.IsDragging = true;
            }
        }

        /// <summary>
        /// "Отпускает" объект, который ранее был "захвачен". Следует вызывать в обработчике отпускания ЛКМ
        /// </summary>
        public static void Drop()
        {
            Element.InputElement?.ReleaseMouseCapture();
        }

        /// <summary>
        /// Перетаскивает или растягивает объект, в зависимости от состояния элемента. Следует вызывать в обработчике движения мыши
        /// </summary>
        /// <param name="mouseX">Х-координата мыши</param>
        /// <param name="mouseY">Y-координата мыши</param>
        /// <param name="cursor">Курсор (по нему определяется направление растяжения)</param>
        public static void DragOrStretch(double mouseX, double mouseY, Cursor cursor = null)
        {
            BringToFront();

            if (Element.IsDragging)
            {
                DragElement(mouseX, mouseY);
            }

            if (Element.IsStretching)
            {
                StretchElement(mouseX, mouseY, cursor);
            }
        }

        /// <summary>
        /// Устанавливает новый элемент для трансформации или скалирования. 
        /// </summary>
        /// <param name="element">Элемент для трансформации или скалирования</param>
        public static void SetNewElement(object element)
        {
            int newZIndex = (int)((Border)element).GetValue(Panel.ZIndexProperty);
            Element.ZIndex = newZIndex > Element.ZIndex ? newZIndex : Element.ZIndex;

            Element.InputElement = (IInputElement)element;
        }

        /// <summary>
        /// Устанавливает новое направление скалирования элемента и возвращает его в виде перечисления StretchDirections.
        /// Следует вызывать в обработчике выхода мыши за границы элемента.
        /// </summary>
        /// <param name="mouseX">х-координата курсора</param>
        /// <param name="mouseY">у-координата курсора</param>
        /// <param name="element">Скалируемый объект</param>
        /// <returns></returns>
        public static StretchDirections GetStretchDirection(double mouseX, double mouseY, object element)
        {
            var stretchDirections = StretchDirections.None;
            var border = (Border)element;
            double rightBorder = border.ActualWidth - border.Padding.Right;
            double bottomBorder = border.ActualHeight - border.Padding.Bottom;

            if ((mouseX >= rightBorder && mouseX < border.ActualWidth) &&
                (mouseY >= bottomBorder && mouseY < border.ActualHeight))
            {
                stretchDirections = StretchDirections.Both;
            }
            else if (mouseX >= rightBorder && mouseX < border.ActualWidth)
            {
                stretchDirections = StretchDirections.Right;
            }
            else if (mouseY >= bottomBorder && mouseY < border.ActualHeight)
            {
                stretchDirections = StretchDirections.Bottom;
            }

            Element.InputElement = (IInputElement)element;
            Element.X = mouseX;
            Element.Y = mouseY;
            Element.IsStretching = true;

            return stretchDirections;
        }

        /// <summary>
        /// Сбрасывает состояния объекта, отвечающие за перетаскивание и скалирование
        /// </summary>
        public static void ResetStates()
        {
            Element.IsDragging = false;
            Element.IsStretching = false;
        }
        #endregion PublicMethods
        #region PrivateMethods

        /// <summary>
        /// Устанавливает самый высокий Z-индекс и выводит элемент на первый план
        /// </summary>
        private static void BringToFront()
        {
            ((Border)Element.InputElement)?.SetValue(Panel.ZIndexProperty, Element.ZIndex++);
        }

        /// <summary>
        /// Скалирует элемент относительно положения курсора мыши
        /// </summary>
        /// <param name="mouseX">X-координата курсора мыши</param>
        /// <param name="mouseY">Y-координата курсора мыши</param>
        /// <param name="cursor">Курсор мыши (нужен для указания направления скалирования)</param>
        private static void StretchElement(double mouseX, double mouseY, Cursor cursor)
        {
            Border border = (Border)Element.InputElement;
            var xDifferent = mouseX - Element.X;
            var yDifferent = mouseY - Element.Y;

            xDifferent = (border.Width + xDifferent) > border.MinWidth ? xDifferent : border.MinWidth;
            yDifferent = (border.Height + yDifferent) > border.MinHeight ? yDifferent : border.MinHeight;

            if (cursor == Cursors.SizeNWSE)
            {
                ((Border)Element.InputElement).Width += xDifferent;
                ((Border)Element.InputElement).Height += yDifferent;
            }
            else if (cursor == Cursors.SizeWE)
            {
                ((Border)Element.InputElement).Width += xDifferent;
            }
            else if (cursor == Cursors.SizeNS)
            {
                ((Border)Element.InputElement).Height += yDifferent;
            }
            else
            {
                cursor = Cursors.Arrow;
                Element.IsStretching = false;
            }

            Element.X = mouseX;
            Element.Y = mouseY;
        }

        /// <summary>
        /// Перетаскивает объект относительно положения курсора мыши
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        private static void DragElement(double mouseX, double mouseY)
        {
            if (((UIElement)Element.InputElement).RenderTransform is not TransformGroup transformGroup)
            {
                return;
            }

            var translateTransforms = transformGroup.Children
                .Where(t => t.GetType().Name == "TranslateTransform").Select(t => t);

            foreach (var transform in translateTransforms)
            {
                var translateTransform = (TranslateTransform)transform;
                translateTransform.X += mouseX - Element.X;
                translateTransform.Y += mouseY - Element.Y;
            }

            Element.X = mouseX;
            Element.Y = mouseY;


        }
#endregion PrivateMethods
    }
}
