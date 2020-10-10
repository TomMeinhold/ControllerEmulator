namespace ControllerEmulatorAPI.Controller.Parts
{
    using ControllerEmulatorAPI.Controller.Parts.Base;
    using ControllerEmulatorAPI.vJoy;
    using RawInput;
    using RawInput.Events;

    public class SpecialButtons : BasePart
    {
        public SpecialButtons(VirualController c, RawInputWrapper i, VirtualJoystick j) : base(c, i, j)
        {
            OverwriteNullSafety = true;
            AlwaysActive = true;
        }

        public override void Process(BaseRawInputEventArgs e)
        {
            if (e.Key == VirualController.Settings.EnableControllerKey && !e.IsDown)
            {
                VirualController.Enabled = !VirualController.Enabled;
            }
            else if (e.Key == VirualController.Settings.ProfileUpKey && !e.IsDown)
            {
                if (VirualController.ProfileIndex == 0)
                {
                    VirualController.ProfileIndex = VirualController.Profiles.Count - 1;
                }
                else
                {
                    VirualController.ProfileIndex--;
                }
            }
            else if (e.Key == VirualController.Settings.ProfileDownKey && !e.IsDown)
            {
                if (VirualController.ProfileIndex == VirualController.Profiles.Count - 1)
                {
                    VirualController.ProfileIndex = 0;
                }
                else
                {
                    VirualController.ProfileIndex++;
                }
            }
        }
    }
}