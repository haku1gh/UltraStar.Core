#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;

namespace UltraStar.Core.Utils
{
    /// <summary>
    /// This class provides system information.
    /// </summary>
    internal static class SystemInformation
    {
        /// <summary>
        /// Initializes <see cref="SystemInformation"/>.
        /// </summary>
        static SystemInformation()
        {
            getPlatform();
            getArchitecture();
        }

        /// <summary>
        /// Gets the current platform.
        /// </summary>
        private static void getPlatform()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX:
                    Platform = Platform.Mac;
                    break;
                case PlatformID.Unix:
                    Platform = Platform.Linux;
                    break;
                case PlatformID.Win32NT:
                    Platform = Platform.Windows;
                    break;
                default:
                    Platform = Platform.Unsupported;
                    break;
            }
        }

        /// <summary>
        /// Gets the platform on which this library is running.
        /// </summary>
        public static Platform Platform { get; private set; }

        /// <summary>
        /// Gets the current architecture.
        /// </summary>
        private static void getArchitecture()
        {
            switch (System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture)
            {
                case System.Runtime.InteropServices.Architecture.X86:
                    Architecture = Architecture.X86;
                    break;
                case System.Runtime.InteropServices.Architecture.X64:
                    Architecture = Architecture.X64;
                    break;
                case System.Runtime.InteropServices.Architecture.Arm:
                    Architecture = Architecture.Arm;
                    break;
                case System.Runtime.InteropServices.Architecture.Arm64:
                    Architecture = Architecture.Arm64;
                    break;
                default:
                    Architecture = Architecture.Unsupported;
                    break;
            }
        }

        /// <summary>
        /// Gets the architecture on which this library is running.
        /// </summary>
        public static Architecture Architecture { get; private set; }
    }
}
