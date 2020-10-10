using System;

namespace ControllerEmulatorAPI.Global.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SettingsNodeAttribute : Attribute
    {
        public SettingsNodeAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}