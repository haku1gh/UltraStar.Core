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
    /// BASS configuration options.
    /// </summary>
    internal enum BassConfigurationOption
    {
        /// <summary>
        /// The playback buffer length for HSTREAM and HMUSIC channels.
        /// </summary>
        /// <remarks>
        /// The buffer length in milliseconds. The minimum length is 10ms, the maximum is 5000 milliseconds. If the length specified is outside this range, it is automatically capped.
        /// 
        /// The default buffer length is 500 milliseconds. Increasing the length, decreases the chance of the sound possibly breaking-up on slower computers,
        /// but also increases the latency for DSP/FX. The buffer length should always be greater than the update period (BASS_CONFIG_UPDATEPERIOD),
        /// which determines how often the buffer is refilled.
        /// Small buffer lengths are only required if the sound is going to be changing in real-time, for example, in a soft-synth.
        /// If you need to use a small buffer, then the minbuf member of BASS_INFO should be used to get the recommended minimum buffer length
        /// supported by the device and its drivers, and add that to the update period plus some margin for the stream's processing. Even then,
        /// it is still possible that the sound could break up on some systems, it is also possible that smaller buffers may be fine.
        /// So when using small buffers, you should have an option in your software for the user to finetune the length used, for optimal performance.
        /// 
        /// Using this config option only affects the HMUSIC/HSTREAM channels that are created afterwards, not any that have already been created.
        /// So you can have channels with differing buffer lengths by using this config option each time before creating them.
        /// A channel's buffer length can be also reduced (or bypassed entirely) at any time via the BASS_ATTRIB_BUFFER attribute.
        /// If automatic updating is disabled, make sure you call BASS_Update frequently enough to keep the buffers updated. 
        /// </remarks>
        PlaybackBufferLength = 0,

        /// <summary>
        /// The update period of HSTREAM and HMUSIC channel playback buffers.
        /// </summary>
        /// <remarks>
        /// 0 = disable automatic updating. The minimum period is 5ms, the maximum is 100ms. If the period specified is outside this range, it is automatically capped.
        /// 
        /// The update period is the amount of time between updates of the playback buffers of HSTREAM/HMUSIC channels that are playing;
        /// no update cycles occur when nothing is playing. Shorter update periods allow smaller buffers to be set with the BASS_CONFIG_BUFFER config option,
        /// but as the rate of updates increases, so the overhead of setting up the updates becomes a greater part of the CPU usage.
        /// The update period only affects HSTREAM and HMUSIC channels; it does not affect samples. Nor does it have any effect on decoding channels, as they are not played.
        /// 
        /// BASS creates one or more threads (determined by BASS_CONFIG_UPDATETHREADS) specifically to perform the updating,
        /// except when automatic updating is disabled (period = 0), in which case BASS_Update or BASS_ChannelUpdate should be used instead.
        /// This allows BASS's CPU usage to be synchronized with your software's. For example, in a game loop you could call BASS_Update once per frame,
        /// to keep all the processing in sync so that the frame rate is as smooth as possible.
        /// 
        /// The update period can be altered at any time, including during playback. The default period is 100ms. 
        /// </remarks>
        UpdatePeriod = 1,

        /// <summary>
        /// The global sample volume level.
        /// </summary>
        /// <remarks>
        /// 0 (silent) to 10000 (full).
        /// This config option allows you to have control over the volume levels of all the samples,
        /// which may be useful for setup options, eg. separate music and effect volume controls.
        /// A channel's final volume = channel volume x global volume / 10000. For example, if a stream's volume is 0.5 and
        /// the global stream volume is 8000, then effectively the stream's volume level is 0.4 (0.5 x 8000 / 10000 = 0.4).
        /// </remarks>
        GlobalSampleVolume = 4,

        /// <summary>
        /// The global stream volume level.
        /// </summary>
        /// <remarks>
        /// 0 (silent) to 10000 (full).
        /// This config option allows you to have control over the volume levels of all the streams,
        /// which may be useful for setup options, eg. separate music and effect volume controls.
        /// A channel's final volume = channel volume x global volume / 10000. For example, if a stream's volume is 0.5 and
        /// the global stream volume is 8000, then effectively the stream's volume level is 0.4 (0.5 x 8000 / 10000 = 0.4).
        /// </remarks>
        GlobalStreamVolume = 5,

        /// <summary>
        /// The global MOD music volume level.
        /// </summary>
        /// <remarks>
        /// 0 (silent) to 10000 (full).
        /// This config option allows you to have control over the volume levels of all the MOD musics,
        /// which may be useful for setup options, eg. separate music and effect volume controls.
        /// A channel's final volume = channel volume x global volume / 10000. For example, if a stream's volume is 0.5 and
        /// the global stream volume is 8000, then effectively the stream's volume level is 0.4 (0.5 x 8000 / 10000 = 0.4).
        /// </remarks>
        GlobalMusicVolume = 6,

        /// <summary>
        /// The translation curve of volume values.
        /// </summary>
        /// <remarks>
        /// Volume curve... FALSE = linear, TRUE = logarithmic.
        /// When using the linear curve, the volume range is from 0% (silent) to 100% (full).
        /// When using the logarithmic curve, the volume range is from -100 dB (effectively silent) to 0 dB (full).
        /// For example, a volume level of 0.5 is 50% linear or -50 dB logarithmic.
        /// The linear curve is used by default.
        /// </remarks>
        LogarithmicVolumeCurve = 7,

        /// <summary>
        /// The translation curve of panning values.
        /// </summary>
        /// <remarks>
        /// Panning curve... FALSE = linear, TRUE = logarithmic.
        /// When using the linear curve, the panning range is from 0% to 100% (full). When using the logarithmic curve, the panning range is from -100 dB to 0 dB (full).
        /// For example, a panning value of 0.5 is 50% linear or -50 dB logarithmic.
        /// The linear curve is used by default.
        /// </remarks>
        LogarithmicPanCurve = 8,

        /// <summary>
        /// Pass 32-bit floating-point sample data to all DSP functions?
        /// </summary>
        /// <remarks>
        /// Normally DSP functions receive sample data in whatever format the channel is using, ie. it can be 8, 16 or 32-bit.
        /// But when this config option is enabled, BASS will convert 8/16-bit sample data to 32-bit floating-point before passing it to DSP functions,
        /// and then convert it back after all the DSP functions are done. As well as simplifying the DSP code (no need for 8/16-bit processing),
        /// this also means that there is no degradation of quality as sample data passes through a chain of DSP.
        /// </remarks>
        FloatDSP = 9,

        /// <summary>
        /// The 3D algorithm for software mixed 3D channels.
        /// </summary>
        /// <remarks>
        /// DEFAULT = 0.
        /// The default algorithm. If the user has selected a surround sound speaker configuration (eg. 4 or 5.1) in the control panel,
        /// the sound is panned among the available directional speakers. Otherwise it equates to OFF.
        /// OFF = 1.
        /// Uses normal left and right panning. The vertical axis is ignored except for scaling of volume due to distance.
        /// Doppler shift and volume scaling are still applied, but the 3D filtering is not performed.
        /// This is the most CPU efficient algorithm, but provides no virtual 3D audio effect. Head Related Transfer Function processing will not be done.
        /// Since only normal stereo panning is used, a channel using this algorithm may be accelerated by a 2D hardware voice if no free 3D hardware voices are available.
        /// FULL = 2.
        /// This algorithm gives the highest quality 3D audio effect, but uses more CPU.
        /// This algorithm requires WDM drivers, if it's not available then OFF will automatically be used instead.
        /// LIGHT = 3.
        /// This algorithm gives a good 3D audio effect, and uses less CPU than the FULL algorithm.
        /// This algorithm also requires WDM drivers, if it's not available then BASS_3DALG_OFF will automatically be used instead.
        /// 
        /// These algorithms only affect 3D channels that are being mixed in software. BASS_ChannelGetInfo can be used to check whether a channel is being software mixed.
        /// Changing the algorithm only affects subsequently created or loaded samples, musics, or streams; it does not affect any that already exist.
        /// 
        /// When using DirectSound output on Windows, DirectX 7 or above is required for this option to have effect.
        /// Otherwise, including when using WASAPI output on Windows, only the BASS_3DALG_DEFAULT and BASS_3DALG_OFF options are available.
        /// </remarks>
        Algorithm3D = 10,

        /// <summary>
        /// The time to wait for a server to respond to a connection request.
        /// </summary>
        /// <remarks>
        /// The time to wait, in milliseconds.
        /// The default timeout is 5 seconds (5000 milliseconds).
        /// </remarks>
        NetTimeOut = 11,

        /// <summary>
        /// The internet download buffer length.
        /// </summary>
        /// <remarks>
        /// The buffer length is in milliseconds.
        /// Increasing the buffer length decreases the chance of the stream stalling, but also increases the time taken to create the stream as more data has to be pre-buffered.
        /// The default buffer length is 5 seconds (5000 milliseconds).
        /// The net buffer length should be larger than the length of the playback buffer (BASS_CONFIG_BUFFER), otherwise the stream is likely to stall soon after starting playback.
        /// </remarks>
        NetBufferLength = 12,

        /// <summary>
        /// Prevent channels being played while the output is paused?
        /// </summary>
        /// <remarks>
        /// When the output is paused using BASS_Pause, and this config option is enabled, channels cannot be played until the output is resumed using BASS_Start.
        /// Any attempts to play a channel will result in a BASS_ERROR_START error.
        /// By default, this config option is enabled. 
        /// </remarks>
        PauseNoPlay = 13,

        /// <summary>
        /// Amount to pre-buffer before playing internet streams.
        /// </summary>
        /// <remarks>
        /// This setting determines what percentage of the buffer length (BASS_CONFIG_NET_BUFFER) should be filled before starting playback.
        /// The default is 75%. This setting is just a minimum; BASS will always pre-download a certain amount to detect the stream's format and initialize the decoder.
        /// 
        /// The pre-buffering can be done by BASS_StreamCreateURL or asynchronously, depending on the BASS_CONFIG_NET_PREBUF_WAIT setting.
        /// As well as internet streams, this config setting also applies to "buffered" user file streams created with BASS_StreamCreateFileUser. 
        /// </remarks>
        NetPreBuffer = 15,

        /// <summary>
        /// Use passive mode in FTP connections?
        /// </summary>
        NetPassive = 18,

        /// <summary>
        /// The buffer length for recording channels.
        /// </summary>
        /// <remarks>
        /// The buffer length in milliseconds... 1000 (min) - 5000 (max). If the length specified is outside this range, it is automatically capped.
        /// 
        /// Unlike a playback buffer, where the aim is to keep the buffer full, a recording buffer is kept as empty as possible and so this setting has no effect on latency.
        /// The default recording buffer length is 2000 milliseconds. Unless processing of the recorded data could cause significant delays,
        /// or you want to use a large recording period with BASS_RecordStart, there should be no need to increase this.
        /// Using this config option only affects the recording channels that are created afterwards, not any that have already been created.
        /// So it is possible to have channels with differing buffer lengths by using this config option each time before creating them.
        /// </remarks>
        RecordingBufferLength = 19,

        /// <summary>
        /// Process URLs in PLS and M3U playlists?
        /// </summary>
        /// <remarks>
        /// 0 = never, 1 = in BASS_StreamCreateURL only, 2 = in BASS_StreamCreateFile and BASS_StreamCreateFileUser too.
        /// 
        /// When enabled, BASS will process PLS and M3U playlists, trying each URL until it finds one that it can play.
        /// BASS_ChannelGetInfo can be used to find out the URL that was successfully opened.
        /// Nested playlists are suported, that is a playlist can contain the URL of another playlist.
        /// The BASS_CONFIG_NET_PLAYLIST_DEPTH option limits the nested playlist depth.
        /// By default, playlist processing is disabled. 
        /// </remarks>
        NetPlaylist = 21,

        /// <summary>
        /// The maximum number of virtual channels to use in the rendering of IT files.
        /// </summary>
        /// <remarks>
        /// 1 (min) to 512 (max). If the value specified is outside this range, it is automatically capped.
        /// This setting only affects IT files, as the other MOD music formats do not have virtual channels.
        /// The default setting is 64. Changes only apply to subsequently loaded files, not any that are already loaded.
        /// </remarks>
        MusicVirtual = 22,

        /// <summary>
        /// The amount of data to check in order to verify/detect the file format.
        /// </summary>
        /// <remarks>
        /// The amount of data to check, in bytes... 1000 (min) to 1000000 (max). If the value specified is outside this range, it is automatically capped.
        /// Of the file formats supported as standard, this setting only affects the detection of MP3/MP2/MP1 formats, but it may also be used by add-ons (see the documentation).
        /// The verification length excludes any tags that may be found at the start of the file. The default length is 16000 bytes.
        /// </remarks>
        FileVerificationBytes = 23,

        /// <summary>
        /// The number of threads to use for updating playback buffers.
        /// </summary>
        /// <remarks>
        /// 0 = disable automatic updating.
        /// The number of update threads determines how many HSTREAM/HMUSIC channel playback buffers can be updated in parallel;
        /// each thread can process one channel at a time.
        /// The default is to use a single thread, but additional threads can be used to take advantage of multiple CPU cores.
        /// There is generally nothing much to be gained by creating more threads than there are CPU cores,
        /// but one benefit of using multiple threads even with a single CPU core is that a slowly updating channel need not delay the updating of other channels.
        /// 
        /// When automatic updating is disabled (threads = 0), BASS_Update or BASS_ChannelUpdate should be used instead.
        /// The number of update threads can be changed at any time, including during playback. 
        /// </remarks>
        UpdateThreads = 24,

        /// <summary>
        /// The output device buffer length.
        /// </summary>
        /// <remarks>
        /// The device buffer is where the final mix of all playing channels is placed, ready for the device to play.
        /// Its length affects the latency of things like starting and stopping playback of a channel,
        /// so you will probably want to avoid setting it unnecessarily high, but setting it too short could result in breaks in the sound.
        /// When using a large device buffer, the BASS_ATTRIB_BUFFER attribute could be used to skip the channel buffering step,
        /// to avoid further increasing latency for real-time generated sound and/or DSP/FX changes.
        /// 
        /// The buffer length needs to be a multiple of, and at least double, the device's update period,
        /// which can be set via the BASS_CONFIG_DEV_PERIOD option. The buffer length will be rounded up automatically if necessary to achieve that.
        /// The system may also choose to use a different buffer length if the requested one is too short or long, or needs rounding for granularity.
        /// 
        /// Changes to this config setting only affect subsequently initialized devices, not any that are already initialized.
        /// The "No Sound" device does not have a buffer, so is unaffected by this option.
        /// 
        /// Platform-specific:
        /// The default setting is 30ms on Windows, 40ms on Linux and Android, 200ms on Windows CE.
        /// This option is not available on OSX or iOS; the device buffer length on those platforms is twice the device update period, which can be set via the BASS_CONFIG_DEV_PERIOD option.
        /// On Windows, this config option only applies when WASAPI output is used.
        /// On Linux, BASS will attempt to set the device buffer-feeding thread to real-time priority (as on other platforms)
        /// to reduce the chances of it getting starved of CPU, but if that is not possible (eg. the user account lacks permission)
        /// then it may be necessary to increase the buffer length to avoid breaks in the output when the CPU is busy.
        /// When using AudioTrack output on Android, the buffer length will be automatically raised to the minimum given by the AudioTrack getMinBufferSize method if necessary.
        /// </remarks>
        DeviceBufferLength = 27,

        /// <summary>
        /// Enable the loopback recording feature.
        /// </summary>
        LoopbackRecording = 28,

        /// <summary>
        /// Enable true play position mode on Windows Vista and newer?
        /// </summary>
        /// <remarks>
        /// Unless this option is enabled, the reported playback position will advance in 10ms steps on Windows Vista and newer.
        /// As well as affecting the precision of BASS_ChannelGetPosition, this also affects the timing of non-mixtime syncs.
        /// When this option is enabled, it allows finer position reporting but it also increases latency.
        /// The default setting is enabled. Changes only affect channels that are created afterwards, not any that already exist.
        /// The latency and minbuf values in the BASS_INFO structure reflect the setting at the time of the device's BASS_Init call.
        /// This config option is only available on Windows. It is available on all Windows versions (not including CE),
        /// but only has effect when using DirectSound output on Windows Vista and newer.
        /// </remarks>
        TruePlayPosition = 30,

        /// <summary>
        /// Audio session configuration on iOS.
        /// </summary>
        IOSSession = 34,

        /// <summary>
        /// Include a "Default" entry in the output device list?
        /// </summary>
        /// <remarks>
        /// This option adds a "Default" entry to the output device list, which maps to the device that is currently the system's default.
        /// Its output will automatically switch over when the system's default device setting changes.
        /// When enabled, the "Default" device will also become the default device to BASS_Init (with device = -1).
        /// Both it and the device that it currently maps to will have the BASS_DEVICE_DEFAULT flag set by BASS_GetDeviceInfo.
        /// This option can only be set before BASS_GetDeviceInfo or BASS_Init has been called.
        /// This config option is only available on Windows and OSX.
        /// On Windows, the automatic device switching feature requires Windows Vista or above (Windows 7 when DirectSound is used).
        /// When the "Default" device is used with DirectSound, the BASS_SetVolume and BASS_GetVolume functions work a bit differently to usual;
        /// they deal with the "session" volume, which only affects the current process's output on the device, rather than the device's volume.
        /// </remarks>
        IncludeDefaultDevice = 36,

        /// <summary>
        /// The time to wait for a server to deliver more data for an internet stream.
        /// </summary>
        /// <remarks>
        /// The time to wait, in milliseconds... 0 = no timeout.
        /// When the timeout is hit, the connection with the server will be closed.
        /// The default setting is 0, no timeout. Changes only affect subsequently created streams, not any that already exist.
        /// </remarks>
        NetReadTimeOut = 37,

        /// <summary>
        /// Enable speaker assignment with panning/balance control on Windows Vista and newer?
        /// </summary>
        /// <remarks>
        /// Panning/balance control via the BASS_ATTRIB_PAN attribute is not available when
        /// speaker assignment is used on Windows with DirectSound due to the way that the speaker assignment needs to be implemented there.
        /// The situation is improved with Windows Vista, and speaker assignment can generally be done in a way that does permit panning/balance control
        /// to be used at the same time, but there may still be some drivers that it does not work properly with, so it is disabled by default and
        /// can be enabled via this config option. Changes only affect channels that are created afterwards, not any that already exist.
        /// </remarks>
        VistaSpeakerAssignment = 38,

        /// <summary>
        /// ???
        /// </summary>
        IOSSpeaker = 39,

        /// <summary>
        /// Disable the use of Media Foundation?
        /// </summary>
        /// <remarks>
        /// This option determines whether Media Foundation codecs can be used to decode files and streams.
        /// It is set to FALSE by default when Media Foundation codecs are available,
        /// which is on Windows 7 and above, and updated versions of Vista. It will otherwise be TRUE and read-only.
        /// This config option is only available on Windows.
        /// </remarks>
        MFDisable = 40,

        /// <summary>
        /// Number of existing HMUSIC / HRECORD / HSAMPLE / HSTREAM handles.
        /// </summary>
        /// <remarks>
        /// This is a read-only config option that gives the total number of HMUSIC / HRECORD / HSAMPLE / HSTREAM handles that currently exist,
        /// which can be useful for detecting leaks, ie. unfreed handles.
        /// </remarks>
        HandleCount = 41,

        /// <summary>
        /// Use the Unicode character set in device information?
        /// </summary>
        /// <remarks>
        /// This config option is only available on Windows.
        /// </remarks>
        UnicodeDeviceInformation = 42,

        /// <summary>
        /// The default sample rate conversion quality.
        /// 0 = linear interpolation,
        /// 1 = 8 point sinc interpolation (Default),
        /// 2 = 16 point sinc interpolation,
        /// 3 = 32 point sinc interpolation.
        /// 4 = 64 point sinc interpolation.
        /// Other values are also accepted.
        /// </summary>
        /// <remarks>
        /// This config option determines what sample rate conversion
        /// quality new channels will initially have, except for sample channels (HCHANNEL),
        /// which use the <see cref="SampleSRCQuality"/> setting.
        /// </remarks>
        SRCQuality = 43,

        /// <summary>
        /// The default sample rate conversion quality for samples.
        /// 0 = linear interpolation (Default),
        /// 1 = 8 point sinc interpolation,
        /// 2 = 16 point sinc interpolation,
        /// 3 = 32 point sinc interpolation.
        /// 4 = 64 point sinc interpolation.
        /// Other values are also accepted.
        /// </summary>
        /// <remarks>
        /// This config option determines what sample rate conversion quality a new sample
        /// channel will initially have, following a BASS_SampleGetChannel call.
        /// </remarks>
        SampleSRCQuality = 44,

        /// <summary>
        /// Asynchronous file reading buffer length (default setting is 65536 bytes (64KB)).
        /// </summary>
        /// <remarks>
        /// This will be rounded up to the nearest 4096 byte (4KB) boundary.
        /// This determines the amount of file data that can be read ahead of time with asynchronous file reading.
        /// Changes only affect streams that are created afterwards, not any that already exist.
        /// So it is possible to have streams with differing Buffer lengths by using this config option before creating each of them.
        /// When asynchronous file reading is enabled, the Buffer level is available from BASS_StreamGetFilePosition.
        /// </remarks>
        AsyncFileBufferLength = 45,

        /// <summary>
        /// Pre-scan chained OGG files?
        /// </summary>
        OggPreScan = 47,

        /// <summary>
        /// Play the audio from videos using Media Foundation?
        /// </summary>
        MFVideo = 48,

        /// <summary>
        /// ???
        /// </summary>
        Airplay = 49,

        /// <summary>
        /// Do not stop an output device when nothing is playing?
        /// </summary>
        DevNonStop = 50,

        /// <summary>
        /// ???
        /// </summary>
        IOSNoCategory = 51,

        /// <summary>
        /// The amount of data to check (in bytes) in order to verify/detect the file format of internet streams... 1000 (min) to 1000000 (max),
        /// or 0 = 25% of the <see cref="FileVerificationBytes"/> setting (with a minimum of 1000 bytes).
        /// </summary>
        NetVerificationBytes = 52,

        /// <summary>
        /// The output device update period.
        /// </summary>
        /// <remarks>
        /// The update period in milliseconds, or in samples if negative.
        /// The device update period determines how often data is generated and placed in an output device's buffer.
        /// A shorter period allows a smaller buffer and lower latency but may use more CPU than a longer period.
        /// The system may choose to use a different period if the requested one is too short or long, or needs rounding for granularity.
        /// The period actually being used can be obtained with BASS_GetInfo (the minbuf value).
        /// The default setting is 10ms. Changes only affect subsequently initialized devices, not any that are already initialized. 
        /// </remarks>
        DevicePeriod = 53,

        /// <summary>
        /// Floating-point sample data is supported?
        /// </summary>
        Float = 54,

        /// <summary>
        /// ???
        /// </summary>
        NetSeek = 56,

        /// <summary>
        /// Disable the use of Android media codecs?
        /// </summary>
        AmDisable = 58,

        /// <summary>
        /// Maximum nested playlist processing depth.
        /// </summary>
        /// <remarks>
        /// 0 = do not process nested playlists.
        /// When playlist processing is enabled via the BASS_CONFIG_NET_PLAYLIST option, this option limits how deep into nested playlists BASS_StreamCreateURL will go.
        /// The default depth is 1, which means playlists within the root playlist will be processed, but not playlists within those playlists.
        /// </remarks>
        NetPlaylistDepth = 59,

        /// <summary>
        /// Wait for pre-buffering when opening internet streams?
        /// </summary>
        NetPrebufWait = 60,

        /// <summary>
        /// Session ID to use for output on Android.
        /// </summary>
        AndroidSessionID = 62,

        /// <summary>
        /// Retain Windows mixer settings across sessions?
        /// </summary>
        WasapiPersist = 65,

        /// <summary>
        /// ???
        /// </summary>
        RecWasapi = 66,

        /// <summary>
        /// Enable AAudio output on Android?
        /// </summary>
        AndroidAAudio = 67
    }
}
