#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;

namespace UltraStar.Core
{
    /// <summary>
    /// Provides information about audio playback devices.
    /// </summary>
    public class USAudioPlaybackDeviceInfo
    {
        /// <summary>
        /// Initializes a new instance of <see cref="USAudioPlaybackDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceID">The device ID of the audio playback device.</param>
        /// <param name="name">The name of audio playback device.</param>
        /// <param name="type">The type of the audio playback device or "Unknown" if this is not available on the platform.</param>
        /// <param name="isDefault">An indicator whether the device is the default device.</param>
        /// <param name="minimumBufferLength">The minimum buffer length in micro seconds [us] of the device (or 0 if unknown).</param>
        /// <param name="latency">The average delay in micro seconds [us] of the device (or 0 if unknown).</param>
        /// <param name="channels">The number of channels supported by the device (or 0 if unknown).</param>
        /// <param name="samplerate">The sample rate of the device (or 0 if unknown).</param>
        /// <param name="volume">The volume of the device (or -1 if unknown). 0=Silent, 1=Max.</param>
        public USAudioPlaybackDeviceInfo(int deviceID, string name, string type, bool isDefault, long minimumBufferLength, long latency, int channels, int samplerate, float volume)
        {
            Name = name;
            Type = type;
            IsDefault = isDefault;
            MinimumBufferLength = minimumBufferLength;
            Latency = latency;
            Channels = channels;
            Samplerate = samplerate;
            Volume = volume;
        }

        /// <summary>
        /// Gets the device ID of the audio playback device.
        /// </summary>
        public int DeviceID { get; }

        /// <summary>
        /// Gets the name of the audio playback device.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the audio playback device or "Unknown" if this is not available on the platform (e.g. on Linux).
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets an indicator whether the device is the default device.
        /// </summary>
        public bool IsDefault { get; }

        /// <summary>
        /// Gets the minimum buffer length in micro seconds [us] of the device (or 0 if unknown).
        /// </summary>
        /// <remarks>
        /// This also impacts the latency for playback of audio recordings.
        /// </remarks>
        public long MinimumBufferLength { get; }

        /// <summary>
        /// Gets the average delay in micro seconds [us] of the device (or 0 if unknown).
        /// </summary>
        public long Latency { get; }

        /// <summary>
        /// Gets the number of channels supported by the device (or 0 if unknown).
        /// </summary>
        public int Channels { get; }

        /// <summary>
        /// Gets the sample rate of the device (or 0 if unknown).
        /// </summary>
        public int Samplerate { get; }

        /// <summary>
        /// Gets the volume of the device (or -1 if unknown).
        /// </summary>
        public float Volume { get; }
    }
}
