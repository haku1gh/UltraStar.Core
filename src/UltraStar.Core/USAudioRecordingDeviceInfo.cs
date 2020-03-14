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
    /// Provides information about audio recording devices.
    /// </summary>
    public class USAudioRecordingDeviceInfo
    {
        /// <summary>
        /// Initializes a new instance of <see cref="USAudioRecordingDeviceInfo"/>.
        /// </summary>
        /// <param name="deviceID">The device ID of the audio recording device.</param>
        /// <param name="name">The name of audio recording device.</param>
        /// <param name="type">The type of the audio recording device or "Unknown" if this is not available on the platform.</param>
        /// <param name="isDefault">An indicator whether the device is the default device.</param>
        /// <param name="inputs">The number of input sources on this device (or 0 if unknown).</param>
        /// <param name="inputNames">The names of all input sources.</param>
        /// <param name="channels">The number of channels supported by the device (or 0 if unknown).</param>
        /// <param name="samplerate">The sample rate of the device (or 0 if unknown).</param>
        /// <param name="volume">The volume of the device (or -1 if unknown). 0=Silent, 1=Max.</param>
        public USAudioRecordingDeviceInfo(int deviceID, string name, string type, bool isDefault, int inputs, string[] inputNames, int channels, int samplerate, float volume)
        {
            Name = name;
            Type = type;
            IsDefault = isDefault;
            Inputs = inputs;
            InputNames = inputNames;
            Channels = channels;
            Samplerate = samplerate;
            Volume = volume;
        }

        /// <summary>
        /// Gets the device ID of the audio recording device.
        /// </summary>
        public int DeviceID { get; }

        /// <summary>
        /// Gets the name of the audio recording device.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the audio recording device or "Unknown" if this is not available on the platform (e.g. on Linux).
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets an indicator whether the device is the default device.
        /// </summary>
        public bool IsDefault { get; }

        /// <summary>
        /// Gets the number of input sources on this device (or 0 if unknown).
        /// </summary>
        public int Inputs { get; }

        /// <summary>
        /// Gets the names of all input sources.
        /// </summary>
        public string[] InputNames { get; }

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
