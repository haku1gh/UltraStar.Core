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
    /// Represents a converter of pixel format and image size in an FFmpeg <see cref="AVFrame"/>. In addition it allows cropping of the image.
    /// </summary>
    internal unsafe class FFmpegFrameConverter : IDisposable
    {
        // Private variables
        private SwsContext* _pSwsContext1;
        private SwsContext* _pSwsContext2;
        private AVFrame* _pFrame1;
        private AVFrame* _pFrame2;
        private AVFrame* _pFrame3;
        private int cropLeft;
        private int cropRight;
        private int cropTop;
        private int cropBottom;
        private bool cropRequired;

        /// <summary>
        /// Indicator if this instance is disposed.
        /// </summary>
        protected bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of <see cref="FFmpegFrameConverter"/>.
        /// </summary>
        /// <param name="srcWidth">The width of the source image.</param>
        /// <param name="srcHeight">The height of the source image.</param>
        /// <param name="srcPixelFormat">The pixel format of the source image.</param>
        /// <param name="scaleWidth">The new width of the image before cropping.</param>
        /// <param name="scaleHeight">The new height of the image before cropping.</param>
        /// <param name="scaleMode">The algorithm used to resizing the image.</param>
        /// <param name="dstCropLeft">The number pixels to crop from the left side of the resulting image.</param>
        /// <param name="dstCropRight">The number pixels to crop from the right side of the resulting image.</param>
        /// <param name="dstCropTop">The number pixels to crop from the top side of the resulting image.</param>
        /// <param name="dstCropBottom">The number pixels to crop from the bottom side of the resulting image.</param>
        /// <param name="dstPixelFormat">The pixel format of the resulting image.</param>
        /// <param name="alignment">The alignment of a scanline in the image.</param>
        public FFmpegFrameConverter(int srcWidth, int srcHeight, AVPixelFormat srcPixelFormat, int scaleWidth, int scaleHeight, FFmpegScaleMode scaleMode,
            int dstCropLeft, int dstCropRight, int dstCropTop, int dstCropBottom, AVPixelFormat dstPixelFormat, int alignment = 1)
        {
            initialize(srcWidth, srcHeight, srcPixelFormat, scaleWidth, scaleHeight, scaleMode, dstCropLeft, dstCropRight, dstCropTop, dstCropBottom, dstPixelFormat, alignment);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FFmpegFrameConverter"/>.
        /// </summary>
        /// <param name="srcWidth">The width of the source image.</param>
        /// <param name="srcHeight">The height of the source image.</param>
        /// <param name="srcPixelFormat">The pixel format of the source image.</param>
        /// <param name="dstPixelFormat">The pixel format of the resulting image.</param>
        /// <param name="alignment">The alignment of a scanline in the image.</param>
        public FFmpegFrameConverter(int srcWidth, int srcHeight, AVPixelFormat srcPixelFormat, AVPixelFormat dstPixelFormat, int alignment = 1)
        {
            initialize(srcWidth, srcHeight, srcPixelFormat, srcWidth, srcHeight, FFmpegScaleMode.FastBilinear, 0, 0, 0, 0, dstPixelFormat, alignment);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FFmpegFrameConverter"/>.
        /// </summary>
        /// <param name="srcWidth">The width of the source image.</param>
        /// <param name="srcHeight">The height of the source image.</param>
        /// <param name="srcPixelFormat">The pixel format of the source image.</param>
        /// <param name="scaleWidth">The new width of the image before cropping.</param>
        /// <param name="scaleHeight">The new height of the image before cropping.</param>
        /// <param name="scaleMode">The algorithm used to resizing the image.</param>
        /// <param name="dstPixelFormat">The pixel format of the resulting image.</param>
        /// <param name="alignment">The alignment of a scanline in the image.</param>
        public FFmpegFrameConverter(int srcWidth, int srcHeight, AVPixelFormat srcPixelFormat, int scaleWidth, int scaleHeight, FFmpegScaleMode scaleMode,
            AVPixelFormat dstPixelFormat, int alignment = 1)
        {
            initialize(srcWidth, srcHeight, srcPixelFormat, scaleWidth, scaleHeight, scaleMode, 0, 0, 0, 0, dstPixelFormat, alignment);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FFmpegFrameConverter"/>. Using a no-streching approach.
        /// </summary>
        /// <remarks>
        /// This constructor transforms an image so that no side is stretched.
        /// The resulting image will be choosen from within the image. No black bars will be added on the sides.
        /// </remarks>
        /// <param name="srcWidth">The width of the source image.</param>
        /// <param name="srcHeight">The height of the source image.</param>
        /// <param name="srcPixelFormat">The pixel format of the source image.</param>
        /// <param name="dstAspectRatio">The aspect ratio of the resulting image.</param>
        /// <param name="dstMinWidth">The minimum width of the resulting image.</param>
        /// <param name="dstMaxWidth">The maximum width of the resulting image.</param>
        /// <param name="scaleMode">The algorithm used to resizing the image.</param>
        /// <param name="dstPixelFormat">The pixel format of the resulting image.</param>
        /// <param name="alignment">The alignment of a scanline in the image.</param>
        public FFmpegFrameConverter(int srcWidth, int srcHeight, AVPixelFormat srcPixelFormat, float dstAspectRatio, int dstMinWidth, int dstMaxWidth,
            FFmpegScaleMode scaleMode, AVPixelFormat dstPixelFormat, int alignment = 1)
        {
            float srcAspectRatio = (float)srcWidth / srcHeight;
            int scaleWidth = 0, scaleHeight = 0;
            int temp = 0, left = 0, right = 0, top = 0, bottom = 0;
            int dstMinHeight = (int)Math.Round(dstMinWidth / dstAspectRatio, 0);
            int dstMaxHeight = (int)Math.Round(dstMaxWidth / dstAspectRatio, 0);
            if (srcAspectRatio > dstAspectRatio)
            {
                scaleHeight = srcHeight;
                if (scaleHeight > dstMaxHeight) scaleHeight = dstMaxHeight;
                if (scaleHeight < dstMinHeight) scaleHeight = dstMinHeight;
                scaleWidth = (int)Math.Round(scaleHeight * srcAspectRatio, 0);
                temp = scaleWidth - (int)Math.Round(scaleHeight * dstAspectRatio, 0);
                left = right = temp / 2;
                if ((temp & 0x1) == 1) right++;
            }
            else
            {
                scaleWidth = srcWidth;
                if (scaleWidth > dstMaxWidth) scaleWidth = dstMaxWidth;
                if (scaleWidth < dstMinWidth) scaleWidth = dstMinWidth;
                scaleHeight = (int)Math.Round(scaleWidth / srcAspectRatio, 0);
                temp = scaleHeight - (int)Math.Round(scaleWidth / dstAspectRatio, 0);
                top = bottom = temp / 2;
                if ((temp & 0x1) == 1) bottom++;
            }
            initialize(srcWidth, srcHeight, srcPixelFormat, scaleWidth, scaleHeight, scaleMode, left, right, top, bottom, dstPixelFormat, alignment);
        }

        /// <summary>
        /// Helpermethod to prepare for future scaling and cropping.
        /// </summary>
        private void initialize(int srcWidth, int srcHeight, AVPixelFormat srcPixelFormat, int scaleWidth, int scaleHeight, FFmpegScaleMode scaleMode,
            int dstCropLeft, int dstCropRight, int dstCropTop, int dstCropBottom, AVPixelFormat dstPixelFormat, int alignment)
        {
            cropRequired = isCropping(dstCropLeft, dstCropRight, dstCropTop, dstCropBottom);
            // Prepare scaling 1
            if(cropRequired && !((srcWidth == scaleWidth) && (srcHeight == scaleHeight) && (srcPixelFormat == AVPixelFormat.AV_PIX_FMT_RGBA)))
            {
                _pSwsContext1 = FFmpeg.SwsGetContext(srcWidth, srcHeight, srcPixelFormat, scaleWidth, scaleHeight, AVPixelFormat.AV_PIX_FMT_RGBA, scaleMode);
                if (_pSwsContext1 == null)
                    throw new FFmpegException("Could not initialize the frame conversion context.");
                allocateImage(ref _pFrame1, scaleWidth, scaleHeight, AVPixelFormat.AV_PIX_FMT_RGBA, 4);
            }
            // Prepare cropping
            if (cropRequired)
            {
                int finalWidth = scaleWidth - dstCropLeft - dstCropRight;
                int finalHeight = scaleHeight - dstCropTop - dstCropBottom;
                cropLeft = dstCropLeft;
                cropRight = dstCropRight;
                cropTop = dstCropTop;
                cropBottom = dstCropBottom;
                allocateImage(ref _pFrame2, finalWidth, finalHeight, AVPixelFormat.AV_PIX_FMT_RGBA, 4);
            }
            // Prepare scaling 2
            if ((cropRequired && dstPixelFormat != AVPixelFormat.AV_PIX_FMT_RGBA))
            {
                _pSwsContext2 = FFmpeg.SwsGetContext(_pFrame2->Width, _pFrame2->Height, AVPixelFormat.AV_PIX_FMT_RGBA, _pFrame2->Width, _pFrame2->Height, dstPixelFormat, FFmpegScaleMode.FastBilinear);
                if (_pSwsContext2 == null)
                    throw new FFmpegException("Could not initialize the frame conversion context.");
                allocateImage(ref _pFrame3, _pFrame2->Width, _pFrame2->Height, dstPixelFormat, alignment);
            }
            else if(!cropRequired && !((srcWidth == scaleWidth) && (srcHeight == scaleHeight) && (srcPixelFormat == dstPixelFormat)))
            {
                _pSwsContext2 = FFmpeg.SwsGetContext(srcWidth, srcHeight, srcPixelFormat, scaleWidth, scaleHeight, dstPixelFormat, scaleMode);
                if (_pSwsContext2 == null)
                    throw new FFmpegException("Could not initialize the frame conversion context.");
                allocateImage(ref _pFrame3, scaleWidth, scaleHeight, dstPixelFormat, alignment);
            }
        }

        /// <summary>
        /// Allocates a new image.
        /// </summary>
        private void allocateImage(ref AVFrame* _pFrame, int width, int height, AVPixelFormat pixelFormat, int alignment)
        {
            // Allocate memory for the frame structure
            _pFrame = FFmpeg.AVFrameAlloc();
            // Set width, height and pixel format
            _pFrame->Width = width;
            _pFrame->Height = height;
            _pFrame->Format = (int)pixelFormat;
            // Allocate memory for image itself
            int ret = FFmpeg.AVImageAlloc(ref _pFrame->Data, ref _pFrame->Linesize, width, height, pixelFormat, alignment);
        }

        /// <summary>
        /// Frees an existing image.
        /// </summary>
        private void freeImage(ref AVFrame* _pFrame)
        {
            if(_pFrame != null)
            {
                FFmpeg.AVFreeP(_pFrame->Data.Element0);
                FFmpeg.AVFrameFree(_pFrame);
            }
            _pFrame = null;
        }

        /// <summary>
        /// Gets an indicator whether the resulting image is cropped.
        /// </summary>
        private bool isCropping(int cropLeft, int cropRight, int cropTop, int cropBottom)
        {
            return cropLeft > 0 || cropRight > 0 || cropTop > 0 || cropBottom > 0;
        }


        /// <summary>
        /// Finalizes this instance.
        /// </summary>
        ~FFmpegFrameConverter()
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
                    freeImage(ref _pFrame1);
                    freeImage(ref _pFrame2);
                    freeImage(ref _pFrame3);
                    FFmpeg.SwsFreeContext(_pSwsContext1);
                    _pSwsContext1 = null;
                    FFmpeg.SwsFreeContext(_pSwsContext2);
                    _pSwsContext2 = null;
                }
                catch { }
            }
        }

        /// <summary>
        /// Converts an <see cref="AVFrame"/> using the previously set parameters.
        /// </summary>
        /// <param name="_pSrcFrame">The source frame.</param>
        /// <returns>The converted frame if successful; otherwise <see langword="null"/>.</returns>
        public AVFrame* Convert(AVFrame* _pSrcFrame)
        {
            // Check if disposed
            if (isDisposed)
                throw new ObjectDisposedException(nameof(FFmpegFrameConverter));
            if (_pSrcFrame == null)
                return null;
            // Init tempory variables
            int result;
            AVFrame* _pFrame;

            // Rescale image and change pixelformat to RGBA
            if (_pSwsContext1 == null)
                _pFrame = _pSrcFrame;
            else
            {
                result = FFmpeg.SwsScale(_pSwsContext1, _pSrcFrame->Data.ToBytePtrArray(), _pSrcFrame->Linesize.ToIntArray(), 0, _pSrcFrame->Height, _pFrame1->Data.ToBytePtrArray(), _pFrame1->Linesize.ToIntArray());
                if (result < 0) return null;
                _pFrame = _pFrame1;
            }
            // Crop image in RGBA
            if (cropRequired)
            {
                byte* convBufPtr = (byte*)_pFrame->Data.Element0 + (_pFrame->Linesize.Element0 * cropTop) + cropLeft * 4;
                byte* cropBufPtr = (byte*)_pFrame2->Data.Element0;
                if ((cropLeft + cropRight) == 0)
                {
                    convBufPtr += _pFrame->Linesize.Element0;
                    long size = _pFrame2->Linesize.Element0 * _pFrame2->Height;
                    Buffer.MemoryCopy(convBufPtr, cropBufPtr, size, size);
                }
                else
                {
                    for (int h = cropTop; h < (_pFrame->Height - cropBottom); h++)
                    {
                        Buffer.MemoryCopy(convBufPtr, cropBufPtr, _pFrame2->Linesize.Element0, _pFrame2->Linesize.Element0);
                        convBufPtr += _pFrame->Linesize.Element0;
                        cropBufPtr += _pFrame2->Linesize.Element0;
                    }
                }
                //// The own implementation, but of course not as efficient, ~50% slower
                //int* convBufPtr = (int*)_pFrame->Data.Element0;
                //int* cropBufPtr = (int*)_pFrame2->Data.Element0;
                //int pos, pos2 = 0;
                //for (int h = cropTop; h < (_pFrame->Height - cropBottom); h++)
                //{
                //    pos = h * _pFrame->Width + cropLeft;
                //    for (int w = cropLeft; w < (_pFrame->Width - cropRight); w++)
                //    {
                //        // Move pixel
                //        cropBufPtr[pos2++] = convBufPtr[pos++];
                //    }
                //}
                _pFrame = _pFrame2;
            }

            // Change pixelformat to dst pixelformat (and perform rescaling if not happened in context 1)
            if (_pSwsContext2 != null)
            {
                result = FFmpeg.SwsScale(_pSwsContext2, _pFrame->Data.ToBytePtrArray(), _pFrame->Linesize.ToIntArray(), 0, _pFrame->Height, _pFrame3->Data.ToBytePtrArray(), _pFrame3->Linesize.ToIntArray());
                if (result < 0) return null;
                _pFrame = _pFrame3;
            }

            return _pFrame;
        }
    }
}
