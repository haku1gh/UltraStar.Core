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
using UltraStar.Core.ThirdParty.Serilog;
using UltraStar.Core.Unmanaged.Bass;
using UltraStar.Core.Utils;

namespace UltraStar.Core.Audio
{
    /// <summary>
    /// Represents an audio recording class using the BASS library.
    /// </summary>
    public class BassAudioRecording : AudioRecording
    {
        private static Object lockBassAccess = new Object();
        private static List<ActiveRecordingDevice> activeRecordingDevices = new List<ActiveRecordingDevice>();
        private ActiveRecordingDevice activeRecordingDevice = null;
        private bool running = false;

        /// <summary>
        /// Initializes a new instance of <see cref="BassAudioRecording"/>.
        /// </summary>
        /// <param name="deviceID">The device ID of the audio recording device.</param>
        /// <param name="channel">The channel to be recorded (0=first channel, ...).</param>
        /// <param name="audioRecordingCallback">A callback function for the audio recording.</param>
        /// <param name="samplerate">The sample rate of the recording.</param>
        /// <param name="input">The used input on the device.</param>
        public BassAudioRecording(int deviceID, int channel, AudioRecordingCallback audioRecordingCallback, int samplerate, int input) :
            base(deviceID, channel, audioRecordingCallback, samplerate, input)
        {
            // Get device info
            USAudioRecordingDeviceInfo deviceInfo = null;
            int channels = 0;
            foreach (USAudioRecordingDeviceInfo info in GetDevices())
            {
                if(info.DeviceID == deviceID)
                {
                    deviceInfo = info;
                    channels = info.Channels;
                }
            }
            if (channels == 0) channels = LibrarySettings.AudioRecordingDefaultChannels; // To be used, when the number of channels is unknown.
            // Check parameters
            if (deviceInfo == null)
                throw new AudioException("Invalid name provided for the new recording channel.");
            if (channel < 0 || channel >= channels)
                throw new AudioException("Invalid channel provided for the new recording channel.");
            if (audioRecordingCallback == null)
                throw new AudioException("No callback provided for the new recording channel.");
            if (samplerate < 0)
                throw new AudioException("Invalid sample rate provided for the new recording channel.");
            if(input < 0 || input >= deviceInfo.Inputs)
                throw new AudioException("Invalid input provided for the new recording channel.");
            // Initialize
            lock (lockBassAccess)
            {
                // Search active devices, and use one if appropiate
                foreach (ActiveRecordingDevice activeDevice in activeRecordingDevices)
                {
                    if (activeRecordingDevice.DeviceInfo.DeviceID == deviceID)
                    {
                        activeRecordingDevice = activeDevice;
                        activeRecordingDevice.AddUser(this);
                    }
                }
                // Nothing found, so open a new recording device
                if (activeRecordingDevice == null)
                {
                    activeRecordingDevice = new ActiveRecordingDevice(deviceID, 2, samplerate, input);
                    activeRecordingDevice.AddUser(this);
                    activeRecordingDevices.Add(activeRecordingDevice);
                }
                activeRecordingDevice.Stopped += ActiveRecordingDevice_Stopped;
                ReInitialize();
            }
        }

        /// <summary>
        /// Helpermethod for Event "Stopped" from ActiveRecordingDevice class.
        /// </summary>
        private void ActiveRecordingDevice_Stopped(object sender, EventArgs e)
        {
            running = false;
            lock (lockBassAccess)
            {
                activeRecordingDevice.Stopped -= ActiveRecordingDevice_Stopped;
            }
            onClosed();
        }

