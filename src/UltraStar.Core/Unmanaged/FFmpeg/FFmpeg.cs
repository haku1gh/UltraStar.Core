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
using UltraStar.Core.Utils;

namespace UltraStar.Core.Unmanaged.FFmpeg
{
    /// <summary>
    /// Represents a wrapper class around the FFmpeg library.
    /// </summary>
    /// <remarks>
    /// FFmpeg consists of a bunch of library files. This library uses 5 of the FFmpeg library, even though not all are addressed here.
    /// This is due to the case, that the libraries have dependencies between them. Therefore all libraries are loaded at once.
    /// For reference the dependencies are the following:
    /// avutil     ==depends on==> -
    /// swresample ==depends on==> avutil
    /// swscale    ==depends on==> avutil
    /// avcodec    ==depends on==> avutil and swresample
    /// avformat   ==depends on==> avutil and avcodec
    /// </remarks>
    internal static class FFmpeg
    {
        /// <summary>
        /// Handle to the library.
        /// </summary>
        private static readonly IntPtr libraryHandleAvUtil;
        private static readonly IntPtr libraryHandleSwResample;
        private static readonly IntPtr libraryHandleSwScale;
        private static readonly IntPtr libraryHandleAvCodec;
        private static readonly IntPtr libraryHandleAvFormat;

        private static readonly int supportedAvUtilVersion = 56;
        private static readonly int supportedSwResampleVersion = 3;
        private static readonly int supportedSwScaleVersion = 5;
        private static readonly int supportedAvCodecVersion = 58;
        private static readonly int supportedAvFormatVersion = 58;

        /// <summary>
        /// Initializes <see cref="FFmpeg"/>.
        /// </summary>
        static FFmpeg()
        {
            // Load the library
            libraryHandleAvUtil     = LibraryLoader.LoadNativeLibraryAsPerConfig(getFullLibraryName("avutil"    , supportedAvUtilVersion));
            libraryHandleSwResample = LibraryLoader.LoadNativeLibraryAsPerConfig(getFullLibraryName("swresample", supportedSwResampleVersion));
            libraryHandleSwScale    = LibraryLoader.LoadNativeLibraryAsPerConfig(getFullLibraryName("swscale"   , supportedSwScaleVersion));
            libraryHandleAvCodec    = LibraryLoader.LoadNativeLibraryAsPerConfig(getFullLibraryName("avcodec"   , supportedAvCodecVersion));
            libraryHandleAvFormat   = LibraryLoader.LoadNativeLibraryAsPerConfig(getFullLibraryName("avformat"  , supportedAvFormatVersion));
        }

        /// <summary>
        /// Gets the full library name, specific for the current running platform.
        /// </summary>
        private static string getFullLibraryName(string libraryName, int version)
        {
            string fullLibraryName = "";
            switch (SystemInformation.Platform)
            {
                case Platform.Mac:
                    fullLibraryName = "lib" + libraryName + "." + version + ".dylib";
                    break;
                case Platform.Linux:
                    fullLibraryName = "lib" + libraryName + ".so." + version;
                    break;
                case Platform.Windows:
                    fullLibraryName = libraryName + "-" + version + ".dll";
                    break;
            }
            return fullLibraryName;
        }

        /// <summary>
        /// Internal time base represented as integer.
        /// </summary>
        public static readonly int AV_TIMEBASE = 1000000;

        /// <summary>
        /// Error: End of file.
        /// </summary>
        public static readonly int AV_ERROR_EOF = -541478725;

        /// <summary>
        /// Error: EAGAIN.
        /// </summary>
        public static readonly int AV_ERROR_EAGAIN = -11;

        /// <summary>
        /// Error: ENOMEM.
        /// </summary>
        public static readonly int AV_ERROR_ENOMEM = -12;

        /// <summary>
        /// Error: EINVAL.
        /// </summary>
        public static readonly int AV_ERROR_EINVAL = -22;

        /// <summary>Converts an <see cref="AVRational"/> to a <see cref="double"/>.</summary>
        /// <param name="a">The <see cref="AVRational"/> to convert.</param>
        /// <returns>The converted value.</returns>
        public static double AVConvertRational(AVRational a)
        {
            return a.Num / (double)a.Den;
        }

        #region avutil

        /// <summary>
        /// Delegate for av_version_info.
        /// </summary>
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate string ffmpeg_av_version_info_delegate();
        /// <summary>
        /// Returns an informative version string.
        /// </summary>
        /// <remarks>
        /// This usually is the actual release version number or a git commit description. This string has no fixed format and can change any time. It should never be parsed by code.
        /// </remarks>
        /// <returns>A version string.</returns>
        public static string AVVersionInfo()
        {
            ffmpeg_av_version_info_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_version_info_delegate>(libraryHandleAvUtil, "av_version_info");
            return del();
        }

        /// <summary>
        /// Delegate for av_log_get_level.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate FFmpegLogLevel ffmpeg_av_log_get_level_delegate();
        /// <summary>
        /// Delegate for av_log_set_level.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate void ffmpeg_av_log_set_level_delegate(FFmpegLogLevel level);
        /// <summary>
        /// Gets or sets the log level for the FFmpeg library.
        /// </summary>
        public static FFmpegLogLevel AVLogLevel
        {
            get
            {
                ffmpeg_av_log_get_level_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_log_get_level_delegate>(libraryHandleAvUtil, "av_log_get_level");
                return del();
            }
            set
            {
                ffmpeg_av_log_set_level_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_log_set_level_delegate>(libraryHandleAvUtil, "av_log_set_level");
                del(value);
            }
        }

