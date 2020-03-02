#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.Collections.Generic;

namespace UltraStar.Core.Audio
{
    /// <summary>
    /// Represents an audio recording class using the BASS library.
    /// </summary>
    internal class BassAudioRecording : AudioRecording
    {
        /// <summary>
        /// Initializes a new instance of <see cref="BassAudioRecording"/>.
        /// </summary>
        /// <param name="name">The name of the audio recording device.</param>
        /// <param name="channel">The channel to be recorded (0=first channel, ...).</param>
        /// <param name="audioRecordingCallback">A callback function for the audio recording.</param>
        /// <param name="samplerate">The sample rate of the recording.</param>
        /// <param name="input">The used input on the device.</param>
        public BassAudioRecording(string name, int channel, AudioRecordingCallback audioRecordingCallback, int samplerate, int input) :
            base(name, channel, audioRecordingCallback, samplerate, input)
        {
            USAudioRecordingDevice device = new USAudioRecordingDevice("", "", false, 0, new string[] { }, 0, 0, 0);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all available recording devices.
        /// </summary>
        /// <returns>An array of type <c>USInputAudioDevice</c>.</returns>
        public static new USAudioRecordingDevice[] GetDevices()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Closes the device.
        /// </summary>
        public override void Close()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Starts the recording.
        /// </summary>
        public override void Start()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stops the recording.
        /// </summary>
        public override void Stop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the sample position of the recording stream.
        /// </summary>
        /// <remarks>
        /// This is not the live position during the recording.
        /// It returns the number of samples already forwarded to the callback function.
        /// </remarks>
        public override long Position
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the used audio recording device.
        /// </summary>
        public override USAudioRecordingDevice Device
        {
            get { throw new NotImplementedException(); }
        }

        private static List<ActiveRecordingDevice> activeRecordingDevices;

        private class ActiveRecordingDevice
        {
            USAudioRecordingDevice Device { get; }

            public ActiveRecordingDevice(string name, int channels, int samplerate, int input)
            {
                // Open BASS recording device here
            }

            public void Attach()
            {
            }

            public void Detach()
            {
            }

            public void Stop()
            {
            }
        }
    }
}
