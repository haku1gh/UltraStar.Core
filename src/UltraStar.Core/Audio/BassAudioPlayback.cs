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
using UltraStar.Core.ThirdParty.Serilog;
using UltraStar.Core.Unmanaged.Bass;
using UltraStar.Core.Utils;

namespace UltraStar.Core.Audio
{
    /// <summary>
    /// Represents an audio playback class using the BASS library.
    /// </summary>
    public class BassAudioPlayback : AudioPlayback
    {
        private static List<BassAudioPlayback> noSoundDeviceUsers = new List<BassAudioPlayback>();
        private static List<BassAudioPlayback> defaultDeviceUsers = new List<BassAudioPlayback>();
        private static readonly Object lockBassAccess = new Object();

        private readonly int deviceID;
        private int handle;
        private float[] buffer;
        private BassStreamProcedure internalCallback;
        private bool running = false;
        private bool paused = false;
        private bool preFillBufferWithSilence;
        private long remainingDelay = 0; // Unit is 1x sample per channel

        /// <summary>
        /// Initializes a new instance of <see cref="BassAudioPlayback"/>.
        /// </summary>
        /// <param name="samplerate">The sample rate of the playback.</param>
        /// <param name="channels">The number of channels the playback shall have.</param>
        /// <param name="audioPlaybackCallback">A callback function for the audio playback.</param>
        /// <param name="noSound"><see langword="true"/> if a virtual playback device shall be used;
        /// otherwise <see langword="false"/> and the default playback device will be used.</param>
        /// <param name="preFillBufferWithSilence">><see langword="true"/> if all buffers should be prefilled with 0 before start; otherwise <see langword="false"/>.</param>
        public BassAudioPlayback(int samplerate, int channels, AudioPlaybackCallback audioPlaybackCallback, bool noSound, bool preFillBufferWithSilence) :
            base(samplerate, channels, audioPlaybackCallback, noSound)
        {
            deviceID = noSound ? 0 : 1;
            // Open device, if its not open yet
            if ((noSound && noSoundDeviceUsers.Count == 0) || (!noSound && defaultDeviceUsers.Count == 0))
            {
                bool success = Bass.DeviceInit(device: deviceID, flags: BassDeviceInitFlags.Latency);
                if (!success) throw new BassException(Bass.GetErrorCode());
                if (deviceID == 0) noSoundDeviceUsers.Add(this);
                else defaultDeviceUsers.Add(this);
            }
            // Open a new BASS stream
            Bass.CurrentDevice = deviceID;
            if (audioPlaybackCallback != null)
            {
                buffer = new float[LibrarySettings.AudioPlaybackBufferSize];
                internalCallback = playbackCallback;
                handle = Bass.StreamCreate(samplerate, channels, BassStreamCreateFlags.Float, internalCallback);
            }
            else
            {
                handle = Bass.StreamCreate(samplerate, channels, BassStreamCreateFlags.Float, BassStreamProcedureType.Push);
            }
            if (handle == 0) throw new BassException(Bass.GetErrorCode());
            int syncHandle = Bass.ChannelSetSyncDeviceFail(handle, deviceFailCallback);
            if (syncHandle == 0) throw new BassException(Bass.GetErrorCode());
            // Pre-buffer
            this.preFillBufferWithSilence = preFillBufferWithSilence;
            if (audioPlaybackCallback != null)
                ReInitialize();
        }

