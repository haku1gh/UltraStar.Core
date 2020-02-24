#region License (MIT)
/*
 * This file is part of UltraStar.Core.
 * 
 * This file is heavily based on the implementation from ManagedBass by Mathew Sachin.
 * ManagedBass is available under the MIT license. For details see <https://github.com/ManagedBass/Home>.
 * In contrast to other files, this file is available under the same license (MIT) as the original.
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
    /// <para>
    /// Depending on the BASS_CONFIG_UNICODE setting, <see cref="Name"/> and <see cref="Driver"/> can be in ANSI or UTF-8 form on Windows.
    /// They are always in UTF-16 form on Windows CE, and UTF-8 on other platforms.
    /// For the sake of simplicity this struct always assumes an UTF-8 form.
    /// </para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct BassDeviceInfo
    {
        IntPtr name;
        IntPtr driver;
        BassDeviceInfoFlags flags;

        /// <summary>
        /// Returns a UTF-8 string from a pointer to a UTF-8 string.
        /// </summary>
        /// <param name="Ptr">The pointer to the UTF-8 string.</param>
        /// <returns>A <see cref="String"/>.</returns>
        public unsafe static string ptrToStringUTF8(IntPtr Ptr)
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
            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// The description of the device.
        /// </summary>
        public string Name => ptrToStringUTF8(name);

        /// <summary>
        /// The ID of the driver.
        /// </summary>
        public string Driver => ptrToStringUTF8(driver);

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
