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
    internal unsafe class FFmpegImageStreamDecoder : FFmpegStreamDecoder
    {
        /// <summary>
        /// Initializes a new instance of <see cref="FFmpegImageStreamDecoder"/>.
        /// </summary>
        /// <param name="url">The URL of the media file.</param>
        public FFmpegImageStreamDecoder(string url) : base(url, AVMediaType.AVMEDIA_TYPE_VIDEO)
        {
            // Set properties
            Width = _pCodecContext->Width;
            Height = _pCodecContext->Height;
            PixelFormat = _pCodecContext->PixelFormat;
        }

        /// <summary>
        /// Gets the width of the image.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the height of the image.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the pixel format of the image.
        /// </summary>
        public AVPixelFormat PixelFormat { get; }
    }
}
