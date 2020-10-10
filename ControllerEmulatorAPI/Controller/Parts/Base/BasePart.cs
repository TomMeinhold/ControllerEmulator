namespace ControllerEmulatorAPI.Controller.Parts.Base
{
    using ControllerEmulatorAPI.Controller.Base;
    using ControllerEmulatorAPI.vJoy;
    using RawInput;
    using RawInput.Events;
    using System;

    public class BasePart : IDisposable
    {
        private bool alwaysActive;
        private bool enabled;

        public BasePart(VirualController c, RawInputWrapper i, VirtualJoystick j)
        {
            VirualController = c;
            RawInputWrapper = i;
            Joystick = j;
            c.OnControllerEnable += OnControllerEnable;
            c.OnControllerDisable += OnControllerDisable;
        }

        ~BasePart()
        {
            Dispose(disposing: false);
        }

        public VirualController VirualController { get; }
        public RawInputWrapper RawInputWrapper { get; }
        public Profile Profile { get => VirualController.CurrentProfile; }
        public VirtualJoystick Joystick { get; set; }

        public bool OverwriteNullSafety { get; set; }

        public bool IsDisposed { get; private set; }

        public bool IsDisposing { get; private set; }

        public bool AlwaysActive
        {
            get => alwaysActive;
            set
            {
                alwaysActive = value;
                Enabled = true;
            }
        }

        public event EventHandler<bool> OnEnabledChanged;

        public event EventHandler<bool> OnDisposing;

        public bool Enabled
        {
            set
            {
                if (alwaysActive)
                {
                    if (enabled)
                    {
                        return;
                    }

                    enabled = true;
                }
                else
                {
                    if (enabled == value)
                    {
                        return;
                    }

                    enabled = value;
                }

                if (value)
                {
                    RawInputWrapper.KeyboardInput += InputReceiver_KeyboardInput;
                    RawInputWrapper.MouseInput += InputReceiver_MouseInput;
                }
                else if (!alwaysActive)
                {
                    RawInputWrapper.KeyboardInput -= InputReceiver_KeyboardInput;
                    RawInputWrapper.MouseInput -= InputReceiver_MouseInput;
                }

                OnEnabledChanged?.Invoke(this, value);
            }

            get => enabled;
        }

        private void OnControllerEnable(object sender, EventArgs e)
        {
            Enabled = true;
        }

        private void OnControllerDisable(object sender, EventArgs e)
        {
            Enabled = false;
        }

        private void InputReceiver_MouseInput(object sender, MouseRawInputEventArgs e)
        {
            if (Profile != null | OverwriteNullSafety)
            {
                Process(e);
                ProcessMouse(e);
            }
        }

        private void InputReceiver_KeyboardInput(object sender, KeyboardRawInputEventArgs e)
        {
            if (Profile != null | OverwriteNullSafety)
            {
                Process(e);
            }
        }

        public virtual void Process(BaseRawInputEventArgs e)
        {
        }

        public virtual void ProcessMouse(MouseRawInputEventArgs e)
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    RawInputWrapper.KeyboardInput -= InputReceiver_KeyboardInput;
                    RawInputWrapper.MouseInput -= InputReceiver_MouseInput;
                    IsDisposing = true;
                    OnDisposing?.Invoke(this, disposing);
                }

                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}