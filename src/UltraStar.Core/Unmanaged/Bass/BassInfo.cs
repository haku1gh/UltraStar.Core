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
using System.Runtime.InteropServices;

namespace UltraStar.Core.Unmanaged.Bass
{
    /// <summary>
    /// Information about the current device.
    /// </summary>
    /// <remarks>
    /// The DSCAPS_SECONDARY flags only indicate which sample formats are supported by hardware mixing.
    /// <para><b>Platform-specific</b></para>
    /// <para>
    /// On Windows, it is possible for speakers to mistakenly be 2 with some devices/drivers when the device in fact supports more speakers.
    /// In that case, the <see cref="BassDeviceInitFlags.CPSpeakers"/> flag can be used (with BASS_Init) to use the Windows control panel setting,
    /// or the <see cref="BassDeviceInitFlags.ForcedSpeakerAssignment"/> flag can be used to force the enabling of speaker assignment to up to 8 speakers,
    /// even though the device may not really support that many speakers.
    /// The result of assigning channels to nonexistent speakers is undefined;
    /// they may be heard on other speakers or not heard at all.
    /// </para>
    /// <para>
    /// The flags, hwsize, hwfree, freesam, free3d, minrate, maxrate, eax, and dsver members are only used on Windows, as DirectSound and hardware mixing are only available there.
    /// The freq member is not available on Windows prior to Vista.
    /// </para>
    /// <para>On Windows, the availability of the latency and minbuf values depends on the <see cref="BassDeviceInitFlags.Latency"/> flag being used when BASS_Init was called.</para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct BassInfo
    {
        private readonly BassInfoFlags flags;
        private readonly int hwsize;
        private readonly int hwfree;
        private readonly int freesam;
        private readonly int free3d;
        private readonly int minrate;
        private readonly int maxrate;
        private readonly bool eax;
        private readonly int minbuf;
        private readonly int dsver;
        private readonly int latency;
        private readonly BassDeviceInitFlags initFlags;
        private readonly int speakers;
        private readonly int freq;

        /// <summary>
        /// The device's total amount of hardware memory.
        /// </summary>
        public int TotalHardwareMemory => hwsize;

        /// <summary>
        /// The device's amount of free hardware memory.
        /// </summary>
        public int FreeHardwareMemory => hwsize;

        /// <summary>
        /// The number of free sample slots in the hardware.
        /// </summary>
        public int FreeSampleSlots => freesam;

        /// <summary>
        /// The number of free 3D sample slots in the hardware.
        /// </summary>
        public int Free3DSampleSlots => free3d;

        /// <summary>
        /// The minimum sample rate supported by the hardware.
        /// </summary>
        public int MinSampleRate => minrate;

        /// <summary>
        /// The maximum sample rate supported by the hardware.
        /// </summary>
        public int MaxSampleRate => maxrate;

        /// <summary>The device supports EAX and has it enabled?
        /// The device's "Hardware acceleration" needs to be set to "Full" in its "Advanced Properties" setup, else EAX is disabled.
        /// This is always FALSE if BASS_DEVICE_3D was not specified when BASS_Init was called.
        /// </summary>
        public bool EAXEnabled => eax;

        /// <summary>The minimum buffer length (rounded up to the nearest millisecond)
        /// recommended for use (with the BASS_CONFIG_BUFFER config option).
        /// </summary>
        public int MinBufferLength => minbuf;

        /// <summary>
        /// DirectSound version.
        /// <para>
        /// 9 = DX9/8/7/5 features are available,
        /// 8 = DX8/7/5 features are available,
        /// 7 = DX7/5 features are available,
        /// 5 = DX5 features are available.
        /// 0 = none of the DX9/8/7/5 features are available.
        /// </para>
        /// </summary>
        public int DSVersion => dsver;

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
        public int SampleRate => freq;

        /// <summary>
        /// The device driver has been certified by Microsoft. Always true for WDM drivers.
        /// </summary>
        public bool IsCertified => flags.HasFlag(BassInfoFlags.Certified);

        /// <summary>
        /// 16-bit samples are supported by hardware mixing.
        /// </summary>
        public bool Supports16BitSamples => flags.HasFlag(BassInfoFlags.Secondary16Bit);

        /// <summary>
        /// 8-bit samples are supported by hardware mixing.
        /// </summary>
        public bool Supports8BitSamples => flags.HasFlag(BassInfoFlags.Secondary8Bit);

        /// <summary>
        /// The device supports all sample rates between minrate and maxrate.
        /// </summary>
        public bool SupportsContinuousRate => flags.HasFlag(BassInfoFlags.ContinuousRate);

        /// <summary>
        /// The device's drivers has DirectSound support
        /// </summary>
        public bool SupportsDirectSound => !flags.HasFlag(BassInfoFlags.EmulatedDrivers);

        /// <summary>
        /// Mono samples are supported by hardware mixing.
        /// </summary>
        public bool SupportsMonoSamples => flags.HasFlag(BassInfoFlags.Mono);

        /// <summary>
        /// Stereo samples are supported by hardware mixing.
        /// </summary>
        public bool SupportsStereoSamples => flags.HasFlag(BassInfoFlags.Stereo);
    }
}
