#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.Runtime.InteropServices;
using UltraStar.Core.Utils;

namespace UltraStar.Core.Unmanaged.Bass
{
    /// <summary>
    /// Represents a wrapper class around the BASS library.
    /// </summary>
    internal static class Bass
    {
        /// <summary>
        /// Handle to the library.
        /// </summary>
        private static readonly IntPtr libraryHandle;

        private static readonly Version minSupportedVersion = new Version(2, 4, 15, 0);
        private static readonly Version maxSupportedVersion = new Version(2, 4, 255, 255);

        /// <summary>
        /// Initializes <see cref="Bass"/>.
        /// </summary>
        static Bass()
        {
            // Create the full library name.
            string libraryName = "bass";
            string fullLibraryName = "";
            switch (SystemInformation.Platform)
            {
                case Platform.Mac:
                    fullLibraryName = "lib" + libraryName + ".dylib";
                    break;
                case Platform.Linux:
                    fullLibraryName = "lib" + libraryName + ".so";
                    break;
                case Platform.Windows:
                    fullLibraryName = libraryName + ".dll";
                    break;
            }
            // Load the library
            libraryHandle = LibraryLoader.LoadNativeLibraryAsPerConfig(fullLibraryName);
            // Check if loading was successful
            if (libraryHandle == IntPtr.Zero)
                throw new DllNotFoundException("Could not find library " + fullLibraryName + ".");
            // Check if UltraStar.Core is compatible with this version.
            Version bassVersion = GetVersion();
            if (bassVersion < minSupportedVersion || bassVersion > maxSupportedVersion)
                throw new NotSupportedException("This version of the BASS library is not supported.");
            // Set UTF-8 mode for Windows (so that strings are correctly parsed)
            if (SystemInformation.Platform == Platform.Windows)
                SetConfiguration(BassConfigurationOption.UnicodeDeviceInformation, 1);
            // Set Default device for Windows and Mac
            if (SystemInformation.Platform == Platform.Windows || SystemInformation.Platform == Platform.Mac)
                SetConfiguration(BassConfigurationOption.IncludeDefaultDevice, 1);
        }

        #region Common

        /// <summary>
        /// Delegate for BASS_GetVersion.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate int bass_getversion_delegate();
        /// <summary>
        /// Gets the version of BASS that is loaded.
        /// </summary>
        /// <remarks>
        /// There is no guarantee that a previous or future version of BASS supports all the BASS functions that you are using,
        /// so you should always use this function to make sure the correct version is loaded.
        /// It is safe to assume that future revisions (indicated in the LOWORD) will be fully compatible.
        /// </remarks>
        /// <returns>The BASS version.</returns>
        public static Version GetVersion()
        {
            bass_getversion_delegate del = LibraryLoader.GetFunctionDelegate<bass_getversion_delegate>(libraryHandle, "BASS_GetVersion");
            int number = del();
            return new Version((number >> 24) & 0xFF, (number >> 16) & 0xFF, (number >> 8) & 0xFF, number & 0xFF);
        }

        /// <summary>
        /// Delegate for BASS_ErrorGetCode.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate BassErrorCode bass_errorgetcode_delegate();
        /// <summary>
        /// Gets the last BASS error code.
        /// </summary>
        /// <remarks>
        /// If no error occurred during the last BASS function call then BASS_OK is returned, else one of the BASS_ERROR values is returned.
        /// See the function description for an explanation of what the error code means.
        /// Error codes are stored for each thread. So if you happen to call 2 or more BASS functions at the same time,
        /// they will not interfere with each other's error codes. 
        /// </remarks>
        /// <returns>The last BASS error code.</returns>
        public static BassErrorCode GetErrorCode()
        {
            bass_errorgetcode_delegate del = LibraryLoader.GetFunctionDelegate<bass_errorgetcode_delegate>(libraryHandle, "BASS_ErrorGetCode");
            return del();
        }

        /// <summary>
        /// Delegate for BASS_SetConfig.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_setconfig_delegate(BassConfigurationOption option, int newValue);
        /// <summary>
        /// Sets a configuration option.
        /// </summary>
        /// <remarks>
        /// Some config options have a restricted range of values, so the config's actual value may not be the same as requested if it was out of range.
        /// BASS_GetConfig can be used to confirm what the value is.
        /// Config options can be used at any time and are independent of initialization, ie.BASS_Init does not need to have been called beforehand.
        /// Where a config option is shown to have a "BOOL" value, 0 (zero) is taken to be "FALSE" and anything else is taken to be "TRUE". 
        /// </remarks>
        /// <param name="option">The configuration option to be changed.</param>
        /// <param name="newValue">The new value to set.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool SetConfiguration(BassConfigurationOption option, int newValue)
        {
            bass_setconfig_delegate del = LibraryLoader.GetFunctionDelegate<bass_setconfig_delegate>(libraryHandle, "BASS_SetConfig");
            return del(option, newValue);
        }

        /// <summary>
        /// Delegate for BASS_GetConfig.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate int bass_getconfig_delegate(BassConfigurationOption option);
        /// <summary>
        /// Gets a configuration option.
        /// </summary>
        /// <param name="option">The configuration option to be retrieved.</param>
        /// <returns>The configuration value or -1 on error. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static int GetConfiguration(BassConfigurationOption option)
        {
            bass_getconfig_delegate del = LibraryLoader.GetFunctionDelegate<bass_getconfig_delegate>(libraryHandle, "BASS_GetConfig");
            return del(option);
        }

        #endregion Common

        #region Playback

        /// <summary>
        /// Delegate for BASS_GetDeviceInfo.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_getdeviceinfo_delegate(int device, out BassDeviceInfo info);
        /// <summary>
        /// Gets information on an playback device.
        /// </summary>
        /// <remarks>
        /// This function can be used to enumerate the available devices for a setup dialog.
        /// Device 0 is always the "no sound" device, so you should start at device 1 if you only want to list real output devices.
        /// 
        /// On Linux, a "Default" device is hardcoded to device number 1, which uses the default output set in the ALSA config,
        /// and the real devices start at number 2.
        /// That is also the case on Windows and OSX when the BASS_CONFIG_DEV_DEFAULT option is enabled.
        /// </remarks>
        /// <param name="device">The device to get information from. 0 is the first device.</param>
        /// <param name="info">A <see cref="BassDeviceInfo" /> object.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool GetDeviceInfo(int device, out BassDeviceInfo info)
        {
            bass_getdeviceinfo_delegate del = LibraryLoader.GetFunctionDelegate<bass_getdeviceinfo_delegate>(libraryHandle, "BASS_GetDeviceInfo");
            return del(device, out info);
        }
        /// <summary>
        /// Gets information on an playback device.
        /// </summary>
        /// <remarks>
        /// This function can be used to enumerate the available devices for a setup dialog.
        /// Device 0 is always the "no sound" device, so you should start at device 1 if you only want to list real output devices.
        /// 
        /// On Linux, a "Default" device is hardcoded to device number 1, which uses the default output set in the ALSA config,
        /// and the real devices start at number 2.
        /// That is also the case on Windows and OSX when the BASS_CONFIG_DEV_DEFAULT option is enabled.
        /// </remarks>
        /// <param name="device">The device to get information from. 0 is the first device.</param>
        /// <returns>A <see cref="BassDeviceInfo" /> object.</returns>
        /// <exception cref="BassException">The device does not exist.</exception>
        public static BassDeviceInfo GetDeviceInfo(int device)
        {
            BassDeviceInfo info;
            if (!GetDeviceInfo(device, out info)) throw new BassException(GetErrorCode());
            return info;
        }

