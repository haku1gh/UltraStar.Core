#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.Reflection;
using UltraStar.Core.Utils;

namespace UltraStar.Core.Video
{
    /// <summary>
    /// Represents an abstract video decoder class.
    /// </summary>
    public abstract class VideoDecoder : Decoder<TimestampItem<byte[]>>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="VideoDecoder"/>.
        /// </summary>
        /// <param name="minimumBufferSize">The minimum size of the internal buffer.</param>
        public VideoDecoder(int minimumBufferSize) : base(minimumBufferSize, LibrarySettings.VideoDecoderNonOverwritingItems)
        { }

        /// <summary>
        /// Opens a new audio decoder.
        /// </summary>
        /// <param name="url">The URL of the media file.</param>
        /// <param name="pixelFormat">The pixel format of the video frames.</param>
        /// <param name="maxWidth">The maximum width of the output video.</param>
        /// <param name="aspectRatio">The aspect ratio of the output video. Set to -1 to keep the existing aspect ratio.</param>
        public static VideoDecoder Open(string url, UsPixelFormat pixelFormat, int maxWidth = 1920, float aspectRatio = -1.0f)
        {
            // Get the type where the class VideoDecoder is implemented
            Type videoDecoderClass = Type.GetType(LibrarySettings.VideoDecoderClassName, true);
            // Get the constructor
            ConstructorInfo cInfo = videoDecoderClass.GetConstructor(new Type[] { typeof(string), typeof(UsPixelFormat), typeof(int), typeof(float) });
            // Return the newly created object
            return (VideoDecoder)cInfo.Invoke(new object[] { url, pixelFormat, maxWidth, aspectRatio });
        }

        /// <summary>
        /// Gets the name of the codec used for decoding.
        /// </summary>
        public abstract string CodecName { get; protected set; }

        /// <summary>
        /// Gets the codec long name.
        /// </summary>
        public abstract string CodecLongName { get; protected set; }

        /// <summary>
        /// Gets the width of the video frames.
        /// </summary>
        public abstract int Width { get; protected set; }

        /// <summary>
        /// Gets the height of the video frames.
        /// </summary>
        public abstract int Height { get; protected set; }

        /// <summary>
        /// Gets the pixel format of the video frames.
        /// </summary>
        public abstract UsPixelFormat PixelFormat { get; protected set; }

        /// <summary>
        /// Gets the duration of the video.
        /// </summary>
        public abstract float Duration { get; protected set; }

        /// <summary>
        /// Gets the frame rate of the video.
        /// </summary>
        public abstract float FrameRate { get; protected set; }

        /// <summary>
        /// Gets the start time of the first video frame.
        /// </summary>
        public abstract long StartTimestamp { get; protected set; }
    }
}
