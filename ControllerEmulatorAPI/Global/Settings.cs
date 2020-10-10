namespace ControllerEmulatorAPI.Global
{
    using ControllerEmulatorAPI.Global.Attributes;
    using ControllerEmulatorAPI.UserInterface.Pages;
    using System.IO;
    using System.Windows.Forms;
    using System.Windows.Media;

    [SettingsRootNode("Settings")]
    public class Settings
    {
        public Settings()
        {
            EnableControllerKey = Keys.F3;
            ProfileUpKey = Keys.NumPad8;
            ProfileDownKey = Keys.NumPad7;
            XCurveFlatness = 100;
            YCurveFlatness = 100;
            BaseSensitivity = 10;
            BaseMultiplier = 100;
        }

        [SettingsNode("General")]
        [SettingsOption("Enable Controller Key")]
        public Keys EnableControllerKey { get; set; }

        [SettingsNode("General")]
        [SettingsOption("Profile Up")]
        public Keys ProfileUpKey { get; set; }

        [SettingsNode("General")]
        [SettingsOption("Profile Down")]
        public Keys ProfileDownKey { get; set; }

        [SettingsNode("Mouse")]
        [SettingsOption("Mouse curve flatness X")]
        public float XCurveFlatness { get; set; }

        [SettingsNode("Mouse")]
        [SettingsOption("Mouse curve flatness Y")]
        public float YCurveFlatness { get; set; }

        [SettingsNode("Mouse")]
        [SettingsOption("Mouse curve base")]
        public float BaseSensitivity { get; set; }

        [SettingsNode("Mouse")]
        [SettingsOption("Mouse curve base multiplier")]
        public float BaseMultiplier { get; set; }

        [SettingsNode("Mouse")]
        [SettingsOption("Mouse buffer")]
        public float MouseBuffer { get; set; }

        #region MainWindow

        [SettingsNode("Theme")]
        [SettingsNode("MainWindow")]
        [SettingsOption("Background")]
        public Color BackgroundMainWindow
        {
            get => MainPage.GetResource<SolidColorBrush>("BackgroundMainWindow").Color;
            set => MainPage.SetResource("BackgroundMainWindow", new SolidColorBrush(value));
        }

        #endregion MainWindow

        #region Buttons

        [SettingsNode("Theme")]
        [SettingsNode("Buttons")]
        [SettingsOption("Background")]
        public Color BackgroundButton
        {
            get => MainPage.GetResource<SolidColorBrush>("BackgroundButton").Color;
            set => MainPage.SetResource("BackgroundButton", new SolidColorBrush(value));
        }

        [SettingsNode("Theme")]
        [SettingsNode("Buttons")]
        [SettingsOption("Foreground")]
        public Color ForegroundButton
        {
            get => MainPage.GetResource<SolidColorBrush>("ForegroundButton").Color;
            set => MainPage.SetResource("ForegroundButton", new SolidColorBrush(value));
        }

        [SettingsNode("Theme")]
        [SettingsNode("Buttons")]
        [SettingsOption("Border")]
        public Color BorderButton
        {
            get => MainPage.GetResource<SolidColorBrush>("BorderButton").Color;
            set => MainPage.SetResource("BorderButton", new SolidColorBrush(value));
        }

        #endregion Buttons

        #region Profiles

        [SettingsNode("Theme")]
        [SettingsNode("Profiles")]
        [SettingsOption("Background")]
        public Color BackgroundProfiles
        {
            get => MainPage.GetResource<SolidColorBrush>("BackgroundProfiles").Color;
            set => MainPage.SetResource("BackgroundProfiles", new SolidColorBrush(value));
        }

        [SettingsNode("Theme")]
        [SettingsNode("Profiles")]
        [SettingsOption("Selected")]
        public Color BackgroundProfilesSelected
        {
            get => MainPage.GetResource<SolidColorBrush>("BackgroundProfilesSelected").Color;
            set => MainPage.SetResource("BackgroundProfilesSelected", new SolidColorBrush(value));
        }

        #endregion Profiles

        #region Profiles-Tab

        [SettingsNode("Theme")]
        [SettingsNode("Profiles-Tab")]
        [SettingsOption("Background")]
        public Color BackgroundProfilesTab
        {
            get => MainPage.GetResource<SolidColorBrush>("BackgroundProfilesTab").Color;
            set => MainPage.SetResource("BackgroundProfilesTab", new SolidColorBrush(value));
        }

        #endregion Profiles-Tab

        #region Selectors

        [SettingsNode("Theme")]
        [SettingsNode("Selector")]
        [SettingsOption("Background")]
        public Color BackgroundSelectors
        {
            get => MainPage.GetResource<SolidColorBrush>("BackgroundSelectors").Color;
            set => MainPage.SetResource("BackgroundSelectors", new SolidColorBrush(value));
        }

        [SettingsNode("Theme")]
        [SettingsNode("Selector")]
        [SettingsOption("Hover")]
        public Color HoverSelectors
        {
            get => MainPage.GetResource<SolidColorBrush>("HoverSelectors").Color;
            set => MainPage.SetResource("HoverSelectors", new SolidColorBrush(value));
        }

        #endregion Selectors

        #region Status

        [SettingsNode("Theme")]
        [SettingsNode("Status")]
        [SettingsOption("On")]
        public Color BackgroundStatusON
        {
            get => MainPage.GetResource<SolidColorBrush>("BackgroundStatusON").Color;
            set => MainPage.SetResource("BackgroundStatusON", new SolidColorBrush(value));
        }

        [SettingsNode("Theme")]
        [SettingsNode("Status")]
        [SettingsOption("Off")]
        public Color BackgroundStatusOFF
        {
            get => MainPage.GetResource<SolidColorBrush>("BackgroundStatusOFF").Color;
            set => MainPage.SetResource("BackgroundStatusOFF", new SolidColorBrush(value));
        }

        #endregion Status

        #region Textbox

        [SettingsNode("Theme")]
        [SettingsNode("Textbox")]
        [SettingsOption("Background")]
        public Color BackgroundTextBox
        {
            get => MainPage.GetResource<SolidColorBrush>("BackgroundTextBox").Color;
            set => MainPage.SetResource("BackgroundTextBox", new SolidColorBrush(value));
        }

        #endregion Textbox

        #region Combobox

        [SettingsNode("Theme")]
        [SettingsNode("Combobox")]
        [SettingsOption("Background")]
        public Color BackgroundComboBox
        {
            get => MainPage.GetResource<SolidColorBrush>("BackgroundComboBox").Color;
            set => MainPage.SetResource("BackgroundComboBox", new SolidColorBrush(value));
        }

        #endregion Combobox

        #region Label

        [SettingsNode("Theme")]
        [SettingsNode("Label")]
        [SettingsOption("Background")]
        public Color BackgroundLabel
        {
            get => MainPage.GetResource<SolidColorBrush>("BackgroundLabel").Color;
            set => MainPage.SetResource("BackgroundLabel", new SolidColorBrush(value));
        }

        #endregion Label

        public static Settings Load(FileInfo file)
        {
            if (!file.Exists)
            {
                return new Settings();
            }

            return Serializer.Deserialize<Settings>(file);
        }

        public void Save(FileInfo file)
        {
            Serializer.Serialize(file, this);
        }
    }
}