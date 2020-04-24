#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 *
 * The file is based on the implementation from FFmpeg.AutoGen by Ruslan Balanukhin.
 * FFmpeg.AutoGen is available under the GNU Lesser General Public License 3. For details see <https://github.com/Ruslan-B/FFmpeg.AutoGen>.
 */
#endregion License

using System;
using System.Runtime.InteropServices;

namespace UltraStar.Core.Unmanaged.FFmpeg
{
    /// <summary>
    /// Represents a custom marshaller to convert a char pointer to string.
    /// </summary>
    internal class ConstCharPtrMarshaler : ICustomMarshaler
    {
        public object MarshalNativeToManaged(IntPtr pNativeData) => Marshal.PtrToStringAnsi(pNativeData);

        public IntPtr MarshalManagedToNative(object managedObj) => IntPtr.Zero;

        public void CleanUpNativeData(IntPtr pNativeData)
        { }

        public void CleanUpManagedData(object managedObj)
        { }

        public int GetNativeDataSize() => IntPtr.Size;

        private static readonly ConstCharPtrMarshaler Instance = new ConstCharPtrMarshaler();

        public static ICustomMarshaler GetInstance(string cookie) => Instance;
    }
}
