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
    /// The codec in which the image was or will be encoded.
    /// </summary>
    internal enum FFmpegImageCodec : int
    {
        /// <summary>
        /// No image codec.
        /// </summary>
        None = 0,
        /// <summary>
        /// Codec is JPG or JPEG.
        /// </summary>
        JPEG = 7,
        /// <summary>
        /// Codec is PNG.
        /// </summary>
        PNG = 61,
        /// <summary>
        /// Codec is BMP.
        /// </summary>
        BMP = 78,
        /// <summary>
        /// Codec is WebP.
        /// </summary>
        WEBP = 171
    }
}
