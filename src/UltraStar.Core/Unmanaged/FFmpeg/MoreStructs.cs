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
    /// Represents a rational number (a pair of numerator and denominator).
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct AVRational
    {
        /// <summary>
        /// Numerator
        /// </summary>
        public int Num;
        /// <summary>
        /// Denominator
        /// </summary>
        public int Den;
    }

    /// <summary>
    /// Represents a <see langword="byte"/> array with 1024 elements.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct ByteArray1024
    {
        /// <summary>
        /// The number of elements in this array.
        /// </summary>
        public static readonly int Size = 1024;
        /// <summary>
        /// The array.
        /// </summary>
        public fixed byte Element[1024];
    }

    /// <summary>
    /// Represents a <see langword="byte"/> array with 17 elements.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct ByteArray17
    {
        /// <summary>
        /// The number of elements in this array.
        /// </summary>
        public static readonly int Size = 17;
        /// <summary>
        /// The array.
        /// </summary>
        public fixed byte Element[17];
    }

    /// <summary>
    /// Represents a <see langword="long"/> array with 17 elements.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct LongArray17
    {
        /// <summary>
        /// The number of elements in this array.
        /// </summary>
        public static readonly int Size = 17;
        /// <summary>
        /// The array.
        /// </summary>
        public fixed long Element[17];
    }

    /// <summary>
    /// Represents a <see langword="ulong"/> array with 8 elements.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct UlongArray8
    {
        /// <summary>
        /// The number of elements in this array.
        /// </summary>
        public static readonly int Size = 8;
        /// <summary>
        /// The array.
        /// </summary>
        public fixed ulong Element[8];
    }

    /// <summary>
    /// Represents an <see langword="int"/> array with 8 elements.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct IntArray8
    {
        /// <summary>
        /// The number of elements in this array.
        /// </summary>
        public static readonly int Size = 8;
        /// <summary>
        /// The Element 0 of the array.
        /// </summary>
        public int Element0;
        /// <summary>
        /// The Element 1 of the array.
        /// </summary>
        public int Element1;
        /// <summary>
        /// The Element 2 of the array.
        /// </summary>
        public int Element2;
        /// <summary>
        /// The Element 3 of the array.
        /// </summary>
        public int Element3;
        /// <summary>
        /// The Element 4 of the array.
        /// </summary>
        public int Element4;
        /// <summary>
        /// The Element 5 of the array.
        /// </summary>
        public int Element5;
        /// <summary>
        /// The Element 6 of the array.
        /// </summary>
        public int Element6;
        /// <summary>
        /// The Element 7 of the array.
        /// </summary>
        public int Element7;

        /// <summary>
        /// Returns an int array with all the elements.
        /// </summary>
        public int[] ToIntArray()
        {
            int[] array = new int[Size];
            array[0] = Element0;
            array[1] = Element1;
            array[2] = Element2;
            array[3] = Element3;
            array[4] = Element4;
            array[5] = Element5;
            array[6] = Element6;
            array[7] = Element7;
            return array;
        }
    }

    /// <summary>
    /// Represents an <see cref="IntPtr"/> array with 8 elements.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct IntPtrArray8
    {
        /// <summary>
        /// The number of elements in this array.
        /// </summary>
        public static readonly int Size = 8;
        /// <summary>
        /// The Element 0 of the array.
        /// </summary>
        public IntPtr Element0;
        /// <summary>
        /// The Element 1 of the array.
        /// </summary>
        public IntPtr Element1;
        /// <summary>
        /// The Element 2 of the array.
        /// </summary>
        public IntPtr Element2;
        /// <summary>
        /// The Element 3 of the array.
        /// </summary>
        public IntPtr Element3;
        /// <summary>
        /// The Element 4 of the array.
        /// </summary>
        public IntPtr Element4;
        /// <summary>
        /// The Element 5 of the array.
        /// </summary>
        public IntPtr Element5;
        /// <summary>
        /// The Element 6 of the array.
        /// </summary>
        public IntPtr Element6;
        /// <summary>
        /// The Element 7 of the array.
        /// </summary>
        public IntPtr Element7;

        /// <summary>
        /// Returns a byte pointer array with all the elements.
        /// </summary>
        public unsafe byte*[] ToBytePtrArray()
        {
            byte*[] array = new byte*[Size];
            array[0] = (byte*)Element0;
            array[1] = (byte*)Element1;
            array[2] = (byte*)Element2;
            array[3] = (byte*)Element3;
            array[4] = (byte*)Element4;
            array[5] = (byte*)Element5;
            array[6] = (byte*)Element6;
            array[7] = (byte*)Element7;
            return array;
        }
    }

    /// <summary>
    /// Represents a dictionary.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct AVDictionary
    { }

    /// <summary>
    /// Represents an SWS context.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct SwsContext
    { }


    /// <summary>
    /// Represents an SWS filter.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct SwsFilter
    { }
}