        /// <summary>
        /// Gets all available recording devices.
        /// </summary>
        /// <returns>An array of type <see cref="USAudioRecordingDeviceInfo"/>.</returns>
        public static new USAudioRecordingDeviceInfo[] GetDevices()
        {
            List<USAudioRecordingDeviceInfo> deviceInfos = new List<USAudioRecordingDeviceInfo>();
            int deviceID = 0;
            lock (lockBassAccess)
            {
                while (Bass.GetRecordingDeviceInfo(deviceID, out BassDeviceInfo info))
                {
                    if (info.IsEnabled && !info.IsLoopback)
                    {
                        if (info.IsInitialized) // Device is already open.
                        {
                            foreach (ActiveRecordingDevice activeDevice in activeRecordingDevices)
                            {
                                if (activeDevice.DeviceID == deviceID)
                                    deviceInfos.Add(activeDevice.DeviceInfo);
                            }
                        }
                        else // Device is not opened yet.
                        {
                            // Shortly initialize the device
                            int currentDeviceID = Bass.CurrentRecordingDevice;
                            bool success = Bass.RecordingDeviceInit(deviceID);
                            if (success)
                            {
                                // Get the device info
                                Bass.CurrentRecordingDevice = deviceID;
                                deviceInfos.Add(ActiveRecordingDevice.GetDeviceInfo(deviceID, 0));
                                // Free the device again
                                Bass.RecordingDeviceFree();
                                if (currentDeviceID != -1)
                                {
                                    try
                                    {
                                        Bass.CurrentRecordingDevice = currentDeviceID;
                                    }
                                    catch { }
                                }
                            }
                            else
                                deviceInfos.Add(ActiveRecordingDevice.GetDeviceInfo(deviceID, 0));
                        }
                    }
                    deviceID++;
                }
            }
            return deviceInfos.ToArray();
        }

        /// <summary>
        /// Gets information about the default recording device. <see langword="null"/> is returned in case no default device is available.
        /// </summary>
        public static new USAudioRecordingDeviceInfo DefaultDevice
        {
            get
            {
                USAudioRecordingDeviceInfo[] devices = GetDevices();
                for (int i = 0; i < devices.Length; i++)
                {
                    if (devices[i].IsDefault) return devices[i];
                }
                return null;
            }
        }

        /// <summary>
        /// Gets an indicator whether the default recording device is available.
        /// </summary>
        public static new bool IsDefaultDeviceAvailable
        {
            get
            {
                USAudioRecordingDeviceInfo[] devices = GetDevices();
                for (int i = 0; i < devices.Length; i++)
                {
                    if (devices[i].IsDefault) return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Finalizes this instance.
        /// </summary>
        ~BassAudioRecording()
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
                    // Stop recording
                    if (running)
                    {
                        running = false;
                        activeRecordingDevice.Detach(this);
                        activeRecordingDevice.Stopped -= ActiveRecordingDevice_Stopped;
                    }
                    activeRecordingDevice.RemoveUser(this);
                    if (activeRecordingDevice.NumberOfUsers == 0)
                    {
                        activeRecordingDevice.Stop(disposing);
                        // Remove from list
                        if (activeRecordingDevices != null)
                        {
                            activeRecordingDevices.Remove(activeRecordingDevice);
                        }
                    }
                }
                // Set references to null
                activeRecordingDevice = null;
                // Raise event
                if (disposing) onClosed();
            }
            base.Dispose(disposing);
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
        public override void ReInitialize()
        {
            // Check if disposed
            if (isDisposed)
                throw new ObjectDisposedException(nameof(BassAudioRecording));
            lock (lockBassAccess)
            {
                if (!running)
                {
                    if (!activeRecordingDevice.Running)
                        activeRecordingDevice.Initialize(SampleRate, Input);
                    activeRecordingDevice.Attach(this, audioRecordingCallback, Channel);
                    running = true;
                }
            }
        }

        /// <summary>
        /// Starts the recording.
        /// </summary>
        /// <param name="delay">The delay in micro seconds [us] before recording shall start.</param>
        public override void Start(long delay = 0)
        {
            startRecording(false, delay);
        }

        /// <summary>
        /// Restarts the recording.
        /// </summary>
        /// <param name="delay">The delay in micro seconds [us] before recording shall start.</param>
        public override void Restart(long delay = 0)
        {
            startRecording(true, delay);
        }

        /// <summary>
        /// Helpermethod to start recording.
        /// </summary>
        private void startRecording(bool restart, long delay = 0)
        {
            // Check if disposed
            if (isDisposed)
                throw new ObjectDisposedException(nameof(BassAudioRecording));
            // Re initialize
            ReInitialize();
            // Start recording
            lock (lockBassAccess)
            {
                activeRecordingDevice.Pause(false);
                activeRecordingDevice.Start(restart, SampleRate, delay);
            }
        }

        /// <summary>
        /// Pauses the recording.
        /// </summary>
        /// <remarks>
        /// This may also pause the recording of other channels from the same device.
        /// </remarks>
        public override void Pause()
        {
            pauseResumeRecording(true);
        }

        /// <summary>
        /// Resumes the recording.
        /// </summary>
        /// <remarks>
        /// This may also resume the recording of other channels from the same device.
        /// </remarks>
        public override void Resume()
        {
            pauseResumeRecording(false);
        }

        /// <summary>
        /// Helpermethod to pause/resume recording.
        /// </summary>
        private void pauseResumeRecording(bool pauseState)
        {
            // Check if disposed
            if (isDisposed)
                throw new ObjectDisposedException(nameof(BassAudioRecording));
            // Pause/resume recording
            lock (lockBassAccess)
            {
                activeRecordingDevice.Pause(pauseState);
            }
        }

        /// <summary>
        /// Stops the recording.
        /// </summary>
        /// <remarks>
        /// This does not stop the recording of other channels from the same device.
        /// If this is the last recording channel from the device, then it will be stopped completely.
        /// </remarks>
        public override void Stop()
        {
            // Check if disposed
            if (isDisposed)
                throw new ObjectDisposedException(nameof(BassAudioRecording));
            // Stop recording
            lock (lockBassAccess)
            {
                running = false;
                activeRecordingDevice.Detach(this);
                activeRecordingDevice.Stopped -= ActiveRecordingDevice_Stopped;
                if (activeRecordingDevice.NumberOfListeners == 0)
                    activeRecordingDevice.Stop(true);
            }
        }

        /// <summary>
        /// Gets an indicator whether the recording stream is active.
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
        /// Gets an indicator whether the recording stream is paused.
        /// </summary>
        public override bool IsPaused
        {
            get
            {
                // Check if disposed
                if (isDisposed)
                    throw new ObjectDisposedException(nameof(BassAudioRecording));
                // Return value
                return activeRecordingDevice.Paused;
            }
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
            get
            {
                // Check if disposed
                if (isDisposed)
                    throw new ObjectDisposedException(nameof(BassAudioRecording));
                // Return value
                return activeRecordingDevice.Position;
            }
        }

        /// <summary>
        /// Gets the number of samples per channel currently buffered in the recording stream.
        /// </summary>
        public override int BufferCount
        {
            get
            {
                // Check if disposed
                if (isDisposed)
                    throw new ObjectDisposedException(nameof(BassAudioRecording));
                // Return value
                return activeRecordingDevice.BufferCount;
            }
        }

        /// <summary>
        /// Gets or sets the volume of the recording stream.
        /// </summary>
        /// <remarks>
        /// The volume is provided on a linear base.
        /// The volume can range from 0 to <see cref="LibrarySettings.AudioRecordingMaximumChannelAmplification"/>.
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
                return activeRecordingDevice.GetChannelVolume(Channel);
            }
            set
            {
                // Check if disposed
                if (isDisposed)
                    throw new ObjectDisposedException(nameof(BassAudioRecording));
                // Set value
                activeRecordingDevice.SetChannelVolume(Channel, value);
            }
        }

