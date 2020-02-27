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
    /// The state of a BASS channel.
    /// </summary>
    internal enum BassChannelState
    {
        /// <summary>
        /// The channel (handle) is invalid.
        /// </summary>
        Invalid = -1,

        /// <summary>
        /// The channel is not active.
        /// </summary>
        Stopped = 0,

        /// <summary>
        /// The channel is playing (or recording).
        /// </summary>
        Running = 1,

        /// <summary>
        /// Playback of the stream has been stalled due to a lack of sample data.
        /// Playback will automatically resume once there is sufficient data to do so.
        /// </summary>
        Stalled = 2,

        /// <summary>
        /// The channel is paused.
        /// </summary>
        Paused = 3,

        /// <summary>
        /// The channel's device is paused.
        /// </summary>
        /// <remarks>
        /// The BASS_ACTIVE_PAUSED_DEVICE state can be the result of a BASS_Pause call or
        /// of the device stopping unexpectedly (eg. a USB soundcard being disconnected).
        /// In either case, playback will be resumed by BASS_Start.
        /// </remarks>
        PausedDevice = 4
    }
}
