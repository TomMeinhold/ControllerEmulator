namespace ControllerEmulatorAPI.UserInterface.UserControls
{
    using ControllerEmulatorAPI.Controller.Base;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class UIProfile : UserControl
    {
        public UIProfile(Profile keymap)
        {
            InitializeComponent();
            Profile = keymap;
            ProfileName.Content = Profile.ProfileName;
        }

        public Profile Profile { get; }

        public event EventHandler OnSelected;

        public event EventHandler OnUnselected;

        public event EventHandler OnRemove;

        public void Select()
        {
            Grid.SetResourceReference(Panel.BackgroundProperty, "BackgroundProfilesSelected");
            OnSelected?.Invoke(this, null);
        }

        public void Unselect()
        {
            Grid.Background = (Brush)Application.Current.Resources["BackgroundProfiles"];
            OnUnselected?.Invoke(this, null);
        }

        public void Update()
        {
            ProfileName.Content = Profile.ProfileName;
        }

        private void Grid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            foreach (var child in ((StackPanel)Parent).Children)
            {
                if (child is UIProfile profile)
                {
                    profile.Unselect();
                }
            }

            Select();
        }

        private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnRemove?.Invoke(this, null);
        }
    }
}