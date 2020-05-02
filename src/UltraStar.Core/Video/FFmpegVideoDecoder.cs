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
using UltraStar.Core.Unmanaged.FFmpeg;
using UltraStar.Core.Utils;

namespace UltraStar.Core.Video
{
    /// <summary>
    /// Represents a video decoder class using FFmpeg.
    /// </summary>
    public class FFmpegVideoDecoder : VideoDecoder
    {
        // Private variables
        private FFmpegVideoStreamDecoder decoder;
        private FFmpegFrameConverter converter;

        /// <summary>
        /// Initializes a new instance of <see cref="FFmpegVideoDecoder"/>.
        /// </summary>
        /// <param name="url">The URL of the media file.</param>
        /// <param name="pixelFormat">The pixel format of the video frames.</param>
        /// <param name="maxWidth">The maximum width of the output video.</param>
        /// <param name="aspectRatio">The aspect ratio of the output video. Set to -1 to keep the existing aspect ratio.</param>
        public FFmpegVideoDecoder(string url, UsPixelFormat pixelFormat, int maxWidth = 1920, float aspectRatio = -1.0f) : base(0)
        {
            // Open video file
            decoder = new FFmpegVideoStreamDecoder(url, LibrarySettings.VideoDecodingThreadCount);
            // Perform frame conversion
            if (aspectRatio <= 0) aspectRatio = (float)decoder.Width / decoder.Height;
            converter = new FFmpegFrameConverter(decoder.Width, decoder.Height, decoder.PixelFormat, aspectRatio, 0, maxWidth, FFmpegScaleMode.FastBilinear, getFFmpegPixelFormat(pixelFormat), getFFmpegAlignment(pixelFormat));
            Width = converter.Width;
            Height = converter.Height;
            // Set properties
            CodecName = decoder.CodecName;
            CodecLongName = decoder.CodecLongName;
            PixelFormat = pixelFormat;
            Duration = (float)decoder.Duration;
            FrameRate = (float)decoder.FrameRate;
            StartTimestamp = (long)(decoder.StartTime * 1000000);
            // Resize buffer
            int minBufferSize = (int)Math.Round(decoder.FrameRate * UsOptions.VideoFilePreBufferLength / 1000);
            if (minBufferSize < 4) minBufferSize = 4;
            resizeBuffer(minBufferSize);
            // Start thread to decode frames
            startThread("FFmpeg Video Decoder Thread");
        }

        /// <summary>
        /// Helpermethod to translate the Ultrastar pixel format to FFmpegs pixel format.
        /// </summary>
        private AVPixelFormat getFFmpegPixelFormat(UsPixelFormat pixelFormat)
        {
            switch(pixelFormat)
            {
                case UsPixelFormat.RGBA:
                    return AVPixelFormat.AV_PIX_FMT_RGBA;
                case UsPixelFormat.BGR24:
                    return AVPixelFormat.AV_PIX_FMT_BGR24;
                default:
                    return AVPixelFormat.AV_PIX_FMT_RGBA;
            }
        }

        /// <summary>
        /// Helpermethod to translate the Ultrastar pixel format to requirements for scan line alignment.
        /// </summary>
        private int getFFmpegAlignment(UsPixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case UsPixelFormat.RGBA:
                    return 4;
                case UsPixelFormat.BGR24:
                    return 4;
                default:
                    return 1;
            }
        }

        /// <summary>
        /// Finalizes this instance.
        /// </summary>
        ~FFmpegVideoDecoder()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases the unmanaged resources used by this instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                // Dispose base class first here
                base.Dispose(disposing);
                // Close converter and decoder
                converter.Dispose();
                decoder.Dispose();
                converter = null;
                decoder = null;
            }
        }

        /// <summary>
        /// Adds a new item to the buffer.
        /// </summary>
        /// <param name="entry">The old entry at the position where the item will be added. This entry shall be modified by this method.</param>
        /// <returns>
        /// <see langword="true"/> if the next item could be set;
        /// otherwise <see langword="false"/> to indicate an error or EOF and the termination of the thread.
        /// </returns>
        protected override unsafe bool addItemToBuffer(ref TimestampItem<byte[]> entry)
        {
            // Decode a frame
            bool success = decoder.DecodeNextFrame(out int errorCode);
            if(!success)
            {
                if (errorCode != 0)
                    Log.Error("FFmpeg Video Decoder stopped working for file {FileName} with {ErrorCode}: {ErrorDescription}", decoder.URL, errorCode, FFmpeg.AVGetErrorDescription(errorCode));
                else
                    Log.Information("FFmpeg Video Decoder reached end of file for file {FileName}.", decoder.URL);
                return false;
            }
            // Convert frame
            AVFrame* convertedFrame = converter.Convert(decoder.CurrentFrame);
            if(convertedFrame == null)
            {
                Log.Error("FFmpeg Video Decoder couldn't convert decoded frame for file {FileName}.", decoder.URL);
                return false;
            }
            // Store result
            if (entry.Item == null) entry.Item = new byte[convertedFrame->Linesize.Element0 * convertedFrame->Height];
            fixed (byte* _pEntryItem = entry.Item)
            {
                byte* _pConvBuf = (byte*)convertedFrame->Data.Element0;
                byte* _pEntryBuf = _pEntryItem;
                for (int h = 0; h < convertedFrame->Height; h++)
                {
                    Buffer.MemoryCopy(_pConvBuf, _pEntryBuf, convertedFrame->Linesize.Element0, convertedFrame->Linesize.Element0);
                    _pConvBuf += convertedFrame->Linesize.Element0;
                    _pEntryBuf += convertedFrame->Linesize.Element0;
                }
            }
            entry.Timestamp = (long)(decoder.TimeBase * convertedFrame->BestEffortTimestamp * 1000000);
            // Return success
            return true;
        }

        /// <summary>
        /// Gets the name of the codec used for decoding.
        /// </summary>
        public override string CodecName { get; protected set; }

        /// <summary>
        /// Gets the codec long name.
        /// </summary>
        public override string CodecLongName { get; protected set; }

        /// <summary>
        /// Gets the width of the video frames.
        /// </summary>
        public override int Width { get; protected set; }

        /// <summary>
        /// Gets the height of the video frames.
        /// </summary>
        public override int Height { get; protected set; }

        /// <summary>
        /// Gets the pixel format of the video frames.
        /// </summary>
        public override UsPixelFormat PixelFormat { get; protected set; }

        /// <summary>
        /// Gets the duration of the video.
        /// </summary>
        public override float Duration { get; protected set; }

        /// <summary>
        /// Gets the frame rate of the video.
        /// </summary>
        public override float FrameRate { get; protected set; }

        /// <summary>
        /// Gets the start time of the first video frame.
        /// </summary>
        public override long StartTimestamp { get; protected set; }
    }
}
