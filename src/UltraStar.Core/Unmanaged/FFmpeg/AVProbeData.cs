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
    /// Represents FFmpeg probe data.
    /// </summary>
    internal unsafe struct AVProbeData
    {
        /// <summary>
        /// The file name.
        /// </summary>
        public byte* Filename;
        /// <summary>
        /// The buffer. It must have AVPROBE_PADDING_SIZE of extra allocated bytes filled with zero.
        /// </summary>
        public byte* Buf;
        /// <summary>
        /// The size of <see cref="Buf"/> except extra allocated bytes
        /// </summary>
        public int BufSize;
        /// <summary>
        /// The MIME type when known.
        /// </summary>
        public byte* MimeType;
    }
}