        /// <summary>
        /// Delegate for av_strerror.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_av_strerror_delegate(int errnum, byte* errbuf, ulong errbuf_size);
        /// <summary>
        /// Gets an error description from an FFmpeg error code.
        /// </summary>
        /// <param name="error">The FFmpeg error code.</param>
        /// <returns>A description of the error code.</returns>
        public unsafe static string AVGetErrorDescription(int error)
        {
            ffmpeg_av_strerror_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_strerror_delegate>(libraryHandleAvUtil, "av_strerror");
            int bufferSize = 1024;
            byte* buffer = stackalloc byte[bufferSize];
            if (del(error, buffer, (ulong)bufferSize) != 0)
                return "Unknown Error";
            else
                return Marshal.PtrToStringAnsi((IntPtr)buffer);
        }

        /// <summary>
        /// Delegate for av_log_set_callback.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate void ffmpeg_av_log_set_callback_delegate(AvLogSetCallback callback);
        /// <summary>
        /// Sets the logging callback.
        /// </summary>
        /// <remarks>
        /// The callback must be thread safe, even if the application does not use threads itself as some codecs are multithreaded.
        /// </remarks>
        /// <param name="callback">The callback must be thread safe, even if the application does not use threads itself as some codecs are multithreaded.</param>
        public static void AVSetLogCallback(AvLogSetCallback callback)
        {
            ffmpeg_av_log_set_callback_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_log_set_callback_delegate>(libraryHandleAvUtil, "av_log_set_callback");
            del(callback);
        }

        /// <summary>
        /// Delegate for av_log_format_line2.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_av_log_format_line2_delegate(void* ptr, FFmpegLogLevel level, [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, byte* vl, byte* line, int line_size, int* print_prefix);
        /// <summary>
        /// Formats a line of log the same way as the default callback.
        /// </summary>
        /// <param name="ptr">A pointer to an arbitrary struct of which the first field is a pointer to an AVClass struct.</param>
        /// <param name="level">The importance level of the message expressed using <see cref="FFmpegLogLevel"/>.</param>
        /// <param name="fmt">The format string (printf-compatible) that specifies how subsequent arguments are converted to output.</param>
        /// <param name="vl">The arguments referenced by the format string.</param>
        /// <param name="line">buffer to receive the formatted line; may be NULL if line_size is 0</param>
        /// <param name="line_size">size of the buffer; at most line_size-1 characters will be written to the buffer, plus one null terminator</param>
        /// <param name="print_prefix">used to store whether the prefix must be printed; must point to a persistent integer initially set to 1</param>
        /// <returns>Returns a negative value if an error occurred, otherwise returns the number of characters that would have been written for a sufficiently large buffer,
        /// not including the terminating null character. If the return value is not less than line_size, it means that the log message was truncated to fit the buffer.</returns>
        public unsafe static int AVFormatLogLine(void* ptr, FFmpegLogLevel level, string fmt, byte* vl, byte* line, int line_size, int* print_prefix)
        {
            ffmpeg_av_log_format_line2_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_log_format_line2_delegate>(libraryHandleAvUtil, "av_log_format_line2");
            return del(ptr, level, fmt, vl, line, line_size, print_prefix);
        }

        /// <summary>
        /// Delegate for av_frame_alloc.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate AVFrame* ffmpeg_av_frame_alloc_delegate();
        /// <summary>
        /// Allocate an <see cref="AVFrame"/> and set its fields to default values.
        /// </summary>
        /// <remarks>
        /// The resulting struct must be freed using <see cref="AVFrameFree"/>.
        /// This only allocates the <see cref="AVFrame"/> itself, not the data buffers. Those must be allocated through other means.
        /// </remarks>
        /// <returns>An <see cref="AVFrame"/> filled with default values or NULL on failure.</returns>
        public unsafe static AVFrame* AVFrameAlloc()
        {
            ffmpeg_av_frame_alloc_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_frame_alloc_delegate>(libraryHandleAvUtil, "av_frame_alloc");
            return del();
        }

        /// <summary>
        /// Delegate for av_frame_free.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate void ffmpeg_av_frame_free_delegate(AVFrame** frame);
        /// <summary>
        /// Free the frame and any dynamically allocated objects in it, e.g. extended_data. If the frame is reference counted, it will be unreferenced first.
        /// </summary>
        /// <param name="frame">Frame to be freed. The pointer will be set to NULL.</param>
        public unsafe static void AVFrameFree(AVFrame* frame)
        {
            ffmpeg_av_frame_free_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_frame_free_delegate>(libraryHandleAvUtil, "av_frame_free");
            del(&frame);
        }

        /// <summary>
        /// Delegate for av_frame_unref.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate void ffmpeg_av_frame_unref_delegate(AVFrame* frame);
        /// <summary>
        /// Unreference all the buffers referenced by frame and reset the frame fields.
        /// </summary>
        /// <param name="frame">The frame to be unreferenced.</param>
        public unsafe static void AVFrameUnref(AVFrame* frame)
        {
            ffmpeg_av_frame_unref_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_frame_unref_delegate>(libraryHandleAvUtil, "av_frame_unref");
            del(frame);
        }

        /// <summary>
        /// Delegate for av_image_get_buffer_size.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_av_image_get_buffer_size_delegate(AVPixelFormat pix_fmt, int width, int height, int align);
        /// <summary>
        /// Return the size in bytes of the amount of data required to store an image with the given parameters.
        /// </summary>
        /// <param name="pix_fmt">The pixel format of the image.</param>
        /// <param name="width">The width of the image in pixels.</param>
        /// <param name="height">The height of the image in pixels.</param>
        /// <param name="align">The assumed linesize alignment.</param>
        /// <returns>The buffer size in bytes, a negative error code in case of failure.</returns>
        public unsafe static int AVImageGetBufferSize(AVPixelFormat pix_fmt, int width, int height, int align)
        {
            ffmpeg_av_image_get_buffer_size_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_image_get_buffer_size_delegate>(libraryHandleAvUtil, "av_image_get_buffer_size");
            return del(pix_fmt, width, height, align);
        }

