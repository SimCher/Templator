using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Templator.TransformElements
{
    /// <summary>
    /// Представляет трансформируемый UI-элемент
    /// </summary>
    public class ElementTransform
    {
        private bool _isDragging;
        private bool _isStretching;
        private IInputElement _inputElement;
        private Type _inputElementChildType;
        private PointXYZ _position;

        /// <summary>
        /// Элемент UI WPF который необходимо изменить в рантайме
        /// </summary>
        internal IInputElement InputElement
        {
            get => _inputElement;
            set
            {
                _inputElement = value;
                _isDragging = false;
                _isStretching = false;

                if (_inputElement is Border border)
                {
                    InputElementChildType = border.Child.GetType();
                }
            }
        }

        /// <summary>
        /// Объект типа Type, который содержит название потомка текущего экземпляра
        /// </summary>
        internal Type InputElementChildType
        {
            get => _inputElementChildType;
            private set => _inputElementChildType = value;
        }

        /// <summary>
        /// X-координата элемента
        /// </summary>
        internal double X
        {
            get => _position.X;
            set => _position.X = value;
        }

        /// <summary>
        /// Y-координата элемента
        /// </summary>
        internal double Y
        {
            get => _position.Y;
            set => _position.Y = value;
        }

        /// <summary>
        /// Индекс элемента по Z
        /// </summary>
        internal int ZIndex
        {
            get => _position.ZIndex;
            set => _position.ZIndex = value;
        }

        /// <summary>
        /// Перетаскивается ли элемент
        /// </summary>
        internal bool IsDragging
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
        internal bool IsStretching
        {
            get => _isStretching;
            set
            {
                _isStretching = value;
                _isDragging = !_isStretching;
            }
        }
    }
}
