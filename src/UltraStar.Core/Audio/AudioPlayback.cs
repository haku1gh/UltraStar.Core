#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.Reflection;
using UltraStar.Core.Utils;

namespace UltraStar.Core.Audio
{
    /// <summary>
    /// Represents an abstract audio playback class.
    /// </summary>
    public abstract class AudioPlayback : IDisposable
    {
        /// <summary>
        /// The audio playback callback.
        /// </summary>
        protected AudioPlaybackCallback audioPlaybackCallback;

        /// <summary>
        /// Indicator if this instance is disposed.
        /// </summary>
        protected bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of <see cref="AudioPlayback"/>.
        /// </summary>
        /// <param name="deviceID">The device ID of the audio playback device.</param>
        /// <param name="channels">The number of channels the playback shall have.</param>
        /// <param name="audioPlaybackCallback">A callback function for the audio playback.</param>
        /// <param name="samplerate">The sample rate of the playback.</param>
        public AudioPlayback(int deviceID, int channels, AudioPlaybackCallback audioPlaybackCallback, int samplerate)
        {
            DeviceID = deviceID;
            Channels = channels;
            this.audioPlaybackCallback = audioPlaybackCallback;
            Samplerate = samplerate;
        }

        /// <summary>
        /// Gets all available playback devices.
        /// </summary>
        /// <returns>An array of type <see cref="USAudioPlaybackDeviceInfo"/>.</returns>
        public static USAudioPlaybackDeviceInfo[] GetDevices()
        {
            // Get the type where the class AudioPlayback is implemented
            Type audioPlaybackClass = Type.GetType(LibrarySettings.AudioPlaybackClassName, true);
            // Get the method "GetDevices"
            MethodInfo minfo = audioPlaybackClass.GetMethod("GetDevices", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            // Call the method to get all audio playback devices
            return (USAudioPlaybackDeviceInfo[])minfo.Invoke(null, new object[] { });
        }

        /// <summary>
        /// Opens a new audio playback device.
        /// </summary>
        /// <param name="device">The audio playback device.</param>
        /// <param name="channels">The number of channels the playback shall have.</param>
        /// <param name="audioPlaybackCallback">A callback function for the audio playback, or NULL if audio data is manually provided.</param>
        /// <returns>A new instance of <see cref="AudioPlayback"/>.</returns>
        public static AudioPlayback Open(USAudioPlaybackDeviceInfo device, int channels, AudioPlaybackCallback audioPlaybackCallback)
        {
            int samplerate = (device.Samplerate != 0) ? device.Samplerate : LibrarySettings.AudioPlaybackDefaultSamplerate;
            return Open(device.DeviceID, channels, audioPlaybackCallback, samplerate);
        }

        /// <summary>
        /// Opens a new audio playback device.
        /// </summary>
        /// <param name="device">The audio playback device.</param>
        /// <param name="channels">The number of channels the playback shall have.</param>
        /// <param name="audioPlaybackCallback">A callback function for the audio playback, or NULL if audio data is manually provided.</param>
        /// <param name="samplerate">The sample rate of the playback. This can be different from the devices sample rate.</param>
        /// <returns>A new instance of <see cref="AudioPlayback"/>.</returns>
        public static AudioPlayback Open(USAudioPlaybackDeviceInfo device, int channels, AudioPlaybackCallback audioPlaybackCallback, int samplerate)
        {
            return Open(device.DeviceID, channels, audioPlaybackCallback, samplerate);
        }

        /// <summary>
        /// Opens a new audio playback device.
        /// </summary>
        /// <param name="deviceID">The device ID of the audio playback device.</param>
        /// <param name="channels">The number of channels the playback shall have.</param>
        /// <param name="audioPlaybackCallback">A callback function for the audio playback, or NULL if audio data is manually provided.</param>
        /// <param name="samplerate">The sample rate of the playback. This can be different from the devices sample rate.</param>
        /// <returns>A new instance of <see cref="AudioPlayback"/>.</returns>
        public static AudioPlayback Open(int deviceID, int channels, AudioPlaybackCallback audioPlaybackCallback, int samplerate)
        {
            // Get the type where the class AudioPlayback is implemented
            Type audioPlaybackClass = Type.GetType(LibrarySettings.AudioPlaybackClassName, true);
            // Get the constructor
            ConstructorInfo cInfo = audioPlaybackClass.GetConstructor(new Type[] { typeof(int), typeof(int), typeof(AudioPlaybackCallback), typeof(int) });
            // Return the newly created object
            return (AudioPlayback)cInfo.Invoke(new object[] { deviceID, channels, audioPlaybackCallback, samplerate });
        }

        /// <summary>
        /// Closes the device.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Releases the unmanaged resources used by this instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
            }
            // Set references to null
            Stopped = null;
        }

        /// <summary>
        /// Occurs when an audio playback stopped.
        /// </summary>
        /// <remarks>
        /// This can happen in two cases. Either the method <see cref="Stop"/> had been called,
        /// or the playback device became unavailable.
        /// </remarks>
        public event EventHandler<EventArgs> Stopped;

        /// <summary>
        /// Helpermethod for event <c>Stopped</c>.
        /// </summary>
        protected virtual void onStopped()
        {
            Stopped?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Starts the playback.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Pauses the playback.
        /// </summary>
        public abstract void Pause();

        /// <summary>
        /// Stops the playback.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Gets the sample position of the playback stream.
        /// </summary>
        public abstract long Position { get; }

        /// <summary>
        /// Gets or sets the volume of the recording stream.
        /// </summary>
        /// <remarks>
        /// The volume is provided on a linear base.
        /// The volume can range from 0 to <see cref="LibrarySettings.AudioRecordingMaximumChannelAmplification"/>.
        /// Any values provided outside this range will be automatically capped.
        /// </remarks>
        public abstract float Volume { get; set; }

        /// <summary>
        /// Gets information about the audio playback device.
        /// </summary>
        public abstract USAudioPlaybackDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Gets the device ID of the audio playback device.
        /// </summary>
        public int DeviceID { get; }

        /// <summary>
        /// Gets the name of the audio playback device.
        /// </summary>
        public string Name => DeviceInfo.Name;

        /// <summary>
        /// Gets the number of channels the playback has.
        /// </summary>
        public int Channels { get; }

        /// <summary>
        /// Gets the sample rate of the playback.
        /// </summary>
        /// <remarks>
        /// This is not necessarily the same as the devices sample rate.
        /// </remarks>
        public int Samplerate { get; }
    }
}
