using ControllerEmulatorAPI.Controller;
using ControllerEmulatorAPI.Controller.Base;
using ControllerEmulatorAPI.Global;
using ControllerEmulatorAPI.UserInterface.UserControls;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ControllerEmulatorAPI.UserInterface.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page, IDisposable
    {
        private bool switchSettings;
        private bool disposedValue;
        private readonly VirualController controller;
        private readonly DirectoryInfo rootDiectory = new DirectoryInfo("UserData");
        private readonly DirectoryInfo profilesDirectory = new DirectoryInfo("UserData/Profiles");
        private readonly FileInfo settingsFile = new FileInfo("UserData/settings.json");
        private readonly Settings settings;
        private readonly SettingsPage settingsPage = new SettingsPage();
        private readonly ProfilePage profilePage = new ProfilePage();

        public static Application Application { get; set; }

        public MainPage(Application application)
        {
            InitializeComponent();
            Application = application;
            GotKeyboardFocus += MainWindow_GotFocus;
            LostKeyboardFocus += MainWindow_LostFocus;
            if (!rootDiectory.Exists) { rootDiectory.Create(); }
            MainFrame.Navigate(profilePage);
            settings = Settings.Load(settingsFile);
            settingsPage.Show(settings);
            Profiles.Load(profilesDirectory);
            Profiles.AddToUI(ProfilePanel.Children);
            controller = new VirualController(Profiles.GetProfiles(), settings);
            controller.OnControllerDisable += Controller_OnControllerDisable;
            controller.OnControllerEnable += Controller_OnControllerEnable;
            controller.OnProfileSwitched += Controller_OnProfileSwitched;
            controller.OnProfileRemove += Controller_OnProfileRemove;
        }

        ~MainPage()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public static T GetResource<T>(string key)
        {
            return (T)Application.Resources[key];
        }

        public static void SetResource(string key, object value)
        {
            Application.Resources[key] = value;
        }

        private void MainWindow_LostFocus(object sender, RoutedEventArgs e)
        {
            controller.Enable();
        }

        private void MainWindow_GotFocus(object sender, RoutedEventArgs e)
        {
            controller.Disable();
        }

        private void Controller_OnProfileRemove(object sender, EventArgs e)
        {
            if (sender is UIProfile profile)
            {
                ProfilePanel.Children.Remove(profile);
            }
        }

        private void Controller_OnProfileSwitched(object sender, EventArgs e)
        {
            profilePage.ShowProfile((UIProfile)sender);
        }

        private void Controller_OnControllerEnable(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                StateLabel.Content = Application.Current.Resources["ActiveText"];
                StateLabel.SetResourceReference(Control.BackgroundProperty, "BackgroundStatusON");
            });
            Windows.NativeMethods.Toggle(false);
        }

        private void Controller_OnControllerDisable(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                StateLabel.Content = Application.Current.Resources["InactiveText"];
                StateLabel.SetResourceReference(Control.BackgroundProperty, "BackgroundStatusOFF");
            });
            Windows.NativeMethods.Toggle(true);
        }

        private void AddProfileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ProfilePanel.Children.Add(controller.NewProfile());
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            switchSettings = !switchSettings;
            if (switchSettings)
            {
                MainFrame.Navigate(settingsPage);
            }
            else
            {
                MainFrame.Navigate(profilePage);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    controller.Dispose();
                    Profiles.Save(profilesDirectory);
                    settings.Save(settingsFile);
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}