        /// <summary>
        /// Gets the used audio recording device.
        /// </summary>
        public override USAudioRecordingDeviceInfo DeviceInfo
        {
            get
            {
                // Check if disposed
                if (isDisposed)
                    throw new ObjectDisposedException(nameof(BassAudioRecording));
                // Return value
                return activeRecordingDevice.DeviceInfo;
            }
        }

        /// <summary>
        /// Represents an active recording device.
        /// </summary>
        private class ActiveRecordingDevice
        {
            private static readonly int[] callbackPeriod = new int[] { 5, 20, 50 }; // 240, 960, 2400 samples ideally with 48000 samplerate
            private List<Listener> listeners;
            private List<BassAudioRecording> users;
            private readonly float[][] buffer;
            private readonly float[] emptyBuffer;
            private readonly float[] channelVolume;
            private int handle;
            private BassRecordProcedure internalCallback = null;
            private BassSyncProcedure failureCallback = null;
            private long remainingDelay = 0; // Unit is 1x sample per channel

            /// <summary>
            /// Gets the recording device info for the active recording.
            /// </summary>
            public USAudioRecordingDeviceInfo DeviceInfo { get; private set; }
            /// <summary>
            /// Gets the device ID of the active recording device.
            /// </summary>
            public int DeviceID { get; private set; }
            /// <summary>
            /// Gets the number of channels for this recording.
            /// </summary>
            public int Channels { get; }
            /// <summary>
            /// Gets the number of currently attached listeners.
            /// </summary>
            public int NumberOfListeners => listeners.Count;
            /// <summary>
            /// Gets the number of current users.
            /// </summary>
            public int NumberOfUsers => users.Count;
            /// <summary>
            /// Gets the recording position.
            /// </summary>
            public int Position { get; private set; } // Datatype int allows roughly 12.5h recording before an overflow occurs. Using long would require additional locks (threading).
            /// <summary>
            /// Gets an indicator whether the recording device is running (being active) or not.
            /// </summary>
            public bool Running => handle != 0;
            /// <summary>
            /// Gets an indicator whether recording is paused.
            /// </summary>
            public bool Paused { get; private set; }

