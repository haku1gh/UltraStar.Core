#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.IO;

namespace UltraStar.Core.Unmanaged.FFmpeg
{
    /// <summary>
    /// Represents a image encoder for FFmpeg frames.
    /// </summary>
    internal static unsafe class FFmpegImagePacketEncoder
    {
        private static readonly int FF_QP2LAMBDA = 0x76;

        /// <summary>
        /// Encode an image with the WEBP codec.
        /// </summary>
        /// <param name="writeStream">The stream to write the encoded image to.</param>
        /// <param name="_pSrcFrame">A pointer to the raw frame.</param>
        /// <param name="quality">The output quality. This must be in the range 0-100, where 100 is the best quality.</param>
        /// <param name="compressionLevel">The compression level. This must be in the 0-6, where 6 is the highest.</param>
        /// <returns>
        /// If greater than zero, then this are the number of bytes written to the <paramref name="writeStream"/>;
        /// otherwise a negative value is returned in case of errors.
        /// </returns>
        public static int EncodeInWEBP(Stream writeStream, AVFrame* _pSrcFrame, int quality = 75, int compressionLevel = 4)
        {
            // Checks
            if (writeStream == null || writeStream.CanWrite == false)
                return int.MinValue + 1;
            if(_pSrcFrame == null)
                return int.MinValue + 2;
            if (quality < 0) quality = 0;
            if (quality > 100) quality = 100;
            if (compressionLevel < 0) compressionLevel = 0;
            if (compressionLevel > 6) compressionLevel = 6;
            // Add specific parameters for the codec to the context
            EncoderContextCallback callback = (_pEncoderContext) =>
            {
                _pEncoderContext->CompressionLevel = compressionLevel;      // Range 0-6. Default is 4. 6 is slowest and has smallest size.
                _pEncoderContext->GlobalQuality = quality * FF_QP2LAMBDA;   // Range 0-100. 100 is highest quality.
            };
            // Encode and return
            return encode(_pSrcFrame, AVPixelFormat.AV_PIX_FMT_YUV420P, FFmpegImageCodec.WEBP, writeStream, callback);
        }

        /// <summary>
        /// Encode an image with the JPEG codec.
        /// </summary>
        /// <param name="writeStream">The stream to write the encoded image to.</param>
        /// <param name="_pSrcFrame">A pointer to the raw frame.</param>
        /// <param name="minQuality">The minimum output quality. This must be in the range 0-63, where 0 is the best quality.</param>
        /// <param name="maxQuality">The maximum output quality. This must be in the range 0-63, where 0 is the best quality.</param>
        /// <returns>
        /// If greater than zero, then this are the number of bytes written to the <paramref name="writeStream"/>;
        /// otherwise a negative value is returned in case of errors.
        /// </returns>
        public static int EncodeInJPEG(Stream writeStream, AVFrame* _pSrcFrame, int minQuality, int maxQuality)
        {
            // Checks
            if (writeStream == null || writeStream.CanWrite == false)
                return int.MinValue + 1;
            if (_pSrcFrame == null)
                return int.MinValue + 2;
            if (minQuality < 0) minQuality = 0;
            if (minQuality > 63) minQuality = 63;
            if (maxQuality < 0) maxQuality = 0;
            if (maxQuality > 63) maxQuality = 63;
            // Add specific parameters for the codec to the context
            EncoderContextCallback callback = (_pEncoderContext) =>
            {
                _pEncoderContext->QMin = minQuality; // 0 = highest quality, 63 = lowest quality
                _pEncoderContext->QMax = maxQuality; // 0 = highest quality, 63 = lowest quality
                _pEncoderContext->MacroBlockMinimumLagrangeMultiplier = minQuality * FF_QP2LAMBDA;
                _pEncoderContext->MacroBlockMaximumLagrangeMultiplier = maxQuality * FF_QP2LAMBDA;
                _pEncoderContext->Flags |= 2; // Enable QSCALE
            };
            // Encode and return
            return encode(_pSrcFrame, AVPixelFormat.AV_PIX_FMT_YUVJ420P, FFmpegImageCodec.JPEG, writeStream, callback);
        }

        /// <summary>
        /// Encode an image with the PNG codec.
        /// </summary>
        /// <param name="writeStream">The stream to write the encoded image to.</param>
        /// <param name="_pSrcFrame">A pointer to the raw frame.</param>
        /// <returns>
        /// If greater than zero, then this are the number of bytes written to the <paramref name="writeStream"/>;
        /// otherwise a negative value is returned in case of errors.
        /// </returns>
        public static int EncodeInPNG(Stream writeStream, AVFrame* _pSrcFrame)
        {
            // Checks
            if (writeStream == null || writeStream.CanWrite == false)
                return int.MinValue + 1;
            if (_pSrcFrame == null)
                return int.MinValue + 2;
            // Encode and return
            return encode(_pSrcFrame, AVPixelFormat.AV_PIX_FMT_RGBA, FFmpegImageCodec.PNG, writeStream, null);
        }

