namespace ControllerEmulatorAPI.Global.Interfaces
{
    using System;

    public interface ISetting
    {
        public event EventHandler OnValueChanged;

        public object Value { get; set; }

        public void SetValue(object obj);

        public void ClearEvents();
    }
}