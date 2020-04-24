#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.Runtime.InteropServices;

namespace UltraStar.Core.Unmanaged.FFmpeg
{
    /// <summary>
    /// Represents an FFmpeg input format.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct AVInputFormat
    {
        /// <summary>
        /// A comma separated list of short names for the format. New names may be appended with a minor bump.
        /// </summary>
        public byte* Name;
        /// <summary>
        /// Descriptive name for the format, meant to be more human-readable than name. You should use the NULL_IF_CONFIG_SMALL() macro to define it.
        /// </summary>
        public byte* LongName;
        /// <summary>
        /// Can use flags: AVFMT_NOFILE, AVFMT_NEEDNUMBER, AVFMT_SHOW_IDS, AVFMT_NOTIMESTAMPS, AVFMT_GENERIC_INDEX, AVFMT_TS_DISCONT, AVFMT_NOBINSEARCH, AVFMT_NOGENSEARCH, AVFMT_NO_BYTE_SEEK, AVFMT_SEEK_TO_PTS.
        /// </summary>
        public int Flags;
        /// <summary>
        /// If extensions are defined, then no probe is done. You should usually not use extension format guessing because it is not reliable enough.
        /// </summary>
        public byte* Extensions;
        /// <summary>
        /// The codec tag.
        /// </summary>
        public IntPtr CodecTag;
        /// <summary>
        /// Private data - interally.
        /// </summary>
        public IntPtr PrivateData;
        /// <summary>
        /// Comma-separated list of mime types. It is used check for matching mime types while probing.
        /// </summary>
        public byte* MimeType;
    }
}
