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

namespace UltraStar.Core.Unmanaged.Bass
{
    /// <summary>
    /// Channel attribute options for BASS channels.
    /// </summary>
    internal enum BassChannelAttribute
    {
        /// <summary>
        /// The sample rate of a channel... 0 = original rate (when the channel was created).
        /// <para>
        /// This attribute applies to playback of the channel, and does not affect the
        /// channel's sample data, so has no real effect on decoding channels.
        /// It is still adjustable though, so that it can be used by the BassMix add-on,
        /// and anything else that wants to use it.
        /// </para>
        /// <para>
        /// It is not possible to change the sample rate of a channel if the "with FX
        /// flag" DX8 effect implementation enabled on it, unless DirectX 9 or above is installed.
        /// </para>
        /// <para>
        /// Increasing the sample rate of a stream or MOD music increases its CPU usage,
        /// and reduces the Length of its playback Buffer in terms of time.
        /// If you intend to raise the sample rate above the original rate, then you may also need
        /// to increase the Buffer Length via the BASS_PlaybackBufferLength
        /// config option to avoid break-ups in the sound.
        /// </para>
        ///
        /// <para><b>Platform-specific</b></para>
        /// <para>On Windows, the sample rate will get rounded down to a whole number during playback.</para>
        /// </summary>
        Frequency = 0x1,

        /// <summary>
        /// The volume level of a channel... 0 (silent) to 1 (full).
        /// <para>This can go above 1.0 on decoding channels.</para>
        /// <para>
        /// This attribute applies to playback of the channel, and does not affect the
        /// channel's sample data, so has no real effect on decoding channels.
        /// It is still adjustable though, so that it can be used by the BassMix add-on,
        /// and anything else that wants to use it.
        /// </para>
        /// <para>
        /// When using BASS_ChannelSlideAttribute
        /// to slide this attribute, a negative volume value can be used to fade-out and then stop the channel.
        /// </para>
        /// </summary>
        Volume = 0x2,

        /// <summary>
        /// The panning/balance position of a channel... -1 (Full Left) to +1 (Full Right), 0 = Centre.
        /// <para>
        /// This attribute applies to playback of the channel, and does not affect the
        /// channel's sample data, so has no real effect on decoding channels.
        /// It is still adjustable though, so that it can be used by the BASSmix add-on,
        /// and anything else that wants to use it.
        /// </para>
        /// <para>
        /// It is not possible to set the pan position of a 3D channel.
        /// It is also not possible to set the pan position when using speaker assignment, but if needed,
        /// it can be done via a <see cref="BassDSPProcedure"/> instead (not on mono channels).
        /// </para>
        ///
        /// <para><b>Platform-specific</b></para>
        /// <para>
        /// On Windows, this attribute has no effect when speaker assignment is used,
        /// except on Windows Vista and newer with the Bass.VistaSpeakerAssignment config option enabled.
        /// Balance control could be implemented via a <see cref="BassDSPProcedure"/> instead
        /// </para>
        /// </summary>
        Pan = 0x3,

        /// <summary>
        /// The wet (reverb) / dry (no reverb) mix ratio... 0 (full dry) to 1 (full wet), -1 = automatically calculate the mix based on the distance (the default).
        /// <para>For a sample, stream, or MOD music channel with 3D functionality.</para>
        /// <para>
        /// Obviously, EAX functions have no effect if the output device does not support EAX.
        /// BASS_GetInfo can be used to check that.
        /// </para>
        /// <para>
        /// EAX only affects 3D channels, but EAX functions do not require BASS_Apply3D to apply the changes.
        /// LastError.NoEAX: The channel does not have EAX support.
        /// EAX only applies to 3D channels that are mixed by the hardware/drivers.
        /// BASS_ChannelGetInfo can be used to check if a channel is being mixed by the hardware.
        /// EAX is only supported on Windows.
        /// </para>
        /// </summary>
        EaxMix = 0x4,

