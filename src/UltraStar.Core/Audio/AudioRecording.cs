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
    /// Represents an abstract audio recording class.
    /// </summary>
    /// <remarks>
    /// Once a recording is started, it will send callbacks using the provided delegate.
    /// A pausing will not stop the callback. Instead there will be an indicator in the callback set that the recording is paused.
    /// During the pause all recorded sample data will be set to 0.
    /// To stop the callback, the <see cref="Stop"/> method needs to be called. This can also effect other recording channels.
    /// </remarks>
    public abstract class AudioRecording : IDisposable
    {
        /// <summary>
        /// The audio recording callback.
        /// </summary>
        protected AudioRecordingCallback audioRecordingCallback;

        /// <summary>
        /// Indicator if this instance is disposed.
        /// </summary>
        protected bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of <see cref="AudioRecording"/>.
        /// </summary>
        /// <param name="deviceID">The device ID of the audio recording device.</param>
        /// <param name="channel">The channel to be recorded (0=first channel, ...).</param>
        /// <param name="audioRecordingCallback">A callback function for the audio recording.</param>
        /// <param name="samplerate">The sample rate of the recording.</param>
        /// <param name="input">The used input on the device.</param>
        public AudioRecording(int deviceID, int channel, AudioRecordingCallback audioRecordingCallback, int samplerate, int input)
        {
            DeviceID = deviceID;
            Channel = channel;
            this.audioRecordingCallback = audioRecordingCallback;
            Samplerate = samplerate;
            Input = input;
        }

        /// <summary>
        /// Gets all available recording devices.
        /// </summary>
        /// <returns>An array of type <see cref="USAudioRecordingDeviceInfo"/>.</returns>
        public static USAudioRecordingDeviceInfo[] GetDevices()
        {
            // Get the type where the class AudioRecording is implemented
            Type audioRecordingClass = Type.GetType(LibrarySettings.AudioRecordingClassName, true);
             // Get the method "GetDevices"
             MethodInfo minfo = audioRecordingClass.GetMethod("GetDevices", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            // Call the method to get all audio recording devices
            return (USAudioRecordingDeviceInfo[])minfo.Invoke(null, new object[] { });
        }

        /// <summary>
        /// Opens a new audio recording device.
        /// </summary>
        /// <param name="device">The audio recording device.</param>
        /// <param name="channel">The channel to be recorded (0=first channel, ...).</param>
        /// <param name="audioRecordingCallback">A callback function for the audio recording.</param>
        /// <returns>A new instance of <see cref="AudioRecording"/>.</returns>
        public static AudioRecording Open(USAudioRecordingDeviceInfo device, int channel, AudioRecordingCallback audioRecordingCallback)
        {
            int samplerate = (device.Samplerate != 0) ? device.Samplerate : LibrarySettings.AudioRecordingDefaultSamplerate;
            return Open(device.DeviceID, channel, audioRecordingCallback, samplerate, LibrarySettings.AudioRecordingDefaultInput);
        }

        /// <summary>
        /// Opens a new audio recording device.
        /// </summary>
        /// <param name="device">The audio recording device.</param>
        /// <param name="channel">The channel to be recorded (0=first channel, ...).</param>
        /// <param name="audioRecordingCallback">A callback function for the audio recording.</param>
        /// <param name="samplerate">The sample rate of the recording. This can be different from the devices sample rate.</param>
        /// <returns>A new instance of <see cref="AudioRecording"/>.</returns>
        public static AudioRecording Open(USAudioRecordingDeviceInfo device, int channel, AudioRecordingCallback audioRecordingCallback, int samplerate)
        {
            return Open(device.DeviceID, channel, audioRecordingCallback, samplerate, LibrarySettings.AudioRecordingDefaultInput);
        }

        /// <summary>
        /// Opens a new audio recording device.
        /// </summary>
        /// <param name="device">The audio recording device.</param>
        /// <param name="channel">The channel to be recorded (0=first channel, ...).</param>
        /// <param name="audioRecordingCallback">A callback function for the audio recording.</param>
        /// <param name="samplerate">The sample rate of the recording. This can be different from the devices sample rate.</param>
        /// <param name="input">The used input on the device.</param>
        /// <returns>A new instance of <see cref="AudioRecording"/>.</returns>
        public static AudioRecording Open(USAudioRecordingDeviceInfo device, int channel, AudioRecordingCallback audioRecordingCallback, int samplerate, int input)
        {
            return Open(device.DeviceID, channel, audioRecordingCallback, samplerate, input);
        }

        /// <summary>
        /// Opens a new audio recording device.
        /// </summary>
        /// <param name="deviceID">The device ID of the audio recording device.</param>
        /// <param name="channel">The channel to be recorded (0=first channel, ...).</param>
        /// <param name="audioRecordingCallback">A callback function for the audio recording.</param>
        /// <param name="samplerate">The sample rate of the recording. This can be different from the devices sample rate.</param>
        /// <param name="input">The used input on the device.</param>
        /// <returns>A new instance of <see cref="AudioRecording"/>.</returns>
        public static AudioRecording Open(int deviceID, int channel, AudioRecordingCallback audioRecordingCallback, int samplerate, int input)
        {
            // Get the type where the class AudioRecording is implemented
            Type audioRecordingClass = Type.GetType(LibrarySettings.AudioRecordingClassName, true);
            // Get the constructor
            ConstructorInfo cInfo = audioRecordingClass.GetConstructor(new Type[] { typeof(int), typeof(int), typeof(AudioRecordingCallback), typeof(int), typeof(int) });
            // Return the newly created object
            return (AudioRecording)cInfo.Invoke(new object[] { deviceID, channel, audioRecordingCallback, samplerate, input });
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
        /// Occurs when an audio recording stopped.
        /// </summary>
        /// <remarks>
        /// This can happen in two cases. Either the method <see cref="Stop"/> had been called,
        /// or the recording device became unavailable.
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
        /// Re-initializes the recording.
        /// </summary>
        /// <remarks>
        /// A call to this method is usually not necessary.
        /// This method has only an effect when the recording is completely stopped on all channels, then
        /// it will re-initialize the recording device as if it will be created anew. This means that the
        /// recording device is already locked and in an internal hold state. This reduces the time it
        /// takes to call the <see cref="Start"/> method.
        /// </remarks>
        public abstract void ReInitialize();

        /// <summary>
        /// Starts the recording.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Restarts the recording.
        /// </summary>
        public abstract void Restart();

        /// <summary>
        /// Pauses the recording.
        /// </summary>
        /// <remarks>
        /// This may also pause the recording of other channels from the same device.
        /// </remarks>
        public abstract void Pause();

        /// <summary>
        /// Resumes the recording.
        /// </summary>
        /// <remarks>
        /// This may also resume the recording of other channels from the same device.
        /// </remarks>
        public abstract void Resume();

        /// <summary>
        /// Stops the recording.
        /// </summary>
        /// <remarks>
        /// This does not stop the recording of other channels from the same device.
        /// If this is the last recording channel from the device, then it will be stopped completely.
        /// </remarks>
        public abstract void Stop();

        /// <summary>
        /// Gets the sample position of the recording stream.
        /// </summary>
        /// <remarks>
        /// This is not the live position during the recording.
        /// It returns the number of samples already forwarded to the callback function.
        /// </remarks>
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
        /// Gets information about the audio recording device.
        /// </summary>
        public abstract USAudioRecordingDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Gets the device ID of the audio recording device.
        /// </summary>
        public int DeviceID { get; }

        /// <summary>
        /// Gets the name of the audio recording device.
        /// </summary>
        public string Name => DeviceInfo.Name;

        /// <summary>
        /// Gets the channel to be recorded (0=first channel, ...).
        /// </summary>
        public int Channel { get; }

        /// <summary>
        /// Gets the sample rate of the recording.
        /// </summary>
        /// <remarks>
        /// This is not necessarily the same as the devices sample rate.
        /// </remarks>
        public int Samplerate { get; }

        /// <summary>
        /// Gets the used input on the device.
        /// </summary>
        public int Input { get; }
    }
}
