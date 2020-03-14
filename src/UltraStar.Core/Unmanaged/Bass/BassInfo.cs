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
using System.Runtime.InteropServices;

namespace UltraStar.Core.Unmanaged.Bass
{
    /// <summary>
    /// Information about the current device.
    /// </summary>
    /// <remarks>
    /// <para>
    /// On Windows, it is possible for speakers to mistakenly be 2 with some devices/drivers when the device in fact supports more speakers.
    /// In that case, the <see cref="BassDeviceInitFlags.CPSpeakers"/> flag can be used (with BASS_Init) to use the Windows control panel setting,
    /// or the <see cref="BassDeviceInitFlags.ForcedSpeakerAssignment"/> flag can be used to force the enabling of speaker assignment to up to 8 speakers,
    /// even though the device may not really support that many speakers.
    /// The result of assigning channels to nonexistent speakers is undefined; they may be heard on other speakers or not heard at all.
    /// </para>
    /// <para>On Windows, the availability of the latency and minbuf values depends on the <see cref="BassDeviceInitFlags.Latency"/> flag being used when BASS_Init was called.</para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct BassInfo
    {
        private readonly int flags;   // Only useful for DirectSound in Windows, therefore not interested
        private readonly int hwsize;  // Only useful for DirectSound in Windows, therefore not interested
        private readonly int hwfree;  // Only useful for DirectSound in Windows, therefore not interested
        private readonly int freesam; // Only useful for DirectSound in Windows, therefore not interested
        private readonly int free3d;  // Only useful for DirectSound in Windows, therefore not interested
        private readonly int minrate; // Only useful for DirectSound in Windows, therefore not interested
        private readonly int maxrate; // Only useful for DirectSound in Windows, therefore not interested
        private readonly bool eax;    // Only useful for DirectSound in Windows, therefore not interested
        private readonly int minbuf;
        private readonly int dsver;   // Only useful for DirectSound in Windows, therefore not interested
        private readonly int latency;
        private readonly BassDeviceInitFlags initFlags;
        private readonly int speakers;
        private readonly int freq;

        /// <summary>The minimum buffer length (rounded up to the nearest millisecond)
        /// recommended for use (with the BASS_CONFIG_BUFFER config option).
        /// </summary>
        public int MinimumBufferLength => minbuf;

        /// <summary>The average delay (rounded up to the nearest millisecond) for playback of HSTREAM/HMUSIC channels to start and be heard.
        /// <para>Requires that <see cref="BassDeviceInitFlags.Latency"/> was used when <see cref="Bass.DeviceInit" /> was called.</para>
        /// </summary>
        public int Latency => latency;

        /// <summary>
        /// The flags parameter of the <see cref="Bass.DeviceInit" /> call (<see cref="BassDeviceInitFlags" />).
        /// </summary>
        public BassDeviceInitFlags InitFlags => initFlags;

        /// <summary>The number of speakers the device/drivers supports... 2 means that there is no support for speaker assignment - this will always be the case with non-WDM drivers in Windows.
        /// <para>It's also possible that it could mistakenly be 2 with some devices/drivers, when the device in fact supports more speakers.</para>
        /// <para>In that case the <see cref="BassDeviceInitFlags.ForcedSpeakerAssignment"/> or <see cref="BassDeviceInitFlags.CPSpeakers"/> flag
        /// can be used in the <see cref="Bass.DeviceInit" /> call to force the enabling of speaker assignment.</para>
        /// </summary>
        public int SpeakerCount => speakers;

        /// <summary>
        /// The device's current output sample rate. This is only available on Windows Vista and OSX.
        /// </summary>
        public int Samplerate => freq;
    }
}
