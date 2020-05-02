#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using UltraStar.Core.ThirdParty.Serilog;

namespace UltraStar.Core.Unmanaged.FFmpeg
{
    /// <summary>
    /// Represents a generic decoder for FFmpeg streams.
    /// </summary>
    /// <remarks>
    /// Streams can be either audio or video. Images are also of type video.
    /// </remarks>
    internal unsafe class FFmpegStreamDecoder : IDisposable
    {
        // Private variables
        private AVPacket* _pPacket;
        private AVFrame* _pFrame;
        private bool firstFrame = true;

        /// <summary>
        /// The handle to the codec (=decoder).
        /// </summary>
        protected AVCodecContext* _pCodecContext;

        /// <summary>
        /// The handle to the media file.
        /// </summary>
        protected AVFormatContext* _pFormatContext;

        /// <summary>
        /// The index of the stream in the media file.
        /// </summary>
        protected readonly int streamIndex;

        /// <summary>
        /// Indicator if this instance is disposed.
        /// </summary>
        protected bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of <see cref="FFmpegStreamDecoder"/>.
        /// </summary>
        /// <param name="url">The URL of the media file.</param>
        /// <param name="mediaType">The media type of the stream.</param>
        /// <param name="threadCount">The number of threads to use for decoding.</param>
        public FFmpegStreamDecoder(string url, AVMediaType mediaType, int threadCount = 1)
        {
            int result;
            URL = url;
            // Get handle of media file
            _pFormatContext = FFmpeg.AVFormatContextAlloc();
            result = FFmpeg.AVFormatOpenInput(_pFormatContext, url);
            if (result < 0) throw new FFmpegException(result);
            // Get best fitting stream
            result = FFmpeg.AVFormatFindStreamInfo(_pFormatContext);
            if (result < 0) throw new FFmpegException(result);
            AVCodec* _pCodec = null;
            streamIndex = FFmpeg.AVFindBestStream(_pFormatContext, mediaType, -1, -1, &_pCodec);
            if (streamIndex < 0) throw new FFmpegException(streamIndex);
            // Get handle to codec context
            _pCodecContext = FFmpeg.AVCodecContextAlloc(_pCodec);
            if(_pCodecContext == null) throw new FFmpegException("Codec context could not be initialized. Codec maybe unknown.");
            // Set thread count
            if (threadCount < 1) threadCount = 1;
            if (threadCount != 1)
            {
                _pCodecContext->ThreadCount = threadCount;
                _pCodecContext->ThreadType = 1;    // == FF_THREAD_FRAME
            }
            // Enable hardware acceleration
            // TODO: Add hardware acceleration support

            // Get handle to codec
            result = FFmpeg.AVCodecContextFillParameters(_pCodecContext, _pFormatContext->Streams[streamIndex]->CodecParameters);
            if (result < 0) throw new FFmpegException(result);
            result = FFmpeg.AVCodecOpen(_pCodecContext, _pCodec, null);
            if (result < 0) throw new FFmpegException(result);
            // Get the name of the used codec
            CodecName = FFmpeg.AVCodecGetName(_pCodec->ID);
            // Allocate memory for a packet and a frame
            _pPacket = FFmpeg.AVPacketAlloc();
            _pFrame = FFmpeg.AVFrameAlloc();
        }

        /// <summary>
        /// Finalizes this instance.
        /// </summary>
        ~FFmpegStreamDecoder()
        {
            Dispose(false);
        }