        /// <summary>
        /// Internal callback from the playback device.
        /// </summary>
        private unsafe int playbackCallback(int handle, IntPtr bufferPtr, int length, IntPtr user)
        {
            if (buffer == null) return 0;

            int maxLength = length >> 2;
            if (maxLength > buffer.Length) maxLength = buffer.Length;
            float* _pBuffer = (float*)bufferPtr;
            int returnedLength = 0;

            if (preFillBufferWithSilence)
            {
                maxLength = length >> 2;
                for (int i = 0; i < maxLength; i++) _pBuffer[i] = 0;
                remainingDelay = -maxLength;
                returnedLength = length;
                preFillBufferWithSilence = false;
            }
            else
            {
                if (remainingDelay > 0)
                {
                    returnedLength = (int)Math.Min(remainingDelay, maxLength);
                    remainingDelay -= returnedLength;
                    returnedLength = returnedLength << 2;
                }
                else
                    returnedLength = audioPlaybackCallback(this, buffer, maxLength) * 4;
                fixed (float* _pInternalBuffer = buffer)
                {
                    Buffer.MemoryCopy(_pInternalBuffer, _pBuffer, length, returnedLength);
                }
            }
            return returnedLength;
        }

        /// <summary>
        /// Internal callback from the recording device, when the underlying device failed.
        /// </summary>
        private void deviceFailCallback(int handle, int channel, int data, IntPtr user)
        {
            this.handle = 0;
            Log.Error("Playback device {Device} stopped unexpectedly (eg. if it is disconnected/disabled). Stopping playback.", deviceID);
            onClosed();
        }

        /// <summary>
        /// Gets all available playback devices.
        /// </summary>
        /// <returns>An array of type <see cref="USAudioPlaybackDeviceInfo"/>.</returns>
        public static new USAudioPlaybackDeviceInfo[] GetDevices()
        {
            List<USAudioPlaybackDeviceInfo> deviceInfos = new List<USAudioPlaybackDeviceInfo>();
            int deviceID = 0;
            while (Bass.GetDeviceInfo(deviceID, out BassDeviceInfo info))
            {
                USAudioPlaybackDeviceInfo deviceInfo = getDeviceInfo(deviceID);
                if (deviceInfo != null)
                    deviceInfos.Add(deviceInfo);
                deviceID++;
            }
            return deviceInfos.ToArray();
        }

        /// <summary>
        /// Gets information about an audio playback device.
        /// </summary>
        /// <param name="deviceID">The device ID.</param>
        /// <returns>A <see cref="USAudioPlaybackDeviceInfo"/> object.</returns>
        private static USAudioPlaybackDeviceInfo getDeviceInfo(int deviceID)
        {
            USAudioPlaybackDeviceInfo returnInfo = null;
            bool success = Bass.GetDeviceInfo(deviceID, out BassDeviceInfo deviceInfo);
            // If it fails, return NULL
            if (!success) return null;
            if (!deviceInfo.IsEnabled) return null;
            // Get current device ID
            int currentDeviceID = Bass.CurrentDevice;
            lock (lockBassAccess)
            {
                // Shortly initialize the device
                if (!deviceInfo.IsInitialized)
                {
                    success = Bass.DeviceInit(device: deviceID, flags: BassDeviceInitFlags.Latency);
                }
                if (success)
                {
                    // Set current device ID
                    if (deviceID != currentDeviceID) Bass.CurrentDevice = deviceID;
                    // Get extended info
                    BassInfo info = Bass.DeviceExtendedInfo;
                    // Get volume
                    float volume = Bass.DeviceVolume;
                    // Free the device again, if it was previously initialized
                    if (!deviceInfo.IsInitialized)
                    {
                        Bass.DeviceFree();
                    }
                    // Restore current active device
                    if (currentDeviceID != -1)
                    {
                        try
                        {
                            Bass.CurrentDevice = currentDeviceID;
                        }
                        catch { }
                    }
                    returnInfo = new USAudioPlaybackDeviceInfo(deviceID, deviceInfo.Name, deviceInfo.Type.ToString(), deviceInfo.IsDefault, info.MinimumBufferLength * 1000,
                    info.Latency * 1000, info.SpeakerCount, info.Samplerate, volume);
                }
                else
                    returnInfo = new USAudioPlaybackDeviceInfo(deviceID, deviceInfo.Name, deviceInfo.Type.ToString(), deviceInfo.IsDefault, 0, 0, 0, 0, -1);
            }
            // Return device info
            return returnInfo;
        }

