using ControllerEmulatorAPI.Global.Attributes;
using ControllerEmulatorAPI.Global.Interfaces;
using ControllerEmulatorAPI.UserInterface.UserControls.Selectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControllerEmulatorAPI.UserInterface.UserControls
{
    /// <summary>
    /// Interaction logic for SettingsRootCategory.xaml
    /// </summary>
    public partial class SettingsRootCategory : UserControl
    {
        public SettingsRootCategory(Type type) : this(type.GetCustomAttribute<SettingsRootNodeAttribute>())
        {
            foreach (var property in type.GetProperties())
            {
                var attribute = property.GetCustomAttribute<SettingsOptionAttribute>();
                var categoryAttributes = property.GetCustomAttributes<SettingsNodeAttribute>();
                ISettingsNode settingsNode = null;

                foreach (var cat in categoryAttributes)
                {
                    if (settingsNode is null)
                    {
                        settingsNode = Add(cat);
                    }
                    else
                    {
                        settingsNode = settingsNode.Add(cat);
                    }
                }

                if (attribute != null)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        settingsNode.Add(property, new StringSelector(attribute));
                    }
                    else if (property.PropertyType == typeof(System.Windows.Forms.Keys))
                    {
                        settingsNode.Add(property, new KeySelector(attribute));
                    }
                    else if (property.PropertyType == typeof(float))
                    {
                        settingsNode.Add(property, new FloatSelector(attribute));
                    }
                    else if (property.PropertyType == typeof(Color))
                    {
                        settingsNode.Add(property, new ColorSelector(attribute));
                    }
                }
            }
        }

        public SettingsRootCategory(SettingsRootNodeAttribute attribute)
        {
            InitializeComponent();
            NameLabel.Content = attribute.Name;
        }

        public Dictionary<PropertyInfo, ISetting> Settings { get; } = new Dictionary<PropertyInfo, ISetting>();

        public Dictionary<string, ISettingsNode> SettingsCategories = new Dictionary<string, ISettingsNode>();

        public event EventHandler OnValueChanged;

        public ISettingsNode Add(SettingsNodeAttribute attribute)
        {
            if (SettingsCategories.ContainsKey(attribute.Name))
            {
                return SettingsCategories[attribute.Name];
            }
            else
            {
                var cat = new SettingsCategory(attribute, this);
                SettingsCategories.Add(attribute.Name, cat);
                CategoryGrid.Children.Add(cat);
                return cat;
            }
        }

        public ISettingsNode GetCategory(string name) => SettingsCategories[name];

        public void Remove(string name)
        {
            CategoryGrid.Children.Remove((System.Windows.UIElement)SettingsCategories[name]);
            SettingsCategories.Remove(name);
        }

        public void ForEach(Action<KeyValuePair<PropertyInfo, ISetting>> action)
        {
            Settings.ToList().ForEach(action);
        }

        public void LinkToInstance(object obj)
        {
            ForEach((x) =>
            {
                x.Value.ClearEvents();
                x.Value.OnValueChanged += OnValueChanged;
                x.Value.OnValueChanged += (s, e) =>
                {
                    x.Key.SetValue(obj, x.Value.Value);
                };
            });
        }

        public void SetValuesByInstance(object obj)
        {
            ForEach((x) =>
            {
                x.Value.SetValue(x.Key.GetValue(obj));
            });
        }
    }
}