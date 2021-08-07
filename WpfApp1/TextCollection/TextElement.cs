using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Templator.Annotations;

namespace Templator.TextCollection
{
    public class TextElement : INotifyPropertyChanged
    {
        private string _name;
        private UIElement _textElement;

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

        public UIElement TextControl
        {
            get => _textElement;
            set
            {
                if (value != null)
                {
                    _textElement = value;
                }
            }
        }

        public TextElement()
        {
            
        }

        public TextElement(string name, UIElement textElement)
        {
            Name = name;
            TextControl = textElement;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Name: {Name} => TextElement:{TextControl}";
        }
    }
}
