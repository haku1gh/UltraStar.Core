#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;

namespace UltraStar.Core.Audio
{
    /// <summary>
    /// Respresents an audio format for WAVE encoding.
    /// </summary>
    public enum WaveAudioFormat : ushort
    {
        /// <summary>
        /// Encoding is in PCM, signed 16bits.
        /// </summary>
        PCM = 1,
        /// <summary>
        /// Encoding is in IEEE FLOAT.
        /// </summary>
        Float = 3
    }
}