        /// <summary>
        /// Delegate for BASS_Init.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_init_delegate(int device, int frequency, BassDeviceInitFlags flags, IntPtr win, IntPtr clsID);
        /// <summary>
        /// Initializes an output device.
        /// </summary>
        /// <param name="device">The device to use... -1 = default device, 0 = no sound, 1 = first real output device.
        /// BASS_GetDeviceInfo can be used to enumerate the available devices.</param>
        /// <param name="frequency">Output sample rate.</param>
        /// <param name="flags">A combination of <see cref="BassDeviceInitFlags"/>.</param>
        /// <param name="win">The application's main window... 0 = the desktop window (use this for console applications). This is only needed when using DirectSound output.</param>
        /// <param name="clsID">Class identifier of the object to create, that will be used to initialize DirectSound... NULL = use default.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool DeviceInit(int device = -1, int frequency = 48000, BassDeviceInitFlags flags = BassDeviceInitFlags.Default, IntPtr win = default(IntPtr), IntPtr clsID = default(IntPtr))
        {
            bass_init_delegate del = LibraryLoader.GetFunctionDelegate<bass_init_delegate>(libraryHandle, "BASS_Init");
            return del(device, frequency, flags, win, clsID);
        }

        /// <summary>
        /// Delegate for BASS_Free.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_free_delegate();
        /// <summary>
        /// Frees all resources used by the output device, including all its samples, streams and MOD musics.
        /// </summary>
        /// <remarks>
        /// This function should be called for all initialized devices before the program closes.
        /// It is not necessary to individually free the samples/streams/musics as these are all automatically freed by this function.
        /// When using multiple devices, the current thread's device setting (as set with BASS_SetDevice) determines which device this function call applies to. 
        /// </remarks>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool DeviceFree()
        {
            bass_free_delegate del = LibraryLoader.GetFunctionDelegate<bass_free_delegate>(libraryHandle, "BASS_Free");
            return del();
        }

        /// <summary>
        /// Delegate for BASS_GetInfo.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_getinfo_delegate(out BassInfo info);
        /// <summary>
        /// Gets extended information on the device being used. 
        /// </summary>
        /// <remarks>
        /// When using multiple devices, the current thread's device setting (as set with BASS_SetDevice) determines which device this function call applies to.
        /// </remarks>
        /// <returns>Information about the current device.</returns>
        /// <exception cref="BassException">Device is not initialized.</exception>
        public static BassInfo DeviceExtendedInfo
        {
            get
            {
                bass_getinfo_delegate del = LibraryLoader.GetFunctionDelegate<bass_getinfo_delegate>(libraryHandle, "BASS_GetInfo");
                BassInfo info;
                if (!del(out info)) throw new BassException(GetErrorCode());
                return info;
            }
        }

        /// <summary>
        /// Delegate for BASS_GetVolume.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate float bass_getvolume_delegate();
        /// <summary>
        /// Delegate for BASS_SetVolume.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_setvolume_delegate(float volume);
        /// <summary>
        /// Gets or sets the devices volume.
        /// </summary>
        /// <remarks>
        /// A returned -1 indicates an error. Call <see cref="GetErrorCode"/> to retrieve the error.
        /// 
        /// The volume level is in the range 0 (silent) to 1 (max).
        /// The actual volume level may not be exactly the same as requested, due to underlying precision differences.
        /// Use a get on <see cref="DeviceVolume"/> to confirm what the volume is.
        /// 
        /// This function affects the volume level of all applications using the same output device.
        /// If you wish to only affect the level of your application's sounds,
        /// the BASS_ATTRIB_VOL attribute and/or the BASS_CONFIG_GVOL_MUSIC / BASS_CONFIG_GVOL_SAMPLE / BASS_CONFIG_GVOL_STREAM config options should be used instead.
        /// 
        /// When using multiple devices, the current thread's device setting (as set with BASS_SetDevice) determines which device this function call applies to. 
        /// </remarks>
        public static float DeviceVolume
        {
            get
            {
                bass_getvolume_delegate del = LibraryLoader.GetFunctionDelegate<bass_getvolume_delegate>(libraryHandle, "BASS_GetVolume");
                return del();
            }
            set
            {
                bass_setvolume_delegate del = LibraryLoader.GetFunctionDelegate<bass_setvolume_delegate>(libraryHandle, "BASS_SetVolume");
                if (!del(value)) throw new BassException(GetErrorCode());
            }
        }

        /// <summary>
        /// Delegate for BASS_GetDevice.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate int bass_getdevice_delegate();
        /// <summary>
        /// Delegate for BASS_SetDevice.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_setdevice_delegate(int device);
        /// <summary>
        /// Gets or sets the device for the current thread.
        /// </summary>
        /// <remarks>
        /// A returned -1 indicates an error. Call <see cref="GetErrorCode"/> to retrieve the error.
        /// 
        /// Simultaneously using multiple devices is supported in the BASS API via a context switching system;
        /// instead of there being an extra "device" parameter in the function calls, the device to be used is set prior to calling the functions.
        /// The device setting is local to the current thread, so calling functions with different devices simultaneously in multiple threads is not a problem.
        /// 
        /// When one of the above functions (or BASS_GetDevice) is called, BASS will check the current thread's device setting,
        /// and if no device is selected (or the selected device is not initialized), BASS will automatically select the lowest device that is initialized.
        /// This means that when using a single device, there is no need to use this function; BASS will automatically use the device that is initialized.
        /// Even if you free the device, and initialize another, BASS will automatically switch to the one that is initialized.
        /// </remarks>
        /// <exception cref="BassException">Device is not initialized or does not exist.</exception>
        public static int CurrentDevice
        {
            get
            {
                bass_getdevice_delegate del = LibraryLoader.GetFunctionDelegate<bass_getdevice_delegate>(libraryHandle, "BASS_GetDevice");
                return del();
            }
            set
            {
                bass_setdevice_delegate del = LibraryLoader.GetFunctionDelegate<bass_setdevice_delegate>(libraryHandle, "BASS_SetDevice");
                if (!del(value)) throw new BassException(GetErrorCode());
            }
        }

