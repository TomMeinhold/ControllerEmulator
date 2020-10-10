namespace ControllerEmulatorAPI.Controller.Parts
{
    using ControllerEmulatorAPI.Controller.Base;
    using ControllerEmulatorAPI.Controller.Parts.Base;
    using ControllerEmulatorAPI.vJoy;
    using RawInput;
    using RawInput.Events;

    public class DPad : BasePart
    {
        private const int CENTER = -1;
        private const int UP = 0;
        private const int RIGHT = 1;
        private const int DOWN = 2;
        private const int LEFT = 3;

        public DPad(VirualController c, RawInputWrapper i, VirtualJoystick j) : base(c, i, j)
        {
        }

        public override void Process(BaseRawInputEventArgs e)
        {
            if (e.Key == Profile.DPad_Up)
            {
                if (e.IsDown)
                {
                    Joystick.SetJoystickHat(UP, Hats.Hat);
                }
                else
                {
                    Joystick.SetJoystickHat(CENTER, Hats.Hat);
                }
            }

            if (e.Key == Profile.DPad_Right)
            {
                if (e.IsDown)
                {
                    Joystick.SetJoystickHat(RIGHT, Hats.Hat);
                }
                else
                {
                    Joystick.SetJoystickHat(CENTER, Hats.Hat);
                }
            }

            if (e.Key == Profile.DPad_Down)
            {
                if (e.IsDown)
                {
                    Joystick.SetJoystickHat(DOWN, Hats.Hat);
                }
                else
                {
                    Joystick.SetJoystickHat(CENTER, Hats.Hat);
                }
            }

            if (e.Key == Profile.DPad_Left)
            {
                if (e.IsDown)
                {
                    Joystick.SetJoystickHat(LEFT, Hats.Hat);
                }
                else
                {
                    Joystick.SetJoystickHat(CENTER, Hats.Hat);
                }
            }
        }
    }
}