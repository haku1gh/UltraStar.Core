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
    /// The log level for any method calls to FFmpeg.
    /// </summary>
    internal enum FFmpegLogLevel : int
    {
        /// <summary>
        /// Print no output.
        /// </summary>
        Quiet = -8,
        /// <summary>
        /// Something went really wrong and we will crash now.
        /// </summary>
        Panic = 0,
        /// <summary>
        /// Something went wrong and recovery is not possible.
        /// </summary>
        /// <remarks>
        /// For example, no header was found for a format which depends on headers or an illegal combination of parameters is used.
        /// </remarks>
        Fatal = 8,
        /// <summary>
        /// Something went wrong and cannot losslessly be recovered. However, not all future data is affected.
        /// </summary>
        Error = 16,
        /// <summary>
        /// Something somehow does not look correct.
        /// </summary>
        /// <remarks>
        /// This may or may not lead to problems. An example would be the use of '-vstrict -2'.
        /// </remarks>
        Warning = 24,
        /// <summary>
        /// Standard information.
        /// </summary>
        Info = 32,
        /// <summary>
        /// Detailed information.
        /// </summary>
        Verbose = 40,
        /// <summary>
        /// Stuff which is only useful for libav* developers.
        /// </summary>
        Debug = 48,
        /// <summary>
        /// Extremely verbose debugging, useful for libav* development.
        /// </summary>
        Trace = 56
    }
}