        #endregion Playback

        #region Streams

        /// <summary>
        /// Delegate for BASS_StreamCreate (normal stream).
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate int bass_streamcreate_delegate(int frequency, int channels, BassStreamCreateFlags flags, BassStreamProcedure procedure, IntPtr user);
        /// <summary>
        /// Delegate for BASS_StreamCreate (push stream).
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate int bass_streamcreatepush_delegate(int frequency, int channels, BassStreamCreateFlags flags, BassStreamProcedureType type, IntPtr user);
        /// <summary>
        /// Creates a user sample stream.
        /// </summary>
        /// <remarks>
        /// Sample streams allow any sample data to be played through BASS,
        /// and are particularly useful for playing a large amount of sample data without requiring a large amount of memory.
        /// If you wish to play a sample format that BASS does not support, then you can create a stream and decode the sample data into it.
        /// </remarks>
        /// <param name="frequency">The default sample rate. The sample rate can be changed using BASS_ChannelSetAttribute.</param>
        /// <param name="channels">The number of channels... 1 = mono, 2 = stereo, 4 = quadraphonic, 6 = 5.1, 8 = 7.1.</param>
        /// <param name="flags">A combination of <see cref="BassStreamCreateFlags"/>.</param>
        /// <param name="procedure">The user defined stream writing function (see <see cref="BassStreamProcedure" />).</param>
        /// <param name="user">User instance data to pass to the callback function.</param>
        /// <returns>If successful, the new stream's handle is returned, else 0 is returned. Use <see cref="GetErrorCode"/> to get the error code.</returns>
        public static int StreamCreate(int frequency, int channels, BassStreamCreateFlags flags, BassStreamProcedure procedure, IntPtr user = default(IntPtr))
        {
            bass_streamcreate_delegate del = LibraryLoader.GetFunctionDelegate<bass_streamcreate_delegate>(libraryHandle, "BASS_StreamCreate");
            return del(frequency, channels, flags, procedure, user);
        }
        /// <summary>
        /// Creates a user sample stream, allowing for a special configuration.
        /// </summary>
        /// <remarks>
        /// Sample streams allow any sample data to be played through BASS,
        /// and are particularly useful for playing a large amount of sample data without requiring a large amount of memory.
        /// If you wish to play a sample format that BASS does not support, then you can create a stream and decode the sample data into it.
        /// 
        /// Each device has a single final output mix stream, which can be used to apply DSP/FX to the device output.
        /// Multiple requests for a final output mix stream (using STREAMPROC_DEVICE) on the same device will receive the same stream handle,
        /// which cannot be freed via BASS_StreamFree. It will automatically be freed if the device's output format (sample rate or channel count) changes.
        /// A BASS_SYNC_FREE sync can be set via BASS_ChannelSetSync to be notified when this happens, at which point a new stream with the device's new format could be created.
        /// </remarks>
        /// <param name="frequency">The default sample rate. The sample rate can be changed using BASS_ChannelSetAttribute.</param>
        /// <param name="channels">The number of channels... 1 = mono, 2 = stereo, 4 = quadraphonic, 6 = 5.1, 8 = 7.1.</param>
        /// <param name="flags">A combination of <see cref="BassStreamCreateFlags"/>.</param>
        /// <param name="type">One of the <see cref="BassStreamProcedureType"/> types.</param>
        /// <returns>If successful, the new stream's handle is returned, else 0 is returned. Use <see cref="GetErrorCode"/> to get the error code.</returns>
        public static int StreamCreate(int frequency, int channels, BassStreamCreateFlags flags, BassStreamProcedureType type)
        {
            bass_streamcreatepush_delegate del = LibraryLoader.GetFunctionDelegate<bass_streamcreatepush_delegate>(libraryHandle, "BASS_StreamCreate");
            return del(frequency, channels, flags, type, default(IntPtr));
        }

        /// <summary>
        /// Delegate for BASS_StreamFree.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_streamfree_delegate(int handle);
        /// <summary>
        /// Frees a sample stream's resources, including any SYNC/DSP/FX it has.
        /// </summary>
        /// <param name="handle">The stream handle.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool StreamFree(int handle)
        {
            bass_streamfree_delegate del = LibraryLoader.GetFunctionDelegate<bass_streamfree_delegate>(libraryHandle, "BASS_StreamFree");
            return del(handle);
        }

        /// <summary>
        /// Delegate for BASS_StreamPutData.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate int bass_streamputdata_delegate(int handle, IntPtr buffer, int length);
        /// <summary>
        /// Adds sample data to a "push" stream.
        /// </summary>
        /// <remarks>
        /// As much data as possible will be placed in the stream's playback buffer, and any remainder will be queued for when more space becomes available,
        /// ie. as the buffered data is played. With a decoding channel, there is no playback buffer, so all data is queued in that case.
        /// There is no limit to the amount of data that can be queued (besides available memory);
        /// the queue buffer will be automatically enlarged as required to hold the data, but it can also be enlarged in advance.
        /// The queue buffer is freed when the stream ends or is reset, eg. via BASS_ChannelPlay (with restart = TRUE) or BASS_ChannelSetPosition (with pos = 0).
        /// 
        /// DSP/FX are applied when the data reaches the playback buffer, or the BASS_ChannelGetData call in the case of a decoding channel.
        /// 
        /// Data should be provided at a rate sufficent to sustain playback. If the buffer gets exhausted, BASS will automatically stall playback of the stream,
        /// until more data is provided.BASS_ChannelGetData (BASS_DATA_AVAILABLE) can be used to check the buffer level,
        /// and BASS_ChannelIsActive can be used to check if playback has stalled.A BASS_SYNC_STALL sync can also be set via BASS_ChannelSetSync,
        /// to be triggered upon playback stalling or resuming. 
        /// </remarks>
        /// <param name="handle">The stream handle.</param>
        /// <param name="buffer">Pointer to the sample data... NULL = allocate space in the queue buffer so that there is at least length bytes of free space.</param>
        /// <param name="length">The amount of data in bytes, optionally using the BASS_STREAMPROC_END flag to signify the end of the stream.
        /// 0 can be used to just check how much data is queued.</param>
        /// <returns>If successful, the amount of queued data is returned, else -1 is returned. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static int StreamPutData(int handle, IntPtr buffer, int length)
        {
            bass_streamputdata_delegate del = LibraryLoader.GetFunctionDelegate<bass_streamputdata_delegate>(libraryHandle, "BASS_StreamPutData");
            return del(handle, buffer, length);
        }

