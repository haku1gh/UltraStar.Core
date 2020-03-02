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
    internal abstract class AudioRecording : IDisposable
    {
        /// <summary>
        /// The audio recording callback.
        /// </summary>
        protected AudioRecordingCallback audioRecordingCallback;

        /// <summary>
        /// Initializes a new instance of <see cref="AudioRecording"/>.
        /// </summary>
        /// <param name="name">The name of the audio recording device.</param>
        /// <param name="channel">The channel to be recorded (0=first channel, ...).</param>
        /// <param name="audioRecordingCallback">A callback function for the audio recording.</param>
        /// <param name="samplerate">The sample rate of the recording.</param>
        /// <param name="input">The used input on the device.</param>
        public AudioRecording(string name, int channel, AudioRecordingCallback audioRecordingCallback, int samplerate, int input)
        {
            Name = name;
            Channel = channel;
            this.audioRecordingCallback = audioRecordingCallback;
            Samplerate = samplerate;
            Input = input;
        }

        /// <summary>
        /// Gets all available recording devices.
        /// </summary>
        /// <returns>An array of type <c>USInputAudioDevice</c>.</returns>
        public static USAudioRecordingDevice[] GetDevices()
        {
            // Get the type where the class AudioRecording is implemented
            Type audioRecordingClass = Type.GetType(LibrarySettings.AudioRecordingClassName, true);
             // Get the method "GetDevices"
             MethodInfo minfo = audioRecordingClass.GetMethod("GetDevices", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            // Call the method to get all audio recording devices
            return (USAudioRecordingDevice[])minfo.Invoke(null, new object[] { });
        }

        /// <summary>
        /// Opens a new audio recording device.
        /// </summary>
        /// <param name="device">The audio recording device.</param>
        /// <param name="channel">The channel to be recorded (0=first channel, ...).</param>
        /// <param name="audioRecordingCallback">A callback function for the audio recording.</param>
        /// <returns>A new instance of <see cref="AudioRecording"/>.</returns>
        public static AudioRecording Open(USAudioRecordingDevice device, int channel, AudioRecordingCallback audioRecordingCallback)
        {
            int samplerate = (device.Samplerate != 0) ? device.Samplerate : LibrarySettings.DefaultAudioRecordingSamplerate;
            return Open(device.Name, channel, audioRecordingCallback, samplerate, LibrarySettings.DefaultAudioRecordingInput);
        }

        /// <summary>
        /// Opens a new audio recording device.
        /// </summary>
        /// <param name="device">The audio recording device.</param>
        /// <param name="channel">The channel to be recorded (0=first channel, ...).</param>
        /// <param name="audioRecordingCallback">A callback function for the audio recording.</param>
        /// <param name="samplerate">The sample rate of the recording. This can be different from the devices sample rate.</param>
        /// <returns>A new instance of <see cref="AudioRecording"/>.</returns>
        public static AudioRecording Open(USAudioRecordingDevice device, int channel, AudioRecordingCallback audioRecordingCallback, int samplerate)
        {
            return Open(device.Name, channel, audioRecordingCallback, samplerate, LibrarySettings.DefaultAudioRecordingInput);
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
        public static AudioRecording Open(USAudioRecordingDevice device, int channel, AudioRecordingCallback audioRecordingCallback, int samplerate, int input)
        {
            return Open(device.Name, channel, audioRecordingCallback, samplerate, input);
        }

        /// <summary>
        /// Opens a new audio recording device.
        /// </summary>
        /// <param name="name">The name of the audio recording device.</param>
        /// <param name="channel">The channel to be recorded (0=first channel, ...).</param>
        /// <param name="audioRecordingCallback">A callback function for the audio recording.</param>
        /// <param name="samplerate">The sample rate of the recording. This can be different from the devices sample rate.</param>
        /// <param name="input">The used input on the device.</param>
        /// <returns>A new instance of <see cref="AudioRecording"/>.</returns>
        public static AudioRecording Open(string name, int channel, AudioRecordingCallback audioRecordingCallback, int samplerate, int input)
        {
            // Get the type where the class AudioRecording is implemented
            Type audioRecordingClass = Type.GetType(LibrarySettings.AudioRecordingClassName, true);
            // Get the constructor
            ConstructorInfo cInfo = audioRecordingClass.GetConstructor(new Type[] { typeof(string), typeof(int), typeof(AudioRecordingCallback), typeof(int), typeof(int) });
            // Return the newly created object
            return (AudioRecording)cInfo.Invoke(new object[] { name, channel, audioRecordingCallback, samplerate, input });
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
        /// Starts the recording.
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Stops the recording.
        /// </summary>
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
        /// Gets the used audio recording device.
        /// </summary>
        public abstract USAudioRecordingDevice Device { get; }

        /// <summary>
        /// Gets the name of the audio recording device.
        /// </summary>
        public string Name { get; }

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
