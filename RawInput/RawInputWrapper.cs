namespace RawInput
{
    using RawInput.Events;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class RawInputWrapper : NativeWindow, IDisposable
    {
        private readonly List<HidUsageAndPage> registeredDevices = new List<HidUsageAndPage>();
        private const int WM_INPUT = 0x00FF;
        private bool disposedValue;

        public RawInputWrapper()
        {
            CreateHandle(CreateParams);
        }

        ~RawInputWrapper()
        {
            Dispose(disposing: false);
        }

        public event EventHandler<KeyboardRawInputEventArgs> KeyboardInput;

        public event EventHandler<MouseRawInputEventArgs> MouseInput;

        public event EventHandler<HIDRawInputEventArgs> HIDInput;

#if NET50
        public CreateParams CreateParams { get; init; } = new CreateParams { X = 0, Y = 0, Width = 0, Height = 0, Style = 0x800000 };
#else
        public CreateParams CreateParams { get; set; } = new CreateParams { X = 0, Y = 0, Width = 0, Height = 0, Style = 0x800000 };
#endif

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public virtual void RegisterControl(HidUsageAndPage device, RawInputDeviceFlags flags = RawInputDeviceFlags.None)
        {
            if (!registeredDevices.Contains(device))
            {
                RawInputDevice.RegisterDevice(device, flags, Handle);
                registeredDevices.Add(device);
            }
        }

        public virtual void UnregisterControl(HidUsageAndPage device)
        {
            if (registeredDevices.Contains(device))
            {
                RawInputDevice.UnregisterDevice(device);
                registeredDevices.Remove(device);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (HidUsageAndPage device in registeredDevices)
                    {
                        RawInputDevice.UnregisterDevice(device);
                    }
                }
                disposedValue = true;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg != WM_INPUT)
            {
                base.WndProc(ref m);
                return;
            }

            // Create an RawInputData from the handle stored in lParam.
            RawInputData data = RawInputData.FromHandle(m.LParam);

            switch (data)
            {
                case RawInputMouseData mouse:
                    MouseInput?.Invoke(this, new MouseRawInputEventArgs(mouse));
                    break;

                case RawInputKeyboardData keyboard:
                    KeyboardInput?.Invoke(this, new KeyboardRawInputEventArgs(keyboard));
                    break;

                case RawInputHidData hid:
                    HIDInput?.Invoke(this, new HIDRawInputEventArgs(hid));
                    break;
            }

            base.WndProc(ref m);
        }
    }
}