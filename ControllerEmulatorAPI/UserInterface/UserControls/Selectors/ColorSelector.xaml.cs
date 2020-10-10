using ControllerEmulatorAPI.Global.Attributes;
using ControllerEmulatorAPI.Global.Interfaces;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControllerEmulatorAPI.UserInterface.UserControls.Selectors
{
    /// <summary>
    /// Interaction logic for ColorSelector.xaml
    /// </summary>
    public partial class ColorSelector : UserControl, ISetting
    {
        public ColorSelector(SettingsOptionAttribute attribute)
        {
            InitializeComponent();
            ColorPicker.Visibility = Visibility.Collapsed;
            ColorPicker.OnValueChanged += ColorPicker_OnValueChanged;
            LabelName.Content = attribute.Name;
        }

        private void ColorPicker_OnValueChanged(object sender, EventArgs e)
        {
            ColorPreview.Background = new SolidColorBrush(ColorPicker.CurrentColor.Color);
            OnValueChanged?.Invoke(sender, e);
        }

        public event EventHandler OnValueChanged;

        public object Value { get => ColorPicker.CurrentColor.Color; set => ColorPicker.SetValue(value as Color? ?? default); }

        public void SetValue(object obj)
        {
            ColorPicker.SetValue(obj);
        }

        public void ClearEvents()
        {
            if (OnValueChanged != null)
            {
                foreach (Delegate @delegate in OnValueChanged.GetInvocationList())
                {
                    OnValueChanged -= (EventHandler)@delegate;
                }
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ColorPicker.Visibility = Visibility.Visible;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ColorPicker.Visibility = Visibility.Collapsed;
        }
    }
}