        /// <summary>
        /// Closes the FFmpeg stream.
        /// </summary>
        public virtual void Close()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases the unmanaged resources used by this instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                // Special for Close() and Dispose()
                if (disposing)
                    GC.SuppressFinalize(this);
                // Free all resources from FFmpeg
                try
                {
                    FFmpeg.AVFrameFree(_pFrame);
                    _pFrame = null;
                    FFmpeg.AVPacketFree(_pPacket);
                    _pPacket = null;
                    FFmpeg.AVCodecContextFree(_pCodecContext);
                    _pCodecContext = null;
                    FFmpeg.AVFormatCloseInput(_pFormatContext);
                    _pFormatContext = null;
                }
                catch { }
            }
        }

        /// <summary>
        /// Gets the name of the codec used for decoding.
        /// </summary>
        public string CodecName { get; }

        /// <summary>
        /// Get the url used for decoding.
        /// </summary>
        public string URL { get; }

        /// <summary>
        /// Gets the current available frame.
        /// </summary>
        public AVFrame* CurrentFrame
        {
            get
            {
                // Check if disposed
                if (isDisposed)
                    throw new ObjectDisposedException(nameof(FFmpegStreamDecoder));
                // Return value
                return _pFrame;
            }
        }

        /// <summary>
        /// Decode the next frame.
        /// </summary>
        /// <param name="errorCode">
        /// 0 if decoder is at end of file; otherwise an error code is returned.
        /// Use <see cref="FFmpeg.AVGetErrorDescription"/> to retrieve an error description.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the next frame could be retrieved;
        /// otherwise <see langword="false"/>, then <paramref name="errorCode"/> indicates what kind of error occurred.
        /// </returns>
        public bool DecodeNextFrame(out int errorCode)
        {
            // Check if disposed
            if (isDisposed)
                throw new ObjectDisposedException(nameof(FFmpegStreamDecoder));
            // Unref frame
            FFmpeg.AVFrameUnref(_pFrame);
            // Variables
            int result;
            errorCode = 0;
            // Repeated-try avcodec_receive_frame until enough packets haven been send
            while (true)
            {
                // try read frame; maybe last avcodec_send_packet resulted in multiple frames to be read; will respond with EAGAIN if more packets are needed
                // Receive next frame
                result = FFmpeg.AVCodecReceiveFrame(_pCodecContext, _pFrame);
                if (result != FFmpeg.AV_ERROR_EAGAIN)
                {
                    // Check for End of file
                    if (result == FFmpeg.AV_ERROR_EOF)
                        return false;
                    // Check for errors
                    if (result < 0)
                    {
                        // If this is the first frame, ignore error and just continue
                        if (firstFrame == true)
                        {
                            firstFrame = false;
                            continue;
                        }
                        errorCode = result;
                        return false;
                    }
                    firstFrame = false;
                    return true;
                }
                // EAGAIN had been responded, which means that more packets are required to do decoding. So lets get the next packet.
                while (true)
                {
                    // Unref packet
                    FFmpeg.AVPacketUnref(_pPacket);
                    // Get the next packet from the stream
                    result = FFmpeg.AVReadFrame(_pFormatContext, _pPacket);
                    // Check for End of file
                    if (result == FFmpeg.AV_ERROR_EOF)
                    {
                        // No more packets available, therefore trigger flush of remaining frames
                        result = FFmpeg.AVCodecSendPacket(_pCodecContext, null);
                        // Check for errors
                        if (result < 0 && isError(result, ref errorCode))
                            return false;
                        // No error
                        break;
                    }
                    // Check for errors
                    if (result < 0)
                    {
                        if (isError(result, ref errorCode)) return false;
                        else continue;
                    }
                    // Packet available
                    if (_pPacket->StreamIndex == streamIndex)
                    {
                        // Send packet to decoder
                        result = FFmpeg.AVCodecSendPacket(_pCodecContext, _pPacket);
                        if (result == 0) break;

                        // In case of errors: ignore these
                        Log.Debug("Sending packet to decoder failed with error code {ErrorCode}: {ErrorDescription}", result, FFmpeg.AVGetErrorDescription(result));
                    }
                }
            }
        }

        /// <summary>
        /// Helpermethod to check for subsequent errors.
        /// </summary>
        private bool isError(int result, ref int errorCode)
        {
            // If this is the first frame, ignore error and just continue
            if (firstFrame == true)
            {
                firstFrame = false;
                return false;
            }
            errorCode = result;
            return true;
        }
    }
}