        /// <summary>
        /// Non-Windows: Disable playback buffering?... 0 = no, else yes..
        /// <para>
        /// A playing channel is normally asked to render data to its playback Buffer in advance,
        /// via automatic Buffer updates or the BASS_Update and BASS_ChannelUpdate functions,
        /// ready for mixing with other channels to produce the final mix that is given to the output device.
        /// </para>
        /// <para>
        /// When this attribute is switched on (the default is off), that buffering is skipped and
        /// the channel will only be asked to produce data as it is needed during the generation of the final mix.
        /// This allows the lowest latency to be achieved, but also imposes tighter timing requirements
        /// on the channel to produce its data and apply any DSP/FX (and run mixtime syncs) that are set on it;
        /// if too long is taken, there will be a break in the output, affecting all channels that are playing on the same device.
        /// </para>
        /// <para>
        /// The channel's data is still placed in its playback Buffer when this attribute is on,
        /// which allows BASS_ChannelGetData and BASS_ChannelGetLevel to be used, although there is
        /// likely to be less data available to them due to the Buffer being less full.
        /// </para>
        /// <para>This attribute can be changed mid-playback.</para>
        /// <para>If switched on, any already buffered data will still be played, so that there is no break in sound.</para>
        /// <para>This attribute is not available on Windows, as BASS does not generate the final mix.</para>
        /// </summary>
        NoBuffer = 0x5,

        /// <summary>
        /// The CPU usage of a channel. (in percentage).
        /// <para>
        /// This attribute gives the percentage of CPU that the channel is using,
        /// including the time taken by decoding and DSP processing, and any FX that are
        /// not using the "with FX flag" DX8 effect implementation.
        /// It does not include the time taken to add the channel's data to the final output mix during playback.
        /// The processing of some add-on stream formats may also not be entirely included,
        /// if they use additional decoding threads; see the add-on documentation for details.
        /// </para>
        /// <para>
        /// Like BASS_CPUUsage, this function does not strictly tell the CPU usage, but rather how timely the processing is.
        /// For example, if it takes 10ms to generate 100ms of data, that would be 10%.
        /// </para>
        /// <para>
        /// If the reported usage exceeds 100%, that means the channel's data is taking longer to generate than to play.
        /// The duration of the data is based on the channel's current sample rate (<see cref="BassChannelAttribute.Frequency"/>).
        /// A channel's CPU usage is updated whenever it generates data.
        /// That could be during a playback Buffer update cycle, or a BASS_Update call, or a BASS_ChannelUpdate call.
        /// For a decoding channel, it would be in a BASS_ChannelGetData or BASS_ChannelGetLevel call.
        /// </para>
        /// <para>This attribute is read-only, so cannot be modified via BASS_ChannelSetAttribute.</para>
        /// </summary>
        CPUUsage = 0x7,

        /// <summary>
        /// The sample rate conversion quality of a channel
        /// <para>
        /// 0 = linear interpolation, 1 = 8 point sinc interpolation, 2 = 16 point sinc interpolation, 3 = 32 point sinc interpolation.
        /// Other values are also accepted but will be interpreted as 0 or 3, depending on whether they are lower or higher.
        /// </para>
        /// <para>
        /// When a channel has a different sample rate to what the output device is using,
        /// the channel's sample data will need to be converted to match the output device's rate during playback.
        /// This attribute determines how that is done.
        /// The linear interpolation option uses less CPU, but the sinc interpolation gives better sound quality (less aliasing),
        /// with the quality and CPU usage increasing with the number of points.
        /// A good compromise for lower spec systems could be to use sinc interpolation for music playback and linear interpolation for sound effects.
        /// </para>
        /// <para>
        /// Whenever possible, a channel's sample rate should match the output device's rate to avoid the need for any sample rate conversion.
        /// The device's sample rate could be used in BASS_CreateStream or BASS_MusicLoad or MIDI stream creation calls, for example.
        /// </para>
        /// <para>
        /// The sample rate conversion occurs (when required) during playback,
        /// after the sample data has left the channel's playback Buffer, so it does not affect the data delivered by BASS_ChannelGetData.
        /// Although this attribute has no direct effect on decoding channels,
        /// it is still available so that it can be used by BASSmix add-on and anything else that wants to use it.
        /// </para>
        /// <para>
        /// This attribute can be set at any time, and changes take immediate effect.
        /// A channel's initial setting is determined by the BASS_SRCQuality config option,
        /// or BASS_SampleSRCQuality in the case of a sample channel.
        /// </para>
        /// <para><b>Platform-specific</b></para>
        /// <para>On Windows, sample rate conversion is handled by Windows or the output device/driver rather than BASS, so this setting has no effect on playback there.</para>
        /// </summary>
        SampleRateConversion = 0x8,