            /// <summary>
            /// Initializes a new instance of <see cref="ActiveRecordingDevice"/>.
            /// </summary>
            /// <param name="deviceID">The device ID of the device.</param>
            /// <param name="channels">The number of channels for recording.</param>
            /// <param name="samplerate">The samplerate for recording.</param>
            /// <param name="input">The input on which recording shall be preformed.</param>
            /// <exception cref="BassException">An unknown error from the BASS library occurred.</exception>
            /// <exception cref="AudioException">The recording device does not exist or is already in use.</exception>
            public ActiveRecordingDevice(int deviceID, int channels, int samplerate, int input)
            {
                BassDeviceInfo info = Bass.GetRecordingDeviceInfo(deviceID);
                if (!info.IsEnabled || info.IsLoopback || info.IsInitialized)
                    throw new AudioException("Recording device " + info.Name + " disabled or already in use.");
                // Initialize variables
                DeviceID = deviceID;
                Channels = channels;
                handle = 0;
                buffer = new float[channels][];
                channelVolume = new float[channels];
                listeners = new List<Listener>();
                users = new List<BassAudioRecording>();
                for (int c = 0; c < channels; c++)
                {
                    buffer[c] = new float[LibrarySettings.AudioRecordingBufferSize];
                    channelVolume[c] = 1.0f;
                }
                emptyBuffer = new float[LibrarySettings.AudioRecordingBufferSize];
                for (int i = 0; i < emptyBuffer.Length; i++)
                    emptyBuffer[i] = 0;
                // Open device
                Initialize(samplerate, input);
            }

            /// <summary>
            /// Initializes or Re-Initializes (in case recording was stopped) the recording device.
            /// </summary>
            /// <param name="samplerate">The samplerate for recording.</param>
            /// <param name="input">The input on which recording shall be preformed.</param>
            /// <exception cref="BassException">An unknown error from the BASS library occurred.</exception>
            /// <exception cref="AudioException">The recording device does not exist or is already in use.</exception>
            public void Initialize(int samplerate, int input)
            {
                // Some basic checks
                if (handle != 0) return; // Nothing to do, the device is already running
                BassDeviceInfo info = Bass.GetRecordingDeviceInfo(DeviceID);
                if (!info.IsEnabled || info.IsLoopback || info.IsInitialized)
                    throw new AudioException("Recording device " + info.Name + " disabled or already in use.");
                // Open device
                bool success = Bass.RecordingDeviceInit(DeviceID);
                if (!success) throw new BassException(Bass.GetErrorCode());
                Bass.CurrentRecordingDevice = DeviceID;
                // Set input
                BassRecordInfo recordInfo = Bass.RecordingDeviceExtendedInfo;
                for (int i = 0; i < recordInfo.Inputs; i++)
                    Bass.RecordingDeviceChangeInput(i, false);
                if (input < 0 || input >= recordInfo.Inputs) input = 0;
                Bass.RecordingDeviceChangeInput(input, true);
                // Get extended info
                if (DeviceInfo != null && info.Name != DeviceInfo.Name)
                    throw new AudioException("Internal association of recording devices changed. Can't open the recording device " + DeviceInfo.Name + " again." +
                        "This could happen when recording devices had been attached or detached in the meantime.");
                DeviceInfo = GetDeviceInfo(DeviceID, input);
                // Prepare class variables
                Position = 0;
                // Start recording
                Bass.SetConfiguration(BassConfigurationOption.RecordingBufferLength, UsOptions.AudioRecordingBufferLength);
                internalCallback = recordingCallback;
                handle = Bass.RecordingDeviceStart(samplerate, Channels, BassRecordStartFlags.RecordPause | BassRecordStartFlags.Float,
                    internalCallback, callbackPeriod[(int)UsOptions.AudioRecordingDelay]);
                if (handle == 0) throw new BassException(Bass.GetErrorCode());
                failureCallback = deviceFailCallback;
                int syncHandle = Bass.ChannelSetSyncDeviceFail(handle, failureCallback);
                if (syncHandle == 0) throw new BassException(Bass.GetErrorCode());
                Paused = true;
            }

