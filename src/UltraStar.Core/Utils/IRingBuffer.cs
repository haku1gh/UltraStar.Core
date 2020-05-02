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
    /// Represents a fast ring buffer of a fixed size to the power of 2.
    /// </summary>
    /// <typeparam name="T">The type of elements in the ring buffer.</typeparam>
    public interface IRingBuffer<T>
    {
        /// <summary>
        /// Resizes the buffer. All items in the buffer are lost, as the buffer is created anew from scratch.
        /// </summary>
        /// <param name="size">The size or minimum size of the ring buffer.</param>
        void Resize(int size);

        /// <summary>
        /// Gets the size of the ring buffer.
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Gets the number of elements in this ring buffer.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the number of push operations performed on the ring buffer.
        /// </summary>
        long PushCount { get; }

        /// <summary>
        /// Push a new element into the buffer, thus overwriting the oldest element.
        /// </summary>
        /// <param name="val">The new element to add in the buffer.</param>
        void Push(T val);

        /// <summary>
        /// Pops the oldest element from the buffer.
        /// </summary>
        /// <remarks>
        /// Using this method would result in a FIFO queue if <see cref="Count"/> is always less than <see cref="Size"/>.
        /// </remarks>
        /// <returns>The oldest element from the buffer, or a <see langword="default"/> value if no element is in the buffer.</returns>
        T Pop();

        /// <summary>
        /// Gets the element at the specified index in the ring buffer.
        /// </summary>
        /// <remarks>
        /// <paramref name="index"/> with 0 retrieves the last added element.
        /// <paramref name="index"/> with <see cref="Size"/>-1 retrieves the oldest element.
        /// With the indexer it is also possible to access not populated buffer entries. These could contain any values.
        /// </remarks>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index in the buffer.</returns>
        T this[int index] { get; }

        /// <summary>
        /// Gets the first (= latest added) element in the ring buffer, or a <see langword="default"/> value if no element is in the buffer.
        /// </summary>
        T First { get; }

        /// <summary>
        /// Gets the last (=oldest) element in the ring buffer, or a <see langword="default"/> value if no element is in the buffer.
        /// </summary>
        T Last { get; }

        /// <summary>
        /// Resets all elements from the <see cref="IRingBuffer{T}"/>.
        /// </summary>
        void Clear();

        /// <summary>
        /// Copies the elements of the <see cref="IRingBuffer{T}"/> to a new array.
        /// </summary>
        /// <returns>An array containing copies of the elements of the <see cref="IRingBuffer{T}"/>.</returns>
        T[] ToArray();

        /// <summary>
        /// Copies the elements of the <see cref="IRingBuffer{T}"/> to a new array up to <paramref name="maxCount"/>.
        /// </summary>
        /// <param name="maxCount">The maximum number of elements to copy.</param>
        /// <returns>An array containing copies of the elements of the <see cref="IRingBuffer{T}"/>.</returns>
        T[] ToArray(int maxCount);
    }
}
