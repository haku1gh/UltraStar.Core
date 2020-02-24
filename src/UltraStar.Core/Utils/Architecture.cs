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
    /// Identifies the architecture on which this library is running.
    /// </summary>
    internal enum Architecture
    {
        /// <summary>
        /// An Intel-based 32-bit architecture.
        /// </summary>
        X86,
        /// <summary>
        /// An Intel-based 64-bit architecture.
        /// </summary>
        X64,
        /// <summary>
        /// An ARM-based 32-bit architecture.
        /// </summary>
        Arm,
        /// <summary>
        /// An ARM-based 64-bit architecture.
        /// </summary>
        Arm64,
        /// <summary>
        /// An unsupported architecture.
        /// </summary>
        Unsupported
    }
}
