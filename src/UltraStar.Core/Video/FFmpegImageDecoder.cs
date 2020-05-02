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

namespace UltraStar.Core.Video
{
    /// <summary>
    /// Represents an image decoder class using FFmpeg.
    /// </summary>
    public class FFmpegImageDecoder : ImageDecoder
    {
        // Private variables
        private readonly UsPixelFormat pixelFormat;
        private readonly int maxWidth;
        private float aspectRatio;

        /// <summary>
        /// Initializes a new instance of <see cref="FFmpegImageDecoder"/>.
        /// </summary>
        /// <param name="pixelFormat">The pixel format of the image.</param>
        /// <param name="maxWidth">The maximum width of the output image.</param>
        /// <param name="aspectRatio">The aspect ratio of the output image. Set to -1 to keep the existing aspect ratio.</param>
        public FFmpegImageDecoder(UsPixelFormat pixelFormat, int maxWidth = 1920, float aspectRatio = -1.0f)
        {
            this.pixelFormat = pixelFormat;
            this.maxWidth = maxWidth;
            this.aspectRatio = aspectRatio;
        }

        /// <summary>
        /// Decodes an image into an <see cref="UsImage"/>.
        /// </summary>
        /// <remarks>
        /// If the decoding fails, a <see langword="default"/> <see cref="UsImage"/> will be returned.
        /// The easiest way to recognize this is that <see cref="UsImage.Data"/> is <see langword="null"/>.
        /// </remarks>
        /// <param name="url">The URL of the media file.</param>
        /// <returns>A <see cref="UsImage"/>.</returns>
        public override unsafe UsImage DecodeImage(string url)
        {
            // Open video file
            FFmpegImageStreamDecoder decoder = new FFmpegImageStreamDecoder(url);
            // Perform frame conversion
            if (aspectRatio <= 0) aspectRatio = (float)decoder.Width / decoder.Height;
            FFmpegFrameConverter converter = new FFmpegFrameConverter(decoder.Width, decoder.Height, decoder.PixelFormat, aspectRatio, 0, maxWidth, FFmpegScaleMode.FastBilinear, getFFmpegPixelFormat(pixelFormat), getFFmpegAlignment(pixelFormat));
            // Create return object
            UsImage image = new UsImage(converter.Height, converter.Width, pixelFormat, null, decoder.CodecName, decoder.CodecName);
            // Decode a frame
            bool success = decoder.DecodeNextFrame(out int errorCode);
            if (!success)
            {
                Log.Error("FFmpeg Image Decoder stopped working for file {FileName} with {ErrorCode}: {ErrorDescription}", decoder.URL, errorCode, FFmpeg.AVGetErrorDescription(errorCode));
                return default;
            }
            // Convert frame
            AVFrame* convertedFrame = converter.Convert(decoder.CurrentFrame);
            if (convertedFrame == null)
            {
                Log.Error("FFmpeg Image Decoder couldn't convert decoded image for file {FileName}.", decoder.URL);
                return default;
            }
            // Store result
            image.Data = new byte[convertedFrame->Linesize.Element0 * convertedFrame->Height];
            fixed (byte* _pData = image.Data)
            {
                byte* _pConvBuf = (byte*)convertedFrame->Data.Element0;
                byte* _pDataBuf = _pData;
                for (int h = 0; h < convertedFrame->Height; h++)
                {
                    Buffer.MemoryCopy(_pConvBuf, _pDataBuf, convertedFrame->Linesize.Element0, convertedFrame->Linesize.Element0);
                    _pConvBuf += convertedFrame->Linesize.Element0;
                    _pDataBuf += convertedFrame->Linesize.Element0;
                }
            }
            // Close converter and decoder
            converter.Close();
            decoder.Close();
            // Return image
            return image;
        }

        /// <summary>
        /// Helpermethod to translate the Ultrastar pixel format to FFmpegs pixel format.
        /// </summary>
        private AVPixelFormat getFFmpegPixelFormat(UsPixelFormat pixelFormat)
        {
            switch (pixelFormat)
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
    }
}