        /// <summary>
        /// Gets information about the default playback device. <see langword="null"/> is returned in case no default device is available.
        /// </summary>
        public static new USAudioPlaybackDeviceInfo DefaultDevice
        {
            get
            {
                if (IsDefaultDeviceAvailable)
                    return getDeviceInfo(1);
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets information about the virtual playback device. <see langword="null"/> is returned in case no virtual device is available.
        /// </summary>
        public static new USAudioPlaybackDeviceInfo NoSoundDevice
        {
            get
            {
                return getDeviceInfo(0);
            }
        }

        /// <summary>
        /// Gets an indicator whether the default playback device is available.
        /// </summary>
        public static new bool IsDefaultDeviceAvailable
        {
            get
            {
                return Bass.GetDeviceInfo(1, out BassDeviceInfo info);
            }
        }

        /// <summary>
        /// Gets an indicator whether the virtual playback device is available.
        /// </summary>
        public static new bool IsNoSoundDeviceAvailable
        {
            get { return true; }
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
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                // Special for Close() and Dispose()
                if (disposing)
                    GC.SuppressFinalize(this);
                lock (lockBassAccess)
                {
                    // Stop playback
                    if (running)
                    {
                        Bass.CurrentDevice = deviceID;
                        Bass.ChannelStop(handle);
                        running = false;
                        paused = false;
                    }
                    // Stop device
                    if (deviceID == 0)
                    {
                        noSoundDeviceUsers.Remove(this);
                        if (noSoundDeviceUsers.Count == 0)
                        {
                            Bass.CurrentDevice = deviceID;
                            Bass.DeviceFree();
                        }
                    }
                    else
                    {
                        defaultDeviceUsers.Remove(this);
                        if (defaultDeviceUsers.Count == 0)
                        {
                            Bass.CurrentDevice = deviceID;
                            Bass.DeviceFree();
                        }
                    }
                }
                // Free other resources
                buffer = null;
                internalCallback = null;
                // Raise event
                if (disposing) onClosed();
            }
            base.Dispose(disposing);
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
        public override void ReInitialize()
        {
            // Check if disposed
            if (isDisposed)
                throw new ObjectDisposedException(nameof(BassAudioRecording));
            // Checks
            if (running) return;
            // Initialize
            lock (lockBassAccess)
            {
                Bass.CurrentDevice = deviceID;
                if (!Bass.ChannelUpdate(handle, 0))
                    throw new BassException(Bass.GetErrorCode());
            }
        }

        /// <summary>
        /// Starts the playback.
        /// </summary>
        /// <param name="delay">The delay in micro seconds [us] before playback shall start.</param>
        public override void Start(long delay = 0)
        {
            // Check if disposed
            if (isDisposed)
                throw new ObjectDisposedException(nameof(BassAudioRecording));
            // Setup delay
            remainingDelay = (delay * Samplerate * Channels / 1000000) + remainingDelay;
            if (remainingDelay > 0)
                Array.Clear(buffer, 0, buffer.Length);
            else
                remainingDelay = 0;
            // Play
            lock (lockBassAccess)
            {
                Bass.CurrentDevice = deviceID;
                if (!Bass.ChannelPlay(handle, false))
                    throw new BassException(Bass.GetErrorCode());
                running = true;
                paused = false;
            }
        }

        /// <summary>
        /// Restarts the playback.
        /// </summary>
        /// <param name="delay">The delay in micro seconds [us] before playback shall start.</param>
        public override void Restart(long delay = 0)
        {
            // Check if disposed
            if (isDisposed)
                throw new ObjectDisposedException(nameof(BassAudioRecording));
            // Setup delay
            remainingDelay = (delay * Samplerate * Channels / 1000000) + remainingDelay;
            if (remainingDelay > 0)
                Array.Clear(buffer, 0, buffer.Length);
            else
                remainingDelay = 0;
            // Play
            lock (lockBassAccess)
            {
                Bass.CurrentDevice = deviceID;
                if (!Bass.ChannelPlay(handle, true))
                    throw new BassException(Bass.GetErrorCode());
                running = true;
                paused = false;
            }
        }

        /// <summary>
        /// Pauses the playback.
        /// </summary>
        public override void Pause()
        {
            // Check if disposed
            if (isDisposed)
                throw new ObjectDisposedException(nameof(BassAudioRecording));
            // Checks
            if (!running || paused) return;
            // Pause
            lock (lockBassAccess)
            {
                Bass.CurrentDevice = deviceID;
                Bass.ChannelPause(handle);
                paused = true;
            }
        }

        /// <summary>
        /// Resumes the playback.
        /// </summary>
        public override void Resume()
        {
            // Check if disposed
            if (isDisposed)
                throw new ObjectDisposedException(nameof(BassAudioRecording));
            // Checks
            if (!running || !paused) return;
            // Play
            lock (lockBassAccess)
            {
                Bass.CurrentDevice = deviceID;
                Bass.ChannelPlay(handle, false);
                paused = false;
            }
        }

        /// <summary>
        /// Stops the playback.
        /// </summary>
        public override void Stop()
        {
            // Check if disposed
            if (isDisposed)
                throw new ObjectDisposedException(nameof(BassAudioRecording));
            // Checks
            if (!running) return;
            // Stop
            lock (lockBassAccess)
            {
                Bass.CurrentDevice = deviceID;
                Bass.ChannelStop(handle);
                running = false;
                paused = false;
            }
        }

        /// <summary>
        /// Gets an indicator whether the playback stream is active.
        /// </summary>
        public override bool IsActive
        {
            get
            {
                // Check if disposed
                if (isDisposed)
                    throw new ObjectDisposedException(nameof(BassAudioRecording));
                // Return value
                return running;
            }
        }

        /// <summary>
        /// Gets an indicator whether the playback stream is paused.
        /// </summary>
        public override bool IsPaused
        {
            get
            {
                // Check if disposed
                if (isDisposed)
                    throw new ObjectDisposedException(nameof(BassAudioRecording));
                // Return value
                return paused;
            }
        }

        /// <summary>
        /// Gets the sample position of the playback stream.
        /// </summary>
        public override long Position
        {
            get
            {
                // Check if disposed
                if (isDisposed)
                    throw new ObjectDisposedException(nameof(BassAudioRecording));
                // Return value
                long value;
                lock (lockBassAccess)
                {
                    value = Bass.ChannelGetPosition(handle);
                    if (value == -1)
                        throw new BassException(Bass.GetErrorCode());
                }
                return value;
            }
        }

        /// <summary>
        /// Gets or sets the volume of the recording stream.
        /// </summary>
        /// <remarks>
        /// The volume is provided on a linear base.
        /// The volume can range from 0 to <see cref="LibrarySettings.AudioPlaybackMaximumChannelAmplification"/>.
        /// Any values provided outside this range will be automatically capped.
        /// </remarks>
        public override float Volume
        {
            get
            {
                // Check if disposed
                if (isDisposed)
                    throw new ObjectDisposedException(nameof(BassAudioRecording));
                // Return value
                float value;
                lock (lockBassAccess)
                {
                    Bass.CurrentDevice = deviceID;
                    value = Bass.ChannelGetAttribute(handle, BassChannelAttribute.Volume);
                }
                return value;
            }
            set
            {
                // Check if disposed
                if (isDisposed)
                    throw new ObjectDisposedException(nameof(BassAudioRecording));
                // Set boundaries
                if (value < 0) value = 0;
                if (value > LibrarySettings.AudioPlaybackMaximumChannelAmplification) value = LibrarySettings.AudioPlaybackMaximumChannelAmplification;
                // Set value
                lock (lockBassAccess)
                {
                    Bass.CurrentDevice = deviceID;
                    if (!Bass.ChannelSetAttribute(handle, BassChannelAttribute.Volume, value))
                        throw new BassException(Bass.GetErrorCode());
                }
            }
        }
    }
}