        #endregion Streams

        #region Recording

        /// <summary>
        /// Delegate for BASS_RecordGetDeviceInfo.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_recordgetdeviceinfo_delegate(int device, out BassDeviceInfo info);
        /// <summary>
        /// Gets information on a recording device.
        /// </summary>
        /// <remarks>
        /// This function can be used to enumerate the available devices for a setup dialog.
        /// 
        /// Recording support requires DirectX 5 (or above) on Windows.
        /// 
        /// On Linux, a "Default" device is hardcoded to device number 0, which uses the default input set in the ALSA config.
        /// </remarks>
        /// <param name="device">The device to get information from. 0 is the first device.</param>
        /// <param name="info">A <see cref="BassDeviceInfo" /> object.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool GetRecordingDeviceInfo(int device, out BassDeviceInfo info)
        {
            bass_recordgetdeviceinfo_delegate del = LibraryLoader.GetFunctionDelegate<bass_recordgetdeviceinfo_delegate>(libraryHandle, "BASS_RecordGetDeviceInfo");
            return del(device, out info);
        }
        /// <summary>
        /// Gets information on a recording device.
        /// </summary>
        /// <remarks>
        /// This function can be used to enumerate the available devices for a setup dialog.
        /// 
        /// Recording support requires DirectX 5 (or above) on Windows.
        /// 
        /// On Linux, a "Default" device is hardcoded to device number 0, which uses the default input set in the ALSA config.
        /// </remarks>
        /// <param name="device">The device to get information from. 0 is the first device.</param>
        /// <returns>A <see cref="BassDeviceInfo" /> object.</returns>
        /// <exception cref="BassException">The device does not exist.</exception>
        public static BassDeviceInfo GetRecordingDeviceInfo(int device)
        {
            BassDeviceInfo info;
            if (!GetRecordingDeviceInfo(device, out info)) throw new BassException(GetErrorCode());
            return info;
        }

        /// <summary>
        /// Delegate for BASS_RecordInit.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_recordinit_delegate(int device);
        /// <summary>
        /// Initializes an output device.
        /// </summary>
        /// <remarks>
        /// This function must be successfully called before using the recording features.
        /// Simultaneously using multiple devices is supported in the BASS API via a context switching system; instead of there being an extra "device" parameter
        /// in the function calls, the device to be used is set prior to calling the functions.BASS_RecordSetDevice is used to switch the current recording device.
        /// When successful, BASS_RecordInit automatically sets the current thread's device to the one that was just initialized.
        /// When using the default device(device = -1), BASS_RecordGetDevice can be used to find out which device it was mapped to.
        /// 
        /// Recording support requires DirectX 5 (or above) on Windows.
        /// On Linux, a "Default" device is hardcoded to device number 0, which uses the default input set in the ALSA config;
        /// that could map directly to one of the other devices or it could use ALSA plugins.
        /// </remarks>
        /// <param name="device">The device to use... -1 = default device, 0 = first. BASS_RecordGetDeviceInfo can be used to enumerate the available devices.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool RecordingDeviceInit(int device = -1)
        {
            bass_recordinit_delegate del = LibraryLoader.GetFunctionDelegate<bass_recordinit_delegate>(libraryHandle, "BASS_RecordInit");
            return del(device);
        }

        /// <summary>
        /// Delegate for BASS_RecordFree.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_recordfree_delegate();
        /// <summary>
        /// Frees all resources used by the recording device.
        /// </summary>
        /// <remarks>
        /// This function should be called for all initialized recording devices before your program exits.
        /// 
        /// When using multiple recording devices, the current thread's device setting (as set with BASS_RecordSetDevice) determines which device this function call applies to. 
        /// </remarks>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool RecordingDeviceFree()
        {
            bass_recordfree_delegate del = LibraryLoader.GetFunctionDelegate<bass_recordfree_delegate>(libraryHandle, "BASS_RecordFree");
            return del();
        }

        /// <summary>
        /// Delegate for BASS_RecordGetInfo.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_recordgetinfo_delegate(out BassRecordInfo info);
        /// <summary>
        /// Gets extended information on the recording device being used. 
        /// </summary>
        /// <remarks>
        /// When using multiple devices, the current thread's device setting (as set with BASS_RecordSetDevice) determines which device this function call applies to.
        /// </remarks>
        /// <returns>Information about the current device.</returns>
        /// <exception cref="BassException">Recording device is not initialized.</exception>
        public static BassRecordInfo RecordingDeviceExtendedInfo
        {
            get
            {
                bass_recordgetinfo_delegate del = LibraryLoader.GetFunctionDelegate<bass_recordgetinfo_delegate>(libraryHandle, "BASS_RecordGetInfo");
                BassRecordInfo info;
                if (!del(out info)) throw new BassException(GetErrorCode());
                return info;
            }
        }

        /// <summary>
        /// Delegate for BASS_RecordGetInputName.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate IntPtr bass_recordgetinputname_delegate(int input);
        /// <summary>
        /// Gets the text description of a recording input source.
        /// </summary>
        /// <param name="input">The input to get the description of... 0 = first, -1 = master.</param>
        /// <returns>If successful, then the text description is returned; otherwise <see langword="null"/> is returned.</returns>
        public static string BASS_RecordingDeviceInputName(int input)
        {
            bass_recordgetinputname_delegate del = LibraryLoader.GetFunctionDelegate<bass_recordgetinputname_delegate>(libraryHandle, "BASS_RecordGetInputName");
            IntPtr ptr = del(input);
            // Check if an error occurred.
            if (ptr == IntPtr.Zero) return null;
            // Return string
            return PtrToStringUTF8(ptr);
        }

        /// <summary>
        /// Delegate for BASS_RecordGetInput.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate int bass_recordgetinput_delegate(int input, out float volume);
        /// <summary>
        /// Retrieves the volume of a recording input source.
        /// </summary>
        /// <param name="input">The input to enable or disable... 0 = first, -1 = master.</param>
        /// <returns>The volume of the recording devices input source or -1 if unknown.</returns>
        public static float RecordingDeviceVolume(int input)
        {
            bass_recordgetinput_delegate del = LibraryLoader.GetFunctionDelegate<bass_recordgetinput_delegate>(libraryHandle, "BASS_RecordGetInput");
            float volume;
            int ret = del(input, out volume);
            if (ret == -1) volume = -1;
            return volume;
        }

