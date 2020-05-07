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
        /// <param name="samplerate">The sample rate of the playback.</param>
        /// <param name="channels">The number of channels the playback shall have.</param>
        /// <param name="audioPlaybackCallback">A callback function for the audio playback.</param>
        /// <param name="noSound"><see langword="true"/> if a virtual playback device shall be used;
        /// otherwise <see langword="false"/> and the default playback device will be used.</param>
        public AudioPlayback(int samplerate, int channels, AudioPlaybackCallback audioPlaybackCallback, bool noSound)
        {
            Samplerate = samplerate;
            Channels = channels;
            this.audioPlaybackCallback = audioPlaybackCallback;
            IsUsingNoSoundDevice = noSound;
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
        /// Gets information about the default playback device. <see langword="null"/> is returned in case no default device is available.
        /// </summary>
        public static USAudioPlaybackDeviceInfo DefaultDevice
        {
            get
            {
                // Get the type where the class AudioPlayback is implemented
                Type audioPlaybackClass = Type.GetType(LibrarySettings.AudioPlaybackClassName, true);
                // Get the property "DefaultDevice"
                PropertyInfo pinfo = audioPlaybackClass.GetProperty("DefaultDevice", BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.DeclaredOnly);
                // Call the property
                return (USAudioPlaybackDeviceInfo)pinfo.GetValue(null);
            }
        }

        /// <summary>
        /// Gets information about the virtual playback device. <see langword="null"/> is returned in case no virtual device is available.
        /// </summary>
        public static USAudioPlaybackDeviceInfo NoSoundDevice
        {
            get
            {
                // Get the type where the class AudioPlayback is implemented
                Type audioPlaybackClass = Type.GetType(LibrarySettings.AudioPlaybackClassName, true);
                // Get the property "NoSoundDevice"
                PropertyInfo pinfo = audioPlaybackClass.GetProperty("NoSoundDevice", BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.DeclaredOnly);
                // Call the property
                return (USAudioPlaybackDeviceInfo)pinfo.GetValue(null);
            }
        }

        /// <summary>
        /// Gets an indicator whether the default playback device is available.
        /// </summary>
        public static bool IsDefaultDeviceAvailable
        {
            get
            {
                // Get the type where the class AudioPlayback is implemented
                Type audioPlaybackClass = Type.GetType(LibrarySettings.AudioPlaybackClassName, true);
                // Get the property "IsDefaultDeviceAvailable"
                PropertyInfo pinfo = audioPlaybackClass.GetProperty("IsDefaultDeviceAvailable", BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.DeclaredOnly);
                // Call the property
                return (bool)pinfo.GetValue(null);
            }
        }

        /// <summary>
        /// Gets an indicator whether the virtual playback device is available.
        /// </summary>
        public static bool IsNoSoundDeviceAvailable
        {
            get
            {
                // Get the type where the class AudioPlayback is implemented
                Type audioPlaybackClass = Type.GetType(LibrarySettings.AudioPlaybackClassName, true);
                // Get the property "IsNoSoundDeviceAvailable"
                PropertyInfo pinfo = audioPlaybackClass.GetProperty("IsNoSoundDeviceAvailable", BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.DeclaredOnly);
                // Call the property
                return (bool)pinfo.GetValue(null);
            }
        }

        /// <summary>
        /// Opens a new audio playback device.
        /// </summary>
        /// <param name="samplerate">The sample rate of the playback. This can be different from the devices sample rate.</param>
        /// <param name="channels">The number of channels the playback shall have. This can be different from the devices number of channels.</param>
        /// <param name="audioPlaybackCallback">A callback function for the audio playback, or NULL if audio data is manually provided.</param>
        /// <param name="noSound"><see langword="true"/> if a virtual playback device shall be used;
        /// otherwise <see langword="false"/> and the default playback device will be used.</param>
        /// <param name="preFillBufferWithSilence">><see langword="true"/> if all buffers should be prefilled with 0 before start; otherwise <see langword="false"/>.</param>
        public static AudioPlayback Open(int samplerate = 48000, int channels = 2, AudioPlaybackCallback audioPlaybackCallback = null, bool noSound = false, bool preFillBufferWithSilence = false)
        {
            // Get the type where the class AudioPlayback is implemented
            Type audioPlaybackClass = Type.GetType(LibrarySettings.AudioPlaybackClassName, true);
            // Get the constructor
            ConstructorInfo cInfo = audioPlaybackClass.GetConstructor(new Type[] { typeof(int), typeof(int), typeof(AudioPlaybackCallback), typeof(bool), typeof(bool) });
            // Return the newly created object
            return (AudioPlayback)cInfo.Invoke(new object[] { samplerate, channels, audioPlaybackCallback, noSound, preFillBufferWithSilence });
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
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
            }
            // Set references to null
            Closed = null;
        }

        /// <summary>
        /// Occurs when an audio playback had been closed.
        /// </summary>
        /// <remarks>
        /// This can happen in two cases. Either the methods <see cref="Close"/> or <see cref="Dispose()"/> had been called,
        /// or the playback device became unavailable.
        /// </remarks>
        public event EventHandler<EventArgs> Closed;

        /// <summary>
        /// Helpermethod for event <c>Closed</c>.
        /// </summary>
        protected virtual void onClosed()
        {
            Closed?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Re-initializes the playback.
        /// </summary>
        /// <remarks>
        /// A call to this method is usually not necessary.
        /// This method has only an effect when the playback had been stopped, then
        /// it will re-initialize the playback device as if it will be created anew. This means that the
        /// playback device is already pre-buffered and a call to the <see cref="Start"/> method will instantly start playback.
        /// </remarks>
        public abstract void ReInitialize();

        /// <summary>
        /// Pushes sample data to the audio playback.
        /// </summary>
        /// <remarks>
        /// This function shall only be used when no callback function had been provided.
        /// All data must be stored in a non-planar way. E.g. if the playback has 2 channels (A and B), the data is stored as ABABAB... in the buffer.
        /// </remarks>
        /// <param name="buffer">The buffer containing the sample data.</param>
        /// <param name="length">The number of audio samples provided in the buffer.</param>
        /// <returns>The number of samples per channel queued outside the main playback buffer.</returns>
        public abstract int Push(float[] buffer, int length);

        /// <summary>
        /// Starts the playback.
        /// </summary>
        /// <param name="delay">The delay in micro seconds [us] before playback shall start.</param>
        public abstract void Start(long delay = 0);

        /// <summary>
        /// Restarts the playback.
        /// </summary>
        /// <param name="delay">The delay in micro seconds [us] before playback shall start.</param>
        public abstract void Restart(long delay = 0);

        /// <summary>
        /// Pauses the playback.
        /// </summary>
        public abstract void Pause();

        /// <summary>
        /// Resumes the playback.
        /// </summary>
        public abstract void Resume();

        /// <summary>
        /// Stops the playback.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Gets an indicator whether the playback stream is active.
        /// </summary>
        public abstract bool IsActive { get; }

        /// <summary>
        /// Gets an indicator whether the playback stream is paused.
        /// </summary>
        public abstract bool IsPaused { get; }

        /// <summary>
        /// Gets an indicator whether this instance is already closed/disposed.
        /// </summary>
        public bool IsClosed => isDisposed;

        /// <summary>
        /// Gets the sample position of the playback stream.
        /// </summary>
        public abstract long Position { get; }

        /// <summary>
        /// Gets the number of samples per channel currently buffered in the playback stream.
        /// </summary>
        public abstract int BufferCount { get; }

        /// <summary>
        /// Gets or sets the volume of the playback stream.
        /// </summary>
        /// <remarks>
        /// The volume is provided on a linear base.
        /// The volume can range from 0 to <see cref="LibrarySettings.AudioRecordingMaximumChannelAmplification"/>.
        /// Any values provided outside this range will be automatically capped.
        /// </remarks>
        public abstract float Volume { get; set; }

        /// <summary>
        /// Gets the sample rate of the playback.
        /// </summary>
        /// <remarks>
        /// This is not necessarily the same as the devices sample rate.
        /// </remarks>
        public int Samplerate { get; }

        /// <summary>
        /// Gets the number of channels the playback has.
        /// </summary>
        public int Channels { get; }

        /// <summary>
        /// Gets an indicator whether this playback is using a virtual playback device.
        /// </summary>
        public bool IsUsingNoSoundDevice { get; }

        /// <summary>
        /// Gets an indicator whether this playback is using the default playback device.
        /// </summary>
        public bool IsUsingDefaultDevice => !IsUsingNoSoundDevice;
    }
}
