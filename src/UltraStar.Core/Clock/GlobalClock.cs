#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.Diagnostics;

namespace UltraStar.Core.Clock
{
    /// <summary>
    /// Respresents a global clock.
    /// </summary>
    internal static class GlobalClock
    {
        private static Stopwatch watch;
        private static long internalOffset = 0;

        /// <summary>
        /// Initializes a new instance of <see cref="GlobalClock"/>.
        /// </summary>
        static GlobalClock()
        {
            watch = new Stopwatch();
            watch.Start();
        }

        /// <summary>
        /// Syncs the clock with a new offset.
        /// </summary>
        /// <param name="offset">The new additional offset in [ms] to apply.</param>
        public static void Sync(long offset)
        {
            internalOffset += offset;
        }

        /// <summary>
        /// Gets the number of [ms] ticks since start of the clock. 
        /// </summary>
        public static long Ticks
        {
            get { return (1000 * watch.ElapsedTicks / Stopwatch.Frequency) + internalOffset; }
        }
    }
}
