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
    /// Flags for level information on a channel.
    /// </summary>
    internal enum BassChannelLevelFlags
    {
        /// <summary>
        /// Retrieves mono levels.
        /// </summary>
        Mono = 0x1,

        /// <summary>
        /// Retrieves stereo levels.
        /// </summary>
        Stereo = 0x2,

        /// <summary>
        /// Optional Flag: If set it returns RMS levels instead of peak leavels.
        /// </summary>
        RMS = 0x4,

        /// <summary>
        /// Apply the current BASS_ATTRIB_VOL and BASS_ATTRIB_PAN values to the level reading.
        /// </summary>
        VolPan = 0x8
    }
}