        /// <summary>
        /// Delegate for av_image_fill_arrays.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_av_image_fill_arrays_delegate(ref IntPtrArray8 dst_data, ref IntArray8 dst_linesize, byte* src, AVPixelFormat pix_fmt, int width, int height, int align);
        /// <summary>
        /// Setup the data pointers and linesizes based on the specified image parameters and the provided array.
        /// </summary>
        /// <remarks>
        /// The fields of the given image are filled in by using the src address which points to the image data buffer.
        /// Depending on the specified pixel format, one or multiple image data pointers and line sizes will be set.
        /// If a planar format is specified, several pointers will be set pointing to the different picture planes and
        /// the line sizes of the different planes will be stored in the lines_sizes array.
        /// Call with src == NULL to get the required size for the src buffer.
        /// </remarks>
        /// <param name="dst_data">The data pointers to be filled in.</param>
        /// <param name="dst_linesize">The linesizes for the image in dst_data to be filled in.</param>
        /// <param name="src">The buffer which will contain or contains the actual image data, can be NULL.</param>
        /// <param name="pix_fmt">The pixel format of the image.</param>
        /// <param name="width">The width of the image in pixels.</param>
        /// <param name="height">The height of the image in pixels.</param>
        /// <param name="align">The value used in src for linesize alignment.</param>
        /// <returns>The buffer size in bytes, a negative error code in case of failure.</returns>
        public unsafe static int AVImageFillArrays(ref IntPtrArray8 dst_data, ref IntArray8 dst_linesize, byte* src, AVPixelFormat pix_fmt, int width, int height, int align)
        {
            ffmpeg_av_image_fill_arrays_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_image_fill_arrays_delegate>(libraryHandleAvUtil, "av_image_fill_arrays");
            return del(ref dst_data, ref dst_linesize, src, pix_fmt, width, height, align);
        }

        /// <summary>
        /// Delegate for av_image_alloc.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate int ffmpeg_av_image_alloc_delegate(ref IntPtrArray8 pointers, ref IntArray8 linesizes, int w, int h, AVPixelFormat pix_fmt, int align);
        /// <summary>
        /// Allocate an image with size w and h and pixel format pix_fmt, and fill pointers and linesizes accordingly.
        /// </summary>
        /// <remarks>
        /// The allocated image buffer has to be freed by using <see cref="AVFreeP"/>.
        /// </remarks>
        /// <param name="pointers">The data pointers of the <see cref="AVFrame"/>.</param>
        /// <param name="linesizes">The linesizes of the <see cref="AVFrame"/>.</param>
        /// <param name="w">The width of the new image.</param>
        /// <param name="h">The height of the new image.</param>
        /// <param name="pix_fmt">The pixelformat of the new image.</param>
        /// <param name="align">The alignment of the image.</param>
        /// <returns>The size in bytes required for the image buffer, a negative error code in case of failure.</returns>
        public static int AVImageAlloc(ref IntPtrArray8 pointers, ref IntArray8 linesizes, int w, int h, AVPixelFormat pix_fmt, int align)
        {
            ffmpeg_av_image_alloc_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_image_alloc_delegate>(libraryHandleAvUtil, "av_image_alloc");
            return del(ref pointers, ref linesizes, w, h, pix_fmt, align);
        }

        /// <summary>
        /// Delegate for av_freep.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate void ffmpeg_av_freep_delegate(void* ptr);
        /// <summary>
        /// Frees a memory block which has been previously allocated and set the pointer pointing to it to NULL. 
        /// </summary>
        /// <param name="ptr">Pointer to the memory block which should be freed.</param>
        public unsafe static void AVFreeP(IntPtr ptr)
        {
            ffmpeg_av_freep_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_freep_delegate>(libraryHandleAvUtil, "av_freep");
            del(&ptr);
        }

        /// <summary>
        /// Delegate for av_hwdevice_ctx_create.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_av_hwdevice_ctx_create_delegate(IntPtr* device_ctx, int type, [MarshalAs(UnmanagedType.LPUTF8Str)] string device, AVDictionary* opts, int flags);
        /// <summary>
        /// Open a device of the specified type and create an AVHWDeviceContext for it.
        /// </summary>
        /// <param name="device_ctx">
        /// On success, a reference to the newly-created device context will be written here.
        /// The reference is owned by the caller and must be released with av_buffer_unref() when no longer needed.
        /// On failure, NULL will be written to this pointer.
        /// </param>
        /// <param name="type">The type of the device to create. </param>
        /// <returns>0 on success, a negative AVERROR code on failure.</returns>
        public unsafe static int AVCreateHWDeviceContext(IntPtr* device_ctx, int type)
        {
            ffmpeg_av_hwdevice_ctx_create_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_hwdevice_ctx_create_delegate>(libraryHandleAvUtil, "av_hwdevice_ctx_create");
            return del(device_ctx, type, null, null, 0);
        }

        #endregion avutil

        #region avformat