        /// <summary>
        /// Encode an image with the BMP codec.
        /// </summary>
        /// <param name="writeStream">The stream to write the encoded image to.</param>
        /// <param name="_pSrcFrame">A pointer to the raw frame.</param>
        /// <returns>
        /// If greater than zero, then this are the number of bytes written to the <paramref name="writeStream"/>;
        /// otherwise a negative value is returned in case of errors.
        /// </returns>
        public static int EncodeInBMP(Stream writeStream, AVFrame* _pSrcFrame)
        {
            // Checks
            if (writeStream == null || writeStream.CanWrite == false)
                return int.MinValue + 1;
            if (_pSrcFrame == null)
                return int.MinValue + 2;
            // Encode and return
            return encode(_pSrcFrame, AVPixelFormat.AV_PIX_FMT_BGRA, FFmpegImageCodec.BMP, writeStream, null);
        }

        /// <summary>
        /// A callback to provide additional codec parameters.
        /// </summary>
        /// <param name="_pEncoderContext"></param>
        private delegate void EncoderContextCallback(AVCodecContext* _pEncoderContext);

        /// <summary>
        /// Encodes an image.
        /// </summary>
        private static int encode(AVFrame* _pSrcFrame, AVPixelFormat pixelFormat, FFmpegImageCodec imageCodec, Stream writeStream, EncoderContextCallback callback)
        {
            AVFrame* _pDecodedFrame = _pSrcFrame;
            // Check pixelformat and adapt if necessary
            FFmpegFrameConverter ffc = null;
            if ((AVPixelFormat)_pSrcFrame->Format != pixelFormat)
            {
                ffc = new FFmpegFrameConverter(_pSrcFrame->Width, _pSrcFrame->Height, (AVPixelFormat)_pSrcFrame->Format, pixelFormat);
                _pDecodedFrame = ffc.Convert(_pSrcFrame);
                if (_pDecodedFrame == null)
                {
                    ffc.Close();
                    return int.MinValue + 3;
                }
            }

            // Get codec and create encoder context
            AVCodecContext* _pEncoderContext = initializeEncodingContext(imageCodec, _pDecodedFrame);
            if (_pEncoderContext == null) return int.MinValue + 4;
            // Add specific parameters for the codec to the context
            callback?.Invoke(_pEncoderContext);

            int result;
            AVPacket* _pEncodedPacket = null;
            // Start encoder
            result = FFmpeg.AVCodecOpen(_pEncoderContext, _pEncoderContext->Codec, null);
            // Encode
            if (result == 0)
                result = FFmpeg.AVCodecSendFrame(_pEncoderContext, _pDecodedFrame);
            if (result == 0)
            {
                _pEncodedPacket = FFmpeg.AVPacketAlloc();
                result = FFmpeg.AVCodecReceivePacket(_pEncoderContext, _pEncodedPacket);
            }
            // Write result to stream
            if (result == 0)
            {
                result = _pEncodedPacket->Size;
                UnmanagedMemoryStream packetStream = new UnmanagedMemoryStream(_pEncodedPacket->Data, _pEncodedPacket->Size);
                packetStream.CopyTo(writeStream);
                //// The own implementation, but very inefficient
                //for (int i = 0; i < _pEncodedPacket->Size; i++)
                //    writeStream.WriteByte(_pEncodedPacket->Data[i]);
            }
            // Unreference and free any allocated memory
            if (ffc != null) ffc.Close();
            FFmpeg.AVPacketFree(_pEncodedPacket);
            FFmpeg.AVCodecContextFree(_pEncoderContext);
            return result;
        }

        /// <summary>
        /// Initializes the encoding context.
        /// </summary>
        private static AVCodecContext* initializeEncodingContext(FFmpegImageCodec imageCodec, AVFrame* _pSrcFrame)
        {
            // Get codec and create encoder context
            AVCodec* _pEncoderCodec = null;
            AVCodecContext* _pEncoderContext;
            _pEncoderCodec = FFmpeg.AVCodecFindEncoder((int)imageCodec);
            if (_pEncoderCodec == null) return null;
            _pEncoderContext = FFmpeg.AVCodecContextAlloc(_pEncoderCodec);
            if (_pEncoderContext == null) return null;
            // Set some basic but important values for encoding context
            _pEncoderContext->PixelFormat = (AVPixelFormat)_pSrcFrame->Format;
            _pEncoderContext->Width = _pSrcFrame->Width;
            _pEncoderContext->Height = _pSrcFrame->Height;
            _pEncoderContext->TimeBase = new AVRational { Num = 1, Den = 25 };
            // Return context
            return _pEncoderContext;
        }
    }
}
