namespace ControllerEmulatorAPI.Controller
{
    using ControllerEmulatorAPI.Controller.Base;
    using ControllerEmulatorAPI.Controller.Parts.Base;
    using ControllerEmulatorAPI.Global;
    using ControllerEmulatorAPI.UserInterface.UserControls;
    using ControllerEmulatorAPI.vJoy;
    using RawInput;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    public class VirualController : IDisposable
    {
        private readonly VirtualJoystick joystick = new VirtualJoystick(1);
        private readonly List<BasePart> parts = new List<BasePart>();
        private bool disposedValue;
        private bool enabled;
        private int profileIndex;

        public VirualController(List<UIProfile> profiles, Settings settings)
        {
            Profiles = profiles;
            Settings = settings;
            Profiles.ForEach(x => x.OnSelected += Profiles_OnSelected);
            Profiles.ForEach(x => x.OnRemove += Profiles_OnRemove);
            Profile profile = profiles.FirstOrDefault()?.Profile ?? null;
            RawInputWrapper.RegisterControl(HidUsageAndPage.Keyboard, RawInputDeviceFlags.InputSink | RawInputDeviceFlags.NoLegacy);
            joystick.Aquire();
            joystick.Reset();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where((x) => x.BaseType == typeof(BasePart)))
            {
                /// Creates an instance of the extension class of the class <see cref="BasePart"/>.
                parts.Add((BasePart)type.Assembly.CreateInstance(type.FullName, false, BindingFlags.CreateInstance, null, new object[] { this, RawInputWrapper, joystick }, CultureInfo.CurrentCulture, null));
            }
        }

        ~VirualController()
        {
            Dispose(disposing: false);
        }

        public RawInputWrapper RawInputWrapper { get; } = new RawInputWrapper();

        public List<UIProfile> Profiles { get; }

        public Profile CurrentProfile { get => Profiles[ProfileIndex].Profile; }

        public UIProfile CurrentUIProfile { get => Profiles[ProfileIndex]; }

        public Settings Settings { get; }

        public event EventHandler OnControllerEnable;

        public event EventHandler OnControllerDisable;

        public event EventHandler OnIndexChanged;

        public event EventHandler OnProfileSwitched;

        public event EventHandler OnProfileRemove;

        public int ProfileIndex
        {
            get { return profileIndex; }
            set
            {
                profileIndex = value;
                Profiles.ForEach(x => x.Unselect());
                Profiles[value].Select();
                OnIndexChanged?.Invoke(this, null);
            }
        }

        public UIProfile NewProfile()
        {
            int index = Profiles.Count;
            Profiles.Add(new UIProfile(new Profile()));
            Profiles[index].OnSelected += Profiles_OnSelected;
            Profiles[index].OnRemove += Profiles_OnRemove;
            ProfileIndex = index;
            return Profiles[index];
        }

        public void RemoveProfile(UIProfile profile)
        {
            if (Profiles.IndexOf(profile) == ProfileIndex)
            {
                ProfileIndex = 0;
            }

            Profiles.Remove(profile);
            profile.OnSelected -= Profiles_OnSelected;
            profile.OnRemove -= Profiles_OnRemove;
            OnProfileRemove?.Invoke(profile, null);
        }

        public bool Enabled
        {
            set
            {
                if (value)
                {
                    OnControllerEnable?.Invoke(this, null);
                    OnIndexChanged?.Invoke(this, null);
                    RawInputWrapper.RegisterControl(HidUsageAndPage.Mouse, RawInputDeviceFlags.ExInputSink | RawInputDeviceFlags.NoLegacy | RawInputDeviceFlags.CaptureMouse);
                }
                else
                {
                    OnControllerDisable?.Invoke(this, null);
                    RawInputWrapper.UnregisterControl(HidUsageAndPage.Mouse);
                }

                enabled = value;
            }
            get { return enabled; }
        }

        public void Disable()
        {
            RawInputWrapper.UnregisterControl(HidUsageAndPage.Mouse);
            RawInputWrapper.UnregisterControl(HidUsageAndPage.Keyboard);
        }

        public void Enable()
        {
            RawInputWrapper.RegisterControl(HidUsageAndPage.Keyboard, RawInputDeviceFlags.InputSink | RawInputDeviceFlags.NoLegacy);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    parts.ForEach(x => x.Dispose());
                }

                disposedValue = true;
            }
        }

        private void Profiles_OnSelected(object sender, EventArgs e)
        {
            OnProfileSwitched?.Invoke(sender, e);
        }

        private void Profiles_OnRemove(object sender, EventArgs e)
        {
            if (sender is UIProfile profile)
            {
                RemoveProfile(profile);
            }
        }
    }
}