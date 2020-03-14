#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 * 
 * The file is based on the implementation from ManagedBass by Mathew Sachin.
 * ManagedBass is available under the MIT license. For details see <https://github.com/ManagedBass/Home>.
 */
#endregion License

using System;

namespace UltraStar.Core.Unmanaged.Bass
{
    /// <summary>
    /// BASS Device Type to be used with <see cref="BassDeviceInfo" />.
    /// </summary>
    internal enum BassDeviceType
    {
        /// <summary>
        /// An audio endpoint Device that the User accesses remotely through a network.
        /// </summary>
        Network = 0x1000000,

        /// <summary>
        /// A set of speakers.
        /// </summary>
        Speakers = 0x2000000,

        /// <summary>
        /// An audio endpoint Device that sends a line-level analog signal to
        /// a line-Input jack on an audio adapter or that receives a line-level analog signal
        /// from a line-output jack on the adapter.
        /// </summary>
        Line = 0x3000000,

        /// <summary>
        /// A set of headphones.
        /// </summary>
        Headphones = 0x4000000,

        /// <summary>
        /// A microphone.
        /// </summary>
        Microphone = 0x5000000,

        /// <summary>
        /// An earphone or a pair of earphones with an attached mouthpiece for two-way communication.
        /// </summary>
        Headset = 0x6000000,

        /// <summary>
        /// The part of a telephone that is held in the hand and
        /// that contains a speaker and a microphone for two-way communication.
        /// </summary>
        Handset = 0x7000000,

        /// <summary>
        /// An audio endpoint Device that connects to an audio adapter through a connector
        /// for a digital interface of unknown Type.
        /// </summary>
        Digital = 0x8000000,

        /// <summary>
        /// An audio endpoint Device that connects to an audio adapter through
        /// a Sony/Philips Digital Interface (S/PDIF) connector.
        /// </summary>
        SPDIF = 0x9000000,

        /// <summary>
        /// An audio endpoint Device that connects to an audio adapter through
        /// a High-Definition Multimedia Interface (HDMI) connector.
        /// </summary>
        HDMI = 0xa000000,

        /// <summary>
        /// An audio endpoint Device that connects to an audio adapter through a DisplayPort connector.
        /// </summary>
        DisplayPort = 0x40000000
    }
}
