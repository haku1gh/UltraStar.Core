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
    /// Represents a video decoder for FFmpeg streams.
    /// </summary>
    internal unsafe class FFmpegVideoStreamDecoder : FFmpegStreamDecoder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="FFmpegVideoStreamDecoder"/>.
        /// </summary>
        /// <param name="url">The URL of the media file.</param>
        /// <param name="threadCount">The number of threads to use for decoding.</param>
        public FFmpegVideoStreamDecoder(string url, int threadCount = -1) :
            base(url, AVMediaType.AVMEDIA_TYPE_VIDEO, threadCount == -1 ? Math.Min(Environment.ProcessorCount / 2, 4) : threadCount)
        {
            // Set properties
            CodecLongName = "unknown";
            if ((IntPtr)(_pFormatContext->FormatInput->LongName) != IntPtr.Zero)
                CodecLongName = Marshal.PtrToStringAnsi((IntPtr)(_pFormatContext->FormatInput->LongName));
            Width = _pCodecContext->Width;
            Height = _pCodecContext->Height;
            PixelFormat = _pCodecContext->PixelFormat;
            TimeBase = FFmpeg.AVConvertRational(_pFormatContext->Streams[streamIndex]->TimeBase);
            Duration = (double)_pFormatContext->Duration / FFmpeg.AV_TIMEBASE;
            FrameRate = FFmpeg.AVConvertRational(_pFormatContext->Streams[streamIndex]->AverageFrameRate);
            if (FrameRate <= 0 || FrameRate > 500)
            {
                FrameRate = FFmpeg.AVConvertRational(_pFormatContext->Streams[streamIndex]->RealBaseFrameRate);
                if (FrameRate <= 0 || FrameRate > 500) FrameRate = 0;
            }
            StartTime = _pFormatContext->StartTime;
            // Further checks
            if (Duration < 0.1)
                throw new FFmpegException("Corrupt video stream in file " + url + ".");
        }

        /// <summary>
        /// Gets the codec long name.
        /// </summary>
        public string CodecLongName { get; }

        /// <summary>
        /// Gets the width of the video frames.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the height of the video frames.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the pixel format of the video frames.
        /// </summary>
        public AVPixelFormat PixelFormat { get; }

        /// <summary>
        /// Gets the internal time base of the video stream.
        /// </summary>
        public double TimeBase { get; }

        /// <summary>
        /// Gets the duration of the video stream.
        /// </summary>
        public double Duration { get; }

        /// <summary>
        /// Gets the frame rate of the video stream.
        /// </summary>
        public double FrameRate { get; }

        /// <summary>
        /// Gets the start time of the first video frame.
        /// </summary>
        public double StartTime { get; }

        /// <summary>
        /// Seeks to the nearest timestamp in the video stream.
        /// </summary>
        /// <param name="timestamp">The largest acceptable and target timestamp.</param>
        /// <returns><see langword="true"/> on success; otherwise <see langword="false"/>.</returns>
        public bool Seek(double timestamp)
        {
            long ts = (long)(timestamp / TimeBase);
            int result = FFmpeg.AVFormatSeekFile(_pFormatContext, streamIndex, ts);
            return (result >= 0);
        }
    }
}
