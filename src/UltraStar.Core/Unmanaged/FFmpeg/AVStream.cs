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
    /// Represents an FFmpeg stream.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct AVStream
    {
        /// <summary>
        /// The stream index in <see cref="AVFormatContext"/>.
        /// </summary>
        public int Index;
        /// <summary>
        /// A format-specific stream ID.
        /// </summary>
        public int ID;
        /// <summary>
        /// The codec used in the stream.
        /// </summary>
        public AVCodecContext* Codec;
        /// <summary>
        /// Private data - interally.
        /// </summary>
        public IntPtr PrivateData;
        /// <summary>
        /// This is the fundamental unit of time (in seconds) in terms of which frame timestamps are represented.
        /// </summary>
        public AVRational TimeBase;
        /// <summary>
        /// Decoding: pts of the first frame of the stream in presentation order, in stream time base.
        /// Only set this if you are absolutely 100% sure that the value you set it to really is the pts of the first frame.
        /// This may be undefined (AV_NOPTS_VALUE).
        /// </summary>
        public long StartTime;
        /// <summary>
        /// Decoding: duration of the stream, in stream time base.
        /// If a source file does not specify a duration, but does specify a bitrate, this value will be estimated from bitrate and file size.
        /// </summary>
        public long Duration;
        /// <summary>
        /// The number of frames in this stream if known or 0.
        /// </summary>
        public long NumberOfFrames;
        /// <summary>
        /// The AV_DISPOSITION_* bit field.
        /// </summary>
        public int Disposition;
        /// <summary>
        /// Selects which packets can be discarded at will and do not need to be demuxed.
        /// </summary>
        public int Discard;
        /// <summary>
        /// The sample aspect ratio (0 if unknown).
        /// </summary>
        public AVRational SampleAspectRatio;
        /// <summary>
        /// The metadata.
        /// </summary>
        public AVDictionary* Metadata;
        /// <summary>
        /// The average framerate.
        /// </summary>
        public AVRational AverageFrameRate;
        /// <summary>
        /// For streams with AV_DISPOSITION_ATTACHED_PIC disposition, this packet will contain the attached picture.
        /// </summary>
        public AVPacket AttachedPicture;
        /// <summary>
        /// An array of side data that applies to the whole stream (i.e. the container does not allow it to change between packets).
        /// </summary>
        public IntPtr SideData;
        /// <summary>
        /// The number of elements in the AVStream.side_data array.
        /// </summary>
        public int SideDataCount;
        /// <summary>
        /// Flags for the user to detect events happening on the stream.
        /// Flags must be cleared by the user once the event has been handled. A combination of AVSTREAM_EVENT_FLAG_*.
        /// </summary>
        public int EventFlags;
        /// <summary>
        /// The real base framerate of the stream. This is the lowest framerate with which all timestamps can be represented accurately
        /// (it is the least common multiple of all framerates in the stream). Note, this value is just a guess! For example,
        /// if the time base is 1/90000 and all frames have either approximately 3600 or 1800 timer ticks, then r_frame_rate will be 50/1.
        /// </summary>
        public AVRational RealBaseFrameRate;
        /// <summary>
        /// A string containing pairs of key and values describing recommended encoder configuration.
        /// Pairs are separated by &apos;,&apos;. Keys are separated from values by &apos;=&apos;.
        /// </summary>
        public byte* RecommendedEncoderConfiguration;
        /// <summary>
        /// The codec parameters associated with this stream.
        /// Allocated and freed by libavformat in avformat_new_stream() and avformat_free_context() respectively.
        /// </summary>
        public IntPtr CodecParameters;
        /// <summary>
        /// A pointer to some information.
        /// </summary>
        public IntPtr Info;
        /// <summary>
        /// The number of bits in pts (used for wrapping control).
        /// </summary>
        public int PTSWrapBits;
        /// <summary>
        /// The timestamp corresponding to the last dts sync point.
        /// </summary>
        public long FirstDTS;
        /// <summary>
        /// The timestamp corresponding to the currnt dts.
        /// </summary>
        public long CurrentDTS;
        /// <summary>
        /// The last IP PTS.
        /// </summary>
        public long LastIpPTS;
        /// <summary>
        /// The last IP duration.
        /// </summary>
        public int LastIpDuration;
        /// <summary>
        /// The number of packets to buffer for codec probing.
        /// </summary>
        public int ProbePackets;
        /// <summary>
        /// The number of frames that have been demuxed during <see cref="FFmpeg.AVFormatFindStreamInfo"/>.
        /// </summary>
        public int CodecInfoNumberOfFrames;
        /// <summary>
        /// Indicator if parsing is required.
        /// </summary>
        public int NeedParsing;
        /// <summary>
        /// A pointer to the parser.
        /// </summary>
        public IntPtr Parser;
        /// <summary>
        /// The last packet in packet_buffer for this stream when muxing.
        /// </summary>
        public IntPtr LastInPacketBuffer;
        /// <summary>
        /// The probe data
        /// </summary>
        public AVProbeData ProbeData;
        /// <summary>
        /// The PTD buffer.
        /// </summary>
        public LongArray17 PTSBuffer;
        /// <summary>
        /// Only used if the format does not support seeking natively.
        /// </summary>
        public IntPtr IndexEntries;
        /// <summary>
        /// The number of entries in <see cref="IndexEntries"/>.
        /// </summary>
        public int IndexEntriesCount;
        /// <summary>
        /// The size of all index entries.
        /// </summary>
        public uint IndexEntriesAllocatedSize;
        /// <summary>
        /// The stream identifier. This is the MPEG-TS stream identifier +1 0 means unknown.
        /// </summary>
        public int StreamIdentifier;
        /// <summary>
        /// Details of the MPEG-TS program which created this stream.
        /// </summary>
        public int ProgramNumber;
        /// <summary>
        /// The PMT version.
        /// </summary>
        public int PMTVersion;
        /// <summary>
        /// The PMT stream index.
        /// </summary>
        public int PMTStreamIndex;
        /// <summary>
        /// The interleaver chunk size.
        /// </summary>
        public long InterleaverChunkSize;
        /// <summary>
        /// The interleaver chunk duration.
        /// </summary>
        public long InterleaverChunkDuration;
        /// <summary>
        /// The stream probing state -1 -&gt; probing finished 0 -&gt; no probing requested rest -&gt; perform probing with request_probe being the minimum score to accept.
        /// NOT PART OF PUBLIC API
        /// </summary>
        public int RequestProbe;
        /// <summary>
        /// Indicates that everything up to the next keyframe should be discarded.
        /// </summary>
        public int SkipToKeyFrame;
        /// <summary>
        /// The number of samples to skip at the start of the frame decoded from the next packet.
        /// </summary>
        public int SkipSamples;
        /// <summary>
        /// If not 0, the number of samples that should be skipped from the start of the stream
        /// (the samples are removed from packets with pts==0, which also assumes negative timestamps do not happen).
        /// Intended for use with formats such as mp3 with ad-hoc gapless audio support.
        /// </summary>
        public long StartSkipSamples;
        /// <summary>
        /// If not 0, the first audio sample that should be discarded from the stream.
        /// This is broken by design (needs global sample count), but can&apos;t be avoided for broken by design formats such as mp3 with ad-hoc gapless audio support.
        /// </summary>
        public long FirstDiscardSample;
        /// <summary>
        /// The sample after last sample that is intended to be discarded after first_discard_sample.
        /// Works on frame boundaries only. Used to prevent early EOF if the gapless info is broken (considered concatenated mp3s).
        /// </summary>
        public long LastDiscardSample;
        /// <summary>
        /// The number of internally decoded frames, used internally in libavformat, do not access its lifetime differs from info which is why it is not in that structure.
        /// </summary>
        public int NumberOfDecodedFrames;
        /// <summary>
        /// The timestamp offset added to timestamps before muxing.
        /// NOT PART OF PUBLIC API
        /// </summary>
        public long MuxingTimestampOffset;
        /// <summary>
        /// Internal data to check for wrapping of the time stamp.
        /// </summary>
        public long PTSWrapReference;
        /// <summary>
        /// Options for behavior, when a wrap is detected.
        /// </summary>
        public int PTSWrapBehavior;
        /// <summary>
        /// Internal data to prevent doing update_initial_durations() twice.
        /// </summary>
        public int UpdateInitialDurationsDone;
        /// <summary>
        /// Internal data to generate dts from pts.
        /// </summary>
        public LongArray17 PTSReorderError;
        /// <summary>
        /// Internal data.
        /// </summary>
        public ByteArray17 PTSReorderErrorCount;
        /// <summary>
        /// Internal data to analyze DTS and detect faulty mpeg streams.
        /// </summary>
        public long LastDTSForOrderCheck;
        /// <summary>
        /// Is DTS ordered.
        /// </summary>
        public byte DTSOrdered;
        /// <summary>
        /// Is DTS misordered.
        /// </summary>
        public byte DTSMisordered;
        /// <summary>
        /// Internal data to inject global side data.
        /// </summary>
        public int InjectGlobalSideData;
        /// <summary>
        /// The display aspect ratio (0 if unknown).
        /// </summary>
        public AVRational DisplayAspectRatio;
        /// <summary>
        /// Private data - interally.
        /// </summary>
        public IntPtr PrivateData2;
    }
}
