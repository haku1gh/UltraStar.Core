#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.IO;
using System.Runtime.InteropServices;
using UltraStar.Core.Utils;

namespace UltraStar.Core.Unmanaged
{
    /// <summary>
    /// This static class provides native library loading features from the underlying platforms.
    /// </summary>
    /// <remarks>
    /// Supported are Windows, Linux and Mac.
    /// </remarks>
    internal static class LibraryLoader
    {
        /// <summary>
        /// This is the P/Invoke method that wraps the native "LoadLibrary" function.
        /// </summary>
        /// <remarks>
        /// See the Microsoft documentation for full details on what it does.
        /// This method works for Windows.
        /// </remarks>
        [DllImport("kernel32", EntryPoint = "LoadLibrary", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto,
            BestFitMapping = false, ThrowOnUnmappableChar = true, SetLastError = true)]
        static extern IntPtr LoadLibraryWin32(string fileName);

        /// <summary>
        /// This is the P/Invoke method that wraps the native "GetProcAddress" function.
        /// </summary>
        /// <remarks>
        /// See the Microsoft documentation for full details on what it does.
        /// This method works for Windows.
        /// </remarks>
        [DllImport("kernel32", EntryPoint = "GetProcAddress", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi, BestFitMapping = false)]
        static extern IntPtr GetFunctionPointerWin32(IntPtr hModule, string lpProcName);

        /// <summary>
        /// This is the P/Invoke method that wraps the native "dlopen" function.
        /// </summary>
        /// <remarks>
        /// See the POSIX documentation for full details on what it does.
        /// This method works for Linux and MacOS.
        /// </remarks>
        [DllImport("libdl", EntryPoint = "dlopen", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            BestFitMapping = false, ThrowOnUnmappableChar = true, SetLastError = true)]
        static extern IntPtr LoadLibraryPosix(string fileName, int flag);

        /// <summary>
        /// This is the P/Invoke method that wraps the native "dlsym" function.
        /// </summary>
        /// <remarks>
        /// See the POSIX documentation for full details on what it does.
        /// This method works for Linux and MacOS.
        /// </remarks>
        [DllImport("libdl", EntryPoint = "dlsym", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi,
            BestFitMapping = false, ThrowOnUnmappableChar = true, SetLastError = true)]
        static extern IntPtr GetFunctionPointerPosix(IntPtr handle, string symbol);

        /// <summary>
        /// For use with dlopen(), bind function calls lazily.
        /// </summary>
        const int DLOPEN_RTLD_LAZY = 0x1;

        /// <summary>
        /// For use with dlopen(), bind function calls immediately.
        /// </summary>
        const int DLOPEN_RTLD_NOW = 0x2;

        /// <summary>
        /// For use with dlopen(), make symbols globally available.
        /// </summary>
        const int DLOPEN_RTLD_GLOBAL = 0x100;

        /// <summary>
        /// For use with dlopen(), opposite of RTLD_GLOBAL, and the default.
        /// </summary>
        const int DLOPEN_RTLD_LOCAL = 0x000;

        /// <summary>
        /// Loads a library using native method calls.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>The native handle to the library upon success; otherwise IntPtr.Zero on failure.</returns>
        /// <exception cref="PlatformNotSupportedException">The current platform is not supported.</exception>
        public static IntPtr LoadNativeLibrary(string fileName)
        {
            switch (SystemInformation.Platform)
            {
                case Platform.Mac:
                case Platform.Linux:
                    return LoadLibraryPosix(fileName, DLOPEN_RTLD_NOW);
                case Platform.Windows:
                    return LoadLibraryWin32(fileName);
                default:
                    // All other windows versions are not supported
                    throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        /// Loads a library using native method calls.
        /// </summary>
        /// <param name="rootPath">The root path to the libraries.</param>
        /// <param name="fullLibraryName">The full library name for the native platform.</param>
        /// <returns></returns>
        /// <remarks>
        /// The final path will be constructed as: path + platform + architecture + fullLibraryName.
        /// </remarks>
        /// <exception cref="PlatformNotSupportedException">The current platform is not supported.</exception>
        public static IntPtr LoadNativeLibrary(string rootPath, string fullLibraryName)
        {
            string platform = SystemInformation.Platform.ToString().ToLower();
            string architecture = SystemInformation.Architecture.ToString().ToLower();
            string fileName;
            if (UsConfig.LibrarySubFoldersExisting)
                fileName = Path.Combine(rootPath, platform, architecture, fullLibraryName);
            else
                fileName = Path.Combine(rootPath, fullLibraryName);
            return LoadNativeLibrary(fileName);
        }

        /// <summary>
        /// Gets the function delegate from the function name.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the delegate to return. This must match the function names delegate.</typeparam>
        /// <param name="nativeLibraryHandle">The handle to the native library.</param>
        /// <param name="functionName">The name of the function. The name is case-sensitive.</param>
        /// <returns>A instance of the specified delegate type.</returns>
        /// <exception cref="ArgumentException">The <typeparamref name="TDelegate"/> generic parameter is not a delegate, or it is an open generic type.</exception>
        /// <exception cref="PlatformNotSupportedException">The current platform is not supported.</exception>
        /// <exception cref="EntryPointNotFoundException">The entry point for <paramref name="functionName"/> could not be found.</exception>
        public static TDelegate GetFunctionDelegate<TDelegate>(IntPtr nativeLibraryHandle, string functionName)
        {
            // Get a function pointer to the function name
            IntPtr ptr = IntPtr.Zero;
            switch (SystemInformation.Platform)
            {
                case Platform.Mac:
                case Platform.Linux:
                    ptr = GetFunctionPointerPosix(nativeLibraryHandle, functionName);
                    break;
                case Platform.Windows:
                    ptr = GetFunctionPointerWin32(nativeLibraryHandle, functionName);
                    break;
                default:
                    // All other windows versions are not supported
                    throw new PlatformNotSupportedException();
            }

            // Check if entry point had be found
            if (ptr == IntPtr.Zero)
            {
                throw new EntryPointNotFoundException("Could not find the entrypoint for " + functionName + ".");
            }

            // Return delegate from function pointer
            return Marshal.GetDelegateForFunctionPointer<TDelegate>(ptr);
        }
    }
}
