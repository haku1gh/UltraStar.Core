#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;

namespace UltraStar.Core.Unmanaged.FFmpeg
{
    /// <summary>
    /// Represents a image decoder for FFmpeg streams.
    /// </summary>
    internal unsafe class FFmpegImagePacketDecoder : IDisposable
    {
        // Private variables
        private AVFrame* _pFrame;

        /// <summary>
        /// The handle to the codec (=decoder).
        /// </summary>
        protected AVCodecContext* _pCodecContext;

        /// <summary>
        /// Indicator if this instance is disposed.
        /// </summary>
        protected bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of <see cref="FFmpegImagePacketDecoder"/>.
        /// </summary>
        /// <param name="codecId">The the codecId.</param>
        public FFmpegImagePacketDecoder(FFmpegImageCodec codecId)
        {
            // Find the decoder
            AVCodec* _pCodec = null;
            _pCodec = FFmpeg.AVCodecFindDecoder((int)codecId);
            // Create the codec context
            _pCodecContext = FFmpeg.AVCodecContextAlloc(_pCodec);
            int result = FFmpeg.AVCodecOpen(_pCodecContext, _pCodec, null);
            if (result < 0) throw new FFmpegException(result);
            // Allocate the frame structure
            _pFrame = FFmpeg.AVFrameAlloc();
        }


        /// <summary>
        /// Finalizes this instance.
        /// </summary>
        ~FFmpegImagePacketDecoder()
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
                    FFmpeg.AVCodecContextFree(_pCodecContext);
                    _pCodecContext = null;
                }
                catch { }
            }
        }

        /// <summary>
        /// Gets the current available frame.
        /// </summary>
        public AVFrame* CurrentFrame
        {
            get
            {
                // Check if disposed
                if (isDisposed)
                    throw new ObjectDisposedException(nameof(FFmpegImagePacketDecoder));
                // Return value
                return _pFrame;
            }
        }

        /// <summary>
        /// Decodes a packet containing an encoded image.
        /// </summary>
        /// <param name="_pEncodedPacket">The packet containing the encoded image.</param>
        /// <param name="errorCode">Will contain a negative error code in case <see langword="false"/> was returned.</param>
        /// <returns>
        /// <see langword="true"/> if the frame could be retrieved;
        /// otherwise <see langword="false"/>, then <paramref name="errorCode"/> indicates what kind of error occurred.
        /// </returns>
        public bool DecodePacket(AVPacket* _pEncodedPacket, out int errorCode)
        {
            // Check if disposed
            if (isDisposed)
                throw new ObjectDisposedException(nameof(FFmpegStreamDecoder));
            // Unref frame
            FFmpeg.AVFrameUnref(_pFrame);
            // Decode packet and retrieve frame.
            errorCode = FFmpeg.AVCodecSendPacket(_pCodecContext, _pEncodedPacket);
            if (errorCode < 0) return false;
            errorCode = FFmpeg.AVCodecReceiveFrame(_pCodecContext, _pFrame);
            if (errorCode < 0) return false;
            // Return success
            return true;
        }
    }
}
