#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;

namespace UltraStar.Core
{
    /// <summary>
    /// Audio recording delay.
    /// </summary>
    public enum UsAudioRecordingDelay
    {
        /// <summary>
        /// The minimum possible delay.
        /// </summary>
        Minimum = 0,
        /// <summary>
        /// A normal delay.
        /// </summary>
        Normal = 1,
        /// <summary>
        /// A large delay.
        /// </summary>
        Large = 2
    }
}
