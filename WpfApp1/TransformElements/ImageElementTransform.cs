using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Templator.TransformElements
{
    class ImageElementTransform : ElementTransform
    {
        private string _name;

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

        public ImageElementTransform()
        {

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