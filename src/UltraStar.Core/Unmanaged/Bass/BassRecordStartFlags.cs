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
    /// Flags for creating a recording stream.
    /// </summary>
    [Flags]
    public enum BassRecordStartFlags : uint
    {
        /// <summary>
        /// 0 = default create stream
        /// </summary>
        Default,

        /// <summary>
        /// Use 8-bit resolution. If neither this or the <see cref="Float"/> flags are specified, then the stream is 16-bit.
        /// </summary>
        Byte = 0x1,

        /// <summary>
        /// Use 32-bit floating-point sample data (see Floating-Point Channels for details).
        /// </summary>
        Float = 0x100,

        /// <summary>
        /// Recording: Start the recording paused. Use BASS_ChannelPlay to start it.
        /// </summary>
        RecordPause = 0x8000,
    }
}