        /// <summary>
        /// Delegate for avformat_alloc_context.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate AVFormatContext* ffmpeg_avformat_alloc_context_delegate();
        /// <summary>
        /// Allocate an AVFormatContext.
        /// </summary>
        /// <remarks>
        /// <see cref="AVFormatContextFree"/> can be used to free the context and everything allocated by the framework within it.
        /// </remarks>
        /// <returns>A pointer to the newly allocated <see cref="AVFormatContext"/> struct.</returns>
        public unsafe static AVFormatContext* AVFormatContextAlloc()
        {
            ffmpeg_avformat_alloc_context_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avformat_alloc_context_delegate>(libraryHandleAvFormat, "avformat_alloc_context");
            return del();
        }

        /// <summary>
        /// Delegate for avformat_free_context.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate void ffmpeg_avformat_free_context_delegate(AVFormatContext* s);
        /// <summary>
        /// Free an AVFormatContext and all its streams.
        /// </summary>
        /// <param name="s">The context to free.</param>
        public unsafe static void AVFormatContextFree(AVFormatContext* s)
        {
            ffmpeg_avformat_free_context_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avformat_free_context_delegate>(libraryHandleAvFormat, "avformat_free_context");
            del(s);
        }

        /// <summary>
        /// Delegate for avformat_open_input.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_avformat_open_input_delegate(AVFormatContext** ps, [MarshalAs(UnmanagedType.LPUTF8Str)] string url, IntPtr fmt, AVDictionary** options);
        /// <summary>
        /// Open an input stream and read the header.
        /// </summary>
        /// <remarks>
        /// The codecs are not opened. The stream must be closed with avformat_close_input().
        /// </remarks>
        /// <param name="ps">
        /// Pointer to user-supplied AVFormatContext (allocated by avformat_alloc_context).
        /// May be a pointer to NULL, in which case an AVFormatContext is allocated by this function and written into ps.
        /// Note that a user-supplied AVFormatContext will be freed on failure.
        /// </param>
        /// <param name="url">URL of the stream to open.</param>
        /// <returns>0 on success, a negative AVERROR on failure.</returns>
        public unsafe static int AVFormatOpenInput(AVFormatContext *ps, string url)
        {
            ffmpeg_avformat_open_input_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avformat_open_input_delegate>(libraryHandleAvFormat, "avformat_open_input");
            return del(&ps, url, IntPtr.Zero, null);
        }

        /// <summary>
        /// Delegate for avformat_close_input.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate void ffmpeg_avformat_close_input_delegate(AVFormatContext** s);
        /// <summary>
        /// Close an opened input AVFormatContext.
        /// </summary>
        /// <remarks>Free it and all its contents and set *s to NULL.</remarks>
        /// <param name="s">The context to free.</param>
        public unsafe static void AVFormatCloseInput(AVFormatContext* s)
        {
            ffmpeg_avformat_close_input_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avformat_close_input_delegate>(libraryHandleAvFormat, "avformat_close_input");
            del(&s);
        }

        /// <summary>
        /// Delegate for avformat_find_stream_info.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_avformat_find_stream_info_delegate(AVFormatContext* ic, AVDictionary** options);
        /// <summary>
        /// Read packets of a media file to get stream information.
        /// </summary>
        /// <remarks>
        /// This is useful for file formats with no headers such as MPEG. This function also computes the real framerate in case of MPEG-2 repeat frame mode.
        /// The logical file position is not changed by this function; examined packets may be buffered for later processing.
        /// </remarks>
        /// <param name="ic">The media file handle.</param>
        /// <returns>&gt;=0 if OK, AVERROR_xxx on error</returns>
        public unsafe static int AVFormatFindStreamInfo(AVFormatContext* ic)
        {
            ffmpeg_avformat_find_stream_info_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avformat_find_stream_info_delegate>(libraryHandleAvFormat, "avformat_find_stream_info");
            return del(ic, null);
        }

        /// <summary>
        /// Delegate for av_find_best_stream.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_av_find_best_stream_delegate(AVFormatContext* ic, AVMediaType type, int wanted_stream_nb, int related_stream, AVCodec** decoder_ret, int flags);
        /// <summary>
        /// Find the "best" stream in the file.
        /// </summary>
        /// <remarks>
        /// The best stream is determined according to various heuristics as the most likely to be what the user expects.
        /// If the decoder parameter is non-NULL, av_find_best_stream will find the default decoder for the stream's codec; streams for which no decoder can be found are ignored.
        /// </remarks>
        /// <param name="ic">Handle to the media file.</param>
        /// <param name="type">stream type: video, audio, subtitles, etc.</param>
        /// <param name="wanted_stream_nb">user-requested stream number, or -1 for automatic selection</param>
        /// <param name="related_stream">try to find a stream related (eg. in the same program) to this one, or -1 if none</param>
        /// <param name="decoder_ret">if non-NULL, returns the decoder for the selected stream</param>
        /// <returns>the non-negative stream number in case of success, AVERROR_STREAM_NOT_FOUND if no stream with the requested type could be found, AVERROR_DECODER_NOT_FOUND if streams were found but no decoder</returns>
        public unsafe static int AVFindBestStream(AVFormatContext* ic, AVMediaType type, int wanted_stream_nb, int related_stream, AVCodec** decoder_ret)
        {
            ffmpeg_av_find_best_stream_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_find_best_stream_delegate>(libraryHandleAvFormat, "av_find_best_stream");
            return del(ic, type, wanted_stream_nb, related_stream, decoder_ret, 0);
        }

