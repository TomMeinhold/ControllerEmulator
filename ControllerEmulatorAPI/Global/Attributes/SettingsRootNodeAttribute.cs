using System;

namespace ControllerEmulatorAPI.Global.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SettingsRootNodeAttribute : Attribute
    {
        public SettingsRootNodeAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}