        /// <summary>
        /// Delegate for BASS_RecordSetInput.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_recordsetinput_delegate(int input, int flags, float volume);
        /// <summary>
        /// Enables or Disables a recording input source.
        /// </summary>
        /// <remarks>
        /// If the device only allows one input at a time, then any previously enabled input will be disabled by this.
        /// If the device only allows one input at a time, then its not allowed to disable the only enabled input.
        /// On OSX, there is no master input (-1).
        /// </remarks>
        /// <param name="input">The input to enable or disable... 0 = first, -1 = master.</param>
        /// <param name="enabled"><see langword="true" /> to enable the input. <see langword="false" /> to disable the input.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool RecordingDeviceChangeInput(int input, bool enabled)
        {
            bass_recordsetinput_delegate del = LibraryLoader.GetFunctionDelegate<bass_recordsetinput_delegate>(libraryHandle, "BASS_RecordSetInput");
            return del(input, enabled ? 0x20000 : 0x10000, -1);
        }

        /// <summary>
        /// Delegate for BASS_RecordStart (normal stream).
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate int bass_recordstart_delegate(int frequency, int channels, int flags, BassRecordProcedure procedure, IntPtr user);
        /// <summary>
        /// Starts recording on the device.
        /// </summary>
        /// <remarks>
        /// Use BASS_ChannelStop to stop the recording and free its resources. BASS_ChannelPause can be used to pause the recording;
        /// it can also be started in a paused state via the BASS_RECORD_PAUSE flag, which allows DSP/FX to be set on it before any data reaches the callback function.
        /// Use BASS_ChannelPlay to resume a paused recording.
        /// 
        /// The sample data will generally arrive from the recording device in blocks rather than in a continuous stream.
        /// So when specifying a very short period between callbacks, some calls may be skipped due to there being no new data available since the last call.
        /// A loopback recording device will only deliver data while the corresponding output device is active.
        /// 
        /// When not using a callback(proc = NULL), the recorded data is instead retrieved via BASS_ChannelGetData.
        /// To keep latency at a minimum, the amount of data in the recording buffer should be monitored (also done via BASS_ChannelGetData,
        /// with the BASS_DATA_AVAILABLE flag) to check that there is not too much data; freshly recorded data will only be retrieved after the older data in the buffer is.
        /// 
        /// When freq and/or chans is set to 0 for the device's current values, BASS_ChannelGetInfo can be used afterwards to find out what values are being used by the recording.
        /// 
        /// A recording may end unexpectedly if the device fails, eg. if it is disconnected/disabled.
        /// A BASS_SYNC_DEV_FAIL sync can be set via BASS_ChannelSetSync to be informed if that happens.
        /// It will not be possible to resume the recording channel then; a new recording channel will need to be created when the device becomes available again.
        /// </remarks>
        /// <param name="frequency">The sample rate to record at... 0 = device's current sample rate.</param>
        /// <param name="channels">The number of channels... 1 = mono, 2 = stereo, etc. 0 = device's current channel count.</param>
        /// <param name="flags">A combination of <see cref="BassRecordStartFlags"/>.</param>
        /// <param name="procedure">The user defined function to receive the recorded sample data... can be NULL if you do not wish to use a callback (see <see cref="BassRecordProcedure" />).</param>
        /// <param name="period">Set the period (in milliseconds) between calls to the callback function.
        /// The minimum period is 5ms, the maximum the maximum is half the BASS_CONFIG_REC_BUFFER setting.
        /// If the period specified is outside this range, it is automatically capped. The default is 100ms.</param>
        /// <returns>If successful, the new stream's recording handle is returned, else 0 is returned. Use <see cref="GetErrorCode"/> to get the error code.</returns>
        public static int RecordingDeviceStart(int frequency, int channels, BassRecordStartFlags flags, BassRecordProcedure procedure, int period = 100)
        {
            bass_recordstart_delegate del = LibraryLoader.GetFunctionDelegate<bass_recordstart_delegate>(libraryHandle, "BASS_RecordStart");
            return del(frequency, channels, (int)flags | (period << 16), procedure, default(IntPtr));
        }

        /// <summary>
        /// Delegate for BASS_RecordGetDevice.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate int bass_recordgetdevice_delegate();
        /// <summary>
        /// Delegate for BASS_RecordSetDevice.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_recordsetdevice_delegate(int device);
        /// <summary>
        /// Gets or sets the recording device for the current thread.
        /// </summary>
        /// <remarks>
        /// A returned -1 indicates an error. Call <see cref="GetErrorCode"/> to retrieve the error.
        /// 
        /// Simultaneously using multiple devices is supported in the BASS API via a context switching system; instead of there being an extra "device" parameter in the function calls,
        /// the device to be used is set prior to calling the functions. The device setting is local to the current thread,
        /// so calling functions with different devices simultaneously in multiple threads is not a problem.
        /// 
        /// The functions that use the recording device selection are the following: BASS_RecordFree, BASS_RecordGetInfo, BASS_RecordGetInput, BASS_RecordGetInputName, BASS_RecordSetInput, BASS_RecordStart.
        /// When one of the above functions(or BASS_RecordGetDevice) is called, BASS will check the current thread's recording device setting,
        /// and if no device is selected (or the selected device is not initialized), BASS will automatically select the lowest device that is initialized.
        /// This means that when using a single device, there is no need to use this function; BASS will automatically use the device that's initialized.
        /// Even if you free the device, and initialize another, BASS will automatically switch to the one that is initialized.
        /// </remarks>
        /// <exception cref="BassException">Device is not initialized or does not exist.</exception>
        public static int CurrentRecordingDevice
        {
            get
            {
                bass_recordgetdevice_delegate del = LibraryLoader.GetFunctionDelegate<bass_recordgetdevice_delegate>(libraryHandle, "BASS_RecordGetDevice");
                return del();
            }
            set
            {
                bass_recordsetdevice_delegate del = LibraryLoader.GetFunctionDelegate<bass_recordsetdevice_delegate>(libraryHandle, "BASS_RecordSetDevice");
                if (!del(value)) throw new BassException(GetErrorCode());
            }
        }

        #endregion Recording

        #region Channels

        /// <summary>
        /// Delegate for BASS_ChannelGetData.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate int bass_channelgetdata_delegate(int handle, IntPtr buffer, int length);
        /// <summary>
        /// Gets the number of bytes buffered for the particular channel.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns>If successful, the number of bytes in the channel's buffer is returned, else -1 is returned. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static int ChannelGetBufferedDataCount(int handle)
        {
            bass_channelgetdata_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelgetdata_delegate>(libraryHandle, "BASS_ChannelGetData");
            return del(handle, IntPtr.Zero, 0);
        }

