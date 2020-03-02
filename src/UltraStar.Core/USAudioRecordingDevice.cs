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
    public class USAudioRecordingDevice
    {
        /// <summary>
        /// Initializes a new instance of <see cref="USAudioRecordingDevice"/>.
        /// </summary>
        /// <param name="name">The name of audio recording device.</param>
        /// <param name="type">The type of the audio recording device or "Unknown" if this is not available on the platform.</param>
        /// <param name="isDefault">An indicator whether the device is the default device.</param>
        /// <param name="inputs">The number of input sources on this device.</param>
        /// <param name="inputNames">The names of all input sources.</param>
        /// <param name="channels">The number of channels supported by the device (or 0 if unknown).</param>
        /// <param name="samplerate">The sample rate of the device (or 0 if unknown).</param>
        /// <param name="volume">The volume of the device (or -1 if unknown).</param>
        public USAudioRecordingDevice(string name, string type, bool isDefault, int inputs, string[] inputNames, int channels, int samplerate, int volume)
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
        /// Gets the name of audio recording device.
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
        /// Gets the number of input sources on this device.
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
        public int Volume { get; }
    }
}
