using System.Windows;

namespace Templator.TransformElements
{
    /// <summary>
    /// Представляет трансформируемый графический UI-элемент
    /// </summary>
    class ImageElementTransform : ElementTransform
    {
        private string _name;

        /// <summary>
        /// Название элемента
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _name = value;
                }
            }
        }
        
        public ImageElementTransform(IInputElement imageElement)
        {
            InputElement = imageElement;
        }

        public ImageElementTransform(string name, IInputElement imageElement)
        {
            Name = name;
            InputElement = imageElement;
        }
    }
}