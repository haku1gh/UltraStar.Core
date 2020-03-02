#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using UltraStar.Core.Audio;

namespace UltraStar.Core.Utils
{
    /// <summary>
    /// Represents static settings which are valid for the whole library.
    /// </summary>
    internal static class LibrarySettings
    {
        /// <summary>
        /// The class name of the audio recording implementation.
        /// </summary>
        public static readonly string AudioRecordingClassName = nameof(BassAudioRecording);

        /// <summary>
        /// The default sample rate for audio recordings.
        /// </summary>
        public static readonly int DefaultAudioRecordingSamplerate = 48000;

        /// <summary>
        /// The default input for audio recordings.
        /// </summary>
        public static readonly int DefaultAudioRecordingInput = 0;
    }
}
