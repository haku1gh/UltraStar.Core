#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 * 
 * The file is based on the implementation from ManagedBass by Mathew Sachin.
 * ManagedBass is available under the MIT license. For details see <https://github.com/ManagedBass/Home>.
 */
#endregion License

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace UltraStar.Core.Unmanaged.Bass
{
    /// <summary>
    /// Device information from a device.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When a device is disabled/disconnected, it is still retained in the device list, but the IsEnabled is set to <see langword="false" /> flag is removed from it.
    /// If the device is subsequently re-enabled, it may become available again with the same device number, or the system may add a new entry for it.
    /// </para>
    /// <para>
    /// When a new device is connected, it can affect the other devices and result in the system moving them to new device entries.
    /// If an affected device is initialized, it will stop working and will need to be reinitialized using its new device number.
    /// </para>
    /// <para>
    /// Depending on the BASS_CONFIG_UNICODE setting, <see cref="Name"/> and <see cref="Driver"/> can be in ANSI or UTF-8 form on Windows.
    /// They are always in UTF-16 form on Windows CE, and UTF-8 on other platforms.
    /// For the sake of simplicity this struct always assumes an UTF-8 form.
    /// </para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct BassDeviceInfo
    {
        private readonly IntPtr name;
        private readonly IntPtr driver;
        private readonly BassDeviceInfoFlags flags;

        /// <summary>
        /// The description of the device.
        /// </summary>
        public string Name => Bass.PtrToStringUTF8(name);

        /// <summary>
        /// The ID of the driver.
        /// </summary>
        public string Driver => Bass.PtrToStringUTF8(driver);

        /// <summary>
        /// The device is the system default device.
        /// </summary>
        public bool IsDefault => flags.HasFlag(BassDeviceInfoFlags.Default);

        /// <summary>
        /// The device is enabled and can be used.
        /// </summary>
        public bool IsEnabled => flags.HasFlag(BassDeviceInfoFlags.Enabled);

        /// <summary>
        /// The device is already initialized.
        /// </summary>
        public bool IsInitialized => flags.HasFlag(BassDeviceInfoFlags.Initialized);

        /// <summary>
        /// The device is a Loopback device.
        /// </summary>
        public bool IsLoopback => flags.HasFlag(BassDeviceInfoFlags.Loopback);

        /// <summary>
        /// The device's Type.
        /// </summary>
        public BassDeviceType Type => (BassDeviceType)(flags & BassDeviceInfoFlags.TypeMask);
    }
}
