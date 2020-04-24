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
    /// Media Type
    /// </summary>
    internal enum AVMediaType : int
    {
        /// <summary>
        /// The stream is of an unknown type. This is usually treated as type DATA.
        /// </summary>
        AVMEDIA_TYPE_UNKNOWN = -1,
        /// <summary>
        /// The stream is of type VIDEO. Also valid for IMAGEs.
        /// </summary>
        AVMEDIA_TYPE_VIDEO = 0,
        /// <summary>
        /// The stream is of tpye AUDIO.
        /// </summary>
        AVMEDIA_TYPE_AUDIO = 1,
        /// <summary>
        /// The stream is of type DATA.
        /// </summary>
        AVMEDIA_TYPE_DATA = 2,
        /// <summary>
        /// The stream is of type SUBTITLE.
        /// </summary>
        AVMEDIA_TYPE_SUBTITLE = 3,
        /// <summary>
        /// The stream is of type ATTACHMENT.
        /// </summary>
        AVMEDIA_TYPE_ATTACHMENT = 4,
        /// <summary>
        /// The stream is of type NB.
        /// </summary>
        AVMEDIA_TYPE_NB = 5,
    }
}
