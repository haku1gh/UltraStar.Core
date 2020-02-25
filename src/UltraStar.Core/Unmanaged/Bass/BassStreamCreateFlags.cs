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

namespace UltraStar.Core.Unmanaged.Bass
{
    /// <summary>
    /// Flags for creating an output stream.
    /// </summary>
    [Flags]
    public enum BassStreamCreateFlags : uint
    {
        /// <summary>
        /// 0 = default create stream: 16 Bit, stereo, no Float, hardware mixing, no Loop, no 3D, no speaker assignments...
        /// </summary>
        Default,

        /// <summary>
        /// Use 8-bit resolution. If neither this or the <see cref="Float"/> flags are specified, then the stream is 16-bit.
        /// </summary>
        Byte = 0x1,

        /// <summary>
        /// Use 3D functionality.
        /// This requires that the BASS_DEVICE_3D flag was specified when calling BASS_Init,
        /// and the stream must be mono (chans=1). The SPEAKER flags cannot be used together with this flag.
        /// </summary>
        Bass3D = 0x8,

        /// <summary>
        /// Force the stream to not use hardware DirectSound mixing. (Windows Only)
        /// </summary>
        SoftwareMixing = 0x10,

        /// <summary>
        /// Enable the old implementation of DirectX 8 effects. See the DX8 effect implementations section for details.
        /// Use BASS_ChannelSetFX to add effects to the stream.
        /// </summary>
        FX = 0x80,

        /// <summary>
        /// Use 32-bit floating-point sample data (see Floating-Point Channels for details).
        /// </summary>
        Float = 0x100,

        /// <summary>
        /// Automatically free the music or stream's resources when it has reached the end,
        /// or when BASS_ChannelStop or BASS_Stop is called.
        /// This flag can be toggled at any time using BASS_ChannelFlags.
        /// </summary>
        AutoFree = 0x40000,

        /// <summary>
        /// Decode the sample data, without playing it. Use BASS_ChannelGetData to retrieve decoded sample data.
        /// The BASS_SAMPLE_3D, BASS_STREAM_AUTOFREE and SPEAKER flags cannot be used together with this flag.
        /// The BASS_SAMPLE_SOFTWARE and BASS_SAMPLE_FX flags are also ignored.
        /// </summary>
        Decode = 0x200000,

        #region Speaker Assignment
        /// <summary>
        /// Front speakers (channel 1/2)
        /// </summary>
        SpeakerFront = 0x1000000,

        /// <summary>
        /// Rear/Side speakers (channel 3/4)
        /// </summary>
        SpeakerRear = 0x2000000,

        /// <summary>
        /// Center and LFE speakers (5.1, channel 5/6)
        /// </summary>
        SpeakerCenterLFE = 0x3000000,

        /// <summary>
        /// Rear Center speakers (7.1, channel 7/8)
        /// </summary>
        SpeakerRearCenter = 0x4000000,

        #region Pairs
        /// <summary>
        /// Speakers Pair 1
        /// </summary>
        SpeakerPair1 = 1 << 24,

        /// <summary>
        /// Speakers Pair 2
        /// </summary>
        SpeakerPair2 = 2 << 24,

        /// <summary>
        /// Speakers Pair 3
        /// </summary>
        SpeakerPair3 = 3 << 24,

        /// <summary>
        /// Speakers Pair 4
        /// </summary>
        SpeakerPair4 = 4 << 24,

        /// <summary>
        /// Speakers Pair 5
        /// </summary>
        SpeakerPair5 = 5 << 24,

        /// <summary>
        /// Speakers Pair 6
        /// </summary>
        SpeakerPair6 = 6 << 24,

        /// <summary>
        /// Speakers Pair 7
        /// </summary>
        SpeakerPair7 = 7 << 24,

        /// <summary>
        /// Speakers Pair 8
        /// </summary>
        SpeakerPair8 = 8 << 24,

        /// <summary>
        /// Speakers Pair 9
        /// </summary>
        SpeakerPair9 = 9 << 24,

        /// <summary>
        /// Speakers Pair 10
        /// </summary>
        SpeakerPair10 = 10 << 24,

        /// <summary>
        /// Speakers Pair 11
        /// </summary>
        SpeakerPair11 = 11 << 24,

        /// <summary>
        /// Speakers Pair 12
        /// </summary>
        SpeakerPair12 = 12 << 24,

        /// <summary>
        /// Speakers Pair 13
        /// </summary>
        SpeakerPair13 = 13 << 24,

        /// <summary>
        /// Speakers Pair 14
        /// </summary>
        SpeakerPair14 = 14 << 24,

        /// <summary>
        /// Speakers Pair 15
        /// </summary>
        SpeakerPair15 = 15 << 24,
        #endregion

        #region Modifiers
        /// <summary>
        /// Speaker Modifier: left channel only
        /// </summary>
        SpeakerLeft = 0x10000000,

        /// <summary>
        /// Speaker Modifier: right channel only
        /// </summary>
        SpeakerRight = 0x20000000,
        #endregion

        /// <summary>
        /// Front Left speaker only (channel 1)
        /// </summary>
        SpeakerFrontLeft = SpeakerFront | SpeakerLeft,

        /// <summary>
        /// Rear/Side Left speaker only (channel 3)
        /// </summary>
        SpeakerRearLeft = SpeakerRear | SpeakerLeft,

        /// <summary>
        /// Center speaker only (5.1, channel 5)
        /// </summary>
        SpeakerCenter = SpeakerCenterLFE | SpeakerLeft,

        /// <summary>
        /// Rear Center Left speaker only (7.1, channel 7)
        /// </summary>
        SpeakerRearCenterLeft = SpeakerRearCenter | SpeakerLeft,

        /// <summary>
        /// Front Right speaker only (channel 2)
        /// </summary>
        SpeakerFrontRight = SpeakerFront | SpeakerRight,

        /// <summary>
        /// Rear/Side Right speaker only (channel 4)
        /// </summary>
        SpeakerRearRight = SpeakerRear | SpeakerRight,

        /// <summary>
        /// LFE speaker only (5.1, channel 6)
        /// </summary>
        SpeakerLFE = SpeakerCenterLFE | SpeakerRight,

        /// <summary>
        /// Rear Center Right speaker only (7.1, channel 8)
        /// </summary>
        SpeakerRearCenterRight = SpeakerRearCenter | SpeakerRight,
        #endregion
    }
}
