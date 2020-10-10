﻿namespace RawInput.Native
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// RID_DEVICE_INFO_HID
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputHidInfo
    {
        readonly int dwVendorId;
        readonly int dwProductId;
        readonly int dwVersionNumber;
        readonly ushort usUsagePage;
        readonly ushort usUsage;

        /// <summary>
        /// dwVendorId
        /// </summary>
        public int VendorId => dwVendorId;

        /// <summary>
        /// dwProductId
        /// </summary>
        public int ProductId => dwProductId;

        /// <summary>
        /// dwVersionNumber
        /// </summary>
        public int VersionNumber => dwVersionNumber;

        /// <summary>
        /// usUsagePage, usUsage
        /// </summary>
        public HidUsageAndPage UsageAndPage => new HidUsageAndPage(usUsagePage, usUsage);
    }
}
