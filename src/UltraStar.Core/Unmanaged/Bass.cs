#region License
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

namespace UltraStar.Core.Unmanaged
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
        }

        /// <summary>
        /// Delegate for BASS_GetVersion.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate int bass_getversion_delegate();

        /// <summary>
        /// Retrieves the version of BASS that is loaded.
        /// </summary>
        /// <returns>The BASS version.</returns>
        public static Version GetVersion()
        {
            bass_getversion_delegate del = LibraryLoader.GetFunctionDelegate<bass_getversion_delegate>(libraryHandle, "BASS_GetVersion");
            int number = del();
            return new Version((number >> 24) & 0xFF, (number >> 16) & 0xFF, (number >> 8) & 0xFF, number & 0xFF);
        }

    }
}
