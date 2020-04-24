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
    /// Represents an FFmpeg pixel format.
    /// </summary>
    internal enum AVPixelFormat : int
    {
        /// <summary>
        /// No pixel format.
        /// </summary>
        AV_PIX_FMT_NONE = -1,
        /// <summary>
        /// Planar YUV 4:2:0, 12bpp, (1 Cr &amp; Cb sample per 2x2 Y samples)
        /// </summary>
        AV_PIX_FMT_YUV420P = 0,
        /// <summary>
        /// Packed YUV 4:2:2, 16bpp, Y0 Cb Y1 Cr
        /// </summary>
        AV_PIX_FMT_YUYV422 = 1,
        /// <summary>
        /// Packed RGB 8:8:8, 24bpp, RGBRGB...
        /// </summary>
        AV_PIX_FMT_RGB24 = 2,
        /// <summary>
        /// Packed RGB 8:8:8, 24bpp, BGRBGR...
        /// </summary>
        AV_PIX_FMT_BGR24 = 3,
        /// <summary>
        /// Planar YUV 4:2:2, 16bpp, (1 Cr &amp; Cb sample per 2x1 Y samples)
        /// </summary>
        AV_PIX_FMT_YUV422P = 4,
        /// <summary>
        /// Planar YUV 4:4:4, 24bpp, (1 Cr &amp; Cb sample per 1x1 Y samples)
        /// </summary>
        AV_PIX_FMT_YUV444P = 5,
        /// <summary>
        /// Planar YUV 4:1:0, 9bpp, (1 Cr &amp; Cb sample per 4x4 Y samples)
        /// </summary>
        AV_PIX_FMT_YUV410P = 6,
        /// <summary>
        /// Planar YUV 4:1:1, 12bpp, (1 Cr &amp; Cb sample per 4x1 Y samples)
        /// </summary>
        AV_PIX_FMT_YUV411P = 7,
        /// <summary>
        /// Y , 8bpp
        /// </summary>
        AV_PIX_FMT_GRAY8 = 8,
        /// <summary>
        /// Y , 1bpp, 0 is white, 1 is black, in each byte pixels are ordered from the msb to the lsb
        /// </summary>
        AV_PIX_FMT_MONOWHITE = 9,
        /// <summary>
        /// Y , 1bpp, 0 is black, 1 is white, in each byte pixels are ordered from the msb to the lsb
        /// </summary>
        AV_PIX_FMT_MONOBLACK = 10,
        /// <summary>
        /// 8 bits with AV_PIX_FMT_RGB32 palette
        /// </summary>
        AV_PIX_FMT_PAL8 = 11,
        /// <summary>
        /// Planar YUV 4:2:0, 12bpp, full scale (JPEG), deprecated in favor of AV_PIX_FMT_YUV420P and setting color_range
        /// </summary>
        AV_PIX_FMT_YUVJ420P = 12,
        /// <summary>
        /// Planar YUV 4:2:2, 16bpp, full scale (JPEG), deprecated in favor of AV_PIX_FMT_YUV422P and setting color_range
        /// </summary>
        AV_PIX_FMT_YUVJ422P = 13,
        /// <summary>
        /// Planar YUV 4:4:4, 24bpp, full scale (JPEG), deprecated in favor of AV_PIX_FMT_YUV444P and setting color_range
        /// </summary>
        AV_PIX_FMT_YUVJ444P = 14,
        /// <summary>
        /// Packed YUV 4:2:2, 16bpp, Cb Y0 Cr Y1
        /// </summary>
        AV_PIX_FMT_UYVY422 = 15,
        /// <summary>
        /// Packed YUV 4:1:1, 12bpp, Cb Y0 Y1 Cr Y2 Y3
        /// </summary>
        AV_PIX_FMT_UYYVYY411 = 16,
        /// <summary>
        /// Packed RGB 3:3:2, 8bpp, (msb)2B 3G 3R(lsb)
        /// </summary>
        AV_PIX_FMT_BGR8 = 17,
        /// <summary>
        /// Packed RGB 1:2:1 bitstream, 4bpp, (msb)1B 2G 1R(lsb), a byte contains two pixels, the first pixel in the byte is the one composed by the 4 msb bits
        /// </summary>
        AV_PIX_FMT_BGR4 = 18,
        /// <summary>
        /// Packed RGB 1:2:1, 8bpp, (msb)1B 2G 1R(lsb)
        /// </summary>
        AV_PIX_FMT_BGR4_BYTE = 19,
        /// <summary>
        /// Packed RGB 3:3:2, 8bpp, (msb)2R 3G 3B(lsb)
        /// </summary>
        AV_PIX_FMT_RGB8 = 20,
        /// <summary>
        /// Packed RGB 1:2:1 bitstream, 4bpp, (msb)1R 2G 1B(lsb), a byte contains two pixels, the first pixel in the byte is the one composed by the 4 msb bits
        /// </summary>
        AV_PIX_FMT_RGB4 = 21,
        /// <summary>
        /// Packed RGB 1:2:1, 8bpp, (msb)1R 2G 1B(lsb)
        /// </summary>
        AV_PIX_FMT_RGB4_BYTE = 22,
        /// <summary>
        /// Planar YUV 4:2:0, 12bpp, 1 plane for Y and 1 plane for the UV components, which are interleaved (first byte U and the following byte V)
        /// </summary>
        AV_PIX_FMT_NV12 = 23,
        /// <summary>
        /// Planar YUV 4:2:0, 12bpp, 1 plane for Y and 1 plane for the UV components, which are interleaved (first byte V and the following byte U)
        /// </summary>
        AV_PIX_FMT_NV21 = 24,
        /// <summary>
        /// Packed ARGB 8:8:8:8, 32bpp, ARGBARGB...
        /// </summary>
        AV_PIX_FMT_ARGB = 25,
        /// <summary>
        /// Packed RGBA 8:8:8:8, 32bpp, RGBARGBA...
        /// </summary>
        AV_PIX_FMT_RGBA = 26,
        /// <summary>
        /// Packed ABGR 8:8:8:8, 32bpp, ABGRABGR...
        /// </summary>
        AV_PIX_FMT_ABGR = 27,
        /// <summary>
        /// Packed BGRA 8:8:8:8, 32bpp, BGRABGRA...
        /// </summary>
        AV_PIX_FMT_BGRA = 28,
        /// <summary>
        /// Y , 16bpp, big-endian
        /// </summary>
        AV_PIX_FMT_GRAY16BE = 29,
        /// <summary>
        /// Y , 16bpp, little-endian
        /// </summary>
        AV_PIX_FMT_GRAY16LE = 30,
        /// <summary>
        /// Planar YUV 4:4:0 (1 Cr &amp; Cb sample per 1x2 Y samples)
        /// </summary>
        AV_PIX_FMT_YUV440P = 31,
        /// <summary>
        /// Planar YUV 4:4:0 full scale (JPEG), deprecated in favor of AV_PIX_FMT_YUV440P and setting color_range
        /// </summary>
        AV_PIX_FMT_YUVJ440P = 32,
        /// <summary>
        /// Planar YUV 4:2:0, 20bpp, (1 Cr &amp; Cb sample per 2x2 Y &amp; A samples)
        /// </summary>
        AV_PIX_FMT_YUVA420P = 33,
        /// <summary>
        /// Packed RGB 16:16:16, 48bpp, 16R, 16G, 16B, the 2-byte value for each R/G/B component is stored as big-endian
        /// </summary>
        AV_PIX_FMT_RGB48BE = 34,
        /// <summary>
        /// Packed RGB 16:16:16, 48bpp, 16R, 16G, 16B, the 2-byte value for each R/G/B component is stored as little-endian
        /// </summary>
        AV_PIX_FMT_RGB48LE = 35,
        /// <summary>
        /// Packed RGB 5:6:5, 16bpp, (msb) 5R 6G 5B(lsb), big-endian
        /// </summary>
        AV_PIX_FMT_RGB565BE = 36,
        /// <summary>
        /// Packed RGB 5:6:5, 16bpp, (msb) 5R 6G 5B(lsb), little-endian
        /// </summary>
        AV_PIX_FMT_RGB565LE = 37,
        /// <summary>
        /// Packed RGB 5:5:5, 16bpp, (msb)1X 5R 5G 5B(lsb), big-endian , X=unused/undefined
        /// </summary>
        AV_PIX_FMT_RGB555BE = 38,
        /// <summary>
        /// Packed RGB 5:5:5, 16bpp, (msb)1X 5R 5G 5B(lsb), little-endian, X=unused/undefined
        /// </summary>
        AV_PIX_FMT_RGB555LE = 39,
        /// <summary>
        /// Packed BGR 5:6:5, 16bpp, (msb) 5B 6G 5R(lsb), big-endian
        /// </summary>
        AV_PIX_FMT_BGR565BE = 40,
        /// <summary>
        /// Packed BGR 5:6:5, 16bpp, (msb) 5B 6G 5R(lsb), little-endian
        /// </summary>
        AV_PIX_FMT_BGR565LE = 41,
        /// <summary>
        /// Packed BGR 5:5:5, 16bpp, (msb)1X 5B 5G 5R(lsb), big-endian , X=unused/undefined
        /// </summary>
        AV_PIX_FMT_BGR555BE = 42,
        /// <summary>
        /// Packed BGR 5:5:5, 16bpp, (msb)1X 5B 5G 5R(lsb), little-endian, X=unused/undefined
        /// </summary>
        AV_PIX_FMT_BGR555LE = 43,
        /// <summary>
        /// HW acceleration through VA API at motion compensation entry-point, Picture.data[3] contains a vaapi_render_state struct which contains macroblocks as well as various fields extracted from headers
        /// </summary>
        AV_PIX_FMT_VAAPI_MOCO = 44,
        /// <summary>
        /// HW acceleration through VA API at IDCT entry-point, Picture.data[3] contains a vaapi_render_state struct which contains fields extracted from headers
        /// </summary>
        AV_PIX_FMT_VAAPI_IDCT = 45,
        /// <summary>
        /// HW decoding through VA API, Picture.data[3] contains a VASurfaceID
        /// </summary>
        AV_PIX_FMT_VAAPI_VLD = 46,
        /// <summary>
        /// HW decoding through VA API.
        /// </summary>
        AV_PIX_FMT_VAAPI = 46,
        /// <summary>
        /// Planar YUV 4:2:0, 24bpp, (1 Cr &amp; Cb sample per 2x2 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV420P16LE = 47,
        /// <summary>
        /// Planar YUV 4:2:0, 24bpp, (1 Cr &amp; Cb sample per 2x2 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV420P16BE = 48,
        /// <summary>
        /// Planar YUV 4:2:2, 32bpp, (1 Cr &amp; Cb sample per 2x1 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV422P16LE = 49,
        /// <summary>
        /// Planar YUV 4:2:2, 32bpp, (1 Cr &amp; Cb sample per 2x1 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV422P16BE = 50,
        /// <summary>
        /// Planar YUV 4:4:4, 48bpp, (1 Cr &amp; Cb sample per 1x1 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV444P16LE = 51,
        /// <summary>
        /// Planar YUV 4:4:4, 48bpp, (1 Cr &amp; Cb sample per 1x1 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV444P16BE = 52,
        /// <summary>
        /// HW decoding through DXVA2, Picture.data[3] contains a LPDIRECT3DSURFACE9 pointer
        /// </summary>
        AV_PIX_FMT_DXVA2_VLD = 53,
        /// <summary>
        /// Packed RGB 4:4:4, 16bpp, (msb)4X 4R 4G 4B(lsb), little-endian, X=unused/undefined
        /// </summary>
        AV_PIX_FMT_RGB444LE = 54,
        /// <summary>
        /// Packed RGB 4:4:4, 16bpp, (msb)4X 4R 4G 4B(lsb), big-endian, X=unused/undefined
        /// </summary>
        AV_PIX_FMT_RGB444BE = 55,
        /// <summary>
        /// Packed BGR 4:4:4, 16bpp, (msb)4X 4B 4G 4R(lsb), little-endian, X=unused/undefined
        /// </summary>
        AV_PIX_FMT_BGR444LE = 56,
        /// <summary>
        /// Packed BGR 4:4:4, 16bpp, (msb)4X 4B 4G 4R(lsb), big-endian, X=unused/undefined
        /// </summary>
        AV_PIX_FMT_BGR444BE = 57,
        /// <summary>
        /// 8 bits gray, 8 bits alpha
        /// </summary>
        AV_PIX_FMT_YA8 = 58,
        /// <summary>
        /// Alias for AV_PIX_FMT_YA8
        /// </summary>
        AV_PIX_FMT_Y400A = 58,
        /// <summary>
        /// Alias for AV_PIX_FMT_YA8
        /// </summary>
        AV_PIX_FMT_GRAY8A = 58,
        /// <summary>
        /// Packed RGB 16:16:16, 48bpp, 16B, 16G, 16R, the 2-byte value for each R/G/B component is stored as big-endian
        /// </summary>
        AV_PIX_FMT_BGR48BE = 59,
        /// <summary>
        /// Packed RGB 16:16:16, 48bpp, 16B, 16G, 16R, the 2-byte value for each R/G/B component is stored as little-endian
        /// </summary>
        AV_PIX_FMT_BGR48LE = 60,
        /// <summary>
        /// Planar YUV 4:2:0, 13.5bpp, (1 Cr &amp; Cb sample per 2x2 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV420P9BE = 61,
        /// <summary>
        /// Planar YUV 4:2:0, 13.5bpp, (1 Cr &amp; Cb sample per 2x2 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV420P9LE = 62,
        /// <summary>
        /// Planar YUV 4:2:0, 15bpp, (1 Cr &amp; Cb sample per 2x2 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV420P10BE = 63,
        /// <summary>
        /// Planar YUV 4:2:0, 15bpp, (1 Cr &amp; Cb sample per 2x2 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV420P10LE = 64,
        /// <summary>
        /// Planar YUV 4:2:2, 20bpp, (1 Cr &amp; Cb sample per 2x1 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV422P10BE = 65,
        /// <summary>
        /// Planar YUV 4:2:2, 20bpp, (1 Cr &amp; Cb sample per 2x1 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV422P10LE = 66,
        /// <summary>
        /// Planar YUV 4:4:4, 27bpp, (1 Cr &amp; Cb sample per 1x1 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV444P9BE = 67,
        /// <summary>
        /// Planar YUV 4:4:4, 27bpp, (1 Cr &amp; Cb sample per 1x1 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV444P9LE = 68,
        /// <summary>
        /// Planar YUV 4:4:4, 30bpp, (1 Cr &amp; Cb sample per 1x1 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV444P10BE = 69,
        /// <summary>
        /// Planar YUV 4:4:4, 30bpp, (1 Cr &amp; Cb sample per 1x1 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV444P10LE = 70,
        /// <summary>
        /// Planar YUV 4:2:2, 18bpp, (1 Cr &amp; Cb sample per 2x1 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV422P9BE = 71,
        /// <summary>
        /// Planar YUV 4:2:2, 18bpp, (1 Cr &amp; Cb sample per 2x1 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV422P9LE = 72,
        /// <summary>
        /// Planar GBR 4:4:4 24bpp
        /// </summary>
        AV_PIX_FMT_GBRP = 73,
        /// <summary>
        /// Planar GBR 4:4:4 24bpp
        /// </summary>
        AV_PIX_FMT_GBR24P = 73,
        /// <summary>
        /// Planar GBR 4:4:4 27bpp, big-endian
        /// </summary>
        AV_PIX_FMT_GBRP9BE = 74,
        /// <summary>
        /// Planar GBR 4:4:4 27bpp, little-endian
        /// </summary>
        AV_PIX_FMT_GBRP9LE = 75,
        /// <summary>
        /// Planar GBR 4:4:4 30bpp, big-endian
        /// </summary>
        AV_PIX_FMT_GBRP10BE = 76,
        /// <summary>
        /// Planar GBR 4:4:4 30bpp, little-endian
        /// </summary>
        AV_PIX_FMT_GBRP10LE = 77,
        /// <summary>
        /// Planar GBR 4:4:4 48bpp, big-endian
        /// </summary>
        AV_PIX_FMT_GBRP16BE = 78,
        /// <summary>
        /// Planar GBR 4:4:4 48bpp, little-endian
        /// </summary>
        AV_PIX_FMT_GBRP16LE = 79,
        /// <summary>
        /// Planar YUV 4:2:2 24bpp, (1 Cr &amp; Cb sample per 2x1 Y &amp; A samples)
        /// </summary>
        AV_PIX_FMT_YUVA422P = 80,
        /// <summary>
        /// Planar YUV 4:4:4 32bpp, (1 Cr &amp; Cb sample per 1x1 Y &amp; A samples)
        /// </summary>
        AV_PIX_FMT_YUVA444P = 81,
        /// <summary>
        /// Planar YUV 4:2:0 22.5bpp, (1 Cr &amp; Cb sample per 2x2 Y &amp; A samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUVA420P9BE = 82,
        /// <summary>
        /// Planar YUV 4:2:0 22.5bpp, (1 Cr &amp; Cb sample per 2x2 Y &amp; A samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUVA420P9LE = 83,
        /// <summary>
        /// Planar YUV 4:2:2 27bpp, (1 Cr &amp; Cb sample per 2x1 Y &amp; A samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUVA422P9BE = 84,
        /// <summary>
        /// Planar YUV 4:2:2 27bpp, (1 Cr &amp; Cb sample per 2x1 Y &amp; A samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUVA422P9LE = 85,
        /// <summary>
        /// Planar YUV 4:4:4 36bpp, (1 Cr &amp; Cb sample per 1x1 Y &amp; A samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUVA444P9BE = 86,
        /// <summary>
        /// Planar YUV 4:4:4 36bpp, (1 Cr &amp; Cb sample per 1x1 Y &amp; A samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUVA444P9LE = 87,
        /// <summary>
        /// Planar YUV 4:2:0 25bpp, (1 Cr &amp; Cb sample per 2x2 Y &amp; A samples, big-endian)
        /// </summary>
        AV_PIX_FMT_YUVA420P10BE = 88,
        /// <summary>
        /// Planar YUV 4:2:0 25bpp, (1 Cr &amp; Cb sample per 2x2 Y &amp; A samples, little-endian)
        /// </summary>
        AV_PIX_FMT_YUVA420P10LE = 89,
        /// <summary>
        /// Planar YUV 4:2:2 30bpp, (1 Cr &amp; Cb sample per 2x1 Y &amp; A samples, big-endian)
        /// </summary>
        AV_PIX_FMT_YUVA422P10BE = 90,
        /// <summary>
        /// Planar YUV 4:2:2 30bpp, (1 Cr &amp; Cb sample per 2x1 Y &amp; A samples, little-endian)
        /// </summary>
        AV_PIX_FMT_YUVA422P10LE = 91,
        /// <summary>
        /// Planar YUV 4:4:4 40bpp, (1 Cr &amp; Cb sample per 1x1 Y &amp; A samples, big-endian)
        /// </summary>
        AV_PIX_FMT_YUVA444P10BE = 92,
        /// <summary>
        /// Planar YUV 4:4:4 40bpp, (1 Cr &amp; Cb sample per 1x1 Y &amp; A samples, little-endian)
        /// </summary>
        AV_PIX_FMT_YUVA444P10LE = 93,
        /// <summary>
        /// Planar YUV 4:2:0 40bpp, (1 Cr &amp; Cb sample per 2x2 Y &amp; A samples, big-endian)
        /// </summary>
        AV_PIX_FMT_YUVA420P16BE = 94,
        /// <summary>
        /// Planar YUV 4:2:0 40bpp, (1 Cr &amp; Cb sample per 2x2 Y &amp; A samples, little-endian)
        /// </summary>
        AV_PIX_FMT_YUVA420P16LE = 95,
        /// <summary>
        /// Planar YUV 4:2:2 48bpp, (1 Cr &amp; Cb sample per 2x1 Y &amp; A samples, big-endian)
        /// </summary>
        AV_PIX_FMT_YUVA422P16BE = 96,
        /// <summary>
        /// Planar YUV 4:2:2 48bpp, (1 Cr &amp; Cb sample per 2x1 Y &amp; A samples, little-endian)
        /// </summary>
        AV_PIX_FMT_YUVA422P16LE = 97,
        /// <summary>
        /// Planar YUV 4:4:4 64bpp, (1 Cr &amp; Cb sample per 1x1 Y &amp; A samples, big-endian)
        /// </summary>
        AV_PIX_FMT_YUVA444P16BE = 98,
        /// <summary>
        /// Planar YUV 4:4:4 64bpp, (1 Cr &amp; Cb sample per 1x1 Y &amp; A samples, little-endian)
        /// </summary>
        AV_PIX_FMT_YUVA444P16LE = 99,
        /// <summary>
        /// HW acceleration through VDPAU, Picture.data[3] contains a VdpVideoSurface
        /// </summary>
        AV_PIX_FMT_VDPAU = 100,
        /// <summary>
        /// Packed XYZ 4:4:4, 36 bpp, (msb) 12X, 12Y, 12Z (lsb), the 2-byte value for each X/Y/Z is stored as little-endian, the 4 lower bits are set to 0
        /// </summary>
        AV_PIX_FMT_XYZ12LE = 101,
        /// <summary>
        /// Packed XYZ 4:4:4, 36 bpp, (msb) 12X, 12Y, 12Z (lsb), the 2-byte value for each X/Y/Z is stored as big-endian, the 4 lower bits are set to 0
        /// </summary>
        AV_PIX_FMT_XYZ12BE = 102,
        /// <summary>
        /// Interleaved chroma YUV 4:2:2, 16bpp, (1 Cr &amp; Cb sample per 2x1 Y samples)
        /// </summary>
        AV_PIX_FMT_NV16 = 103,
        /// <summary>
        /// Interleaved chroma YUV 4:2:2, 20bpp, (1 Cr &amp; Cb sample per 2x1 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_NV20LE = 104,
        /// <summary>
        /// Interleaved chroma YUV 4:2:2, 20bpp, (1 Cr &amp; Cb sample per 2x1 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_NV20BE = 105,
        /// <summary>
        /// Packed RGBA 16:16:16:16, 64bpp, 16R, 16G, 16B, 16A, the 2-byte value for each R/G/B/A component is stored as big-endian
        /// </summary>
        AV_PIX_FMT_RGBA64BE = 106,
        /// <summary>
        /// Packed RGBA 16:16:16:16, 64bpp, 16R, 16G, 16B, 16A, the 2-byte value for each R/G/B/A component is stored as little-endian
        /// </summary>
        AV_PIX_FMT_RGBA64LE = 107,
        /// <summary>
        /// Packed RGBA 16:16:16:16, 64bpp, 16B, 16G, 16R, 16A, the 2-byte value for each R/G/B/A component is stored as big-endian
        /// </summary>
        AV_PIX_FMT_BGRA64BE = 108,
        /// <summary>
        /// Packed RGBA 16:16:16:16, 64bpp, 16B, 16G, 16R, 16A, the 2-byte value for each R/G/B/A component is stored as little-endian
        /// </summary>
        AV_PIX_FMT_BGRA64LE = 109,
        /// <summary>
        /// Packed YUV 4:2:2, 16bpp, Y0 Cr Y1 Cb
        /// </summary>
        AV_PIX_FMT_YVYU422 = 110,
        /// <summary>
        /// 16 bits gray, 16 bits alpha (big-endian)
        /// </summary>
        AV_PIX_FMT_YA16BE = 111,
        /// <summary>
        /// 16 bits gray, 16 bits alpha (little-endian)
        /// </summary>
        AV_PIX_FMT_YA16LE = 112,
        /// <summary>
        /// Planar GBRA 4:4:4:4 32bpp
        /// </summary>
        AV_PIX_FMT_GBRAP = 113,
        /// <summary>
        /// Planar GBRA 4:4:4:4 64bpp, big-endian
        /// </summary>
        AV_PIX_FMT_GBRAP16BE = 114,
        /// <summary>
        /// Planar GBRA 4:4:4:4 64bpp, little-endian
        /// </summary>
        AV_PIX_FMT_GBRAP16LE = 115,
        /// <summary>
        /// HW acceleration through QSV, data[3] contains a pointer to the mfxFrameSurface1 structure.
        /// </summary>
        AV_PIX_FMT_QSV = 116,
        /// <summary>
        /// HW acceleration though MMAL, data[3] contains a pointer to the MMAL_BUFFER_HEADER_T structure.
        /// </summary>
        AV_PIX_FMT_MMAL = 117,
        /// <summary>
        /// HW decoding through Direct3D11 via old API, Picture.data[3] contains a ID3D11VideoDecoderOutputView pointer
        /// </summary>
        AV_PIX_FMT_D3D11VA_VLD = 118,
        /// <summary>
        /// HW acceleration through CUDA. data[i] contain CUdeviceptr pointers exactly as for system memory frames.
        /// </summary>
        AV_PIX_FMT_CUDA = 119,
        /// <summary>
        /// Packed RGB 8:8:8, 32bpp, XRGBXRGB... X=unused/undefined
        /// </summary>
        AV_PIX_FMT_0RGB = 120,
        /// <summary>
        /// Packed RGB 8:8:8, 32bpp, RGBXRGBX... X=unused/undefined
        /// </summary>
        AV_PIX_FMT_RGB0 = 121,
        /// <summary>
        /// Packed BGR 8:8:8, 32bpp, XBGRXBGR... X=unused/undefined
        /// </summary>
        AV_PIX_FMT_0BGR = 122,
        /// <summary>
        /// Packed BGR 8:8:8, 32bpp, BGRXBGRX... X=unused/undefined
        /// </summary>
        AV_PIX_FMT_BGR0 = 123,
        /// <summary>
        /// Planar YUV 4:2:0,18bpp, (1 Cr &amp; Cb sample per 2x2 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV420P12BE = 124,
        /// <summary>
        /// Planar YUV 4:2:0,18bpp, (1 Cr &amp; Cb sample per 2x2 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV420P12LE = 125,
        /// <summary>
        /// Planar YUV 4:2:0,21bpp, (1 Cr &amp; Cb sample per 2x2 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV420P14BE = 126,
        /// <summary>
        /// Planar YUV 4:2:0,21bpp, (1 Cr &amp; Cb sample per 2x2 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV420P14LE = 127,
        /// <summary>
        /// Planar YUV 4:2:2,24bpp, (1 Cr &amp; Cb sample per 2x1 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV422P12BE = 128,
        /// <summary>
        /// Planar YUV 4:2:2,24bpp, (1 Cr &amp; Cb sample per 2x1 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV422P12LE = 129,
        /// <summary>
        /// Planar YUV 4:2:2,28bpp, (1 Cr &amp; Cb sample per 2x1 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV422P14BE = 130,
        /// <summary>
        /// Planar YUV 4:2:2,28bpp, (1 Cr &amp; Cb sample per 2x1 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV422P14LE = 131,
        /// <summary>
        /// Planar YUV 4:4:4,36bpp, (1 Cr &amp; Cb sample per 1x1 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV444P12BE = 132,
        /// <summary>
        /// Planar YUV 4:4:4,36bpp, (1 Cr &amp; Cb sample per 1x1 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV444P12LE = 133,
        /// <summary>
        /// Planar YUV 4:4:4,42bpp, (1 Cr &amp; Cb sample per 1x1 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV444P14BE = 134,
        /// <summary>
        /// Planar YUV 4:4:4,42bpp, (1 Cr &amp; Cb sample per 1x1 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV444P14LE = 135,
        /// <summary>
        /// Planar GBR 4:4:4 36bpp, big-endian
        /// </summary>
        AV_PIX_FMT_GBRP12BE = 136,
        /// <summary>
        /// Planar GBR 4:4:4 36bpp, little-endian
        /// </summary>
        AV_PIX_FMT_GBRP12LE = 137,
        /// <summary>
        /// Planar GBR 4:4:4 42bpp, big-endian
        /// </summary>
        AV_PIX_FMT_GBRP14BE = 138,
        /// <summary>
        /// Planar GBR 4:4:4 42bpp, little-endian
        /// </summary>
        AV_PIX_FMT_GBRP14LE = 139,
        /// <summary>
        /// Planar YUV 4:1:1, 12bpp, (1 Cr &amp; Cb sample per 4x1 Y samples) full scale (JPEG), deprecated in favor of AV_PIX_FMT_YUV411P and setting color_range
        /// </summary>
        AV_PIX_FMT_YUVJ411P = 140,
        /// <summary>
        /// Bayer, BGBG..(odd line), GRGR..(even line), 8-bit samples
        /// </summary>
        AV_PIX_FMT_BAYER_BGGR8 = 141,
        /// <summary>
        /// Bayer, RGRG..(odd line), GBGB..(even line), 8-bit samples
        /// </summary>
        AV_PIX_FMT_BAYER_RGGB8 = 142,
        /// <summary>
        /// Bayer, GBGB..(odd line), RGRG..(even line), 8-bit samples
        /// </summary>
        AV_PIX_FMT_BAYER_GBRG8 = 143,
        /// <summary>
        /// Bayer, GRGR..(odd line), BGBG..(even line), 8-bit samples
        /// </summary>
        AV_PIX_FMT_BAYER_GRBG8 = 144,
        /// <summary>
        /// Bayer, BGBG..(odd line), GRGR..(even line), 16-bit samples, little-endian
        /// </summary>
        AV_PIX_FMT_BAYER_BGGR16LE = 145,
        /// <summary>
        /// Bayer, BGBG..(odd line), GRGR..(even line), 16-bit samples, big-endian
        /// </summary>
        AV_PIX_FMT_BAYER_BGGR16BE = 146,
        /// <summary>
        /// Bayer, RGRG..(odd line), GBGB..(even line), 16-bit samples, little-endian
        /// </summary>
        AV_PIX_FMT_BAYER_RGGB16LE = 147,
        /// <summary>
        /// Bayer, RGRG..(odd line), GBGB..(even line), 16-bit samples, big-endian
        /// </summary>
        AV_PIX_FMT_BAYER_RGGB16BE = 148,
        /// <summary>
        /// Bayer, GBGB..(odd line), RGRG..(even line), 16-bit samples, little-endian
        /// </summary>
        AV_PIX_FMT_BAYER_GBRG16LE = 149,
        /// <summary>
        /// Bayer, GBGB..(odd line), RGRG..(even line), 16-bit samples, big-endian
        /// </summary>
        AV_PIX_FMT_BAYER_GBRG16BE = 150,
        /// <summary>
        /// Bayer, GRGR..(odd line), BGBG..(even line), 16-bit samples, little-endian
        /// </summary>
        AV_PIX_FMT_BAYER_GRBG16LE = 151,
        /// <summary>
        /// Bayer, GRGR..(odd line), BGBG..(even line), 16-bit samples, big-endian
        /// </summary>
        AV_PIX_FMT_BAYER_GRBG16BE = 152,
        /// <summary>
        /// XVideo Motion Acceleration via common packet passing
        /// </summary>
        AV_PIX_FMT_XVMC = 153,
        /// <summary>
        /// Planar YUV 4:4:0,20bpp, (1 Cr &amp; Cb sample per 1x2 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV440P10LE = 154,
        /// <summary>
        /// Planar YUV 4:4:0,20bpp, (1 Cr &amp; Cb sample per 1x2 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV440P10BE = 155,
        /// <summary>
        /// Planar YUV 4:4:0,24bpp, (1 Cr &amp; Cb sample per 1x2 Y samples), little-endian
        /// </summary>
        AV_PIX_FMT_YUV440P12LE = 156,
        /// <summary>
        /// Planar YUV 4:4:0,24bpp, (1 Cr &amp; Cb sample per 1x2 Y samples), big-endian
        /// </summary>
        AV_PIX_FMT_YUV440P12BE = 157,
        /// <summary>
        /// Packed AYUV 4:4:4,64bpp (1 Cr &amp; Cb sample per 1x1 Y &amp; A samples), little-endian
        /// </summary>
        AV_PIX_FMT_AYUV64LE = 158,
        /// <summary>
        /// Packed AYUV 4:4:4,64bpp (1 Cr &amp; Cb sample per 1x1 Y &amp; A samples), big-endian
        /// </summary>
        AV_PIX_FMT_AYUV64BE = 159,
        /// <summary>
        /// Hardware decoding through Videotoolbox
        /// </summary>
        AV_PIX_FMT_VIDEOTOOLBOX = 160,
        /// <summary>
        /// Like NV12, with 10bpp per component, data in the high bits, zeros in the low bits, little-endian
        /// </summary>
        AV_PIX_FMT_P010LE = 161,
        /// <summary>
        /// Like NV12, with 10bpp per component, data in the high bits, zeros in the low bits, big-endian
        /// </summary>
        AV_PIX_FMT_P010BE = 162,
        /// <summary>
        /// Planar GBR 4:4:4:4 48bpp, big-endian
        /// </summary>
        AV_PIX_FMT_GBRAP12BE = 163,
        /// <summary>
        /// Planar GBR 4:4:4:4 48bpp, little-endian
        /// </summary>
        AV_PIX_FMT_GBRAP12LE = 164,
        /// <summary>
        /// Planar GBR 4:4:4:4 40bpp, big-endian
        /// </summary>
        AV_PIX_FMT_GBRAP10BE = 165,
        /// <summary>
        /// Planar GBR 4:4:4:4 40bpp, little-endian
        /// </summary>
        AV_PIX_FMT_GBRAP10LE = 166,
        /// <summary>
        /// Hardware decoding through MediaCodec
        /// </summary>
        AV_PIX_FMT_MEDIACODEC = 167,
        /// <summary>
        /// Y , 12bpp, big-endian
        /// </summary>
        AV_PIX_FMT_GRAY12BE = 168,
        /// <summary>
        /// Y , 12bpp, little-endian
        /// </summary>
        AV_PIX_FMT_GRAY12LE = 169,
        /// <summary>
        /// Y , 10bpp, big-endian
        /// </summary>
        AV_PIX_FMT_GRAY10BE = 170,
        /// <summary>
        /// Y , 10bpp, little-endian
        /// </summary>
        AV_PIX_FMT_GRAY10LE = 171,
        /// <summary>
        /// Like NV12, with 16bpp per component, little-endian
        /// </summary>
        AV_PIX_FMT_P016LE = 172,
        /// <summary>
        /// Like NV12, with 16bpp per component, big-endian
        /// </summary>
        AV_PIX_FMT_P016BE = 173,
        /// <summary>
        /// Hardware surfaces for Direct3D11.
        /// </summary>
        AV_PIX_FMT_D3D11 = 174,
        /// <summary>
        /// Y , 9bpp, big-endian
        /// </summary>
        AV_PIX_FMT_GRAY9BE = 175,
        /// <summary>
        /// Y , 9bpp, little-endian
        /// </summary>
        AV_PIX_FMT_GRAY9LE = 176,
        /// <summary>
        /// IEEE-754 single precision planar GBR 4:4:4, 96bpp, big-endian
        /// </summary>
        AV_PIX_FMT_GBRPF32BE = 177,
        /// <summary>
        /// IEEE-754 single precision planar GBR 4:4:4, 96bpp, little-endian
        /// </summary>
        AV_PIX_FMT_GBRPF32LE = 178,
        /// <summary>
        /// IEEE-754 single precision planar GBRA 4:4:4:4, 128bpp, big-endian
        /// </summary>
        AV_PIX_FMT_GBRAPF32BE = 179,
        /// <summary>
        /// IEEE-754 single precision planar GBRA 4:4:4:4, 128bpp, little-endian
        /// </summary>
        AV_PIX_FMT_GBRAPF32LE = 180,
        /// <summary>
        /// DRM-managed buffers exposed through PRIME buffer sharing.
        /// </summary>
        AV_PIX_FMT_DRM_PRIME = 181,
        /// <summary>
        /// Hardware surfaces for OpenCL.
        /// </summary>
        AV_PIX_FMT_OPENCL = 182,
        /// <summary>
        /// Y , 14bpp, big-endian
        /// </summary>
        AV_PIX_FMT_GRAY14BE = 183,
        /// <summary>
        /// Y , 14bpp, little-endian
        /// </summary>
        AV_PIX_FMT_GRAY14LE = 184,
        /// <summary>
        /// IEEE-754 single precision Y, 32bpp, big-endian
        /// </summary>
        AV_PIX_FMT_GRAYF32BE = 185,
        /// <summary>
        /// IEEE-754 single precision Y, 32bpp, little-endian
        /// </summary>
        AV_PIX_FMT_GRAYF32LE = 186,
        /// <summary>
        /// Planar YUV 4:2:2,24bpp, (1 Cr &amp; Cb sample per 2x1 Y samples), 12b alpha, big-endian
        /// </summary>
        AV_PIX_FMT_YUVA422P12BE = 187,
        /// <summary>
        /// Planar YUV 4:2:2,24bpp, (1 Cr &amp; Cb sample per 2x1 Y samples), 12b alpha, little-endian
        /// </summary>
        AV_PIX_FMT_YUVA422P12LE = 188,
        /// <summary>
        /// Planar YUV 4:4:4,36bpp, (1 Cr &amp; Cb sample per 1x1 Y samples), 12b alpha, big-endian
        /// </summary>
        AV_PIX_FMT_YUVA444P12BE = 189,
        /// <summary>
        /// Planar YUV 4:4:4,36bpp, (1 Cr &amp; Cb sample per 1x1 Y samples), 12b alpha, little-endian
        /// </summary>
        AV_PIX_FMT_YUVA444P12LE = 190,
        /// <summary>
        /// Planar YUV 4:4:4, 24bpp, 1 plane for Y and 1 plane for the UV components, which are interleaved (first byte U and the following byte V)
        /// </summary>
        AV_PIX_FMT_NV24 = 191,
        /// <summary>
        /// Planar YUV 4:4:4, 24bpp, 1 plane for Y and 1 plane for the UV components, which are interleaved (first byte V and the following byte U)
        /// </summary>
        AV_PIX_FMT_NV42 = 192
    }
}
