#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;

namespace UltraStar.Core.Unmanaged.FFmpeg
{
    /// <summary>
    /// Represents a channel layout with a bit set for every channel.
    /// </summary>
    /// <remarks>
    /// The number of bits set must be equal to the number of channels.
    /// The value 0 means that the channel layout is not known.
    /// <b>Note:</b> This data structure is not powerful enough to handle channels
    /// combinations that have the same channel multiple times, such as dual-mono.
    /// </remarks>
    [Flags]
    internal enum AVChannelLayout : ulong
    {
        /// <summary>
        /// Channel layout Unknown.
        /// </summary>
        AV_CH_LAYOUT_UNKNOWN = 0x00000000,
        /// <summary>
        /// Channel layout "Mono".
        /// </summary>
        AV_CH_LAYOUT_MONO = 0x00000004,          // = AV_CH_FRONT_CENTER
        /// <summary>
        /// Channel layout "Stereo".
        /// </summary>
        AV_CH_LAYOUT_STEREO = 0x00000003,        // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT
        /// <summary>
        /// Channel layout "2.1".
        /// </summary>
        AV_CH_LAYOUT_2POINT1 = 0x0000000B,       // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_LOW_FREQUENCY
        /// <summary>
        /// Channel layout "2_1".
        /// </summary>
        AV_CH_LAYOUT_2_UNDER_1 = 0x00000103,           // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_BACK_CENTER
        /// <summary>
        /// Channel layout "Surround".
        /// </summary>
        AV_CH_LAYOUT_SURROUND = 0x00000007,      // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_FRONT_CENTER
        /// <summary>
        /// Channel layout "3.1".
        /// </summary>
        AV_CH_LAYOUT_3POINT1 = 0x0000000F,       // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_FRONT_CENTER  | AV_CH_LOW_FREQUENCY
        /// <summary>
        /// Channel layout "4.0".
        /// </summary>
        AV_CH_LAYOUT_4POINT0 = 0x00000107,       // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_FRONT_CENTER  | AV_CH_BACK_CENTER
        /// <summary>
        /// Channel layout "4.1".
        /// </summary>
        AV_CH_LAYOUT_4POINT1 = 0x0000010F,       // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_FRONT_CENTER  | AV_CH_BACK_CENTER   | AV_CH_LOW_FREQUENCY
        /// <summary>
        /// Channel layout "2_2".
        /// </summary>
        AV_CH_LAYOUT_2_UNDER_2 = 0x00000603,           // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_SIDE_LEFT     | AV_CH_SIDE_RIGHT
        /// <summary>
        /// Channel layout "Quad".
        /// </summary>
        AV_CH_LAYOUT_QUAD = 0x00000033,          // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_BACK_LEFT     | AV_CH_BACK_RIGHT
        /// <summary>
        /// Channel layout "5.0".
        /// </summary>
        AV_CH_LAYOUT_5POINT0 = 0x00000607,       // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_FRONT_CENTER  | AV_CH_SIDE_LEFT     | AV_CH_SIDE_RIGHT
        /// <summary>
        /// Channel layout "5.1".
        /// </summary>
        AV_CH_LAYOUT_5POINT1 = 0x0000060F,       // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_FRONT_CENTER  | AV_CH_SIDE_LEFT     | AV_CH_SIDE_RIGHT           | AV_CH_LOW_FREQUENCY
        /// <summary>
        /// Channel layout "5.0 Back".
        /// </summary>
        AV_CH_LAYOUT_5POINT0_SPACE_BACK = 0x00000037,  // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_FRONT_CENTER  | AV_CH_BACK_LEFT     | AV_CH_BACK_RIGHT
        /// <summary>
        /// Channel layout "5.1 Back".
        /// </summary>
        AV_CH_LAYOUT_5POINT1_SPACE_BACK = 0x0000003F,  // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_FRONT_CENTER  | AV_CH_BACK_LEFT     | AV_CH_BACK_RIGHT           | AV_CH_LOW_FREQUENCY
        /// <summary>
        /// Channel layout "6.0".
        /// </summary>
        AV_CH_LAYOUT_6POINT0 = 0x00000707,       // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_FRONT_CENTER  | AV_CH_SIDE_LEFT     | AV_CH_SIDE_RIGHT           | AV_CH_BACK_CENTER
        /// <summary>
        /// Channel layout "6.0 Front".
        /// </summary>
        AV_CH_LAYOUT_6POINT0_SPACE_FRONT = 0x000006C3, // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_SIDE_LEFT     | AV_CH_SIDE_RIGHT    | AV_CH_FRONT_LEFT_OF_CENTER | AV_CH_FRONT_RIGHT_OF_CENTER
        /// <summary>
        /// Channel layout "Hexagonal".
        /// </summary>
        AV_CH_LAYOUT_HEXAGONAL = 0x00000137,     // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_FRONT_CENTER  | AV_CH_BACK_LEFT     | AV_CH_BACK_RIGHT           | AV_CH_BACK_CENTER
        /// <summary>
        /// Channel layout "6.1".
        /// </summary>
        AV_CH_LAYOUT_6POINT1 = 0x0000070F,       // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_FRONT_CENTER  | AV_CH_SIDE_LEFT     | AV_CH_SIDE_RIGHT           | AV_CH_LOW_FREQUENCY         | AV_CH_BACK_CENTER
        /// <summary>
        /// Channel layout "6.1 Back".
        /// </summary>
        AV_CH_LAYOUT_6POINT1_SPACE_BACK = 0x0000013F,  // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_FRONT_CENTER  | AV_CH_BACK_LEFT     | AV_CH_BACK_RIGHT           | AV_CH_LOW_FREQUENCY         | AV_CH_BACK_CENTER
        /// <summary>
        /// Channel layout "6.1 Front".
        /// </summary>
        AV_CH_LAYOUT_6POINT1_SPACE_FRONT = 0x000006CB, // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_SIDE_LEFT     | AV_CH_SIDE_RIGHT    | AV_CH_FRONT_LEFT_OF_CENTER | AV_CH_FRONT_RIGHT_OF_CENTER | AV_CH_LOW_FREQUENCY
        /// <summary>
        /// Channel layout "7.0".
        /// </summary>
        AV_CH_LAYOUT_7POINT0 = 0x00000637,       // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_FRONT_CENTER  | AV_CH_SIDE_LEFT     | AV_CH_SIDE_RIGHT           | AV_CH_BACK_LEFT             | AV_CH_BACK_RIGHT
        /// <summary>
        /// Channel layout "7.0 Front".
        /// </summary>
        AV_CH_LAYOUT_7POINT0_SPACE_FRONT = 0x000006C7, // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_FRONT_CENTER  | AV_CH_SIDE_LEFT     | AV_CH_SIDE_RIGHT           | AV_CH_FRONT_LEFT_OF_CENTER  | AV_CH_FRONT_RIGHT_OF_CENTER
        /// <summary>
        /// Channel layout "7.1".
        /// </summary>
        AV_CH_LAYOUT_7POINT1 = 0x0000063F,       // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_FRONT_CENTER  | AV_CH_SIDE_LEFT     | AV_CH_SIDE_RIGHT           | AV_CH_LOW_FREQUENCY         | AV_CH_BACK_LEFT             | AV_CH_BACK_RIGHT
        /// <summary>
        /// Channel layout "7.0 Wide".
        /// </summary>
        AV_CH_LAYOUT_7POINT1_SPACE_WIDE = 0x000006CF,  // = AV_CH_FRONT_LEFT   | AV_CH_FRONT_RIGHT | AV_CH_FRONT_CENTER  | AV_CH_SIDE_LEFT     | AV_CH_SIDE_RIGHT           | AV_CH_LOW_FREQUENCY         | AV_CH_FRONT_LEFT_OF_CENTER  | AV_CH_FRONT_RIGHT_OF_CENTER
        /// <summary>
        /// Channel layout "7.0 Wide Back".
        /// </summary>
        AV_CH_LAYOUT_7POINT1_SPACE_WIDE_SPACE_BACK = 0x000000FF,
        /// <summary>
        /// Channel layout "Octagonal".
        /// </summary>
        AV_CH_LAYOUT_OCTAGONAL = 0x00000737,
        /// <summary>
        /// Channel layout "Hexadecagonal".
        /// </summary>
        AV_CH_LAYOUT_HEXADECAGONAL = 0x000000018003F737UL,
        /// <summary>
        /// Channel layout "Stereo Downmix".
        /// </summary>
        AV_CH_LAYOUT_STEREO_SPACE_DOWNMIX = 0x60000000, // = AV_CH_STEREO_LEFT | AV_CH_STEREO_RIGHT