        /// <summary>
        /// Delegate for BASS_ChannelGetAttribute.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_channelgetattribute_delegate(int handle, BassChannelAttribute attribute, out float value);
        /// <summary>
        /// Gets the value of a channel attribute.
        /// </summary>
        /// <param name="handle">The channel handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD.</param>
        /// <param name="attribute">The channel attribute to get the value from.</param>
        /// <param name="value">The attribute value.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool ChannelGetAttribute(int handle, BassChannelAttribute attribute, out float value)
        {
            bass_channelgetattribute_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelgetattribute_delegate>(libraryHandle, "BASS_ChannelGetAttribute");
            return del(handle, attribute, out value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle">The channel handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD.</param>
        /// <param name="attribute">The channel attribute to get the value from.</param>
        /// <returns>The attribute value.</returns>
        /// <exception cref="BassException">Either handle is invalid, or attribute is not available/existing.</exception>
        public static float ChannelGetAttribute(int handle, BassChannelAttribute attribute)
        {
            float value;
            if (!ChannelGetAttribute(handle, attribute, out value)) throw new BassException(GetErrorCode());
            return value;
        }

        /// <summary>
        /// Delegate for BASS_ChannelGetDevice.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate int bass_channelgetdevice_delegate(int handle);
        /// <summary>
        /// Gets the device that a channel is using.
        /// </summary>
        /// <param name="handle">The channel handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD. HSAMPLE handles may also be used.</param>
        /// <returns>If successful, the device number is returned, else -1 is returned. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static int ChannelGetDevice(int handle)
        {
            bass_channelgetdevice_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelgetdevice_delegate>(libraryHandle, "BASS_ChannelGetDevice");
            return del(handle);
        }

        /// <summary>
        /// Delegate for BASS_ChannelGetLevelEx.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_channelgetlevelex_delegate(int handle, [In, Out] float[] levels, float length, BassChannelLevelFlags flags);
        /// <summary>
        /// Retrieves the level of a sample, stream, MOD music, or recording channel.
        /// </summary>
        /// <remarks>
        /// The levels are not clipped, so may exceed +/-1.0 on floating-point channels.
        /// </remarks>
        /// <param name="handle">The channel handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD.</param>
        /// <param name="channels"></param>
        /// <param name="length">The amount of data to inspect to calculate the level, in seconds. The maximum is 1 second.
        /// Less data than requested may be used if the full amount is not available, eg. if the channel's playback buffer is shorter.</param>
        /// <param name="flags">What levels to retrieve.</param>
        /// <returns>Array of levels on success, else <see langword="null" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static float[] ChannelGetLevel(int handle, int channels, float length, BassChannelLevelFlags flags)
        {
            if (channels < 1) return null;
            float[] levels = new float[channels];
            bass_channelgetlevelex_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelgetlevelex_delegate>(libraryHandle, "BASS_ChannelGetLevelEx");
            return del(handle, levels, length, flags) ? levels : null;
        }

        /// <summary>
        /// Delegate for BASS_ChannelGetPosition.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate long bass_channelgetposition_delegate(int handle, int mode);
        /// <summary>
        /// Retrieves the playback position of a sample, stream, or MOD music. Can also be used with a recording channel.
        /// </summary>
        /// <param name="handle">The channel handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD.</param>
        /// <returns>If successful, then the channel's position is returned, else -1 is returned. Use <see cref="GetErrorCode"/> to get the error code.</returns>
        public static long ChannelGetPosition(int handle)
        {
            bass_channelgetposition_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelgetposition_delegate>(libraryHandle, "BASS_ChannelGetPosition");
            return del(handle, 0) / 4;
        }

        /// <summary>
        /// Delegate for BASS_ChannelIsActive.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate BassChannelState bass_channelisactive_delegate(int handle);
        /// <summary>
        /// Returns the state of the channel.
        /// </summary>
        /// <param name="handle">The channel handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD.</param>
        /// <returns>The state of the channel.</returns>
        public static BassChannelState ChannelState(int handle)
        {
            bass_channelisactive_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelisactive_delegate>(libraryHandle, "BASS_ChannelIsActive");
            BassChannelState state = del(handle);
            if (GetErrorCode() != BassErrorCode.Success) state = BassChannelState.Invalid;
            return state;
        }

        /// <summary>
        /// Delegate for BASS_ChannelIsSliding.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_channelissliding_delegate(int handle, BassChannelAttribute attribute);
        /// <summary>
        /// Checks if an attribute (or any attribute) of a sample, stream, or MOD music is sliding.
        /// </summary>
        /// <param name="handle">The channel handle... a HCHANNEL, HSTREAM or HMUSIC.</param>
        /// <param name="attribute">The attribute to check for sliding.</param>
        /// <returns><see langword="true" /> if the attribute is sliding; otherwise <see langword="false" />.</returns>
        public static bool ChannelIsSliding(int handle, BassChannelAttribute attribute)
        {
            bass_channelissliding_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelissliding_delegate>(libraryHandle, "BASS_ChannelIsSliding");
            return del(handle, attribute);
        }

        /// <summary>
        /// Delegate for BASS_ChannelPause.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_channelpause_delegate(int handle);
        /// <summary>
        /// Pauses a sample, stream, MOD music, or recording.
        /// </summary>
        /// <remarks>
        /// Use BASS_ChannelPlay to resume a paused channel. BASS_ChannelStop can be used to stop a paused channel.
        /// </remarks>
        /// <param name="handle">The channel handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool ChannelPause(int handle)
        {
            bass_channelpause_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelpause_delegate>(libraryHandle, "BASS_ChannelPause");
            return del(handle);
        }

        /// <summary>
        /// Delegate for BASS_ChannelStop.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_channelstop_delegate(int handle);
        /// <summary>
        /// Stops a sample, stream, MOD music, or recording.
        /// </summary>
        /// <remarks>
        /// Stopping a user stream (created with BASS_StreamCreate) will clear its buffer contents, and stopping a sample channel (HCHANNEL) will result in it being freed.
        /// Use BASS_ChannelPause instead if you wish to stop a user stream or sample and then resume it from the same point.
        /// </remarks>
        /// <param name="handle">The channel handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool ChannelStop(int handle)
        {
            bass_channelstop_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelstop_delegate>(libraryHandle, "BASS_ChannelStop");
            return del(handle);
        }

        /// <summary>
        /// Delegate for BASS_ChannelPlay.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_channelplay_delegate(int handle, bool restart);
        /// <summary>
        /// Starts (or resumes) playback of a sample, stream, MOD music, or recording.
        /// </summary>
        /// <remarks>
        /// When streaming in blocks (BASS_STREAM_BLOCK), the restart parameter is ignored as it is not possible to go back to the start.
        /// The restart parameter is also of no consequence with recording channels.
        /// 
        /// If other channels have been linked to the specified channel via BASS_ChannelSetLink,
        /// this function will attempt to simultaneously start playing them too but if any fail, it will be silently.
        /// The return value and error code only reflects what happened with the specified channel.
        /// BASS_ChannelIsActive can be used to confirm the status of linked channels. 
        /// </remarks>
        /// <param name="handle">The channel handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD.</param>
        /// <param name="restart">Restart playback from the beginning? If handle is a user stream (created with BASS_StreamCreate), its current buffer contents are cleared.
        /// If it is a MOD music, its BPM/etc are reset to their initial values.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool ChannelPlay(int handle, bool restart = false)
        {
            bass_channelplay_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelplay_delegate>(libraryHandle, "BASS_ChannelPlay");
            return del(handle, restart);
        }

        /// <summary>
        /// Delegate for BASS_ChannelRemoveDSP.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_channelremovedsp_delegate(int channelHandle, int dspHandle);
        /// <summary>
        /// Removes a DSP function from a stream, MOD music, or recording channel.
        /// </summary>
        /// <param name="channelHandle">The channel handle... a HSTREAM, HMUSIC, or HRECORD.</param>
        /// <param name="dspHandle">Handle of the DSP function to remove from the channel. This can also be an HFX handle to remove an effect.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool ChannelRemoveDSP(int channelHandle, int dspHandle)
        {
            bass_channelremovedsp_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelremovedsp_delegate>(libraryHandle, "BASS_ChannelRemoveDSP");
            return del(channelHandle, dspHandle);
        }

        /// <summary>
        /// Delegate for BASS_ChannelSeconds2Bytes.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate long bass_channelseconds2bytes_delegate(int handle, double position);
        /// <summary>
        /// Translates a time (seconds) position into bytes, based on a channel's format.
        /// </summary>
        /// <remarks>
        /// The translation is based on the channel's initial sample rate, when it was created.
        /// The return value is rounded down to the position of the nearest sample.
        /// </remarks>
        /// <param name="handle">The channel handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD. HSAMPLE handles may also be used.</param>
        /// <param name="position">The position to translate.</param>
        /// <returns>If successful, then the translated length is returned, else -1 is returned. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static long ChannelSeconds2Bytes(int handle, double position)
        {
            bass_channelseconds2bytes_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelseconds2bytes_delegate>(libraryHandle, "BASS_ChannelSeconds2Bytes");
            return del(handle, position);
        }

        /// <summary>
        /// Delegate for BASS_ChannelSetAttribute.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_channelsetattribute_delegate(int handle, BassChannelAttribute attribute, float value);
        /// <summary>
        /// Sets the value of a channel attribute.
        /// </summary>
        /// <remarks>
        /// The actual attribute value may not be exactly the same as requested, due to precision differences.
        /// For example, an attribute might only allow whole number values. BASS_ChannelGetAttribute can be used to confirm what the value is.
        /// </remarks>
        /// <param name="handle">The channel handle... a HCHANNEL, HMUSIC, HSTREAM, or HRECORD.</param>
        /// <param name="attribute">The channel attribute to set the value to.</param>
        /// <param name="value">The new attribute value.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool ChannelSetAttribute(int handle, BassChannelAttribute attribute, float value)
        {
            bass_channelsetattribute_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelsetattribute_delegate>(libraryHandle, "BASS_ChannelSetAttribute");
            return del(handle, attribute, value);
        }

        /// <summary>
        /// Delegate for BASS_ChannelSetDevice.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_channelsetdevice_delegate(int handle, int device);
        /// <summary>
        /// Sets the device that a stream, MOD music or sample is using.
        /// </summary>
        /// <remarks>
        /// All of the channel's current settings are carried over to the new device, but if the channel is using the "with FX flag" DX8 effect implementation,
        /// the internal state (eg. buffers) of the DX8 effects will be reset. When using the "without FX flag" DX8 effect implementation, the state of the DX8 effects is preserved.
        /// 
        /// When changing a sample's device, all the sample's existing channels(HCHANNELs) are freed.It is not possible to change the device of an individual sample channel.
        /// 
        /// The BASS_NODEVICE option can be used to disassociate a decoding channel from a device, so that it does not get freed when BASS_Free is called.
        /// </remarks>
        /// <param name="handle">The channel or sample handle... a HMUSIC, HSTREAM or HSAMPLE.</param>
        /// <param name="device">The device to use... 0 = no sound, 1 = first real output device, BASS_NODEVICE (0x20000) = no device.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool ChannelSetDevice(int handle, int device)
        {
            bass_channelsetdevice_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelsetdevice_delegate>(libraryHandle, "BASS_ChannelSetDevice");
            return del(handle, device);
        }

        /// <summary>
        /// Delegate for BASS_ChannelSetDSP.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate int bass_channelsetdsp_delegate(int handle, BassDSPProcedure procedure, IntPtr user, int priority);
        /// <summary>
        /// Sets up a user DSP function on a stream, MOD music, or recording channel.
        /// </summary>
        /// <remarks>
        /// DSP functions can set and removed at any time, including mid-playback. Use BASS_ChannelRemoveDSP to remove a DSP function.
        /// 
        /// Multiple DSP functions may be used per channel, in which case the order that the functions are called is determined by their priorities.
        /// The priorities can be changed via BASS_FXSetPriority. Any DSPs that have the same priority are called in the order that they were given that priority.
        /// 
        /// DSP functions can be applied to MOD musics and streams, but not samples.If you want to apply a DSP function to a sample then you should stream it instead.
        /// </remarks>
        /// <param name="handle">The channel handle... a HSTREAM, HMUSIC, or HRECORD.</param>
        /// <param name="procedure">The callback function.</param>
        /// <param name="priority">The priority of the new DSP, which determines its position in the DSP chain. DSPs with higher priority are called before those with lower.</param>
        /// <returns>If successful, then the new DSP's handle is returned, else 0 is returned. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static int ChannelSetDSP(int handle, BassDSPProcedure procedure, int priority = 0)
        {
            bass_channelsetdsp_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelsetdsp_delegate>(libraryHandle, "BASS_ChannelSetDSP");
            return del(handle, procedure, default(IntPtr), priority);
        }

        /// <summary>
        /// Delegate for BASS_ChannelSetPosition.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_channelsetposition_delegate(int handle, long position, int mode);
        /// <summary>
        /// Sets the playback position of a sample, MOD music, or stream.
        /// </summary>
        /// <remarks>
        /// User streams (created with BASS_StreamCreate) are not seekable, but it is possible to reset a user stream (including its buffer contents) by setting its position to byte 0.
        /// </remarks>
        /// <param name="handle">The channel handle... a HCHANNEL, HSTREAM or HMUSIC.</param>
        /// <param name="position">The new sample position in the stream.</param>
        /// <param name="relative">Indicator whether the position is relative or absolute.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool ChannelSetPosition(int handle, long position, bool relative = false)
        {
            bass_channelsetposition_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelsetposition_delegate>(libraryHandle, "BASS_ChannelSetPosition");
            if (relative)
                return del(handle, position * 4, 0x4000000);
            else
                return del(handle, position * 4, 0);
        }

        /// <summary>
        /// Delegate for BASS_ChannelSetSync.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate int bass_channelsetsync_delegate(int handle, int type, long param, BassSyncProcedure procedure, IntPtr user);
        /// <summary>
        /// Sets up a synchronizer on a MOD music, stream or recording channel.
        /// </summary>
        /// <param name="handle">The channel handle... a HMUSIC, HSTREAM or HRECORD.</param>
        /// <param name="type">The type of sync.</param>
        /// <param name="param">The sync parameter. Depends on the sync type.</param>
        /// <param name="procedure">The callback function.</param>
        /// <returns>If successful, then the new synchronizer's handle is returned, else 0 is returned. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static int ChannelSetSync(int handle, int type, long param, BassSyncProcedure procedure)
        {
            bass_channelsetsync_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelsetsync_delegate>(libraryHandle, "BASS_ChannelSetSync");
            return del(handle, type, param, procedure, default(IntPtr));
        }
        /// <summary>
        /// Sets up a synchronizer on a MOD music, stream or recording channel to get notified when the channel's device failed.
        /// </summary>
        /// <param name="handle">The channel handle... a HMUSIC, HSTREAM or HRECORD.</param>
        /// <param name="procedure">The callback function.</param>
        /// <returns>If successful, then the new synchronizer's handle is returned, else 0 is returned. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static int ChannelSetSyncDeviceFail(int handle, BassSyncProcedure procedure)
        {
            bass_channelsetsync_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelsetsync_delegate>(libraryHandle, "BASS_ChannelSetSync");
            return del(handle, 14, 0, procedure, default(IntPtr));
        }

        /// <summary>
        /// Delegate for BASS_ChannelSlideAttribute.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_channelslideattribute_delegate(int handle, int attribute, float value, int time);
        /// <summary>
        /// Slides a channel's attribute from its current value to a new value.
        /// </summary>
        /// <remarks>
        /// This function is similar to BASS_ChannelSetAttribute, except that the attribute is ramped to the value over the specified period of time.
        /// Another difference is that the value is not pre-checked. If it is invalid, the slide will simply end early.
        /// 
        /// If an attribute is already sliding, then the old slide is stopped and replaced by the new one.
        /// 
        /// BASS_ChannelIsSliding can be used to check if an attribute is currently sliding.A BASS_SYNC_SLIDE sync can also be set via BASS_ChannelSetSync,
        /// to be triggered at the end of a slide.The sync will not be triggered in the case of an existing slide being replaced by a new one.
        /// 
        /// Attribute slides are unaffected by whether the channel is playing, paused or stopped; they carry on regardless.
        /// </remarks>
        /// <param name="handle">The channel handle... a HCHANNEL, HSTREAM, HMUSIC, or HRECORD.</param>
        /// <param name="attribute">The attribute to slide the value of.</param>
        /// <param name="value">The new attribute value. See the attribute's documentation for details on the possible values.</param>
        /// <param name="time">The length of time (in milliseconds) that it should take for the attribute to reach the value.</param>
        /// <param name="logarithmic">An indicator whether to slide logarithmically.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool ChannelSlideAttribute(int handle, BassChannelAttribute attribute, float value, int time, bool logarithmic = false)
        {
            bass_channelslideattribute_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelslideattribute_delegate>(libraryHandle, "BASS_ChannelSlideAttribute");
            int attr = (int)attribute;
            if (logarithmic) attr |= 0x1000000;
            return del(handle, attr, value, time);
        }

        /// <summary>
        /// Delegate for BASS_ChannelUpdate.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        private delegate bool bass_channelupdate_delegate(int handle, int length);
        /// <summary>
        /// Updates the playback buffer of a stream or MOD music.
        /// </summary>
        /// <remarks>
        /// When starting playback of a stream or MOD music, after creating it or changing its position, there will be a slight delay while the initial data is generated for playback.
        /// Usually the delay is not noticeable or important, but if you need playback to start instantly when you call BASS_ChannelPlay, then use this function first.
        /// The length parameter should be at least equal to the update period.
        /// 
        /// It may not always be possible to render the requested amount of data, in which case this function will still succeed.
        /// BASS_ChannelGetData(BASS_DATA_AVAILABLE) can be used to check how much data a channel has buffered for playback.
        /// 
        /// When automatic updating is disabled(BASS_CONFIG_UPDATEPERIOD = 0 or BASS_CONFIG_UPDATETHREADS = 0),
        /// this function could be used instead of BASS_Update to implement different update periods for different channels,
        /// instead of a single update period for all.Unlike BASS_Update, this function can also be used while automatic updating is enabled.
        /// </remarks>
        /// <param name="handle">The channel handle... a HMUSIC or HSTREAM.</param>
        /// <param name="length">The amount of data to render, in milliseconds... 0 = default (2 x update period). This is capped at the space available in the buffer.</param>
        /// <returns><see langword="true" /> if successful; otherwise <see langword="false" />. Use <see cref="GetErrorCode" /> to get the error code.</returns>
        public static bool ChannelUpdate(int handle, int length)
        {
            bass_channelupdate_delegate del = LibraryLoader.GetFunctionDelegate<bass_channelupdate_delegate>(libraryHandle, "BASS_ChannelUpdate");
            return del(handle, length);
        }

        #endregion Channels

        /// <summary>
        /// Returns a UTF-8 string from a pointer to a UTF-8 string.
        /// </summary>
        /// <param name="Ptr">The pointer to the UTF-8 string.</param>
        /// <returns>A <see cref="String"/>.</returns>
        public unsafe static string PtrToStringUTF8(IntPtr Ptr)
        {
            // Get pointer to byte array
            byte* bytes = (byte*)Ptr.ToPointer();
            // Checks
            if (Ptr == IntPtr.Zero) return null;
            if (bytes[0] == 0) return String.Empty;
            // Get size of the array
            int size = 0;
            while (bytes[size] != 0) size++;
            // Copy data to a managed array
            byte[] buffer = new byte[size];
            Marshal.Copy(Ptr, buffer, 0, size);
            // Return UTF-8 string
            return System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }
    }
}
