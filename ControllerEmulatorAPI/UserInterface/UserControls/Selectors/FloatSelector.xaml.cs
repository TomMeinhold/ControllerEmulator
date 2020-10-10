namespace ControllerEmulatorAPI.UserInterface.UserControls.Selectors
{
    using ControllerEmulatorAPI.Global.Attributes;
    using ControllerEmulatorAPI.Global.Interfaces;
    using System;
    using System.Globalization;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for FloatSelector.xaml
    /// </summary>
    public partial class FloatSelector : UserControl, ISetting
    {
        public FloatSelector(SettingsOptionAttribute attribute)
        {
            InitializeComponent();
            LabelName.Content = attribute.Name;
        }

        public object Value { get; set; }

        public event EventHandler OnValueChanged;

        public void SetValue(object obj)
        {
            if (obj is float f)
            {
                Value = f;
                TextBox.Text = f.ToString();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse(TextBox.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out float f))
            {
                Value = f;
                OnValueChanged?.Invoke(this, null);
            }
            else
            {
                TextBox.Text = ((float)Value).ToString();
            }
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
    }
}