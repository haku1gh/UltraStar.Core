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
    /// Represents an FFmpeg codec.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct AVCodec
    {
        /// <summary>
        /// The name of the codec implementation. The name is globally unique among encoders and among decoders (but an encoder and a decoder can share the same name).
        /// </summary>
        /// <remarks>
        /// This is the primary way to find a codec from the user perspective.
        /// </remarks>
        public byte* Name;
        /// <summary>
        /// The descriptive name for the codec, meant to be more human readable than name.
        /// </summary>
        public byte* LongName;
        /// <summary>
        /// The type of the codec.
        /// </summary>
        public AVMediaType Type;
        /// <summary>
        /// The ID of the codec.
        /// </summary>
        public int ID;
        /// <summary>
        /// The codec capabilities.
        /// </summary>
        public int Capabilities;
        /// <summary>
        /// An array of supported frame rates.
        /// </summary>
        /// <remarks>
        /// It is <see langword="null"/> if there are not restrictions. The array is terminated by {0,0}.
        /// </remarks>
        public AVRational* SupportedFrameRates;
        /// <summary>
        /// An array of supported pixel formats.
        /// </summary>
        /// <remarks>
        /// It is <see langword="null"/> if unknown. The array is terminated by -1.
        /// </remarks>
        public AVPixelFormat* SupportedPixelFormats;
        /// <summary>
        /// An array of supported audio sample rates.
        /// </summary>
        /// <remarks>
        /// It is <see langword="null"/> if unknown. The array is terminated by 0.
        /// </remarks>
        public int* SupportedSampleRates;
        /// <summary>
        /// An array of supported sample formats.
        /// </summary>
        /// <remarks>
        /// It is <see langword="null"/> if unknown. The array is terminated by -1.
        /// </remarks>
        public AVSampleFormat* SupportedSampleFormats;
        /// <summary>
        /// An array of supported channel layouts.
        /// </summary>
        /// <remarks>
        /// It is <see langword="null"/> if unknown. The array is terminated by 0.
        /// </remarks>
        public ulong* SupportedChannelLayouts;
        /// <summary>
        /// The maximum value for lowres supported by the decoder.
        /// </summary>
        public byte MaximumLowres;
        /// <summary>
        /// Private data - interally.
        /// </summary>
        public IntPtr PrivateData;
        /// <summary>
        /// An array of supported profiles.
        /// </summary>
        /// <remarks>
        /// It is <see langword="null"/> if unknown. The array is terminated by {FF_PROFILE_UNKNOWN}.
        /// </remarks>
        public IntPtr SupportedProfiles;
        /// <summary>
        /// The group name of the codec implementation. This is a short symbolic name of the wrapper backing this codec.
        /// </summary>
        /// <remarks>
        /// A wrapper uses some kind of external implementation for the codec, such as an external library, or a codec implementation provided by the OS or the hardware.
        /// If this field is <see langword="null"/>, this is a builtin, libavcodec native codec.
        /// If non-<see langword="null"/>, this will be the suffix in AVCodec.name in most cases
        /// (usually AVCodec.name will be of the form &quot;&lt;codec_name&gt;_&lt;wrapper_name&gt;&quot;).
        /// </remarks>
        public byte* GroupName;
    }
}
