using ControllerEmulatorAPI.Global.Attributes;
using ControllerEmulatorAPI.Global.Interfaces;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace ControllerEmulatorAPI.UserInterface.UserControls
{
    /// <summary>
    /// Interaction logic for SettingsCategory.xaml
    /// </summary>
    public partial class SettingsCategory : UserControl, ISettingsNode
    {
        public SettingsCategory(SettingsNodeAttribute attribute, SettingsRootCategory settingsRoot)
        {
            InitializeComponent();
            NameLabel.Content = attribute.Name;
            SettingsGrid.Visibility = Visibility.Collapsed;
            SettingsRoot = settingsRoot;
        }

        public Dictionary<PropertyInfo, ISetting> Settings { get; } = new Dictionary<PropertyInfo, ISetting>();

        public Dictionary<string, ISettingsNode> SettingsNodes { get; } = new Dictionary<string, ISettingsNode>();
        public SettingsRootCategory SettingsRoot { get; }

        public void Add(PropertyInfo property, ISetting setting)
        {
            SettingsRoot.Settings.Add(property, setting);
            Settings.Add(property, setting);
            SettingsGrid.Children.Add(setting as UIElement);
        }

        public ISettingsNode Add(SettingsNodeAttribute attribute)
        {
            if (SettingsNodes.ContainsKey(attribute.Name))
            {
                return SettingsNodes[attribute.Name];
            }
            else
            {
                var cat = new SettingsCategory(attribute, SettingsRoot);
                SettingsNodes.Add(attribute.Name, cat);
                SettingsGrid.Children.Add(cat);
                return cat;
            }
        }

        public void Remove(PropertyInfo property, ISetting setting)
        {
            Settings.Remove(property);
            SettingsGrid.Children.Remove(setting as UIElement);
        }

        public void Remove(ISettingsNode node)
        {
            SettingsNodes.Remove(node.Name);
            SettingsGrid.Children.Remove(node as UIElement);
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            SettingsGrid.Visibility = Visibility.Collapsed;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            SettingsGrid.Visibility = Visibility.Visible;
        }
    }
}