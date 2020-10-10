﻿namespace RawInput.Native
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// RAWINPUTHEADER
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputHeader
    {
        readonly RawInputDeviceType dwType;
        readonly int dwSize;
        readonly RawInputDeviceHandle hDevice;
        readonly IntPtr wParam;

        public RawInputDeviceType Type => dwType;
        public int Size => dwSize;
        public RawInputDeviceHandle DeviceHandle => hDevice;
        public IntPtr WParam => wParam;

        public override string ToString() =>
            $"{{{Type}: {DeviceHandle}, WParam: {WParam}}}";
    }
}