        /// <summary>
        /// Delegate for av_read_frame.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_av_read_frame_delegate(AVFormatContext* s, AVPacket* pkt);
        /// <summary>
        /// Return the next frame of a stream.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This function returns what is stored in the file, and does not validate that what is there are valid frames for the decoder.
        /// It will split what is stored in the file into frames and return one for each call.
        /// It will not omit invalid data between valid frames so as to give the decoder the maximum information possible for decoding.
        /// </para>
        /// <para>
        /// On success, the returned packet is reference-counted (pkt->buf is set) and valid indefinitely.
        /// The packet must be freed with av_packet_unref() when it is no longer needed. For video, the packet contains exactly one frame.
        /// For audio, it contains an integer number of frames if each frame has a known fixed size (e.g. PCM or ADPCM data).
        /// If the audio frames have a variable size (e.g. MPEG audio), then it contains one frame.
        /// </para>
        /// </remarks>
        /// <param name="s">Handle to the media file.</param>
        /// <param name="pkt">The new packet to be returned (can contain 1-n frames).</param>
        /// <returns>0 if OK, &lt; 0 on error or end of file. On error, pkt will be blank (as if it came from <see cref="AVPacketAlloc"/>).</returns>
        public unsafe static int AVReadFrame(AVFormatContext* s, AVPacket* pkt)
        {
            ffmpeg_av_read_frame_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_read_frame_delegate>(libraryHandleAvFormat, "av_read_frame");
            return del(s, pkt);
        }

        /// <summary>
        /// Delegate for avformat_seek_file.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_avformat_seek_file_delegate(AVFormatContext* s, int stream_index, long min_ts, long ts, long max_ts, int flags);
        /// <summary>
        /// Seek to timestamp.
        /// </summary>
        /// <remarks>
        /// Seeking will be done so that the point from which all active streams can be presented successfully will be closest, but lower, to <paramref name="timestamp"/>.
        /// </remarks>
        /// <param name="s">Handle to the media file.</param>
        /// <param name="stream_index">Index of the stream which is used as time base reference.</param>
        /// <param name="timestamp">The largest acceptable and target timestamp.</param>
        /// <returns>&gt;=0 on success, error code otherwise</returns>
        public unsafe static int AVFormatSeekFile(AVFormatContext* s, int stream_index, long timestamp)
        {
            ffmpeg_avformat_seek_file_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avformat_seek_file_delegate>(libraryHandleAvFormat, "avformat_seek_file");
            return del(s, stream_index, long.MinValue, timestamp, timestamp, 0);
        }

        #endregion avformat

        #region avcodec

        /// <summary>
        /// Delegate for avcodec_alloc_context3.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate AVCodecContext* ffmpeg_avcodec_alloc_context3_delegate(AVCodec* codec);
        /// <summary>
        /// Allocate an AVCodecContext and set its fields to default values.
        /// </summary>
        /// <remarks>
        /// The resulting struct should be freed with <see cref="AVCodecContextFree"/>.
        /// </remarks>
        /// <param name="codec">
        /// if non-NULL, allocate private data and initialize defaults for the given codec. It is illegal to then call <see cref="AVCodecOpen"/> with a different codec.
        /// If NULL, then the codec-specific defaults won't be initialized, which may result in suboptimal default settings (this is important mainly for encoders, e.g. libx264).
        /// </param>
        /// <returns>An <see cref="AVCodecContext"/> filled with default values or NULL on failure.</returns>
        public unsafe static AVCodecContext* AVCodecContextAlloc(AVCodec* codec)
        {
            ffmpeg_avcodec_alloc_context3_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avcodec_alloc_context3_delegate>(libraryHandleAvCodec, "avcodec_alloc_context3");
            return del(codec);
        }

        /// <summary>
        /// Delegate for avcodec_free_context.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate void ffmpeg_avcodec_free_context_delegate(AVCodecContext** avctx);
        /// <summary>
        /// Free the codec context and everything associated with it and write NULL to the provided pointer.
        /// </summary>
        /// <param name="avctx">Pointer to the <see cref="AVCodecContext"/>.</param>
        public unsafe static void AVCodecContextFree(AVCodecContext* avctx)
        {
            ffmpeg_avcodec_free_context_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avcodec_free_context_delegate>(libraryHandleAvCodec, "avcodec_free_context");
            del(&avctx);
        }

        /// <summary>
        /// Delegate for avcodec_parameters_to_context.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_avcodec_parameters_to_context_delegate(AVCodecContext* codec, IntPtr par);
        /// <summary>
        /// Fill the codec context based on the values from the supplied codec parameters.
        /// </summary>
        /// <remarks>
        /// Any allocated fields in codec that have a corresponding field in par are freed and replaced with duplicates of the corresponding field in par.
        /// Fields in codec that do not have a counterpart in par are not touched.
        /// </remarks>
        /// <param name="codec">The <see cref="AVCodecContext"/> to fill with codec parameters.</param>
        /// <param name="par">The codec parameters.</param>
        /// <returns>&gt;= 0 on success, a negative AVERROR code on failure.</returns>
        public unsafe static int AVCodecContextFillParameters(AVCodecContext* codec, IntPtr par)
        {
            ffmpeg_avcodec_parameters_to_context_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avcodec_parameters_to_context_delegate>(libraryHandleAvCodec, "avcodec_parameters_to_context");
            return del(codec, par);
        }

