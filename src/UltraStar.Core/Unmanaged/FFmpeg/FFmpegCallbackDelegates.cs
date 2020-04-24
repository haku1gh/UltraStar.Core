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
    /// Logging callback function.
    /// </summary>
    /// <param name="avcl">A pointer to an arbitrary struct of which the first field is a pointer to an AVClass struct.</param>
    /// <param name="level">The importance level of the message expressed using <see cref="FFmpegLogLevel"/>.</param>
    /// <param name="fmt">The format string (printf-compatible) that specifies how subsequent arguments are converted to output.</param>
    /// <param name="vl">The arguments referenced by the format string.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal unsafe delegate void AvLogSetCallback(void* avcl, FFmpegLogLevel level, [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt, byte* vl);
}
