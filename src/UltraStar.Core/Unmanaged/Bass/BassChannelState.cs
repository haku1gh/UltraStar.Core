#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 * 
 * The file is based on the implementation from ManagedBass by Mathew Sachin.
 * ManagedBass is available under the MIT license. For details see <https://github.com/ManagedBass/Home>.
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
