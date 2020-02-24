﻿#region License (MIT)
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
    /// BASS Info flags.
    /// </summary>
    [Flags]
    enum BassInfoFlags
    {
        /// <summary>
        /// None of the flags are set
        /// </summary>
        None,

        /// <summary>
        /// The device supports all sample rates between minrate and maxrate.
        /// </summary>
        ContinuousRate = 0x10,

        /// <summary>
        /// The device's drivers do NOT have DirectSound support, so it is being emulated.
        /// Updated drivers should be installed.
        /// </summary>
        EmulatedDrivers = 0x20,

        /// <summary>
        /// The device driver has been certified by Microsoft.
        /// This flag is always set on WDM drivers.
        /// </summary>
        Certified = 0x40,

        /// <summary>
        /// Mono samples are supported by hardware mixing.
        /// </summary>
        Mono = 0x100,

        /// <summary>
        /// Stereo samples are supported by hardware mixing.
        /// </summary>
        Stereo = 0x200,

        /// <summary>
        /// 8-bit samples are supported by hardware mixing.
        /// </summary>
        Secondary8Bit = 0x400,

        /// <summary>
        /// 16-bit samples are supported by hardware mixing.
        /// </summary>
        Secondary16Bit = 0x800
    }
}
