#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;

namespace UltraStar.Core.Utils
{
    /// <summary>
    /// Identifies the platform on which this library is running.
    /// </summary>
    internal enum Platform
    {
        /// <summary>
        /// The platform is based on the MacOSX operating system.
        /// </summary>
        Mac,
        /// <summary>
        /// The platform is based on the Linux operating system.
        /// </summary>
        Linux,
        /// <summary>
        /// The platform is based on the Windows operating system.
        /// </summary>
        Windows,
        /// <summary>
        /// The platform is not supported.
        /// </summary>
        Unsupported
    }
}
