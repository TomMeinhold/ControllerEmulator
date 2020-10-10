namespace ControllerEmulatorAPI.Controller.Parts
{
    using ControllerEmulatorAPI.Controller.Base;
    using ControllerEmulatorAPI.Controller.Enums;
    using ControllerEmulatorAPI.Controller.Parts.Base;
    using ControllerEmulatorAPI.vJoy;
    using RawInput;
    using RawInput.Events;

    public class ShoulderButtons : BasePart
    {
        public ShoulderButtons(VirualController c, RawInputWrapper i, VirtualJoystick j) : base(c, i, j)
        {
        }

        public override void Process(BaseRawInputEventArgs e)
        {
            if (e.Key == Profile.ZL)
            {
                if (e.IsDown)
                {
                    Joystick.SetJoystickAxis(32000, Axis.HID_USAGE_Z);
                }
                else
                {
                    Joystick.SetJoystickAxis(16000, Axis.HID_USAGE_Z);
                }
            }

            if (e.Key == Profile.ZR)
            {
                if (e.IsDown)
                {
                    Joystick.SetJoystickAxis(-16000, Axis.HID_USAGE_Z);
                }
                else
                {
                    Joystick.SetJoystickAxis(16000, Axis.HID_USAGE_Z);
                }
            }

            if (e.Key == Profile.L)
            {
                Joystick.SetJoystickButton(e.IsDown, (uint)ControllerKey.L);
            }

            if (e.Key == Profile.R)
            {
                Joystick.SetJoystickButton(e.IsDown, (uint)ControllerKey.R);
            }
        }
    }
}