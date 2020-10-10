using ControllerEmulatorAPI.Global.Attributes;
using System.Collections.Generic;
using System.Reflection;

namespace ControllerEmulatorAPI.Global.Interfaces
{
    public interface ISettingsNode
    {
        public string Name { get; set; }

        Dictionary<PropertyInfo, ISetting> Settings { get; }
        Dictionary<string, ISettingsNode> SettingsNodes { get; }

        public ISettingsNode GetSettingsNode(string name)
        {
            foreach (var node in SettingsNodes)
            {
                if (node.Value.Name == name)
                {
                    return node.Value;
                }
            }

            return null;
        }

        public void SetValues(object obj)
        {
            foreach (var setting in Settings)
            {
                setting.Value.SetValue(obj);
            }
        }

        public ISettingsNode Add(SettingsNodeAttribute node);

        public void Add(PropertyInfo property, ISetting setting);

        public void Remove(ISettingsNode node);

        public void Remove(PropertyInfo property, ISetting setting);
    }
}