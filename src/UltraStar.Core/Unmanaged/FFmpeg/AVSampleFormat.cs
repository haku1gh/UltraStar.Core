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
    /// Represents FFmpeg audio sample formats.
    /// </summary>
    internal enum AVSampleFormat : int
    {
        /// <summary>
        /// No sample format.
        /// </summary>
        AV_SAMPLE_FMT_NONE = -1,
        /// <summary>
        /// Sample format: unsigned 8 bits.
        /// </summary>
        AV_SAMPLE_FMT_U8 = 0,
        /// <summary>
        /// Sample format: signed 16 bits.
        /// </summary>
        AV_SAMPLE_FMT_S16 = 1,
        /// <summary>
        /// Sample format: signed 32 bits.
        /// </summary>
        AV_SAMPLE_FMT_S32 = 2,
        /// <summary>
        /// Sample format: float.
        /// </summary>
        AV_SAMPLE_FMT_FLT = 3,
        /// <summary>
        /// Sample format: double.
        /// </summary>
        AV_SAMPLE_FMT_DBL = 4,
        /// <summary>
        /// Sample format: unsigned 8 bits, planar.
        /// </summary>
        AV_SAMPLE_FMT_U8P = 5,
        /// <summary>
        /// Sample format: signed 16 bits, planar.
        /// </summary>
        AV_SAMPLE_FMT_S16P = 6,
        /// <summary>
        /// Sample format: signed 32 bits, planar.
        /// </summary>
        AV_SAMPLE_FMT_S32P = 7,
        /// <summary>
        /// Sample format: float, planar.
        /// </summary>
        AV_SAMPLE_FMT_FLTP = 8,
        /// <summary>
        /// Sample format: double, planar.
        /// </summary>
        AV_SAMPLE_FMT_DBLP = 9,
        /// <summary>
        /// Sample format: signed 64 bits.
        /// </summary>
        AV_SAMPLE_FMT_S64 = 10,
        /// <summary>
        /// Sample format: signed 64 bits, planar.
        /// </summary>
        AV_SAMPLE_FMT_S64P = 11
    }
}
