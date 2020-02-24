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

        private static Version minSupportedVersion = new Version(2, 4, 15, 0);
        private static Version maxSupportedVersion = new Version(2, 4, 255, 255);

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
            bass_init_delegate del = LibraryLoader.GetFunctionDelegate<bass_init_delegate>(libraryHandle, "BASS_GetInfo");
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
                if(!del(value)) throw new BassException(GetErrorCode());
            }
        }

        #endregion Playback

        #region Streams

        // StreamCreate
        // StreamFree
        // StreamPutData

        #endregion Streams

        #region Recording

        // GetRecordingDevice
        // RecordingDeviceInfo
        // RecordingInit
        // RecordingFree
        // RecordingExtendedInfo
        // RecordingInput
        // RecordingInputName
        // RecordingStart
        // CurrentRecordingDevice

        #endregion Recording

        #region Channels
        #endregion Channels
    }
}
