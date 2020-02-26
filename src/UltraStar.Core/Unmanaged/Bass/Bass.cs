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
    /// This class is a wrapper around the BASS library.
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
            libraryHandle = LibraryLoader.LoadNativeLibrary(UsConfig.LibraryRootPath, fullLibraryName);
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
        }

        #region Common

        /// <summary>
        /// Delegate for BASS_GetVersion.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        public static bool DeviceInit(int device = -1, int frequency = 44100, BassDeviceInitFlags flags = BassDeviceInitFlags.Default, IntPtr win = default(IntPtr), IntPtr clsID = default(IntPtr))
        {
            bass_init_delegate del = LibraryLoader.GetFunctionDelegate<bass_init_delegate>(libraryHandle, "BASS_Init");
            return del(device, frequency, flags, win, clsID);
        }

        /// <summary>
        /// Delegate for BASS_Free.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate float bass_getvolume_delegate();
        /// <summary>
        /// Delegate for BASS_SetVolume.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate int bass_getdevice_delegate();
        /// <summary>
        /// Delegate for BASS_SetDevice.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate int bass_streamcreate_delegate(int frequency, int channels, BassStreamCreateFlags flags, BassStreamProcedure procedure, IntPtr user);
        /// <summary>
        /// Delegate for BASS_StreamCreate (push stream).
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        /// Delegate for BASS_RecordSetInput.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate int bass_recordstart_delegate(int frequency, int channels, BassRecordStartFlags flags, BassRecordProcedure procedure, IntPtr user);
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
        /// <param name="user">User instance data to pass to the callback function.</param>
        /// <returns>If successful, the new stream's recording handle is returned, else 0 is returned. Use <see cref="GetErrorCode"/> to get the error code.</returns>
        public static int RecordingDeviceStart(int frequency, int channels, BassRecordStartFlags flags, BassRecordProcedure procedure, IntPtr user = default(IntPtr))
        {
            bass_recordstart_delegate del = LibraryLoader.GetFunctionDelegate<bass_recordstart_delegate>(libraryHandle, "BASS_RecordStart");
            return del(frequency, channels, flags, procedure, user);
        }

        /// <summary>
        /// Delegate for BASS_RecordGetDevice.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate int bass_recordgetdevice_delegate();
        /// <summary>
        /// Delegate for BASS_RecordSetDevice.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
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

        // ChannelGetAttribute
        // ChannelGetData
        // ChannelGetDevice
        // ChannelGetLevelEx
        // ChannelGetPosition
        // ChannelIsActive
        // ChannelIsSliding
        // ChannelPause
        // ChannelPlay
        // ChannelRemoveDSP
        // ChannelRemoveSync
        // ChannelSeconds2Bytes
        // ChannelSetAttribute
        // ChannelSetDevice
        // ChannelSetDSP
        // ChannelSetPosition
        // ChannelSetSync
        // ChannelSlideAttribute
        // ChannelUpdate

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
