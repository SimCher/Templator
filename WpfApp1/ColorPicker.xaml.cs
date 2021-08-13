using System;
using System.Windows;
using System.Windows.Controls;
using Templator.FontChooser;

namespace Templator
{
#nullable enable
    /// <summary>
    /// Логика взаимодействия для ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        private ColorPickerViewModel _viewModel;

        public FontColor SelectedFontColor
        {
            get
            {
                FontColor fc = (FontColor)GetValue(SelectedColorProperty);

                return fc;
            }

            set
            {
                _viewModel.SelectedFontColor = value;
                SetValue(SelectedColorProperty, value);
            }
        }

        public static readonly RoutedEvent ColorChangedEvent = EventManager.RegisterRoutedEvent(
            "ColorChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ColorPicker));

        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
            "SelectedColor", typeof(FontColor), typeof(ColorPicker), new UIPropertyMetadata(null));

        public ColorPicker()
        {
            InitializeComponent();
            _viewModel = new ColorPickerViewModel();
            DataContext = _viewModel;
        }

        public event RoutedEventHandler ColorChanged
        {
            add => AddHandler(ColorChangedEvent, value);
            remove => RemoveHandler(ColorChangedEvent, value);
        }

        private void RaiseColorChangedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(ColorPicker.ColorChangedEvent);
            RaiseEvent(newEventArgs);
        }

        private void SuperCombo_OnDropDownClosed(object? sender, EventArgs e)
        {
            SetValue(SelectedColorProperty, _viewModel.SelectedFontColor);
            RaiseColorChangedEvent();
        }

        private void SuperCombo_OnLoaded(object sender, RoutedEventArgs e)
        {
            SetValue(SelectedColorProperty, _viewModel.SelectedFontColor);
        }
    }
}