        /// <summary>
        /// Delegate for avcodec_open2.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_avcodec_open2_delegate(AVCodecContext* avctx, AVCodec* codec, AVDictionary** options);
        /// <summary>
        /// Initialize the AVCodecContext to use the given AVCodec.
        /// </summary>
        /// <remarks>
        /// Prior to using this function the context has to be allocated with <see cref="AVCodecContextAlloc"/>.
        /// Warning: This function is not thread safe!
        /// </remarks>
        /// <param name="avctx">The context to initialize.</param>
        /// <param name="codec">
        /// The codec to open this context for. If a non-NULL codec has been previously passed to <see cref="AVCodecContextAlloc"/> or
        /// for this context, then this parameter MUST be either NULL or equal to the previously passed codec. </param>
        /// <param name="options">A dictionary filled with AVCodecContext and codec-private options. On return this object will be filled with options that were not found.</param>
        /// <returns></returns>
        public unsafe static int AVCodecOpen(AVCodecContext* avctx, AVCodec* codec, AVDictionary** options)
        {
            ffmpeg_avcodec_open2_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avcodec_open2_delegate>(libraryHandleAvCodec, "avcodec_open2");
            return del(avctx, codec, options);
        }

        /// <summary>
        /// Delegate for avcodec_get_name.
        /// </summary>
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstCharPtrMarshaler))]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate string ffmpeg_avcodec_get_name_delegate(int id);
        /// <summary>
        /// Get the name of a codec.
        /// </summary>
        /// <param name="id">The ID of the codec.</param>
        /// <returns>A static string identifying the codec; never NULL</returns>
        public unsafe static string AVCodecGetName(int id)
        {
            ffmpeg_avcodec_get_name_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avcodec_get_name_delegate>(libraryHandleAvCodec, "avcodec_get_name");
            return del(id);
        }

        /// <summary>
        /// Delegate for av_packet_alloc.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate AVPacket* ffmpeg_av_packet_alloc_delegate();
        /// <summary>
        /// Allocate an <see cref="AVPacket"/> and set its fields to default values.
        /// </summary>
        /// <remarks>
        /// The resulting struct must be freed using <see cref="AVPacketFree"/>.
        /// This only allocates the <see cref="AVPacket"/> itself, not the data buffers. Those must be allocated through other means.
        /// </remarks>
        /// <returns>An <see cref="AVPacket"/> filled with default values or NULL on failure.</returns>
        public unsafe static AVPacket* AVPacketAlloc()
        {
            ffmpeg_av_packet_alloc_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_packet_alloc_delegate>(libraryHandleAvCodec, "av_packet_alloc");
            return del();
        }

        /// <summary>
        /// Delegate for av_packet_free.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate void ffmpeg_av_packet_free_delegate(AVPacket** pkt);
        /// <summary>
        /// Free the packet, if the packet is reference counted, it will be unreferenced first.
        /// </summary>
        /// <param name="pkt">Packet to be freed. The pointer will be set to NULL.</param>
        public unsafe static void AVPacketFree(AVPacket* pkt)
        {
            ffmpeg_av_packet_free_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_packet_free_delegate>(libraryHandleAvCodec, "av_packet_free");
            del(&pkt);
        }

        /// <summary>
        /// Delegate for av_packet_unref.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate void ffmpeg_av_packet_unref_delegate(AVPacket* pkt);
        /// <summary>
        /// Unreference the buffer referenced by the packet and reset the remaining packet fields to their default values.
        /// </summary>
        /// <param name="pkt">The packet to be unreferenced.</param>
        public unsafe static void AVPacketUnref(AVPacket* pkt)
        {
            ffmpeg_av_packet_unref_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_av_packet_unref_delegate>(libraryHandleAvCodec, "av_packet_unref");
            del(pkt);
        }

        /// <summary>
        /// Delegate for avcodec_receive_packet.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_avcodec_receive_packet_delegate(AVCodecContext* avctx, AVPacket* avpkt);
        /// <summary>
        /// Read encoded data from the encoder.
        /// </summary>
        /// <param name="avctx">The codec context.</param>
        /// <param name="avpkt">
        /// This will be set to a reference-counted packet allocated by the encoder.
        /// Note that the function will always call <see cref="AVPacketUnref"/> before doing anything else.
        /// </param>
        /// <returns>
        /// 0 on success, otherwise negative error code:
        /// <para>
        /// <see cref="AV_ERROR_EAGAIN"/>: The output is not available in the current state - user must try to send input.
        /// </para>
        /// <para>
        /// <see cref="AV_ERROR_EOF"/>: The encoder has been fully flushed, and there will be no more output packets.
        /// </para>
        /// <para>
        /// <see cref="AV_ERROR_EINVAL"/>: The codec is not opened or it is a decoder.
        /// </para>
        /// <para>
        /// Other negative values: Legitimate decoding errors.
        /// </para>
        /// </returns>
        public unsafe static int AVCodecReceivePacket(AVCodecContext* avctx, AVPacket* avpkt)
        {
            ffmpeg_avcodec_receive_packet_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avcodec_receive_packet_delegate>(libraryHandleAvCodec, "avcodec_receive_packet");
            return del(avctx, avpkt);
        }

        /// <summary>
        /// Delegate for avcodec_send_packet.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_avcodec_send_packet_delegate(AVCodecContext* avctx, AVPacket* avpkt);
        /// <summary>
        /// Supply raw packet data as input to a decoder.
        /// </summary>
        /// <remarks>
        /// The <see cref="AVCodecContext"/> MUST have been opened with <see cref="AVCodecOpen"/> before packets may be fed to the decoder.
        /// </remarks>
        /// <param name="avctx">The codec context.</param>
        /// <param name="avpkt">
        /// The input <see cref="AVPacket"/>. Usually, this will be a single video frame, or several complete audio frames.
        /// Ownership of the packet remains with the caller, and the decoder will not write to the packet.
        /// The decoder may create a reference to the packet data (or copy it if the packet is not reference-counted).
        /// Unlike with older APIs, the packet is always fully consumed, and if it contains multiple frames (e.g. some audio codecs),
        /// will require you to call <see cref="AVCodecReceiveFrame"/> multiple times afterwards before you can send a new packet.
        /// It can be NULL (or an <see cref="AVPacket"/> with data set to NULL and size set to 0); in this case, it is considered a flush packet,
        /// which signals the end of the stream. Sending the first flush packet will return success.
        /// Subsequent ones are unnecessary and will return <see cref="AV_ERROR_EOF"/>.
        /// If the decoder still has frames buffered, it will return them after sending a flush packet.
        /// </param>
        /// <returns>
        /// 0 on success, otherwise negative error code:
        /// <para>
        /// <see cref="AV_ERROR_EAGAIN"/>: Input is not accepted in the current state - user must read output with <see cref="AVCodecReceiveFrame"/>
        /// (once all output is read, the packet should be resent, and the call will not fail with <see cref="AV_ERROR_EAGAIN"/>).
        /// </para>
        /// <para>
        /// <see cref="AV_ERROR_EOF"/>: The decoder has been flushed, and no new packets can be sent to it (also returned if more than 1 flush packet is sent).
        /// </para>
        /// <para>
        /// <see cref="AV_ERROR_EINVAL"/>: The codec is not opened, it is an encoder, or requires flush.
        /// </para>
        /// <para>
        /// <see cref="AV_ERROR_ENOMEM"/>: Failed to add packet to internal queue, or similar other errors: legitimate decoding errors.
        /// </para>
        /// </returns>
        public unsafe static int AVCodecSendPacket(AVCodecContext* avctx, AVPacket* avpkt)
        {
            ffmpeg_avcodec_send_packet_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avcodec_send_packet_delegate>(libraryHandleAvCodec, "avcodec_send_packet");
            return del(avctx, avpkt);
        }

        /// <summary>
        /// Delegate for avcodec_receive_frame.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_avcodec_receive_frame_delegate(AVCodecContext* avctx, AVFrame* frame);
        /// <summary>
        /// Return decoded output data from a decoder.
        /// </summary>
        /// <param name="avctx">The codec context.</param>
        /// <param name="frame">
        /// This will be set to a reference-counted video or audio frame (depending on the decoder type) allocated by the decoder.
        /// Note that the function will always call <see cref="AVFrameUnref"/> before doing anything else.
        /// </param>
        /// <returns>
        /// 0 on success, otherwise negative error code:
        /// <para>
        /// <see cref="AV_ERROR_EAGAIN"/>: The output is not available in this state - user must try to send new input.
        /// </para>
        /// <para>
        /// <see cref="AV_ERROR_EOF"/>: The decoder has been fully flushed, and there will be no more output frames.
        /// </para>
        /// <para>
        /// <see cref="AV_ERROR_EINVAL"/>: The codec is not opened, or it is an encoder.
        /// </para>
        /// <para>
        /// AVERROR_INPUT_CHANGED: Current decoded frame has changed parameters with respect to first decoded frame. Applicable when flag AV_CODEC_FLAG_DROPCHANGED is set.
        /// </para>
        /// <para>
        /// Other negative values: Legitimate decoding errors.
        /// </para>
        /// </returns>
        public unsafe static int AVCodecReceiveFrame(AVCodecContext* avctx, AVFrame* frame)
        {
            ffmpeg_avcodec_receive_frame_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avcodec_receive_frame_delegate>(libraryHandleAvCodec, "avcodec_receive_frame");
            return del(avctx, frame);
        }

        /// <summary>
        /// Delegate for avcodec_send_frame.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_avcodec_send_frame_delegate(AVCodecContext* avctx, AVFrame* frame);
        /// <summary>
        /// Supply a raw video or audio frame to the encoder.
        /// </summary>
        /// <remarks>
        /// The <see cref="AVCodecContext"/> MUST have been opened with <see cref="AVCodecOpen"/> before packets may be fed to the encoder.
        /// </remarks>
        /// <param name="avctx">The codec context.</param>
        /// <param name="frame">
        /// The <see cref="AVFrame"/> containing the raw audio or video frame to be encoded. Ownership of the frame remains with the caller, and the encoder will not write to the frame.
        /// The encoder may create a reference to the frame data (or copy it if the frame is not reference-counted).
        /// It can be NULL, in which case it is considered a flush packet. This signals the end of the stream.
        /// If the encoder still has packets buffered, it will return them after this call.
        /// Once flushing mode has been entered, additional flush packets are ignored, and sending frames will return AVERROR_EOF.
        /// </param>
        /// <returns>
        /// 0 on success, otherwise negative error code:
        /// <para>
        /// <see cref="AV_ERROR_EAGAIN"/>: The input is not accepted in the current state - the user must read output with <see cref="AVCodecReceivePacket"/>
        /// (once all output is read, the packet should be resent, and the call will not fail with EAGAIN).
        /// </para>
        /// <para>
        /// <see cref="AV_ERROR_EOF"/>: The encoder has been flushed, and no new frames can be sent to it.
        /// </para>
        /// <para>
        /// <see cref="AV_ERROR_EINVAL"/>: The codec is not opened, refcounted_frames not set, it is a decoder, or requires flush.
        /// </para>
        /// <para>
        /// AV_ERROR_ENOMEM: Failed to add packet to internal queue.
        /// </para>
        /// <para>
        /// Other negative values: Legitimate decoding errors.
        /// </para>
        /// </returns>
        public unsafe static int AVCodecSendFrame(AVCodecContext* avctx, AVFrame* frame)
        {
            ffmpeg_avcodec_send_frame_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avcodec_send_frame_delegate>(libraryHandleAvCodec, "avcodec_send_frame");
            return del(avctx, frame);
        }

        /// <summary>
        /// Delegate for avcodec_find_decoder.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate AVCodec* ffmpeg_avcodec_find_decoder_delegate(int id);
        /// <summary>
        /// Find a registered decoder with a matching codec ID.
        /// </summary>
        /// <param name="id">AVCodecID of the requested decoder.</param>
        /// <returns>A decoder if one was found, <see langword="null"/> otherwise.</returns>
        public unsafe static AVCodec* AVCodecFindDecoder(int id)
        {
            ffmpeg_avcodec_find_decoder_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avcodec_find_decoder_delegate>(libraryHandleAvCodec, "avcodec_find_decoder");
            return del(id);
        }

        /// <summary>
        /// Delegate for avcodec_find_encoder.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate AVCodec* ffmpeg_avcodec_find_encoder_delegate(int id);
        /// <summary>
        /// Find a registered encoder with a matching codec ID.
        /// </summary>
        /// <param name="id">AVCodecID of the requested encoder.</param>
        /// <returns>An encoder if one was found, <see langword="null"/> otherwise.</returns>
        public unsafe static AVCodec* AVCodecFindEncoder(int id)
        {
            ffmpeg_avcodec_find_encoder_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_avcodec_find_encoder_delegate>(libraryHandleAvCodec, "avcodec_find_encoder");
            return del(id);
        }

        #endregion avcodec

        #region swscale

        /// <summary>
        /// Delegate for sws_freeContext.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate void ffmpeg_sws_freeContext_delegate(SwsContext* swsContext);
        /// <summary>
        /// Free the swscaler context <paramref name="swsContext"/>.
        /// </summary>
        /// <remarks>
        /// If <paramref name="swsContext"/> is <see langword="null"/>, then does nothing.
        /// </remarks>
        /// <param name="swsContext">The context to be freed.</param>
        public unsafe static void SwsFreeContext(SwsContext* swsContext)
        {
            ffmpeg_sws_freeContext_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_sws_freeContext_delegate>(libraryHandleSwScale, "sws_freeContext");
            del(swsContext);
        }

        /// <summary>
        /// Delegate for sws_getContext.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate SwsContext* ffmpeg_sws_getContext_delegate(int srcW, int srcH, AVPixelFormat srcFormat, int dstW, int dstH, AVPixelFormat dstFormat, int flags, SwsFilter* srcFilter, SwsFilter* dstFilter, double* param);
        /// <summary>
        /// Allocate and return an <see cref="SwsContext"/>.
        /// </summary>
        /// <remarks>
        /// You need it to perform scaling/conversion operations using <see cref="SwsScale"/>.
        /// </remarks>
        /// <param name="srcW">The width of the source image.</param>
        /// <param name="srcH">The height of the source image.</param>
        /// <param name="srcFormat">The source image format.</param>
        /// <param name="dstW">The width of the destination image.</param>
        /// <param name="dstH">The height of the destination image.</param>
        /// <param name="dstFormat">The destination image format.</param>
        /// <param name="scaleMode">Specify which algorithm and options to use for rescaling.</param>
        /// <returns>A pointer to an allocated context, or <see langword="null"/> in case of an error.</returns>
        public unsafe static SwsContext* SwsGetContext(int srcW, int srcH, AVPixelFormat srcFormat, int dstW, int dstH, AVPixelFormat dstFormat, FFmpegScaleMode scaleMode)
        {
            ffmpeg_sws_getContext_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_sws_getContext_delegate>(libraryHandleSwScale, "sws_getContext");
            return del(srcW, srcH, srcFormat, dstW, dstH, dstFormat, (int)scaleMode, null, null, null);
        }

        /// <summary>
        /// Delegate for sws_scale.
        /// </summary>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private unsafe delegate int ffmpeg_sws_scale_delegate(SwsContext* c, byte*[] srcSlice, int[] srcStride, int srcSliceY, int srcSliceH, byte*[] dst, int[] dstStride);
        /// <summary>
        /// Scale the image slice in <paramref name="srcSlice"/> and put the resulting scaled slice in the image in <paramref name="dst"/>.
        /// </summary>
        /// <remarks>
        /// A slice is a sequence of consecutive rows in an image.
        /// Slices have to be provided in sequential order, either in top-bottom or bottom-top order.
        /// If slices are provided in non-sequential order the behavior of the function is undefined.
        /// </remarks>
        /// <param name="c">The scaling context previously created with <see cref="SwsGetContext"/>.</param>
        /// <param name="srcSlice">The array containing the pointers to the planes of the source slice.</param>
        /// <param name="srcStride">The array containing the strides for each plane of the source image.</param>
        /// <param name="srcSliceY">The position in the source image of the slice to process, that is the number (counted starting from zero) in the image of the first row of the slice.</param>
        /// <param name="srcSliceH">The height of the source slice, that is the number of rows in the slice.</param>
        /// <param name="dst">The array containing the pointers to the planes of the destination image.</param>
        /// <param name="dstStride">The array containing the strides for each plane of the destination image.</param>
        /// <returns>The height of the output slice.</returns>
        public unsafe static int SwsScale(SwsContext* c, byte*[] srcSlice, int[] srcStride, int srcSliceY, int srcSliceH, byte*[] dst, int[] dstStride)
        {
            ffmpeg_sws_scale_delegate del = LibraryLoader.GetFunctionDelegate<ffmpeg_sws_scale_delegate>(libraryHandleSwScale, "sws_scale");
            return del(c, srcSlice, srcStride, srcSliceY, srcSliceH, dst, dstStride);
        }

        #endregion swscale

    }
}
