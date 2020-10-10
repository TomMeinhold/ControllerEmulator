namespace ControllerEmulatorAPI.Controller.Parts
{
    using ControllerEmulatorAPI.Controller.Base;
    using ControllerEmulatorAPI.Controller.Enums;
    using ControllerEmulatorAPI.Controller.Parts.Base;
    using ControllerEmulatorAPI.vJoy;
    using RawInput;
    using RawInput.Events;

    public class FaceButtons : BasePart
    {
        public FaceButtons(VirualController c, RawInputWrapper i, VirtualJoystick j) : base(c, i, j)
        {
        }

        public override void Process(BaseRawInputEventArgs e)
        {
            if (e.Key == Profile.A)
            {
                Joystick.SetJoystickButton(e.IsDown, (int)ControllerKey.A);
            }

            if (e.Key == Profile.B)
            {
                Joystick.SetJoystickButton(e.IsDown, (int)ControllerKey.B);
            }

            if (e.Key == Profile.X)
            {
                Joystick.SetJoystickButton(e.IsDown, (int)ControllerKey.X);
            }

            if (e.Key == Profile.Y)
            {
                Joystick.SetJoystickButton(e.IsDown, (int)ControllerKey.Y);
            }
        }
    }
}