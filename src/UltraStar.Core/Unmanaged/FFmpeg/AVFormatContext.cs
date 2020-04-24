#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.Runtime.InteropServices;

namespace UltraStar.Core.Unmanaged.FFmpeg
{
    /// <summary>
    /// Represents an FFmpeg format context.
    /// </summary>
    /// <remarks>
    /// You can also call this the handle to the media file.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct AVFormatContext
    {
        /// <summary>
        /// Information of this struct.
        /// </summary>
        public IntPtr AVClass;
        /// <summary>
        /// The input container format.
        /// </summary>
        public AVInputFormat* FormatInput;
        /// <summary>
        /// The output container format.
        /// </summary>
        public IntPtr FormatOutput;
        /// <summary>
        /// Private data - interally.
        /// </summary>
        public IntPtr PrivateData;
        /// <summary>
        /// Pointer to I/O context.
        /// </summary>
        public IntPtr PB;
        /// <summary>
        /// Flags signalling stream properties.
        /// </summary>
        public int CTXFlags;
        /// <summary>
        /// Number of elements in <see cref="AVFormatContext.Streams"/>.
        /// </summary>
        public uint StreamsCount;
        /// <summary>
        /// A list of all streams in the file.
        /// </summary>
        public AVStream** Streams;
        /// <summary>
        /// Deprecated: The input or output filename. Use url instead.
        /// </summary>
        public ByteArray1024 Filename;
        /// <summary>
        /// The input or output URL. Unlike the old filename field, this field has no length restriction.
        /// </summary>
        public byte* URL;
        /// <summary>
        /// The position of the first frame of the component, in AV_TIME_BASE fractional seconds.
        /// </summary>
        /// <remarks>
        /// NEVER set this value directly: It is deduced from the AVStream values.
        /// </remarks>
        public long StartTime;
        /// <summary>
        /// The duration of the stream, in AV_TIME_BASE fractional seconds.
        /// </summary>
        /// <remarks>
        /// Only set this value if you know none of the individual stream durations and also do not set any of them.
        /// This is deduced from the AVStream values if not set.
        /// </remarks>
        public long Duration;
        /// <summary>
        /// The total stream bitrate in bit/s, 0 if not available.
        /// </summary>
        /// <remarks>
        /// Never set it directly if the file_size and the duration are known as FFmpeg can compute it automatically.
        /// </remarks>
        public long BitRate;
        /// <summary>
        /// The packet size.
        /// </summary>
        public uint PacketSize;
        /// <summary>
        /// The maximum delay.
        /// </summary>
        public int MaximumDelay;
        /// <summary>
        /// Flags modifying the (de)muxer behaviour.
        /// </summary>
        public int Flags;
        /// <summary>
        /// The maximum size of the data read from input for determining the input container format.
        /// </summary>
        public long ProbeSize;
        /// <summary>
        /// The maximum duration (in AV_TIME_BASE units) of the data read from input in <see cref="FFmpeg.AVFormatFindStreamInfo"/>.
        /// </summary>
        /// <remarks>
        /// Demuxing only, set by the caller before <see cref="FFmpeg.AVFormatFindStreamInfo"/>. Can be set to 0 to let avformat choose using a heuristic.
        /// </remarks>
        public long MaximumAnalyzeDuration;
        /// <summary>
        /// A pointer to a key.
        /// </summary>
        public IntPtr Key;
        /// <summary>
        /// The length of the key.
        /// </summary>
        public int KeyLength;
        /// <summary>
        /// The number of <see cref="AVFormatContext.Programs"/> available.
        /// </summary>
        public uint ProgramsCount;
        /// <summary>
        /// The programs in the format context.
        /// </summary>
        public IntPtr Programs;
        /// <summary>
        /// Forced video codec ID.
        /// </summary>
        public int VideoCodecID;
        /// <summary>
        /// Forced audio codec ID.
        /// </summary>
        public int AudioCodecID;
        /// <summary>
        /// Forced subtitle codec ID.
        /// </summary>
        public int SubtitleCodecID;
        /// <summary>
        /// The maximum amount of memory in bytes to use for the index of each stream.
        /// </summary>
        /// <remarks>
        /// If the index exceeds this size, entries will be discarded as needed to maintain a smaller size.
        /// This can lead to slower or less accurate seeking (depends on demuxer).
        /// Demuxers for which a full in-memory index is mandatory will ignore this.
        /// </remarks>
        public uint MaximumIndexSize;
        /// <summary>
        /// The maximum amount of memory in bytes to use for buffering frames obtained from realtime capture devices.
        /// </summary>
        public uint MaximumPictureBuffer;
        /// <summary>
        /// The number of chapters in <see cref="AVFormatContext.Chapters"/>.
        /// </summary>
        public uint ChaptersCount;
        /// <summary>
        /// A pointer to an array of chapters in the file.
        /// </summary>
        public IntPtr Chapters;
        /// <summary>
        /// Metadata that applies to the whole file.
        /// </summary>
        public AVDictionary* Metadata;
        /// <summary>
        /// The start time of the stream in real world time, in microseconds since the Unix epoch (00:00 1st January 1970).
        /// </summary>
        public long StartTimeRealtime;
        /// <summary>
        /// The number of frames used for determining the framerate in <see cref="FFmpeg.AVFormatFindStreamInfo"/>.
        /// </summary>
        public int ProbeSizeForFPSEstimation;
        /// <summary>
        /// The error recognition; higher values will detect more errors but may misdetect some more or less valid parts as errors.
        /// </summary>
        public int ErrorRecognition;
        /// <summary>
        /// Custom interrupt callbacks for the I/O layer.
        /// </summary>
        public IntPtr InterruptCallback;
        /// <summary>
        /// Flags to enable debugging.
        /// </summary>
        public int DebugFlags;
        /// <summary>
        /// The maximum buffering duration for interleaving.
        /// </summary>
        public long MaximumInterleaveDelta;
        /// <summary>
        /// Disallow/Allow non-standard and experimental extension.
        /// </summary>
        public int StrictStandardCompliance;
        /// <summary>
        /// Flags for the user to detect events happening on the file.
        /// </summary>
        /// <remarks>
        /// Flags must be cleared by the user once the event has been handled.
        /// </remarks>
        public int EventFlags;
        /// <summary>
        /// The maximum number of packets to read while waiting for the first timestamp.
        /// </summary>
        public int MaximumTimestampProbe;
        /// <summary>
        /// Avoid negative timestamps during muxing.
        /// </summary>
        public int AvoidNegativeTimestamps;
        /// <summary>
        /// Deprecated: The transport stream id. This will be moved into demuxer private options.
        /// </summary>
        public int TransportStreamID;
        /// <summary>
        /// The audio preload in microseconds.
        /// </summary>
        /// <remarks>
        /// Note, not all formats support this and unpredictable things may happen if it is used when not supported.
        /// </remarks>
        public int AudioPreLoad;
        /// <summary>
        /// The maximum chunk time in microseconds.
        /// </summary>
        /// <remarks>
        /// Note, not all formats support this and unpredictable things may happen if it is used when not supported.
        /// </remarks>
        public int MaximumChunkDuration;
        /// <summary>
        /// The maximum chunk size in bytes.
        /// </summary>
        /// <remarks>
        /// Note, not all formats support this and unpredictable things may happen if it is used when not supported.
        /// </remarks>
        public int MaximumChunkSize;
        /// <summary>
        /// Forces the use of wallclock timestamps as pts/dts of packets.
        /// </summary>
        /// <remarks>
        /// This has undefined results in the presence of B frames.
        /// </remarks>
        public int UseWallclockAsTimestamps;
        /// <summary>
        /// AVIO flags.
        /// </summary>
        public int AVIOFlags;
        /// <summary>
        /// The method how the duration shall be estimated.
        /// </summary>
        public int DurationEstimationMethod;
        /// <summary>
        /// Skip initial bytes when opening a stream.
        /// </summary>
        public long SkipInitialBytes;
        /// <summary>
        /// Correct single timestamp overflows.
        /// </summary>
        public uint CorrectTimestampOverflows;
        /// <summary>
        /// Force seeking to any (also non key) frames.
        /// </summary>
        public int SeekToAny;
        /// <summary>
        /// Flush the I/O context after each packet.
        /// </summary>
        public int FlushPackets;
        /// <summary>
        /// The formats probing score.
        /// </summary>
        public int ProbeScore;
        /// <summary>
        /// The number of bytes to read maximally to identify format.
        /// </summary>
        public int FormatProbeSize;
        /// <summary>
        /// A &apos;,&apos; separated list of allowed decoders. If NULL then all are allowed.
        /// </summary>
        public IntPtr CodecWhitelist;
        /// <summary>
        /// A &apos;,&apos; separated list of allowed demuxers. If NULL then all are allowed.
        /// </summary>
        public IntPtr FormatWhitelist;
        /// <summary>
        /// Private data - interally.
        /// </summary>
        public IntPtr PrivateData2;
        /// <summary>
        /// The I/O repositioned flag.
        /// </summary>
        /// <remarks>
        /// This is set by avformat when the underlaying IO context read pointer is repositioned, for example when doing byte based seeking.
        /// Demuxers can use the flag to detect such changes.
        /// </remarks>
        public int IORepositionedFlag;
        /// <summary>
        /// Forced video codec.
        /// </summary>
        /// <remarks>
        /// This allows forcing a specific decoder, even when there are multiple with the same codec_id.
        /// </remarks>
        public IntPtr VideoCodec;
        /// <summary>
        /// Forced audio codec.
        /// </summary>
        /// <remarks>
        /// This allows forcing a specific decoder, even when there are multiple with the same codec_id.
        /// </remarks>
        public IntPtr AudioCodec;
        /// <summary>
        /// Forced subtitle codec.
        /// </summary>
        /// <remarks>
        /// This allows forcing a specific decoder, even when there are multiple with the same codec_id.
        /// </remarks>
        public IntPtr SubtitleCodec;
        /// <summary>
        /// Forced data codec.
        /// </summary>
        /// <remarks>
        /// This allows forcing a specific decoder, even when there are multiple with the same codec_id.
        /// </remarks>
        public IntPtr DataCodec;
        /// <summary>
        /// The number of bytes to be written as padding in a metadata header.
        /// </summary>
        public int MetadataHeaderPadding;
        /// <summary>
        /// Private data of the user, can be used to carry app specific stuff.
        /// </summary>
        public IntPtr PrivateData3;
        /// <summary>
        /// A callback used by devices to communicate with application.
        /// </summary>
        public IntPtr controlMessageCallback;
        /// <summary>
        /// The output timestamp offset, in microseconds.
        /// </summary>
        public long OutputTimestampOffset;
        /// <summary>
        /// The dump format separator. Can be &quot;, &quot; or &quot; &quot; or anything else.
        /// </summary>
        public IntPtr DumpSeparator;
        /// <summary>
        /// The forced data codec ID.
        /// </summary>
        public int DataCodecID;
        /// <summary>
        /// Deprecated: A callback to open further IO contexts when needed for demuxing.
        /// </summary>
        public IntPtr openCallback;
        /// <summary>
        /// A &apos;,&apos; separated list of allowed protocols.
        /// </summary>
        public IntPtr ProtocolWhitelist;
        /// <summary>
        /// A callback for opening new IO streams.
        /// </summary>
        public IntPtr ioOpenCallback;
        /// <summary>
        /// A callback for closing the streams previously opened.
        /// </summary>
        public IntPtr ioCloseCallback;
        /// <summary>
        /// A &apos;,&apos; separated list of disallowed protocols.
        /// </summary>
        public IntPtr ProtocolBlacklist;
        /// <summary>
        /// The maximum number of streams.
        /// </summary>
        public int MaximumNumberOfStreams;
        /// <summary>
        /// Skip duration calcuation in estimate_timings_from_pts.
        /// </summary>
        public int SkipEstimateDurationFromPTS;
        /// <summary>
        /// The maximum number of packets that can be probed.
        /// </summary>
        public int MaximumProbePackets;
    }
}
