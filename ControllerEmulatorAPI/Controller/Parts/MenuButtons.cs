namespace ControllerEmulatorAPI.Controller.Parts
{
    using ControllerEmulatorAPI.Controller.Base;
    using ControllerEmulatorAPI.Controller.Enums;
    using ControllerEmulatorAPI.Controller.Parts.Base;
    using ControllerEmulatorAPI.vJoy;
    using RawInput;
    using RawInput.Events;

    public class MenuButtons : BasePart
    {
        public MenuButtons(VirualController c, RawInputWrapper i, VirtualJoystick j) : base(c, i, j)
        {
        }

        public override void Process(BaseRawInputEventArgs e)
        {
            if (e.Key == Profile.Plus)
            {
                Joystick.SetJoystickButton(e.IsDown, (uint)ControllerKey.Plus);
            }
            if (e.Key == Profile.Minus)
            {
                Joystick.SetJoystickButton(e.IsDown, (uint)ControllerKey.Minus);
            }
        }
    }
}