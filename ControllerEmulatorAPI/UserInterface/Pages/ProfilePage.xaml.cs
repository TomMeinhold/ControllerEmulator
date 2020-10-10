using ControllerEmulatorAPI.Controller.Base;
using ControllerEmulatorAPI.UserInterface.UserControls;
using System.Linq;
using System.Windows.Controls;

namespace ControllerEmulatorAPI.UserInterface.Pages
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private readonly SettingsRootCategory settingsRoot = new SettingsRootCategory(typeof(Profile));
        private UIProfile activeProfile;

        public ProfilePage()
        {
            InitializeComponent();
            MainPanel.Children.Add(settingsRoot);
            settingsRoot.Settings.ToList().ForEach((x) =>
            {
                x.Value.OnValueChanged += (s, e) =>
                {
                    x.Key.SetValue(activeProfile.Profile, x.Value.Value);
                    activeProfile.Dispatcher.Invoke(() => activeProfile.Update());
                };
            });

            IsEnabled = false;
        }

        public void ShowProfile(UIProfile profile)
        {
            activeProfile = profile;
            settingsRoot.Settings.ToList().ForEach((x) =>
            {
                x.Value.SetValue(x.Key.GetValue(activeProfile.Profile));
            });

            IsEnabled = true;
        }
    }
}