using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace Templator.FontChooser
{
    public class ColorPickerViewModel : INotifyPropertyChanged
    {
        private ReadOnlyCollection<FontColor> _fontColors;
        private FontColor _selectedColor;

        public ColorPickerViewModel()
        {
            _selectedColor = AvailableColors.GetFontColor(Colors.Black);
            _fontColors = new ReadOnlyCollection<FontColor>(new AvailableColors());
        }

        public ReadOnlyCollection<FontColor> FontColors
        {
            get => _fontColors;
        }

        public FontColor SelectedFontColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor == value) return;
                _selectedColor = value;
                OnPropertyChanged("SelectedFontColor");
            }
        }

        #region Члены INotifyPropertyChanged

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}