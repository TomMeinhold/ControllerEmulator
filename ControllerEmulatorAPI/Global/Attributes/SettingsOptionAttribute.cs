namespace ControllerEmulatorAPI.Global.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SettingsOptionAttribute : Attribute
    {
        public SettingsOptionAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}