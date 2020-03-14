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
using System.Runtime.CompilerServices;
using Serilog;
using UltraStar.Core.Unmanaged.Bass;
using UltraStar.Core.Utils;

namespace UltraStar.Core.Audio
{
    /// <summary>
    /// Represents an audio playback class using the BASS library.
    /// </summary>
    public class BassAudioPlayback : AudioPlayback
    {
        private static Object lockBassAccess = new Object();
        private static List<ActivePlaybackDevice> activePlaybackDevices = new List<ActivePlaybackDevice>();
        private ActivePlaybackDevice activePlaybackDevice = null;

        /// <summary>
        /// Initializes a new instance of <see cref="BassAudioPlayback"/>.
        /// </summary>
        /// <param name="deviceID">The device ID of the audio playback device.</param>
        /// <param name="channels">The number of channels the playback shall have.</param>
        /// <param name="audioPlaybackCallback">A callback function for the audio playback.</param>
        /// <param name="samplerate">The sample rate of the playback.</param>
        public BassAudioPlayback(int deviceID, int channels, AudioPlaybackCallback audioPlaybackCallback, int samplerate) :
            base(deviceID, channels, audioPlaybackCallback, samplerate)
        { }

        /// <summary>
        /// Gets all available playback devices.
        /// </summary>
        /// <returns>An array of type <see cref="USAudioPlaybackDeviceInfo"/>.</returns>
        public static new USAudioPlaybackDeviceInfo[] GetDevices()
        {
            List<USAudioPlaybackDeviceInfo> deviceInfos = new List<USAudioPlaybackDeviceInfo>();
            int deviceID = 0;
            lock (lockBassAccess)
            {
                while (Bass.GetDeviceInfo(deviceID, out BassDeviceInfo info))
                {
                    if (info.IsEnabled)
                    {
                        if (info.IsInitialized) // Device is already open.
                        {
                            foreach (ActivePlaybackDevice activeDevice in activePlaybackDevices)
                            {
                                if (activeDevice.DeviceID == deviceID)
                                    deviceInfos.Add(activeDevice.DeviceInfo);
                            }
                        }
                        else // Device is not opened yet.
                        {
                            // Shortly initialize the device
                            int currentDeviceID = Bass.CurrentDevice;
                            bool success = Bass.DeviceInit(device: deviceID, flags: BassDeviceInitFlags.Latency);
                            if (success)
                            {
                                // Get the device info
                                Bass.CurrentDevice = deviceID;
                                deviceInfos.Add(ActivePlaybackDevice.GetDeviceInfo(deviceID, 0));
                                // Free the device again
                                Bass.RecordingDeviceFree();
                                if (currentDeviceID != -1)
                                {
                                    try
                                    {
                                        Bass.CurrentDevice = currentDeviceID;
                                    }
                                    catch { }
                                }
                            }
                            else
                                deviceInfos.Add(ActivePlaybackDevice.GetDeviceInfo(deviceID, 0));
                        }
                    }
                    deviceID++;
                    if (deviceID == 1) deviceID++; // Skip the default device, to ensure that its not listed twice.
                }
            }
            return deviceInfos.ToArray();
        }

        /// <summary>
        /// Finalizes this instance.
        /// </summary>
        ~BassAudioPlayback()
        {
            Dispose(false);
        }

        /// <summary>
        /// Closes the device.
        /// </summary>
        public override void Close()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public override void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases the unmanaged resources used by this instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                // Special for Close() and Dispose()
                if (disposing)
                    GC.SuppressFinalize(this);
                // TODO
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Starts the playback.
        /// </summary>
        public override void Start()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pauses the playback.
        /// </summary>
        public override void Pause()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stops the playback.
        /// </summary>
        public override void Stop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the sample position of the playback stream.
        /// </summary>
        public override long Position => throw new NotImplementedException();

        /// <summary>
        /// Gets or sets the volume of the recording stream.
        /// </summary>
        /// <remarks>
        /// The volume is provided on a linear base.
        /// The volume can range from 0 to <see cref="LibrarySettings.AudioRecordingMaximumChannelAmplification"/>.
        /// Any values provided outside this range will be automatically capped.
        /// </remarks>
        public override float Volume { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets information about the audio playback device.
        /// </summary>
        public override USAudioPlaybackDeviceInfo DeviceInfo => throw new NotImplementedException();

        /// <summary>
        /// Represents an active playback device.
        /// </summary>
        private class ActivePlaybackDevice
        {
            /// <summary>
            /// Gets the playback device info for the active playback.
            /// </summary>
            public USAudioPlaybackDeviceInfo DeviceInfo { get; private set; }
            /// <summary>
            /// Gets the device ID of the active playback device.
            /// </summary>
            public int DeviceID { get; private set; }

            /// <summary>
            /// Gets information about an audio playback device.
            /// </summary>
            /// <remarks>
            /// Try to ensure that you are only calling this function when the device is initialized. Otherwise it will contain only limited information.
            /// </remarks>
            /// <param name="deviceID">The device ID.</param>
            /// <param name="input">The input from which the volume information will be used.</param>
            /// <returns>A <see cref="USAudioRecordingDeviceInfo"/> object.</returns>
            public static USAudioPlaybackDeviceInfo GetDeviceInfo(int deviceID, int input)
            {
                bool result = Bass.GetRecordingDeviceInfo(deviceID, out BassDeviceInfo deviceInfo);
                // If it fails, return NULL
                if (!result) return null;
                // If it is initialized, then we can add more information        
                if (deviceInfo.IsInitialized)
                {
                    // Set current device ID
                    int currentDeviceID = Bass.CurrentDevice;
                    if (deviceID != currentDeviceID) Bass.CurrentDevice = deviceID;
                    // Get extended info
                    BassInfo info = Bass.DeviceExtendedInfo;
                    // Get volume
                    float volume = Bass.RecordingDeviceVolume(input);
                    // Return device info
                    if (deviceID != currentDeviceID) Bass.CurrentRecordingDevice = currentDeviceID;
                    return new USAudioPlaybackDeviceInfo(deviceID, deviceInfo.Name, deviceInfo.Type.ToString(), deviceInfo.IsDefault, info.MinimumBufferLength,
                        info.Latency, info.SpeakerCount, info.Samplerate, volume);
                }
                else
                {
                    return new USAudioPlaybackDeviceInfo(deviceID, deviceInfo.Name, deviceInfo.Type.ToString(), deviceInfo.IsDefault, 0, 0, 0, 0, -1);
                }
            }
        }
    }
}
