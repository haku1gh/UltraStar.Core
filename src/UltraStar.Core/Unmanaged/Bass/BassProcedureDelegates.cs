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
    /// User stream writing callback delegate.
    /// </summary>
    /// <param name="Handle">The stream that needs writing.</param>
    /// <param name="Buffer">The pointer to the Buffer to write the sample data in. The sample data must be written in standard Windows PCM format - 8-bit samples are unsigned, 16-bit samples are signed, 32-bit floating-point samples range from -1 to 1.</param>
    /// <param name="Length">The number of bytes to write.</param>
    /// <param name="User">The user instance data given when BASS_StreamCreate was called.</param>
    /// <returns>The number of bytes written by the function, optionally using the BASS_STREAMPROC_END flag to signify that the end of the stream is reached.</returns>
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
    internal delegate int BassStreamProcedure(int Handle, IntPtr Buffer, int Length, IntPtr User);

    /// <summary>
    /// User defined callback function to process recorded sample data.
    /// </summary>
    /// <param name="Handle">The recording Handle that the data is from.</param>
    /// <param name="Buffer">
    /// The pointer to the Buffer containing the recorded sample data.
    /// The sample data is in standard Windows PCM format, that is 8-bit samples are unsigned, 16-bit samples are signed, 32-bit floating-point samples range from -1 to +1.
    /// </param>
    /// <param name="Length">The number of bytes in the Buffer.</param>
    /// <param name="User">The User instance data given when BASS_RecordStart was called.</param>
    /// <returns>Return <see langword="true" /> to stop recording, and anything else to continue recording.</returns>
    /// <remarks>
    /// BASS_RecordFree should not be used to free the recording device within a recording callback function.
    /// Nor should BASS_ChannelStop be used to stop the recording; return <see langword="false" /> to do that instead.
    /// </remarks>
    internal delegate bool BassRecordProcedure(int Handle, IntPtr Buffer, int Length, IntPtr User);
}
