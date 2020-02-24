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
        /// Gets information on the device being used. 
        /// </summary>
        /// <remarks>
        /// When using multiple devices, the current thread's device setting (as set with BASS_SetDevice) determines which device this function call applies to.
        /// </remarks>
        /// <returns>Information about the current device.</returns>
        /// <exception cref="BassException">Device is not initialized.</exception>
        public static BassInfo GetDeviceInfoUsed()
        {
            bass_getinfo_delegate del = LibraryLoader.GetFunctionDelegate<bass_getinfo_delegate>(libraryHandle, "BASS_GetInfo");
            BassInfo info;
            if (!del(out info)) throw new BassException(GetErrorCode());
            return info;
        }

        #endregion Playback

        #region Recording
        #endregion Recording

        #region Channels
        #endregion Channels
    }
}
