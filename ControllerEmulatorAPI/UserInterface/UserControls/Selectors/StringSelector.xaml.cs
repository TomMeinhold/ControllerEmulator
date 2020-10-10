namespace ControllerEmulatorAPI.UserInterface.UserControls.Selectors
{
    using ControllerEmulatorAPI.Global.Attributes;
    using ControllerEmulatorAPI.Global.Interfaces;
    using System;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for StringSelector.xaml
    /// </summary>
    public partial class StringSelector : UserControl, ISetting
    {
        public StringSelector(SettingsOptionAttribute attribute)
        {
            InitializeComponent();
            NameLabel.Content = attribute.Name;
        }

        public object Value { get; set; }

        public event EventHandler OnValueChanged;

        public void SetValue(object obj) => TextBox.Text = obj as string ?? string.Empty;

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Value = TextBox.Text;
            OnValueChanged?.Invoke(this, null);
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