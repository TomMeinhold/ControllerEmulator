namespace ControllerEmulatorAPI.Controller.Parts
{
    using ControllerEmulatorAPI.Controller.Base;
    using ControllerEmulatorAPI.Controller.Enums;
    using ControllerEmulatorAPI.Controller.Parts.Base;
    using ControllerEmulatorAPI.vJoy;
    using RawInput;
    using RawInput.Events;

    public class AnalogStickLeft : BasePart
    {
        private bool togglerun;

        public AnalogStickLeft(VirualController c, RawInputWrapper i, VirtualJoystick j) : base(c, i, j)
        {
        }

        public override void Process(BaseRawInputEventArgs e)
        {
            if (e.Key == Profile.Analog_L_Up)
            {
                if (e.IsDown)
                {
                    if (togglerun)
                    {
                        Joystick.SetJoystickAxis(16000, Axis.HID_USAGE_Y);
                    }
                    else
                    {
                        Joystick.SetJoystickAxis(8000, Axis.HID_USAGE_Y);
                    }
                }
                else
                {
                    Joystick.SetJoystickAxis(0, Axis.HID_USAGE_Y);
                }
            }

            if (e.Key == Profile.Analog_L_Down)
            {
                if (e.IsDown)
                {
                    if (togglerun)
                    {
                        Joystick.SetJoystickAxis(-16000, Axis.HID_USAGE_Y);
                    }
                    else
                    {
                        Joystick.SetJoystickAxis(-8000, Axis.HID_USAGE_Y);
                    }
                }
                else
                {
                    Joystick.SetJoystickAxis(0, Axis.HID_USAGE_Y);
                }
            }

            if (e.Key == Profile.Analog_L_Left)
            {
                if (e.IsDown)
                {
                    if (togglerun)
                    {
                        Joystick.SetJoystickAxis(16000, Axis.HID_USAGE_X);
                    }
                    else
                    {
                        Joystick.SetJoystickAxis(8000, Axis.HID_USAGE_X);
                    }
                }
                else
                {
                    Joystick.SetJoystickAxis(0, Axis.HID_USAGE_X);
                }
            }

            if (e.Key == Profile.Analog_L_Right)
            {
                if (e.IsDown)
                {
                    if (togglerun)
                    {
                        Joystick.SetJoystickAxis(-16000, Axis.HID_USAGE_X);
                    }
                    else
                    {
                        Joystick.SetJoystickAxis(-8000, Axis.HID_USAGE_X);
                    }
                }
                else
                {
                    Joystick.SetJoystickAxis(0, Axis.HID_USAGE_X);
                }
            }

            if (e.Key == Profile.Analog_L_Click)
            {
                Joystick.SetJoystickButton(e.IsDown, (uint)ControllerKey.Analog_L_Click);
            }

            if (e.Key == Profile.RunKey)
            {
                if (e.IsDown)
                {
                    togglerun = !togglerun;
                }
            }
        }
    }
}