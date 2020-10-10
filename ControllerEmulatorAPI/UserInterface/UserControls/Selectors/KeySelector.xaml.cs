namespace ControllerEmulatorAPI.UserInterface.UserControls.Selectors
{
    using ControllerEmulatorAPI.Global.Attributes;
    using ControllerEmulatorAPI.Global.Interfaces;
    using System;
    using System.Linq;
    using System.Windows.Controls;
    using Keys = System.Windows.Forms.Keys;

    /// <summary>
    /// Interaction logic for Keyselector.xaml
    /// </summary>
    public partial class KeySelector : UserControl, ISetting
    {
        private readonly static string[] keyStrings = Enum.GetNames(typeof(Keys)).ToArray();

        public KeySelector()
        {
            InitializeComponent();
            KeyBox.ItemsSource = keyStrings;
            KeyBox.IsTextSearchEnabled = true;
        }

        public KeySelector(SettingsOptionAttribute attribute)
        {
            InitializeComponent();
            KeyName.Content = attribute.Name;
            KeyBox.ItemsSource = keyStrings;
            KeyBox.IsTextSearchEnabled = true;
        }

        public object Value { get; set; }

        public event EventHandler OnValueChanged;

        private void KeyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Value = Enum.Parse<Keys>(keyStrings[KeyBox.SelectedIndex]);
            OnValueChanged?.Invoke(this, null);
        }

        public void SetValue(object obj)
        {
            if (obj is Keys keys)
            {
                string name = Enum.GetName(typeof(Keys), keys) ?? "";
                KeyBox.SelectedIndex = keyStrings.ToList().LastIndexOf(name);
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