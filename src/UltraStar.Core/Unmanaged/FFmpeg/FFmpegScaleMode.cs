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
    /// The scale mode for FFmpeg scale frame operations.
    /// </summary>
    internal enum FFmpegScaleMode : int
    {
        /// <summary>
        /// ScaleMode: Fast Bilinear.
        /// </summary>
        FastBilinear = 1,
        /// <summary>
        /// ScaleMode: Bilinear.
        /// </summary>
        Bilinear = 2,
        /// <summary>
        /// ScaleMode: Bicubic.
        /// </summary>
        Bicubic = 4,
        /// <summary>
        /// ScaleMode: X.
        /// </summary>
        X = 8,
        /// <summary>
        /// ScaleMode: Point.
        /// </summary>
        Point = 0x10,
        /// <summary>
        /// ScaleMode: Area.
        /// </summary>
        Area = 0x20,
        /// <summary>
        /// ScaleMode: Bicubic Linear.
        /// </summary>
        BicubicLinear = 0x40,
        /// <summary>
        /// ScaleMode: Gaussian.
        /// </summary>
        Gaussian = 0x80,
        /// <summary>
        /// ScaleMode: Sinc.
        /// </summary>
        Sinc = 0x100,
        /// <summary>
        /// ScaleMode: Lanczos.
        /// </summary>
        Lanczos = 0x200,
        /// <summary>
        /// ScaleMode: Spline.
        /// </summary>
        Spline = 0x400,
    }
}
