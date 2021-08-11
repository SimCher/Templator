using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Templator.Annotations;

namespace Templator.TransformElements
{
    public class TextElementTransform : ElementTransform, INotifyPropertyChanged
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
                    OnPropertyChanged(Name);
                }
            }
        }

        public TextElementTransform(string name, IInputElement element)
        {
            Name = name;
            InputElement = element;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Name: {Name} => TextElement:{InputElement}";
        }
    }
}
