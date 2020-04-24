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
    /// Represents an FFmpeg packet.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct AVPacket
    {
        /// <summary>
        /// A reference to the reference-counted buffer where the packet data is stored.
        /// </summary>
        /// <remarks>
        /// May be <see langword="null"/>, then the packet data is not reference-counted.
        /// </remarks>
        public IntPtr Buf;
        /// <summary>
        /// The presentation timestamp in AVStream-&gt;time_base units; the time at which the decompressed packet will be presented to the user.
        /// </summary>
        /// <remarks>
        /// Can be AV_NOPTS_VALUE if it is not stored in the file. pts MUST be larger or equal to dts as presentation cannot happen before decompression, unless one wants to view hex dumps.
        /// Some formats misuse the terms dts and pts/cts to mean something different. Such timestamps must be converted to true pts/dts before they are stored in AVPacket.
        /// </remarks>
        public long PTS;
        /// <summary>
        /// The decompression timestamp in AVStream-&gt;time_base units; the time at which the packet is decompressed.
        /// </summary>
        /// <remarks>
        /// Can be AV_NOPTS_VALUE if it is not stored in the file.
        /// </remarks>
        public long DTS;
        /// <summary>
        /// The data of the packet.
        /// </summary>
        public byte* Data;
        /// <summary>
        /// The size of the <see cref="Data"/>.
        /// </summary>
        public int Size;
        /// <summary>
        /// The stream index.
        /// </summary>
        public int StreamIndex;
        /// <summary>
        /// Flags for the packet.
        /// </summary>
        public int Flags;
        /// <summary>
        /// Additional packet data that can be provided by the container. Packet can contain several types of side information.
        /// </summary>
        public IntPtr SideData;
        /// <summary>
        /// The number of elements in <see cref="SideData"/>.
        /// </summary>
        public int SideDataCount;
        /// <summary>
        /// The duration of this packet in AVStream-&gt;time_base units, 0 if unknown. Equals next_pts - this_pts in presentation order.
        /// </summary>
        public long Duration;
        /// <summary>
        /// The byte position in the sstream, -1 if unknown.
        /// </summary>
        public long Position;
        /// <summary>
        /// The convergence duration.
        /// </summary>
        public long ConvergenceDuration;
    }
}
