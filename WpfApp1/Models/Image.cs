using System.Windows;
using System.Windows.Media;

namespace Templator.Models
{
    public class Image
    {
        public Size InitialSize { get; private set; }
        public ImageSource StoredImage { get; private set; }

        public Image(ImageSource image, Size size)
        {
            InitialSize = size;
            StoredImage = image;
        }

        public override string ToString()
        {
            return $"Stored Image: {StoredImage} Initial Size {InitialSize}";
        }
    }
}