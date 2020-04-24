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
    /// This structure describes decoded (raw) audio or video data.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="AVFrame"/> must be allocated using <see cref="FFmpeg.AVFrameAlloc"/>. Note that this only allocates the <see cref="AVFrame"/> itself, the buffers for the data must be managed through other means (see below).
    /// <see cref="AVFrame"/> must be freed with <see cref="FFmpeg.AVFrameFree"/>.
    /// </para>
    /// <para>
    /// <see cref="AVFrame"/> is typically allocated once and then reused multiple times to hold different data (e.g. a single <see cref="AVFrame"/> to hold frames received from a decoder).
    /// In such a case, <see cref="FFmpeg.AVFrameUnref"/> will free any references held by the frame and reset it to its original clean state before it is reused again.
    /// </para>
    /// <para>
    /// The data described by an <see cref="AVFrame"/> is usually reference counted through the AVBuffer API. The underlying buffer references are stored in <see cref="AVFrame.Buf"/> and <see cref="AVFrame.ExtendedBuf"/>.
    /// An <see cref="AVFrame"/> is considered to be reference counted if at least one reference is set, i.e. if <see cref="AVFrame.Buf"/>[0] != <see langword="null"/>.
    /// In such a case, every single data plane must be contained in one of the buffers in <see cref="AVFrame.Buf"/> or <see cref="AVFrame.ExtendedBuf"/>.
    /// There may be a single buffer for all the data, or one separate buffer for each plane, or anything in between.
    /// </para>
    /// <para>
    /// sizeof(<see cref="AVFrame"/>) is not a part of the public ABI, so new fields may be added to the end with a minor bump.
    /// </para>
    /// <para>
    /// Fields can be accessed through AVOptions, the name string used, matches the C structure field name for fields accessible through AVOptions.
    /// The AVClass for <see cref="AVFrame"/> can be obtained from avcodec_get_frame_class().
    /// </para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct AVFrame
    {
        /// <summary>
        /// The pointer to the picture/channel planes. This might be different from the first allocated byte.
        /// </summary>
        public IntPtrArray8 Data;
        /// <summary>
        /// For video, size in bytes of each picture line. For audio, size in bytes of each plane.
        /// </summary>
        public IntArray8 Linesize;
        /// <summary>
        /// Pointers to the data planes/channels.
        /// </summary>
        public byte** ExtendedData;
        /// <summary>
        /// Video frames only. The coded dimensions (in pixels) of the video frame, i.e. the size of the rectangle that contains some well-defined values.
        /// </summary>
        public int Width;
        /// <summary>
        /// Video frames only. The coded dimensions (in pixels) of the video frame, i.e. the size of the rectangle that contains some well-defined values.
        /// </summary>
        public int Height;
        /// <summary>
        /// THe number of audio samples (per channel) described by this frame.
        /// </summary>
        public int SamplesCount;
        /// <summary>
        /// The format of the frame, -1 if unknown or unset Values correspond to enum AVPixelFormat for video frames, enum AVSampleFormat for audio.
        /// </summary>
        public int Format;
        /// <summary>
        /// 1 -&gt; keyframe, 0-&gt; not.
        /// </summary>
        public int KeyFrame;
        /// <summary>
        /// The picture type of the frame.
        /// </summary>
        public int PictureType;
        /// <summary>
        /// The sample aspect ratio for the video frame, 0/1 if unknown/unspecified.
        /// </summary>
        public AVRational SampleAspectRatio;
        /// <summary>
        /// The presentation timestamp in time_base units (time when frame should be shown to user).
        /// </summary>
        public long PTS;
        /// <summary>
        /// Deprecated: The PTS copied from the <see cref="AVPacket"/> that was decoded to produce this frame.
        /// </summary>
        /// <remarks>
        /// Use the pts field instead.
        /// </remarks>
        public long PacketPTS;
        /// <summary>
        /// The DTS copied from the <see cref="AVPacket"/> that triggered returning this frame.
        /// </summary>
        /// <remarks>
        /// (if frame threading isn&apos;t used) This is also the Presentation time of this AVFrame calculated from only AVPacket.dts values without pts values.
        /// </remarks>
        public long PacketDTS;
        /// <summary>
        /// The picture number in bitstream order.
        /// </summary>
        public int CodedPictureNumber;
        /// <summary>
        /// The picture number in display order.
        /// </summary>
        public int DisplayPictureNumber;
        /// <summary>
        /// The quality (between 1 (good) and FF_LAMBDA_MAX (bad)).
        /// </summary>
        public int Quality;
        /// <summary>
        /// Private data of the user, can be used to carry app specific stuff.
        /// </summary>
        public IntPtr PrivateData3;
        /// <summary>
        /// Deprecated: Unused.
        /// </summary>
        public UlongArray8 Error;
        /// <summary>
        /// When decoding, this signals how much the picture must be delayed. extra_delay = repeat_pict / (2*fps).
        /// </summary>
        public int RepeatPict;
        /// <summary>
        /// An indicator that the content of the picture is interlaced.
        /// </summary>
        public int InterlacedFrame;
        /// <summary>
        /// If the content is interlaced, is top field displayed first.
        /// </summary>
        public int TopFieldFirst;
        /// <summary>
        /// Tell the user application that palette has changed from previous frame.
        /// </summary>
        public int PaletteHasChanged;
        /// <summary>
        /// Reordered opaque 64 bits (generally an integer or a double precision float PTS but can be anything).
        /// </summary>
        public long ReorderedOpaque;
        /// <summary>
        /// The sample rate of the audio data.
        /// </summary>
        public int SampleRate;
        /// <summary>
        /// The channel layout of the audio data.
        /// </summary>
        public ulong ChannelLayout;
        /// <summary>
        /// AVBuffer references backing the data for this frame.
        /// </summary>
        /// <remarks>
        /// If all elements of this array are NULL, then this frame is not reference counted.
        /// This array must be filled contiguously -- if buf[i] is non-NULL then buf[j] must also be non-NULL for all j &lt; i.
        /// </remarks>
        public IntPtrArray8 Buf;
        /// <summary>
        /// For planar audio which requires more than AV_NUM_DATA_POINTERS AVBufferRef pointers, this array will hold all the references which cannot fit into AVFrame.buf.
        /// </summary>
        public IntPtr ExtendedBuf;
        /// <summary>
        /// The number of elements in <see cref="ExtendedBuf"/>.
        /// </summary>
        public int ExtendedBufCount;
        /// <summary>
        /// A pointer to the side data.
        /// </summary>
        public IntPtr SideData;
        /// <summary>
        /// Number of elements in <see cref="SideData"/>.
        /// </summary>
        public int SideDataCount;
        /// <summary>
        /// Frame flags.
        /// </summary>
        public int Flags;
        /// <summary>
        /// MPEG vs JPEG YUV color range.
        /// </summary>
        public int ColorRange;
        /// <summary>
        /// The chromaticity coordinates of the source primaries.
        /// </summary>
        public int ColorPrimaries;
        /// <summary>
        /// The color Transfer Characteristic.
        /// </summary>
        public int ColorTransferCharacteristic;
        /// <summary>
        /// The YUV colorspace type.
        /// </summary>
        public int ColorSpace;
        /// <summary>
        /// The location of chroma samples.
        /// </summary>
        public int ChromaLocation;
        /// <summary>
        /// The frame timestamp estimated using various heuristics, in stream time base.
        /// </summary>
        public long BestEffortTimestamp;
        /// <summary>
        /// Reordered pos from the last <see cref="AVPacket"/> that has been input into the decoder.
        /// </summary>
        public long PacketPosition;
        /// <summary>
        /// The duration of the corresponding packet, expressed in AVStream-&gt;time_base units, 0 if unknown.
        /// </summary>
        public long PacketDuration;
        /// <summary>
        /// The frames Metadata.
        /// </summary>
        public IntPtr Metadata;
        /// <summary>
        /// The decode error flags of the frame.
        /// </summary>
        public int DecodeErrorFlags;
        /// <summary>
        /// The number of audio channels, only used for audio.
        /// </summary>
        public int Channels;
        /// <summary>
        /// The size of the corresponding packet containing the compressed frame. It is set to a negative value if unknown.
        /// </summary>
        public int PacketSize;
        /// <summary>
        /// Deprecated: QP table.
        /// </summary>
        public sbyte* QScaleTable;
        /// <summary>
        ///Deprecated:  QP store stride.
        /// </summary>
        public int QStride;
        /// <summary>
        /// Deprecated: QP table type.
        /// </summary>
        public int QScaleType;
        /// <summary>
        /// Deprecated: QP table buffer.
        /// </summary>
        public IntPtr QpTableBuffer;
        /// <summary>
        /// For hwaccel-format frames, this should be a reference to the AVHWFramesContext describing the frame.
        /// </summary>
        public IntPtr HWFramesCTX;
        /// <summary>
        /// AVBufferRef for free use by the API user.
        /// </summary>
        public IntPtr OpaqueRef;
        /// <summary>
        /// Cropping Video frames only. The number of pixels to discard from the the top/bottom/left/right border
        /// of the frame to obtain the sub-rectangle of the frame intended for presentation.
        /// </summary>
        public ulong CropTop;
        /// <summary>
        /// Cropping Video frames only. The number of pixels to discard from the the top/bottom/left/right border
        /// of the frame to obtain the sub-rectangle of the frame intended for presentation.
        /// </summary>
        public ulong CropBottom;
        /// <summary>
        /// Cropping Video frames only. The number of pixels to discard from the the top/bottom/left/right border
        /// of the frame to obtain the sub-rectangle of the frame intended for presentation.
        /// </summary>
        public ulong CropLeft;
        /// <summary>
        /// Cropping Video frames only. The number of pixels to discard from the the top/bottom/left/right border
        /// of the frame to obtain the sub-rectangle of the frame intended for presentation.
        /// </summary>
        public ulong CropRight;
        /// <summary>
        /// Private data - interally.
        /// </summary>
        public IntPtr PrivateData;
    }
}
