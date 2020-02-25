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
    /// Flags used to create special output streams.
    /// </summary>
    public enum BassStreamProcedureType
    {
        /// <summary>
        /// Flag to signify that the end of the stream is reached.
        /// </summary>
        End = -2147483648,

        /// <summary>
        /// Create a "push" stream.
        /// Instead of BASS pulling data from a StreamProcedure function, data is pushed to BASS via BASS_StreamPutData.
        /// </summary>
        Push = -1,

        /// <summary>
        /// Create a "dummy" stream.
        /// A dummy stream does not have any sample data of its own, but a decoding dummy stream (with BASS_STREAM_DECODE flag) can be used to apply DSP/FX processing to any sample data,
        /// by setting DSP/FX on the stream and feeding the data through BASS_ChannelGetData. The dummy stream should have the same sample format as the data being fed through it.
        /// </summary>
        Dummy = 0,

        /// <summary>
        /// Create a "dummy" stream for the device's final output mix.
        /// This allows DSP/FX to be applied to all channels that are playing on the device, rather than individual channels.
        /// DSP/FX parameter change latency is also reduced because channel playback buffering is avoided.
        /// The stream is created with the device's current output sample format; the freq, chans, and flags parameters are ignored.
        /// It will always be floating-point except on platforms/architectures that do not support floating-point, where it will be 16-bit instead.
        /// </summary>
        Device = -2
    }
}
