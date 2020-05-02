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
    /// Represents an item with a timestamp.
    /// </summary>
    /// <typeparam name="T">The type of the item.</typeparam>
    public struct TimestampItem<T>
    {
        /// <summary>
        /// Initializes the struct <see cref="TimestampItem{T}"/>.
        /// </summary>
        /// <param name="timestamp">The timestamp of the item in micro seconds [us].</param>
        /// <param name="item">The item.</param>
        internal TimestampItem(long timestamp, T item)
        {
            Timestamp = timestamp;
            Item = item;
        }

        /// <summary>
        /// A timestamp of the item in micro seconds [us].
        /// </summary>
        public long Timestamp { get; internal set; }

        /// <summary>
        /// The item itself.
        /// </summary>
        public T Item { get; internal set; }
    }
}