            /// <summary>
            /// Finalizes this instance.
            /// </summary>
            ~ActiveRecordingDevice()
            {
                Stop(false);
                Stopped = null;
                internalCallback = null;
                failureCallback = null;
            }

            /// <summary>
            /// Internal callback from the recording device.
            /// </summary>
            private unsafe bool recordingCallback(int handle, IntPtr bufferPtr, int length, IntPtr user)
            {
                if (this.handle != handle)
                {
                    this.handle = 0;
                    lock (listeners)
                    {
                        listeners.Clear();
                    }
                    Log.Error("Recording device {Device} provided an invalid handle. Stopping recording.", DeviceInfo.Name);
                    onStopped();
                    return false; // This will also stop a recording. But as the handle is invalid, no problem here.
                }
                // Split channels and raise individual callbacks
                float* _pBuffer = (float*)bufferPtr;
                if (length % 4 != 0)
                    Log.Error("Unaligned recording data. Recording data may be temporary invalid. Length in bytes of the recording buffer is {Length} and not a multiple of 4.", length);
                length /= 4;
                int pos = 0, bufferPos = 0;
                while (pos < length)
                {
                    if (remainingDelay > 0)
                    {
                        pos++;
                        remainingDelay--;
                        continue;
                    }
                    for (int c = 0; c < Channels; c++)
                        buffer[c][bufferPos] = _pBuffer[pos++] * channelVolume[c];
                    bufferPos++;
                    if (bufferPos == LibrarySettings.AudioRecordingBufferSize) // This should usually not be called
                    {
                        // Inform listeners
                        informListeners(bufferPos);
                        // And continue
                        bufferPos = 0;
                    }
                }
                // Also inform listeners after all data has been processed
                if (bufferPos != 0)
                    informListeners(bufferPos);
                // Return
                return true;
            }

            /// <summary>
            /// Internal callback from the recording device, when the underlying device failed.
            /// </summary>
            private void deviceFailCallback(int handle, int channel, int data, IntPtr user)
            {
                this.handle = 0;
                lock (listeners)
                {
                    listeners.Clear();
                }
                Log.Error("Recording device {Device} stopped unexpectedly (eg. if it is disconnected/disabled). Stopping recording.", DeviceInfo.Name);
                onStopped();
            }

            /// <summary>
            /// Informs all listeners about new data.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void informListeners(int length)
            {
                lock (listeners)
                {
                    if (!Paused) Position += length;
                    foreach (Listener listener in listeners)
                    {
                        if (Paused)
                            listener.Callback(listener.Handle, emptyBuffer, length, true);
                        else
                            listener.Callback(listener.Handle, buffer[listener.Channel], length, false);
                    }
                }
            }

