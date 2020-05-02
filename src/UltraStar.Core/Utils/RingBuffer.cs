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
    /// Represents a ring buffer.
    /// </summary>
    /// <typeparam name="T">The type of elements in the ring buffer.</typeparam>
    public class RingBuffer<T> : IRingBuffer<T>
    {
        // Private variables
        private T[] items;
        private int mask;
        private int pos;

        /// <summary>
        /// Initializes a new instance of <see cref="RingBuffer{T}"/>.
        /// </summary>
        /// <param name="size">The size of the ring buffer.</param>
        public RingBuffer(int size)
        {
            Resize(size);
        }

        /// <summary>
        /// Resizes the buffer. All items in the buffer are lost, as the buffer is created anew from scratch.
        /// </summary>
        /// <param name="size">The size of the ring buffer.</param>
        public void Resize(int size)
        {
            int capacity = Math.Max(size, 1);
            items = new T[capacity];
            mask = capacity - 1;
            pos = mask;
            Count = 0;
            PushCount = 0;
        }

        /// <summary>
        /// Gets the size of the ring buffer.
        /// </summary>
        public int Size => items.Length;

        /// <summary>
        /// Gets the number of elements in this ring buffer.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the number of push operations performed on the ring buffer.
        /// </summary>
        public long PushCount { get; private set; }

        /// <summary>
        /// Push a new element into the buffer, thus overwriting the oldest element.
        /// </summary>
        /// <param name="val">The new element to add in the buffer.</param>
        public void Push(T val)
        {
            pos++;
            if (pos > mask) pos = 0;
            items[pos] = val;
            if (Count < Size) Count++;
            PushCount++;
        }

        /// <summary>
        /// Pops the oldest element from the buffer.
        /// </summary>
        /// <remarks>
        /// Using this method would result in a FIFO queue if <see cref="Count"/> is always less than <see cref="Size"/>.
        /// </remarks>
        /// <returns>The oldest element from the buffer, or a <see langword="default"/> value if no element is in the buffer.</returns>
        public T Pop()
        {
            if (Count == 0) return default;
            int index = pos - Count + 1 + Size;
            if (index >= Size) index -= Size;
            Count--;
            return items[index];
        }

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
        public T this[int index]
        {
            get
            {
                index = pos - index + Size;
                if (index >= Size) index -= Size;
                return items[index];
            }
        }

        /// <summary>
        /// Gets the first (= latest added) element in the ring buffer, or a <see langword="default"/> value if no element is in the buffer.
        /// </summary>
        public T First
        {
            get
            {
                if (Count == 0) return default;
                else return items[pos];
            }
        }

        /// <summary>
        /// Gets the last (=oldest) element in the ring buffer, or a <see langword="default"/> value if no element is in the buffer.
        /// </summary>
        public T Last
        {
            get
            {
                if (Count == 0) return default;
                int index = pos - Count + 1 + Size;
                if (index >= Size) index -= Size;
                return items[index];
            }
        }

        /// <summary>
        /// Resets all elements from the <see cref="RingBuffer{T}"/>.
        /// </summary>
        public void Clear()
        {
            Array.Clear(items, 0, Size);
            pos = 0;
            Count = 0;
            PushCount = 0;
        }

        /// <summary>
        /// Copies the elements of the <see cref="RingBuffer{T}"/> to a new array.
        /// </summary>
        /// <returns>An array containing copies of the elements of the <see cref="RingBuffer{T}"/>.</returns>
        public T[] ToArray()
        {
            // Create new array
            T[] returnArray = new T[Count];
            // Copy elements (this might look slower than a for loop, but isn't)
            int last = pos - Count + 1 + Size;
            if (last >= Size) last -= Size;
            int count = Count < (Size - last) ? Count : (Size - last);
            Array.Copy(items, last, returnArray, 0, count);
            if(count < Count)
            {
                Array.Copy(items, 0, returnArray, count, pos + 1);
            }
            // Return new array
            return returnArray;
        }

        /// <summary>
        /// Copies the elements of the <see cref="RingBuffer{T}"/> to a new array up to <paramref name="maxCount"/>.
        /// </summary>
        /// <param name="maxCount">The maximum number of elements to copy.</param>
        /// <returns>An array containing copies of the elements of the <see cref="RingBuffer{T}"/>.</returns>
        public T[] ToArray(int maxCount)
        {
            int realCount = maxCount < Count ? maxCount : Count;
            // Create new array
            T[] returnArray = new T[realCount];
            // Copy elements (this might look slower than a for loop, but isn't)
            int last = pos - realCount + 1 + Size;
            if (last >= Size) last -= Size;
            int count = realCount < (Size - last) ? realCount : (Size - last);
            Array.Copy(items, last, returnArray, 0, count);
            if (count < realCount)
            {
                Array.Copy(items, 0, returnArray, count, realCount - count);
            }
            // Return new array
            return returnArray;
        }
    }
}
