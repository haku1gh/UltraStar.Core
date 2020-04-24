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
    /// Represents an audio decoder for FFmpeg streams.
    /// </summary>
    internal unsafe class FFmpegAudioStreamDecoder : FFmpegStreamDecoder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="FFmpegAudioStreamDecoder"/>.
        /// </summary>
        /// <param name="url">The URL of the media file.</param>
        public FFmpegAudioStreamDecoder(string url) : base(url, AVMediaType.AVMEDIA_TYPE_AUDIO)
        {
            // Set properties
            CodecLongName = "unknown";
            if ((IntPtr)(_pFormatContext->FormatInput->LongName) != IntPtr.Zero)
                CodecLongName = Marshal.PtrToStringAnsi((IntPtr)(_pFormatContext->FormatInput->LongName));
            TimeBase = FFmpeg.AVConvertRational(_pFormatContext->Streams[streamIndex]->TimeBase);
            Duration = (double)_pFormatContext->Duration / FFmpeg.AV_TIMEBASE;
            Channels = _pCodecContext->Channels;
            ChannelLayout = (AVChannelLayout)_pCodecContext->ChannelLayout;
            SampleRate = _pCodecContext->SampleRate;
            SampleFormat = _pCodecContext->SampleFormat;
            StartTime = _pFormatContext->StartTime;
            // Further checks
            if (Duration < 0.1)
                throw new FFmpegException("Corrupt audio stream in file " + url + ".");
        }

        /// <summary>
        /// Gets the codec long name.
        /// </summary>
        public string CodecLongName { get; }

        /// <summary>
        /// Gets the internal time base of the audio stream.
        /// </summary>
        public double TimeBase { get; }

        /// <summary>
        /// Gets the duration of the audio stream.
        /// </summary>
        public double Duration { get; }

        /// <summary>
        /// Gets the number of channels in the audio stream.
        /// </summary>
        public int Channels { get; }

        /// <summary>
        /// Gets the sample rate of the audio stream.
        /// </summary>
        public int SampleRate { get; }

        /// <summary>
        /// Gets the sample format of the audio stream.
        /// </summary>
        public AVSampleFormat SampleFormat { get; }

        /// <summary>
        /// Gets the channel layout of the audio stream.
        /// </summary>
        public AVChannelLayout ChannelLayout { get; }

        /// <summary>
        /// Gets the start time of the first audio sample.
        /// </summary>
        public double StartTime { get; }

        /// <summary>
        /// Seeks to the nearest timestamp in the audio stream.
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