        /// <summary>
        /// The download Buffer level required to resume stalled playback in percent... 0 - 100 (the default is 50%).
        /// <para>
        /// This attribute determines what percentage of the download Buffer (BASS_NetBufferLength)
        /// needs to be filled before playback of a stalled internet stream will resume.
        /// It also applies to 'buffered' User file streams created with BASS_CreateStream.
        /// </para>
        /// </summary>
        NetworkResumeBufferLevel = 0x9,

        /// <summary>
        /// The scanned info of a channel.
        /// </summary>
        ScannedInfo = 0xa,

        /// <summary>
        /// Disable playback ramping? 
        /// </summary>
        NoRamp = 0xB,

        /// <summary>
        /// The average bitrate of a file stream. 
        /// </summary>
        Bitrate = 0xC,

        /// <summary>
        /// Playback buffering length.
        /// </summary>
        Buffer = 0xD,

        /// <summary>
        /// The processing granularity of a channel.
        /// </summary>
        /// <remarks>
        /// This attribute allows a channel's processing to be in units of a certain size, which can be helpful for some DSP processing.
        /// It does not apply to decoding channels or recording channels without a RECORDPROC; their procesing is controlled by BASS_ChannelGetData.
        /// 
        /// Each processing cycle will be on a whole number of units, not necessarily only one unit,
        /// and the number of units can vary between cycles depending on how much space there is in the channel's playback buffer (or captured data in a recording buffer).
        /// There might not be a whole number of units at the end of a file or when a stream stalls.
        /// 
        /// When granularity is enabled (non-0), it can change the timing of any DSP/FX changes that are made in mixtime sync callbacks.
        /// That is because the DSP/FX processing is done on the entire block of data at the end of the processing cycle then (rather than splitting it at the sync positions),
        /// to maintain the specified granularity, resulting in the changes effectively being applied at the start of the processing cycle.
        /// 
        /// The default value is 0 (none). Changes take immediate effect. 
        /// </remarks>
        Granule = 0xE,

        #region MOD Music
        /// <summary>
        /// The amplification level of a MOD music... 0 (min) to 100 (max).
        /// <para>This will be rounded down to a whole number.</para>
        /// <para>
        /// As the amplification level get's higher, the sample data's range increases, and therefore, the resolution increases.
        /// But if the level is set too high, then clipping can occur, which can result in distortion of the sound.
        /// You can check the current level of a MOD music at any time by BASS_ChannelGetLevel.
        /// By doing so, you can decide if a MOD music's amplification level needs adjusting.
        /// The default amplification level is 50.
        /// </para>
        /// <para>
        /// During playback, the effect of changes to this attribute are not heard instantaneously, due to buffering.
        /// To reduce the delay, use the BASS_PlaybackBufferLength config option to reduce the Buffer Length.
        /// </para>
        /// </summary>
        MusicAmplify = 0x100,

        /// <summary>
        /// The pan separation level of a MOD music... 0 (min) to 100 (max), 50 = linear.
        /// <para>
        /// This will be rounded down to a whole number.
        /// By default BASS uses a linear panning "curve".
        /// If you want to use the panning of FT2, use a pan separation setting of around 35.
        /// To use the Amiga panning (ie. full left and right) set it to 100.
        /// </para>
        /// </summary>
        MusicPanSeparation = 0x101,

        /// <summary>
        /// The position scaler of a MOD music... 1 (min) to 256 (max).
        /// <para>
        /// This will be rounded down to a whole number.
        /// When calling BASS_ChannelGetPosition, the row (HIWORD) will be scaled by this value.
        /// By using a higher scaler, you can get a more precise position indication.
        /// The default scaler is 1.
        /// </para>
        /// </summary>
        MusicPositionScaler = 0x102,