        /// <summary>
        /// Channel Front-Left.
        /// </summary>
        AV_CH_FRONT_LEFT = 0x00000001,
        /// <summary>
        /// Channel Front-Right.
        /// </summary>
        AV_CH_FRONT_RIGHT = 0x00000002,
        /// <summary>
        /// Channel Front-Center.
        /// </summary>
        AV_CH_FRONT_CENTER = 0x00000004,
        /// <summary>
        /// Channel Low-Frequency.
        /// </summary>
        AV_CH_LOW_FREQUENCY = 0x00000008,
        /// <summary>
        /// Channel Back-Left.
        /// </summary>
        AV_CH_BACK_LEFT = 0x00000010,
        /// <summary>
        /// Channel Back-Right.
        /// </summary>
        AV_CH_BACK_RIGHT = 0x00000020,
        /// <summary>
        /// Channel Front-Left of Center.
        /// </summary>
        AV_CH_FRONT_LEFT_OF_CENTER = 0x00000040,
        /// <summary>
        /// Channel Front-Right of Center.
        /// </summary>
        AV_CH_FRONT_RIGHT_OF_CENTER = 0x00000080,
        /// <summary>
        /// Channel Back-Center.
        /// </summary>
        AV_CH_BACK_CENTER = 0x00000100,
        /// <summary>
        /// Channel Side-Left.
        /// </summary>
        AV_CH_SIDE_LEFT = 0x00000200,
        /// <summary>
        /// Channel Side-Right.
        /// </summary>
        AV_CH_SIDE_RIGHT = 0x00000400,
        /// <summary>
        /// Channel Top-Center.
        /// </summary>
        AV_CH_TOP_CENTER = 0x00000800,
        /// <summary>
        /// Channel Top-Front-Left.
        /// </summary>
        AV_CH_TOP_FRONT_LEFT = 0x00001000,
        /// <summary>
        /// Channel Top-Front-Center.
        /// </summary>
        AV_CH_TOP_FRONT_CENTER = 0x00002000,
        /// <summary>
        /// Channel Top-Front-Right.
        /// </summary>
        AV_CH_TOP_FRONT_RIGHT = 0x00004000,
        /// <summary>
        /// Channel Top-Back-Left.
        /// </summary>
        AV_CH_TOP_BACK_LEFT = 0x00008000,
        /// <summary>
        /// Channel Top-Back-Center.
        /// </summary>
        AV_CH_TOP_BACK_CENTER = 0x00010000,
        /// <summary>
        /// Channel Top-Back-Right.
        /// </summary>
        AV_CH_TOP_BACK_RIGHT = 0x00020000,
        /// <summary>
        /// Channel Sterio-Downmix-Left.
        /// </summary>
        AV_CH_STEREO_LEFT = 0x20000000,
        /// <summary>
        /// Channel Sterio-Downmix-Right.
        /// </summary>
        AV_CH_STEREO_RIGHT = 0x40000000,
        /// <summary>
        /// Channel Wide-Left.
        /// </summary>
        AV_CH_WIDE_LEFT = 0x0000000080000000UL,
        /// <summary>
        /// Channel Wide-Right.
        /// </summary>
        AV_CH_WIDE_RIGHT = 0x0000000100000000UL,
        /// <summary>
        /// Channel Surround-Direct-Left.
        /// </summary>
        AV_CH_SURROUND_DIRECT_LEFT = 0x0000000200000000UL,
        /// <summary>
        /// Channel Surround-Direct-Right.
        /// </summary>
        AV_CH_SURROUND_DIRECT_RIGHT = 0x0000000400000000UL,
        /// <summary>
        /// Channel Low-Frequency-2.
        /// </summary>
        AV_CH_LOW_FREQUENCY_2 = 0x0000000800000000UL
    }

    /// <summary>
    /// Respresents an extension class for <see cref="AVChannelLayout"/>.
    /// </summary>
    internal static class AVChannelLayoutExtensions
    {
        /// <summary>
        /// Returns the name of the channel layout.
        /// </summary>
        /// <param name="chLayout">The channel layout.</param>
        /// <returns>The name of the channel layout.</returns>
        public static string Name(this AVChannelLayout chLayout)
        {
            // Get channel layout "raw" name
            string tempName = chLayout.ToString();
            // Do some replacements
            tempName = tempName.Replace("AV_CH_", "");
            tempName = tempName.Replace("LAYOUT_", "");
            tempName = tempName.Replace("POINT", ".");
            // Convert to CamelCase
            string[] nameParts = tempName.Split(new char[] { '_' });
            for(int i = 0;i < nameParts.Length;i++)
            {
                nameParts[i] = nameParts[i][0] + nameParts[i].Substring(1).ToLower();
            }
            string name = string.Concat(nameParts);
            // Do some final replacements
            name = name.Replace("Space", " ");
            name = name.Replace("Under", "_");
            // Return
            return name;
        }
    }
}