            /// <summary>
            /// Gets the listener index from a handle, or -1 if not found.
            /// </summary>
            private int getListenerIndex(BassAudioRecording handle)
            {
                for (int i = 0; i < listeners.Count; i++)
                {
                    if (listeners[i].Handle == handle) return i;
                }
                return -1;
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
            private void onStopped()
            {
                Stopped?.Invoke(this, new EventArgs());
            }

            /// <summary>
            /// Adds a user to this instance.
            /// </summary>
            /// <param name="user">The user to be added.</param>
            public void AddUser(BassAudioRecording user)
            {
                lock (users)
                    if (!users.Contains(user)) users.Add(user);
            }

            /// <summary>
            /// Removes a user from this instance.
            /// </summary>
            /// <param name="user">The user to be removed.</param>
            public void RemoveUser(BassAudioRecording user)
            {
                lock (users)
                    if (users.Contains(user)) users.Remove(user);
            }

            /// <summary>
            /// Attaches a listener from the recording device.
            /// </summary>
            public void Attach(BassAudioRecording handle, AudioRecordingCallback callback, int channel)
            {
                // Check if recording already stopped
                if (this.handle == 0)
                {
                    Log.Error("Could not attach channel. The recording of device {Device} already stopped recording.", DeviceInfo.Name);
                    return;
                }
                // Check if channels are valid
                if (channel >= Channels)
                {
                    Log.Fatal("Channel {Channel} on recording device {Device} does not exist.", (channel + 1), DeviceInfo.Name);
                    throw new AudioException("Channel " + (channel + 1).ToString() + " on recording device " + DeviceInfo.Name + " does not exist.");
                }
                lock (listeners)
                {
                    // Check if this is the first request from that handle
                    if (getListenerIndex(handle) == -1)
                    {
                        // Add the new listener
                        Listener listener = new Listener(handle, callback, channel);
                        listeners.Add(listener);
                    }
                }
            }

            /// <summary>
            /// Detaches a listener from the recording device.
            /// </summary>
            public void Detach(BassAudioRecording handle)
            {
                // Check if recording already stopped
                if (this.handle == 0) return;

                lock (listeners)
                {
                    // Get listener index
                    int index = getListenerIndex(handle);
                    // Check if index is existing
                    if (index != -1)
                    {
                        // Remove listener
                        listeners.RemoveAt(index);
                    }
                }
            }

            /// <summary>
            /// Starts the recording.
            /// </summary>
            /// <param name="restart"><see langword="true"/> if the recording should be restarted; otherwise <see langword="false"/>.</param>
            /// <param name="samplerate">The samplerate for recording.</param>
            /// <param name="delay">The delay in micro seconds [us] before recording shall start.</param>
            public void Start(bool restart, int samplerate, long delay = 0)
            {
                // Check if recording already stopped
                if (handle == 0) return;
                // Setup delay
                remainingDelay = (delay * samplerate * Channels / 1000000);
                if (remainingDelay < 0)
                    remainingDelay = 0;
                // Start recording
                lock (listeners)
                {
                    if (restart) Position = 0;
                }
                Bass.CurrentRecordingDevice = DeviceID;
                bool success = Bass.ChannelPlay(this.handle, false);
                if (!success)
                    Log.Error("Start recording of device {Device} failed. Error: {ErrorCode}.", DeviceInfo.Name, Bass.GetErrorCode());
            }

            /// <summary>
            /// Changes the pause state of a recording.
            /// </summary>
            /// <param name="newPauseState"><see langword="true"/> if the recording should be paused; otherwise <see langword="false"/>.</param>
            public void Pause(bool newPauseState)
            {
                if (Paused == newPauseState) return;
                lock (listeners)
                {
                    Paused = newPauseState;
                }
            }

            /// <summary>
            /// Stops the recording device. Any resources will be freed.
            /// </summary>
            /// <remarks>
            /// It is not possible to start a recording from this instance again. Please create a new instance instead.
            /// </remarks>
            /// <param name="raiseEvent"><see langword="true"/> if <see cref="Stopped"/> event should be raised; otherwise <see langword="false"/>.</param>
            public void Stop(bool raiseEvent)
            {
                // Check if recording already stopped
                if (handle == 0) return;
                // Temporary store handle
                int tempHandle = handle;
                handle = 0; // This line could lead to a pre-close of the device in the recording callback <see cref="recordingCallback"/>. Which is intended.
                // Remove all listeners
                lock (listeners)
                {
                    listeners.Clear();
                }
                // Stop recording and free recording device's resources
                try
                {
                    Bass.CurrentRecordingDevice = DeviceID; // This could throw an exception if the device is already closed.
                    Bass.ChannelStop(tempHandle); // According to documentation, there should be only an error when the device is already closed.
                    Bass.RecordingDeviceFree(); // According to documentation, there should be only an error when the device is not opened. But then there is also nothing to free here.
                }
                catch (BassException bassException)
                {
                    Log.Error("Recording device {Device} is not available anymore. Error: {ErrorCode}.", DeviceInfo.Name, bassException.ErrorCode);
                }
                // Raise event
                if(raiseEvent)
                    onStopped();
            }

            /// <summary>
            /// Gets the volume of a channel.
            /// </summary>
            /// <param name="channel">A channel of this recording.</param>
            /// <returns>The volume. This can range from 0 to <see cref="LibrarySettings.AudioRecordingMaximumChannelAmplification"/>.</returns>
            public float GetChannelVolume(int channel)
            {
                // Checks
                if (channel < 0 || channel >= channelVolume.Length)
                    throw new AudioException("Invalid channel from recording device accessed.");
                // Return value
                return channelVolume[channel];
            }

            /// <summary>
            /// Gets the number of samples per channel currently buffered in the recording stream.
            /// </summary>
            public int BufferCount
            {
                get
                {
                    // Check if recording already stopped
                    if (handle == 0) return 0;
                    // Return value
                    int value;
                    lock (lockBassAccess)
                    {
                        value = Bass.ChannelGetBufferedDataCount(handle);
                        if (value == -1)
                            throw new BassException(Bass.GetErrorCode());
                        value /= (4 * Channels); // A single audio sample is of type float = 4 bytes.
                    }
                    return value;
                }
            }

            /// <summary>
            /// Sets the volume of a channel.
            /// </summary>
            /// <param name="channel">A channel of this recording.</param>
            /// <param name="newVolume">The new volume. This can range from 0 to <see cref="LibrarySettings.AudioRecordingMaximumChannelAmplification"/>.</param>
            public void SetChannelVolume(int channel, float newVolume)
            {
                // Checks
                if (channel < 0 || channel >= channelVolume.Length)
                    throw new AudioException("Invalid channel from recording device accessed.");
                // Set boundaries
                if (newVolume < 0) newVolume = 0;
                if (newVolume > LibrarySettings.AudioRecordingMaximumChannelAmplification) newVolume = LibrarySettings.AudioRecordingMaximumChannelAmplification;
                // Set value
                channelVolume[channel] = newVolume;
            }

            /// <summary>
            /// Gets information about an audio recording device.
            /// </summary>
            /// <remarks>
            /// Try to ensure that you are only calling this function when the device is initialized. Otherwise it will contain only limited information.
            /// </remarks>
            /// <param name="deviceID">The device ID.</param>
            /// <param name="input">The input from which the volume information will be used.</param>
            /// <returns>A <see cref="USAudioRecordingDeviceInfo"/> object.</returns>
            public static USAudioRecordingDeviceInfo GetDeviceInfo(int deviceID, int input)
            {
                bool result = Bass.GetRecordingDeviceInfo(deviceID, out BassDeviceInfo deviceInfo);
                // If it fails, return NULL
                if (!result) return null;
                // If it is initialized, then we can add more information        
                if (deviceInfo.IsInitialized)
                {
                    // Set current device ID
                    int currentDeviceID = Bass.CurrentRecordingDevice;
                    if (deviceID != currentDeviceID) Bass.CurrentRecordingDevice = deviceID;
                    // Get extended info
                    BassRecordInfo recordInfo = Bass.RecordingDeviceExtendedInfo;
                    // Get input names
                    string[] names = new string[recordInfo.Inputs];
                    for (int i = 0; i < recordInfo.Inputs; i++)
                    {
                        names[i] = Bass.BASS_RecordingDeviceInputName(i);
                        if (names[i] == null) names[i] = "";
                    }
                    // Get volume
                    float volume = Bass.RecordingDeviceVolume(input);
                    // Return device info
                    if (deviceID != currentDeviceID) Bass.CurrentRecordingDevice = currentDeviceID;
                    return new USAudioRecordingDeviceInfo(deviceID, deviceInfo.Name, deviceInfo.Type.ToString(), deviceInfo.IsDefault, recordInfo.Inputs, names, recordInfo.Channels, recordInfo.Frequency, volume);
                }
                else
                {
                    return new USAudioRecordingDeviceInfo(deviceID, deviceInfo.Name, deviceInfo.Type.ToString(), deviceInfo.IsDefault, 0, new string[] { }, 0, 0, -1);
                }
            }

            /// <summary>
            /// A listener to a BASS audio recording device.
            /// </summary>
            private struct Listener
            {
                /// <summary>
                /// Initializes <see cref="Listener"/>.
                /// </summary>
                public Listener(BassAudioRecording handle, AudioRecordingCallback callback, int channel)
                {
                    Handle = handle;
                    Callback = callback;
                    Channel = channel;
                }

                /// <summary>
                /// Gets the handle of the listener.
                /// </summary>
                public BassAudioRecording Handle { get; }
                /// <summary>
                /// Gets the channel of which the listener is interested in.
                /// </summary>
                public int Channel { get; }
                /// <summary>
                /// Gets the callback method to be called when data is available for the channel.
                /// </summary>
                public AudioRecordingCallback Callback { get; }
            }
        }
    }
}
