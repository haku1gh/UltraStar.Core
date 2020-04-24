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
    /// Represents an FFmpeg codec context.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct AVCodecContext
    {
        /// <summary>
        /// Information of this struct.
        /// </summary>
        public IntPtr AVClass;
        /// <summary>
        /// The offset to the log level.
        /// </summary>
        public int LogLevelOffset;
        /// <summary>
        /// The type of the codec.
        /// </summary>
        public AVMediaType CodecType;
        /// <summary>
        /// A pointer to the codec.
        /// </summary>
        public AVCodec* Codec;
        /// <summary>
        /// The ID of the codec.
        /// </summary>
        public int CodecID;
        /// <summary>
        /// The tag of the codec.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is used to work around some encoder bugs. A demuxer should set this to what is stored in the field used to identify the codec.
        /// If there are multiple such fields in a container then the demuxer should choose the one which maximizes the information about the used codec.
        /// If the codec tag field in a container is larger than 32 bits then the demuxer should remap the longer ID to 32 bits with a table or other structure.
        /// Alternatively a new extra_codec_tag + size could be added but for this a clear advantage must be demonstrated first.
        /// </para>
        /// <para>
        /// - encoding: Set by user, if not then the default based on codec_id will be used.
        /// </para>
        /// <para>
        /// - decoding: Set by user, will be converted to uppercase by libavcodec during init.
        /// </para>
        /// </remarks>
        public uint CodecTag;
        /// <summary>
        /// Private data - interally.
        /// </summary>
        public IntPtr PrivateData;
        /// <summary>
        /// Private data - interally.
        /// </summary>
        public IntPtr PrivateData2;
        /// <summary>
        /// Private data of the user, can be used to carry app specific stuff.
        /// </summary>
        public IntPtr PrivateData3;
        /// <summary>
        /// The average bitrate.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user; unused for constant quantizer encoding.
        /// </para>
        /// <para>
        /// - decoding: Set by user, may be overwritten by libavcodec if this info is available in the stream
        /// </para>
        /// </remarks>
        public long BitRate;
        /// <summary>
        /// The number of bits the bitstream is allowed to diverge from the reference. The reference can be CBR (for CBR pass1) or VBR (for pass2).
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user; unused for constant quantizer encoding.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int BitRateTolerance;
        /// <summary>
        /// The global quality for codecs which cannot change it per frame. This should be proportional to MPEG-1/2/4 qscale.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int GlobalQuality;
        /// <summary>
        /// The compression level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int CompressionLevel;
        /// <summary>
        /// Codec flags.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int Flags;
        /// <summary>
        /// More codec flags.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int Flags2;
        /// <summary>
        /// Additional data for codecs.
        /// </summary>
        /// <remarks>
        /// <para>
        /// </para>
        /// Some codecs need or can use extradata like Huffman tables.
        /// MJPEG: Huffman tables rv10: additional flags
        /// MPEG-4: global headers (they can be in the bitstream or here)
        /// The allocated memory should be AV_INPUT_BUFFER_PADDING_SIZE bytes larger than extradata_size to avoid problems if it is read with the bitstream reader.
        /// The bytewise contents of extradata must not depend on the architecture or CPU endianness. Must be allocated with the av_malloc() family of functions.
        /// <para>
        /// - encoding: Set/allocated/freed by libavcodec.
        /// </para>
        /// <para>
        /// - ddecoding: Set/allocated/freed by user.
        /// </para>
        /// </remarks>
        public byte* ExtraData;
        /// <summary>
        /// The size of the additional data array.
        /// </summary>
        public int ExtraDataLength;
        /// <summary>
        /// The fundamental unit of time (in seconds) in terms of which frame timestamps are represented.
        /// </summary>
        /// <remarks>
        /// For fixed-fps content, timebase should be 1/framerate and timestamp increments should be identically 1.
        /// This often, but not always is the inverse of the frame rate or field rate for video.
        /// 1/time_base is not the average frame rate if the frame rate is not constant.
        /// </remarks>
        public AVRational TimeBase;
        /// <summary>
        /// Ticks per frame.
        /// </summary>
        /// <remarks>
        /// For some codecs, the time base is closer to the field rate than the frame rate.
        /// Most notably, H.264 and MPEG-2 specify time_base as half of frame duration if no telecine is used.
        /// </remarks>
        public int TicksPerFrame;
        /// <summary>
        /// The codec delay.
        /// </summary>
        public int CodecDelay;
        /// <summary>
        /// The width of pictures/video.
        /// </summary>
        public int Width;
        /// <summary>
        /// The height of pictures/video.
        /// </summary>
        public int Height;
        /// <summary>
        /// The width of the bitstream.
        /// </summary>
        /// <remarks>
        /// May be different from <see cref="Width"/>. E.g. when the decoded frame is cropped before being output or lowres is enabled.
        /// </remarks>
        public int BitstreamWidth;
        /// <summary>
        /// The height of the bitstream.
        /// </summary>
        /// <remarks>
        /// May be different from <see cref="Height"/>. E.g. when the decoded frame is cropped before being output or lowres is enabled.
        /// </remarks>
        public int BitstreamHeight;
        /// <summary>
        /// The number of pictures in a group of pictures or 0 for intra_only.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int GOPSize;
        /// <summary>
        /// The pixel format of the codec context.
        /// </summary>
        /// <remarks>
        /// May be set by the demuxer if known from headers. May be overridden by the decoder if it knows better.
        /// </remarks>
        public AVPixelFormat PixelFormat;
        /// <summary>
        /// A pointer to draw a horizontal band.
        /// </summary>
        /// <remarks>
        /// If non NULL, &apos;draw_horiz_band&apos; is called by the libavcodec decoder to draw a horizontal band.
        /// It improves cache usage. Not all codecs can do that. You must check the codec capabilities beforehand.
        /// When multithreading is used, it may be called from multiple threads at the same time; threads might draw different parts of the same AVFrame, or
        /// multiple AVFrames, and there is no guarantee that slices will be drawn in order. The function is also used by hardware acceleration APIs.
        /// It is called at least once during frame decoding to pass the data needed for hardware render. In that mode instead of pixel data, AVFrame points to a structure specific to the acceleration API.
        /// The application reads the structure and can change some fields to indicate progress or mark state. - encoding: unused - decoding: Set by user.
        /// </remarks>
        public IntPtr DrawHorizontalBand;
        /// <summary>
        /// A callback to negotiate the pixelFormat.
        /// </summary>
        public IntPtr getPixelFormat;
        /// <summary>
        /// The maximum number of B-frames between non-B-frames.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <b>Note</b>: The output will be delayed by max_b_frames+1 relative to the input.
        /// </para>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int MaximumBFrames;
        /// <summary>
        /// The qscale factor between IP and B-frames. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// If &gt; 0 then the last P-frame quantizer will be used (q= lastp_q*factor+offset).
        /// If &lt; 0 then normal ratecontrol will be done (q= -normal_q*factor+offset).
        /// </para>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public float BQuantFactor;
        /// <summary>
        /// The B-frame strategy.
        /// </summary>
        public int BFrameStrategy;
        /// <summary>
        /// The qscale offset between IP and B-frames.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public float BQuantOffset;
        /// <summary>
        /// The size of the frame reordering buffer in the decoder.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For MPEG-2 it is 1 IPB or 0 low delay IP.
        /// </para>
        /// <para>
        /// - encoding: Set by libavcodec.
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public int ReorderingBufferLength;
        /// <summary>
        /// Deprecitated. Do not use.
        /// </summary>
        public int MpegQuant;
        /// <summary>
        /// The qscale factor between P- and I-frames.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If &gt; 0 then the last P-frame quantizer will be used (q = lastp_q * factor + offset).
        /// If &lt; 0 then normal ratecontrol will be done (q= -normal_q*factor+offset).
        /// </para>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public float IQuantFactor;
        /// <summary>
        /// The qscale offset between P and I-frames.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public float IQuantOffset;
        /// <summary>
        /// The luminance masking (0-&gt; disabled).
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public float LuminanceMasking;
        /// <summary>
        /// The temporary complexity masking (0-&gt; disabled).
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public float TemporalComplexityMasking;
        /// <summary>
        /// The spatial complexity masking (0-&gt; disabled).
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public float SpatialComplexityMasking;
        /// <summary>
        /// The p block masking (0-&gt; disabled).
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public float PBlockMasking;
        /// <summary>
        /// The darkness masking (0-&gt; disabled).
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public float DarknessMasking;
        /// <summary>
        /// The slice count.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by libavcodec.
        /// </para>
        /// <para>
        /// - decoding: Set by user (or 0).
        /// </para>
        /// </remarks>
        public int SliceCount;
        /// <summary>
        /// The prediction method.
        /// </summary>
        public int PredictionMethod;
        /// <summary>
        /// The slice offsets in the frame in bytes.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set/allocated by libavcodec.
        /// </para>
        /// <para>
        /// - decoding: Set/allocated by user (or NULL).
        /// </para>
        /// </remarks>
        public int* SliceOffset;
        /// <summary>
        /// The sample aspect ratio (or 0 if unknown). That is the width of a pixel divided by the height of the pixel.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The numerator and denominator must be relatively prime and smaller than 256 for some video standards.
        /// </para>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public AVRational SampleAspectRatio;
        /// <summary>
        /// The motion estimation comparison function.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int MotionEstimationComparison;
        /// <summary>
        /// The subpixel motion estimation comparison function.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int MotionEstimationSubPixelComparison;
        /// <summary>
        /// The macroblock comparison function (not supported yet).
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int MacroBlockComparison;
        /// <summary>
        /// The interlaced DCT comparison function.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int InterlacedDCTComparison;
        /// <summary>
        /// The motion estimation diamond size &amp; shape.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int MotionEstimationDiamondSize;
        /// <summary>
        /// The amount of previous MV predictors (2a+1 x 2a+1 square).
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int LastPredictorCount;
        /// <summary>
        /// The prepass motion estimation.
        /// </summary>
        public int PrePassMotionEstimation;
        /// <summary>
        /// The motion estimation prepass comparison function.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int PrePassMotionEstimationComparison;
        /// <summary>
        /// The motion estimation prepass diamond size &amp; shape.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int PrePassMotionEstimationDiamondSize;
        /// <summary>
        /// The subpel motion estimation quality.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int MotionEstimationSubpelQuality;
        /// <summary>
        /// The maximum motion estimation search range in subpel units. If 0 then no limit.
        /// </summary>
        public int MotionEstimationRange;
        /// <summary>
        /// The slice flags.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int SliceFlags;
        /// <summary>
        /// The macroblock decision mode.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int MacroBlockDecisionMode;
        /// <summary>
        /// The custom intra quantization matrix.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Must be allocated with the av_malloc() family of functions, and will be freed in avcodec_free_context().
        /// </para>
        /// <para>
        /// - encoding: Set/allocated by user, freed by libavcodec. Can be <see langword="null"/>.
        /// </para>
        /// <para>
        /// - decoding: Set/allocated/freed by libavcodec.
        /// </para>
        /// </remarks>
        public ushort* IntraQuantizationMatrix;
        /// <summary>
        /// The custom inter quantization matrix.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Must be allocated with the av_malloc() family of functions, and will be freed in avcodec_free_context().
        /// </para>
        /// <para>
        /// - encoding: Set/allocated by user, freed by libavcodec. Can be <see langword="null"/>.
        /// </para>
        /// <para>
        /// - decoding: Set/allocated/freed by libavcodec.
        /// </para>
        /// </remarks>
        public ushort* InterQuantizationMatrix;
        /// <summary>
        /// Deprecated: The scene change threshold.
        /// </summary>
        public int SceneChangeThreshold;
        /// <summary>
        /// Deprecated: The noise reduction.
        /// </summary>
        public int NoiseReduction;
        /// <summary>
        /// The precision of the intra DC coefficient.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public int IntraDCPrecision;
        /// <summary>
        /// The number of macroblock rows at the top which are skipped.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int SkipTop;
        /// <summary>
        /// The number of macroblock rows at the bottom which are skipped.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int SkipBottom;
        /// <summary>
        /// The minimum macro block Lagrange multiplier.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int MacroBlockMinimumLagrangeMultiplier;
        /// <summary>
        /// The maximum macro block Lagrange multiplier.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int MacroBlockMaximumLagrangeMultiplier;
        /// <summary>
        /// Deprecated: The motion estimation penalty compensation.
        /// </summary>
        public int MotionEstimationPenaltyCompensation;
        /// <summary>
        /// Bidir refine.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int BidirRefine;
        /// <summary>
        /// Deprecated.
        /// </summary>
        public int BrdScale;
        /// <summary>
        /// The minimum GOP size.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int MinimumGOPSize;
        /// <summary>
        /// The number of reference frames.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by lavc.
        /// </para>
        /// </remarks>
        public int NumberOfReferenceFrames;
        /// <summary>
        /// Deprecated: The chroma offset.
        /// </summary>
        public int ChromaOffset;
        /// <summary>
        /// The MV0 threshold.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <b>Note</b>: Value depends upon the compare function used for fullpel ME.
        /// </para>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int MV0Threshold;
        /// <summary>
        /// Deprecated: B-Frame sensitivity.
        /// </summary>
        public int BSensitivity;
        /// <summary>
        /// The chromaticity coordinates of the source primaries.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public int ColorPrimaries;
        /// <summary>
        /// The color Transfer Characteristic.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public int ColorTransferCharacteristic;
        /// <summary>
        /// The YUV colorspace type.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public int ColorSpace;
        /// <summary>
        /// The MPEG vs JPEG YUV color range.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public int ColorRange;
        /// <summary>
        /// The location of chroma samples.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public int ChromaSampleLocation;
        /// <summary>
        /// The number of slices.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Indicates the number of picture subdivisions. Used for parallelized decoding.
        /// </para>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int Slices;
        /// <summary>
        /// The field order.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by libavcodec.
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int FieldOrder;
        /// <summary>
        /// The samples per second. (Audio only)
        /// </summary>
        public int SampleRate;
        /// <summary>
        /// The number of audio channels. (Audio only)
        /// </summary>
        public int Channels;
        /// <summary>
        /// The sample format. (Audio only)
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public AVSampleFormat SampleFormat;
        /// <summary>
        /// The number of samples per channel in an audio frame.
        /// </summary>
        public int FrameSize;
        /// <summary>
        /// The frame counter, set by libavcodec.
        /// </summary>
        public int FrameNumber;
        /// <summary>
        /// The number of bytes per packet if constant and known or 0.
        /// </summary>
        /// <remarks>
        /// Used by some WAV based audio codecs.
        /// </remarks>
        public int BlockAlign;
        /// <summary>
        /// The audio cutoff bandwidth (0 means &quot;automatic&quot;).
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int CutOffBandwidth;
        /// <summary>
        /// The audio channel layout.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by user, may be overwritten by libavcodec.
        /// </para>
        /// </remarks>
        public ulong ChannelLayout;
        /// <summary>
        /// Request the decoder to use this channel layout if it can (0 for default).
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public ulong RequestChannelLayout;
        /// <summary>
        /// The type of service that the audio stream conveys.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public int AudioServiceType;
        /// <summary>
        /// Request the decoder to use this sample format.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user. Decoder will decode to this format if it can.
        /// </para>
        /// </remarks>
        public AVSampleFormat RequestSampleFormat;
        /// <summary>
        /// A callback to get the data buffers.
        /// </summary>
        public IntPtr getBuffer2;
        /// <summary>
        /// Deprecated: Indicator for reference counted frames.
        /// </summary>
        /// <remarks>
        /// If non-zero, the decoded audio and video frames returned from avcodec_decode_video2() and avcodec_decode_audio4() are reference-counted and are valid indefinitely.
        /// The caller must free them with av_frame_unref() when they are not needed anymore.
        /// Otherwise, the decoded frames must not be freed by the caller and are only valid until the next decode call.
        /// </remarks>
        public int ReferenceCountedFrames;
        /// <summary>
        /// The amount of qscale change between easy &amp; hard scenes (0.0-1.0).
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public float QCompress;
        /// <summary>
        /// The amount of qscale smoothing over time (0.0-1.0).
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public float QBlur;
        /// <summary>
        /// The minimum quantizer.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int QMin;
        /// <summary>
        /// The maximum quantizer.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int QMax;
        /// <summary>
        /// The maximum quantizer difference between frames.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int QDiffMax;
        /// <summary>
        /// The decoder bitstream buffer size.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int RateControlBufferSize;
        /// <summary>
        /// The ratecontrol override count.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Allocated/set/freed by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int RateControlOverrideCount;
        /// <summary>
        /// The ratecontrol override.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Allocated/set/freed by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public IntPtr RateControlOverride;
        /// <summary>
        /// The maximum ratecontrol bitrate.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by user, may be overwritten by libavcodec.
        /// </para>
        /// </remarks>
        public long RateControlMaximumBitRate;
        /// <summary>
        /// The minimum ratecontrol bitrate.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public long RateControlMinimumBitRate;
        /// <summary>
        /// The ratecontrol attempt to use, at maximum, &lt;value&gt; of what can be used without an underflow.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public float RateControlMaxAvailableVBVUse;
        /// <summary>
        /// The ratecontrol attempt to use, at least, &lt;value&gt; times the amount needed to prevent a vbv overflow.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public float RateControlMinVBVOverflowUse;
        /// <summary>
        /// The number of bits which should be loaded into the rc buffer before decoding starts.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int RateControlInitialBufferOccupancy;
        /// <summary>
        /// Deprecated: The coder type.
        /// </summary>
        public int CoderType;
        /// <summary>
        /// Deprecated: The context model.
        /// </summary>
        public int ContextModel;
        /// <summary>
        /// Deprecated: The frame skip threshold.
        /// </summary>
        public int FrameSkipThreshold;
        /// <summary>
        /// Deprecated: The frame skip factor.
        /// </summary>
        public int FrameSkipFactor;
        /// <summary>
        /// Deprecated: The frame skip exp.
        /// </summary>
        public int FrameSkipExp;
        /// <summary>
        /// Deprecated: The frame skip cmp.
        /// </summary>
        public int FrameSkipCmp;
        /// <summary>
        /// The trellis RD quantization.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int Trellis;
        /// <summary>
        /// Deprecated: The minimum prediction order.
        /// </summary>
        public int MinPredictionOrder;
        /// <summary>
        /// Deprecated: The maximum prediction order.
        /// </summary>
        public int MaxPredictionOrder;
        /// <summary>
        /// Deprecated: The timecode frame start.
        /// </summary>
        public long TimecodeFrameStart;
        /// <summary>
        /// Deprecated: A callback for RTP.
        /// </summary>
        public IntPtr rtpCallback;
        /// <summary>
        /// Deprecated: The size of the RTP payload.
        /// </summary>
        public int RTPPayloadSize;
        /// <summary>
        /// Deprecated: Statistics mv bits.
        /// </summary>
        public int StatisticsMVBits;
        /// <summary>
        /// Deprecated: Statistics header bits.
        /// </summary>
        public int StatisticsHeaderBits;
        /// <summary>
        /// Deprecated: Statistics i tex bits.
        /// </summary>
        public int StatisticsITexBits;
        /// <summary>
        /// Deprecated: Statistics p tex bits.
        /// </summary>
        public int StatisticsPTexBits;
        /// <summary>
        /// Deprecated: Statistics i count.
        /// </summary>
        public int StatisticsICount;
        /// <summary>
        /// Deprecated: Statistics p count.
        /// </summary>
        public int StatisticsPCount;
        /// <summary>
        /// Deprecated: Statistics skip count.
        /// </summary>
        public int StatisticsSkipCount;
        /// <summary>
        /// Deprecated: Statistics misc bits.
        /// </summary>
        public int StatisticsMiscBits;
        /// <summary>
        /// Deprecated: Statistics frame bits.
        /// </summary>
        public int StatisticsFrameBits;
        /// <summary>
        /// The pass1 encoding statistics output buffer.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by libavcodec.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public byte* StatisticsOut;
        /// <summary>
        /// The pass2 encoding statistics input buffer Concatenated stuff from stats_out of pass1 should be placed here.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Allocated/set/freed by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public byte* StatisticsIn;
        /// <summary>
        /// Work around bugs in encoders which sometimes cannot be detected automatically.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int WorkAroundBugs;
        /// <summary>
        /// Strictly follow the standard (MPEG-4, ...).
        /// </summary>
        /// <remarks>
        /// <para>
        /// Setting this to STRICT or higher means the encoder and decoder will generally do stupid things,
        /// whereas setting it to unofficial or lower will mean the encoder might produce output that is not supported by all spec-compliant decoders.
        /// Decoders don&apos;t differentiate between normal, unofficial and experimental
        /// (that is, they always try to decode things when they can) unless they are explicitly asked to behave stupidly (=strictly conform to the specs)
        /// </para>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int StrictStandardCompliance;
        /// <summary>
        /// The error concealment flags.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int ErrorConcealmentFlags;
        /// <summary>
        /// The debug flags.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int DebugFlags;
        /// <summary>
        /// The error recognition; may misdetect some more or less valid parts as errors.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int ErrorRecognition;
        /// <summary>
        /// An opaque 64-bit number (generally a PTS) that will be reordered and output in <see cref="AVFrame.ReorderedOpaque"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by libavcodec to the reordered_opaque of the input frame corresponding to the last returned packet. Only supported by encoders with the AV_CODEC_CAP_ENCODER_REORDERED_OPAQUE capability.
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public long ReorderedOpaque;
        /// <summary>
        /// A pointer to the hardware accelerator in use.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public IntPtr HWAccelerator;
        /// <summary>
        /// The hardware accelerator context.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For some hardware accelerators, a global context needs to be provided by the user.
        /// In that case, this holds display-dependent data FFmpeg cannot instantiate itself.
        /// Please refer to the FFmpeg HW accelerator documentation to know how to fill this is. e.g. for VA API, this is a struct vaapi_context.
        /// </para>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public void* HWAcceleratorContext;
        /// <summary>
        /// An error array.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by libavcodec if flags &amp; AV_CODEC_FLAG_PSNR.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public UlongArray8 Error;
        /// <summary>
        /// The DCT algorithm.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int DCTAlgorithm;
        /// <summary>
        /// The IDCT algorithm.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int IDCTAlgorithm;
        /// <summary>
        /// The bits per sample/pixel from the demuxer (needed for huffyuv).
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by libavcodec.
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int BitsPerCodedSample;
        /// <summary>
        /// The bits per sample/pixel of internal libavcodec pixel/sample format.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public int BitsPerRawSample;
        /// <summary>
        /// The low resolution decoding, 1-&gt; 1/2 size, 2-&gt;1/4 size.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int LowResSize;
        /// <summary>
        /// Deprecated: The picture in the bitstream.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by libavcodec.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public AVFrame* CodedFrame;
        /// <summary>
        /// The thread count.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int ThreadCount;
        /// <summary>
        /// The threading type.
        /// </summary>
        /// <remarks>
        /// Which multithreading methods to use. Use of FF_THREAD_FRAME will increase decoding delay by one frame per thread, so clients which cannot provide future frames should not use it.
        /// </remarks>
        public int ThreadType;
        /// <summary>
        /// The threading type in use by the codec.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by libavcodec.
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public int ActiveThreadType;
        /// <summary>
        /// Indicator if callbacks are thread safe.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Set by the client if its custom get_buffer() callback can be called synchronously from another thread, which allows faster multithreaded decoding.
        /// draw_horiz_band() will be called from other threads regardless of this setting. Ignored if the default get_buffer() is used.
        /// </para>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int ThreadSafeCallbacks;
        /// <summary>
        /// A callback for execute.
        /// </summary>
        public IntPtr execute;
        /// <summary>
        /// A callback for execute2.
        /// </summary>
        public IntPtr execute2;
        /// <summary>
        /// The noise vs. sse weight for the nsse comparison function.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int NsseWeight;
        /// <summary>
        /// The codec profile.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public int Profile;
        /// <summary>
        /// The codec level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public int Level;
        /// <summary>
        /// Skip loop filtering for selected frames.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int SkipLoopFilter;
        /// <summary>
        /// Skip IDCT/dequantization for selected frames.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int SkipIDCT;
        /// <summary>
        /// Skip decoding for selected frames.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int SkipFrame;
        /// <summary>
        /// The header containing style information for text subtitles.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For SUBTITLE_ASS subtitle type, it should contain the whole ASS [Script Info] and [V4+ Styles] section, plus the [Events] line and the Format line following.
        /// It shouldn&apos;t include any Dialogue line.
        /// </para>
        /// <para>
        /// - encoding: Set/allocated/freed by user (before <see cref="FFmpeg.AVCodecOpen"/>)
        /// </para>
        /// <para>
        /// - decoding: Set/allocated/freed by libavcodec (by <see cref="FFmpeg.AVCodecOpen"/>)
        /// </para>
        /// </remarks>
        public byte* SubtitleHeader;
        /// <summary>
        /// The size of the subtitle header object.
        /// </summary>
        public int SubtitleHeaderSize;
        /// <summary>
        /// The VBV delay coded in the last frame (in periods of a 27 MHz clock). Used for compliant TS muxing.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by libavcodec.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public ulong VBVDelay;
        /// <summary>
        /// Allow encoders to output packets that do not contain any encoded data, only side data.
        /// </summary>
        /// <remarks>
        /// Encoding only and set by default.
        /// </remarks>
        public int SideDataOnlyPackets;
        /// <summary>
        /// The number of &quot;priming&quot; samples (padding) inserted by the encoder at the beginning of the audio.
        /// </summary>
        /// <remarks>
        /// I.e. this number of leading decoded samples must be discarded by the caller to get the original audio without leading padding.
        /// </remarks>
        public int InitialAudioPadding;
        /// <summary>
        /// The frame rate.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: May be used to signal the frame rate of CFR content to an encoder.
        /// </para>
        /// <para>
        /// - decoding: For codecs that store a frame rate value in the compressed bitstream, the decoder may export it here. { 0, 1} when unknown.
        /// </para>
        /// </remarks>
        public AVRational FrameRate;
        /// <summary>
        /// The nominal unaccelerated pixel format.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec before calling get_format().
        /// </para>
        /// </remarks>
        public AVPixelFormat SWPixelFormat;
        /// <summary>
        /// The timebase in which pkt_dts/pts and AVPacket.dts/pts are.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public AVRational PacketTimebase;
        /// <summary>
        /// The codec descriptor.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public IntPtr CodecDescriptor;
        /// <summary>
        /// The number of incorrect PTS values so far.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Maintained and used by libavcodec, not intended to be used by user apps.
        /// </para>
        /// </remarks>
        public long pts_correction_num_faulty_pts;
        /// <summary>
        /// The number of incorrect DTS values so far.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Maintained and used by libavcodec, not intended to be used by user apps.
        /// </para>
        /// </remarks>
        public long pts_correction_num_faulty_dts;
        /// <summary>
        /// The PTS of the last frame.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Maintained and used by libavcodec, not intended to be used by user apps.
        /// </para>
        /// </remarks>
        public long pts_correction_last_pts;
        /// <summary>
        /// The DTS of the last frame.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Maintained and used by libavcodec, not intended to be used by user apps.
        /// </para>
        /// </remarks>
        public long pts_correction_last_dts;
        /// <summary>
        /// The character encoding of the input subtitles file.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public byte* SubtitleCharacterEncoding;
        /// <summary>
        /// The subtitles character encoding mode. Formats or codecs might be adjusting this setting (if they are doing the conversion themselves for instance).
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public int SubtitleCharacterEncodingMode;
        /// <summary>
        /// Skip processing alpha if supported by codec. 
        /// </summary>
        /// <remarks>
        /// Note that if the format uses pre-multiplied alpha (common with VP6, and recommended due to better video quality/compression)
        /// the image will look as if alpha-blended onto a black background.
        /// However for formats that do not use pre-multiplied alpha there might be serious artefacts (though e.g. libswscale currently assumes pre-multiplied alpha anyway).
        /// </remarks>
        public int SkipAlpha;
        /// <summary>
        /// The number of samples to skip after a discontinuity.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by libavcodec.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public int SeekPreroll;
        /// <summary>
        /// The debug motion vectors.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int DebugMotionVectors;
        /// <summary>
        /// The custom intra quantization matrix.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user, can be <see langword="null"/>.
        /// </para>
        /// <para>
        /// - decoding: unused
        /// </para>
        /// </remarks>
        public ushort* ChromaIntraMatrix;
        /// <summary>
        /// The dump format separator. Can be &quot;, &quot; or &quot; &quot; or anything else.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public byte* DumpSeparator;
        /// <summary>
        /// A &apos;,&apos; separated list of allowed decoders. If <see langword="null"/> then all are allowed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public byte* CodecWhitelist;
        /// <summary>
        /// The properties of the stream that gets decoded.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by libavcodec.
        /// </para>
        /// </remarks>
        public uint Properties;
        /// <summary>
        /// Additional data associated with the entire coded stream.
        /// </summary>
        public IntPtr CodedSideData;
        /// <summary>
        /// Number of additional coded side data.
        /// </summary>
        public int NumberOfCodedSideData;
        /// <summary>
        /// A reference to the AVHWFramesContext describing the input (for encoding) or output (decoding) frames.
        /// </summary>
        /// <remarks>
        /// The reference is set by the caller and afterwards owned (and freed) by libavcodec - it should never be read by the caller after being set.
        /// </remarks>
        public IntPtr HWFramesCTX;
        /// <summary>
        /// Controls the form of subtitles.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int SubtitleTextFormat;
        /// <summary>
        /// The amount of padding (in samples) appended by the encoder to the end of the audio.
        /// </summary>
        /// <remarks>
        /// I.e. this number of decoded samples must be discarded by the caller from the end of the stream to get the original audio without any trailing padding.
        /// </remarks>
        public int TrailingAudioPadding;
        /// <summary>
        /// The number of pixels per image to maximally accept.
        /// </summary>
        public long MaximumNumberOfPixels;
        /// <summary>
        /// A reference to the AVHWDeviceContext describing the device which will be used by a hardware encoder/decoder.
        /// </summary>
        /// <remarks>
        /// The reference is set by the caller and afterwards owned (and freed) by libavcodec.
        /// </remarks>
        public IntPtr HWDeviceCTX;
        /// <summary>
        /// Bit set of AV_HWACCEL_FLAG_* flags, which affect hardware accelerated decoding (if active).
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user (before <see cref="FFmpeg.AVCodecOpen"/>).
        /// </para>
        /// </remarks>
        public int HWAccelerationFlags;
        /// <summary>
        /// Controls how cropping is handled by libavcodec.
        /// </summary>
        /// <remarks>
        /// Video decoding only. Certain video codecs support cropping, meaning that only a sub-rectangle of the decoded frame is intended for display.
        /// </remarks>
        public int ApplyCropping;
        /// <summary>
        /// Sets the number of extra hardware frames which the decoder will allocate for use by the caller.
        /// </summary>
        /// <remarks>
        /// Video decoding only. This must be set before <see cref="FFmpeg.AVCodecOpen"/> is called.
        /// Some hardware decoders require all frames that they will use for output to be defined in advance before decoding starts.
        /// For such decoders, the hardware frame pool must therefore be of a fixed size.
        /// The extra frames set here are on top of any number that the decoder needs internally in order to operate normally
        /// (for example, frames used as reference pictures).
        /// </remarks>
        public int ExtraHWFrames;
        /// <summary>
        /// The percentage of damaged samples to discard a frame.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: unused
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int DiscardDamagedPercentage;
        /// <summary>
        /// The number of samples per frame to maximally accept.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public long MaxSamples;
        /// <summary>
        /// Bit set of AV_CODEC_EXPORT_DATA_* flags, which affects the kind of metadata exported in frame, packet, or coded stream side data by decoders and encoders.
        /// </summary>
        /// <remarks>
        /// <para>
        /// - encoding: Set by user.
        /// </para>
        /// <para>
        /// - decoding: Set by user.
        /// </para>
        /// </remarks>
        public int ExportSideData;
    }
}
