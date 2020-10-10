namespace ControllerEmulatorAPI.Controller.Parts
{
    using ControllerEmulatorAPI.Controller.Base;
    using ControllerEmulatorAPI.Controller.Enums;
    using ControllerEmulatorAPI.Controller.Native;
    using ControllerEmulatorAPI.Controller.Parts.Base;
    using ControllerEmulatorAPI.Extensions;
    using ControllerEmulatorAPI.vJoy;
    using RawInput;
    using RawInput.Events;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;

    public class AnalogStickRight : BasePart, IDisposable
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private readonly Thread resetThread;
        private Point buffered;
        private int buffering;

        public AnalogStickRight(VirualController c, RawInputWrapper i, VirtualJoystick j) : base(c, i, j)
        {
            resetThread = new Thread(ResetThread);
            resetThread.Start();
        }

        public override void ProcessMouse(MouseRawInputEventArgs e)
        {
            if (VirualController.Settings.MouseBuffer == 0)
            {
                NativeMethods.SetCursorPos(500, 500);
            }

            if (buffering < VirualController.Settings.MouseBuffer)
            {
                NativeMethods.SetCursorPos(500, 500);
                buffered = new Point(e.Data.Mouse.LastX * buffered.X, e.Data.Mouse.LastY * buffered.Y);
                buffering++;
            }
            else
            {
                Point point = new Point(e.Data.Mouse.LastX + buffered.X, e.Data.Mouse.LastY + buffered.Y);
                buffering = 0;
                buffered = default;

                if (point.X != 0)
                {
                    int val = (int)(MathF.Pow(point.X.Amplify(VirualController.Settings.BaseSensitivity), 3) / VirualController.Settings.XCurveFlatness * VirualController.Settings.BaseMultiplier) * -1;
                    Joystick.SetJoystickAxis(val, Axis.HID_USAGE_RX);
                }
                else
                {
                    Joystick.SetJoystickAxis(0, Axis.HID_USAGE_RX);
                }

                if (point.Y != 0)
                {
                    int val = (int)(MathF.Pow(point.Y.Amplify(VirualController.Settings.BaseSensitivity), 3) / VirualController.Settings.YCurveFlatness * VirualController.Settings.BaseMultiplier) * -1;
                    Joystick.SetJoystickAxis(val, Axis.HID_USAGE_RY);
                }
                else
                {
                    Joystick.SetJoystickAxis(0, Axis.HID_USAGE_RY);
                }
            }
            stopwatch.Restart();
        }

        public override void Process(BaseRawInputEventArgs e)
        {
            if (e.Key == Profile.Analog_R_Click)
            {
                Joystick.SetJoystickButton(e.IsDown, (uint)ControllerKey.Analog_R_Click);
            }
        }

        private void ResetThread()
        {
            while (!IsDisposing)
            {
                if (!Enabled)
                {
                    Thread.Sleep(1);
                }

                if (stopwatch.ElapsedMilliseconds > 2)
                {
                    Joystick.SetJoystickAxis(0, Axis.HID_USAGE_RX);
                    Joystick.SetJoystickAxis(0, Axis.HID_USAGE_RY);
                    stopwatch.Stop();
                    stopwatch.Reset();
                }

                Thread.Sleep(1);
            }
        }
    }
}