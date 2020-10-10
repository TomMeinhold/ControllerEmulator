using ControllerEmulatorAPI.Global;
using ControllerEmulatorAPI.UserInterface.UserControls;
using System.Windows.Controls;

namespace ControllerEmulatorAPI.UserInterface.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private readonly SettingsRootCategory settingsRoot = new SettingsRootCategory(typeof(Settings));
        private Settings activeSettings;

        public SettingsPage()
        {
            InitializeComponent();
            MainPanel.Children.Add(settingsRoot);
            IsEnabled = false;
        }

        public void Show(Settings settings)
        {
            activeSettings = settings;
            settingsRoot.SetValuesByInstance(activeSettings);
            settingsRoot.LinkToInstance(activeSettings);
            IsEnabled = true;
        }
    }
}