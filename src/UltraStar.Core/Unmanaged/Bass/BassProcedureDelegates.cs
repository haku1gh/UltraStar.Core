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
    /// User stream writing callback function.
    /// </summary>
    /// <remarks>
    /// <para>A stream writing function should be as quick as possible because other playing streams (and MOD musics) cannot be updated until it has finished (unless multiple update threads are enabled via the BASS_CONFIG_UPDATETHREADS option).
    /// It is better to return less data quickly, rather than spending a long time delivering exactly the amount requested.</para>
    ///
    /// <para>Although a STREAMPROC may return less data than BASS requests, be careful not to do so by too much, too often. If the buffer gets exhausted, BASS will automatically stall playback of the stream, until more data is provided.
    /// BASS_ChannelGetData (BASS_DATA_AVAILABLE) can be used to check the buffer level, and BASS_ChannelIsActive can be used to check if playback has stalled.
    /// A BASS_SYNC_STALL sync can also be set via BASS_ChannelSetSync, to be triggered upon playback stalling or resuming. If you do return less than the requested amount of data, the number of bytes should still equate to a whole number of samples. </para>
    ///
    /// <para>The BASS_ATTRIB_GRANULE attribute can be used to control the granularity of the amount of data requested. </para>
    ///
    /// <para>Some functions can cause problems if called from within a stream (or DSP) function. Do not call BASS_Stop or BASS_Free from within a stream callback,
    /// and do not call BASS_ChannelStop or BASS_StreamFree with the same handle as received by the callback.</para>
    ///
    /// <para>When streaming multi-channel sample data, the channel order of each sample is as follows:</para>
    /// <para>3 channels: left-front, right-front, center.</para>
    /// <para>4 channels: left-front, right-front, left-rear/side, right-rear/side.</para>
    /// <para>6 channels(5.1): left-front, right-front, center, LFE, left-rear/side, right-rear/side.</para>
    /// <para>8 channels(7.1): left-front, right-front, center, LFE, left-rear/side, right-rear/side, left-rear center, right-rear center.</para>
    /// </remarks>
    /// <param name="handle">The stream that needs writing.</param>
    /// <param name="buffer">The pointer to the Buffer to write the sample data in. The sample data must be written in standard Windows PCM format - 8-bit samples are unsigned, 16-bit samples are signed, 32-bit floating-point samples range from -1 to 1.</param>
    /// <param name="length">The number of bytes to write.</param>
    /// <param name="user">The user instance data given when BASS_StreamCreate was called.</param>
    /// <returns>The number of bytes written by the function, optionally using the BASS_STREAMPROC_END flag to signify that the end of the stream is reached.</returns>
    internal delegate int BassStreamProcedure(int handle, IntPtr buffer, int length, IntPtr user);

    /// <summary>
    /// User defined callback function to process recorded sample data.
    /// </summary>
    /// <remarks>
    /// BASS_RecordFree should not be used to free the recording device within a recording callback function.
    /// Nor should BASS_ChannelStop be used to stop the recording; return <see langword="false" /> to do that instead.
    /// </remarks>
    /// <param name="handle">The recording Handle that the data is from.</param>
    /// <param name="buffer">
    /// The pointer to the Buffer containing the recorded sample data.
    /// The sample data is in standard Windows PCM format, that is 8-bit samples are unsigned, 16-bit samples are signed, 32-bit floating-point samples range from -1 to +1.
    /// </param>
    /// <param name="length">The number of bytes in the Buffer.</param>
    /// <param name="user">The User instance data given when BASS_RecordStart was called.</param>
    /// <returns>Return <see langword="true" /> to stop recording, and anything else to continue recording.</returns>
    internal delegate bool BassRecordProcedure(int handle, IntPtr buffer, int length, IntPtr user);

    /// <summary>
    /// User defined DSP callback function (to be used with BASS_ChannelSetDSP.
    /// </summary>
    /// <remarks>
    /// <para>The format of the sample data is as stated by BASS_ChannelGetInfo, except that it will always be floating-point if the BASS_CONFIG_FLOATDSP config option is enabled.</para>
    /// <para>If the DSP processing requires a particular amount of data, the BASS_ATTRIB_GRANULE attribute can be used to specify that.</para>
    /// <para>A DSP function should be as quick as possible, as any lengthy delays can result in stuttering playback.
    /// Some functions can cause problems if called from within a DSP function. Do not call BASS_Stop or BASS_Free from within a DSP callback,
    /// and do not call BASS_ChannelStop, BASS_MusicFree or BASS_StreamFree with the same channel handle as received by the callback.</para>
    /// </remarks>
    /// <param name="handle">The DSP Handle (as returned by BASS_ChannelSetDSP.</param>
    /// <param name="channel">Channel that the DSP is being applied to.</param>
    /// <param name="buffer">
    /// The pointer to the Buffer to apply the DSP to.
    /// The sample data is in standard Windows PCM format - 8-bit samples are unsigned, 16-bit samples are signed, 32-bit floating-point samples range from -1 to 1 (not clipped, so can actually be outside this range).
    /// </param>
    /// <param name="length">The number of bytes to process.</param>
    /// <param name="user">The User instance data given when BASS_ChannelSetDSP was called.</param>
    internal delegate void BassDSPProcedure(int handle, int channel, IntPtr buffer, int length, IntPtr user);

    /// <summary>
    /// User defined synchronizer callback function.
    /// </summary>
    /// <remarks>
    /// BASS creates a single thread dedicated to executing sync callback functions, so a callback function should be quick as other syncs cannot be processed until it has finished.
    /// Attribute slides (initiated with BASS_ChannelSlideAttribute) are also performed by the sync thread, so are also affected if a sync callback takes a long time.
    /// 
    /// "Mixtime" syncs are usually not executed in the sync thread(unless the BASS_SYNC_THREAD flag is used) but rather immediately in whichever thread triggers them.
    /// In most cases that will be an update thread, and so the same restrictions that apply to stream callbacks (STREAMPROC) also apply here,
    /// except that BASS_ChannelStop can be used in a BASS_SYNC_POS sync's callback to stop a channel at a particular position.
    /// Other exceptions are that the channel can be freed in a BASS_SYNC_DEV_FAIL or BASS_SYNC_DEV_FORMAT or BASS_SYNC_SLIDE sync's callback.
    /// BASS_ChannelSetPosition can be used in a mixtime sync (without BASS_SYNC_THREAD set) to implement custom looping,
    /// eg. set a BASS_SYNC_POS sync at the loop end position and seek to the loop start position in the callback.
    /// </remarks>
    /// <param name="handle">The sync that has occurred.</param>
    /// <param name="channel">The channel that the sync occurred on. </param>
    /// <param name="data">Additional data associated with the sync's occurrence.</param>
    /// <param name="user">The user instance data given when BASS_ChannelSetSync was called.</param>
    internal delegate void BassSyncProcedure(int handle, int channel, int data, IntPtr user);
}
