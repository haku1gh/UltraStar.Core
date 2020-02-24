#region License (MIT)
/*
 * This file is part of UltraStar.Core.
 * 
 * This file is heavily based on the implementation from ManagedBass by Mathew Sachin.
 * ManagedBass is available under the MIT license. For details see <https://github.com/ManagedBass/Home>.
 * In contrast to other files, this file is available under the same license (MIT) as the original.
 */
#endregion License

using System;

namespace UltraStar.Core.Unmanaged.Bass
{
    /// <summary>
    /// BASS DeviceInfo flags.
    /// </summary>
    [Flags]
    internal enum BassDeviceInfoFlags
    {
        /// <summary>
        /// The device is not enabled and not initialized.
        /// </summary>
        None,

        /// <summary>
        /// The device is enabled.
        /// It will not be possible to initialize the device if this flag is not present.
        /// </summary>
        Enabled = 0x1,

        /// <summary>
        /// The device is the system default.
        /// </summary>
        Default = 0x2,

        /// <summary>
        /// The device is initialized, ie. initialization functions had been called.
        /// </summary>
        Initialized = 0x4,

        /// <summary>
        /// The device is a Loopback device.
        /// </summary>
        Loopback = 0x8,

        /// <summary>
        /// Bitmask to identify the device Type.
        /// </summary>
        TypeMask = -16777216
    }
}