        /// <summary>
        /// The BPM of a MOD music... 1 (min) to 255 (max).
        /// <para>
        /// This will be rounded down to a whole number.
        /// This attribute is a direct mapping of the MOD's BPM, so the value can be changed via effects in the MOD itself.
        /// Note that by changing this attribute, you are changing the playback Length.
        /// During playback, the effect of changes to this attribute are not heard instantaneously, due to buffering.
        /// To reduce the delay, use the BASS_PlaybackBufferLength config option to reduce the Buffer Length.
        /// </para>
        /// </summary>
        MusicBPM = 0x103,

        /// <summary>
        /// The speed of a MOD music... 0 (min) to 255 (max).
        /// <para>
        /// This will be rounded down to a whole number.
        /// This attribute is a direct mapping of the MOD's speed, so the value can be changed via effects in the MOD itself.
        /// The "speed" is the number of ticks per row.
        /// Setting it to 0, stops and ends the music.
        /// Note that by changing this attribute, you are changing the playback Length.
        /// During playback, the effect of changes to this attribute are not heard instantaneously, due to buffering.
        /// To reduce the delay, use the BASS_PlaybackBufferLength config option to reduce the Buffer Length.
        /// </para>
        /// </summary>
        MusicSpeed = 0x104,

        /// <summary>
        /// The global volume level of a MOD music... 0 (min) to 64 (max, 128 for IT format).
        /// <para>
        /// This will be rounded down to a whole number.
        /// This attribute is a direct mapping of the MOD's global volume, so the value can be changed via effects in the MOD itself.
        /// The "speed" is the number of ticks per row.
        /// Setting it to 0, stops and ends the music.
        /// Note that by changing this attribute, you are changing the playback Length.
        /// During playback, the effect of changes to this attribute are not heard instantaneously, due to buffering.
        /// To reduce the delay, use the BASS_PlaybackBufferLength config option to reduce the Buffer Length.
        /// </para>
        /// </summary>
        MusicVolumeGlobal = 0x105,

        /// <summary>
        /// The number of active channels in a MOD music.
        /// <para>
        /// This attribute gives the number of channels (including virtual) that are currently active in the decoder,
        /// which may not match what is being heard during playback due to buffering.
        /// To reduce the time difference, use the BASS_PlaybackBufferLength config option to reduce the Buffer Length.
        /// This attribute is read-only, so cannot be modified via BASS_ChannelSetAttribute.
        /// </para>
        /// </summary>
        MusicActiveChannelCount = 0x106,

        /// <summary>
        /// The volume level... 0 (silent) to 1 (full) of a channel in a MOD music + channel#.
        /// <para>channel: The channel to set the volume of... 0 = 1st channel.</para>
        /// <para>
        /// The volume curve used by this attribute is always linear, eg. 0.5 = 50%.
        /// The BASS_LogarithmicVolumeCurve config option setting has no effect on this.
        /// The volume level of all channels is initially 1 (full).
        /// This attribute can also be used to count the number of channels in a MOD Music.
        /// During playback, the effect of changes to this attribute are not heard instantaneously, due to buffering.
        /// To reduce the delay, use the BASS_PlaybackBufferLength config option to reduce the Buffer Length.
        /// </para>
        /// </summary>
        MusicVolumeChannel = 0x200,

        /// <summary>
        /// The volume level... 0 (silent) to 1 (full) of an instrument in a MOD music + inst#.
        /// <para>inst: The instrument to set the volume of... 0 = 1st instrument.</para>
        /// <para>
        /// The volume curve used by this attribute is always linear, eg. 0.5 = 50%.
        /// The BASS_LogarithmicVolumeCurve config option setting has no effect on this.
        /// The volume level of all instruments is initially 1 (full).
        /// For MOD formats that do not use instruments, read "sample" for "instrument".
        /// This attribute can also be used to count the number of instruments in a MOD music.
        /// During playback, the effect of changes to this attribute are not heard instantaneously, due to buffering.
        /// To reduce the delay, use the BASS_PlaybackBufferLength config option to reduce the Buffer Length.
        /// </para>
        /// </summary>
        MusicVolumeInstrument = 0x300,
        #endregion
    }
}
