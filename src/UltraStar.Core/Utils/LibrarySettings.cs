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
        /// The full qualified class name of the audio playback implementation.
        /// </summary>
        public static readonly string AudioPlaybackClassName = typeof(BassAudioPlayback).AssemblyQualifiedName;

        /// <summary>
        /// The default sample rate for audio playbacks.
        /// </summary>
        public static readonly int AudioPlaybackDefaultSamplerate = 48000;

        /// <summary>
        /// The maximum allowed amplification of the channel volume for an audio playback.
        /// </summary>
        public static readonly int AudioPlaybackMaximumChannelAmplification = 8;

        /// <summary>
        /// The maximum buffer size for audio playbacks.
        /// </summary>
        public static readonly int AudioPlaybackBufferSize = 24576;

        /// <summary>
        /// The full qualified class name of the audio recording implementation.
        /// </summary>
        public static readonly string AudioRecordingClassName = typeof(BassAudioRecording).AssemblyQualifiedName;

        /// <summary>
        /// The default channels for audio recordings.
        /// </summary>
        public static readonly int AudioRecordingDefaultChannels = 2;

        /// <summary>
        /// The default input for audio recordings.
        /// </summary>
        public static readonly int AudioRecordingDefaultInput = 0;

        /// <summary>
        /// The default sample rate for audio recordings.
        /// </summary>
        public static readonly int AudioRecordingDefaultSamplerate = 48000;

        /// <summary>
        /// The maximum allowed amplification of the channel volume for an audio recording.
        /// </summary>
        public static readonly int AudioRecordingMaximumChannelAmplification = 8;

        /// <summary>
        /// The maximum buffer size for audio recordings.
        /// </summary>
        public static readonly int AudioRecordingBufferSize = 1024;

        /// <summary>
        /// The number of threads to be used for video decoding.
        /// </summary>
        /// <remarks>
        /// This value can have the values -1 or greater then 0.
        /// Where -1 represents an automatic detection based on available processor cores.
        /// Value equal or greater than 1 represent a fixed number of cores.
        /// </remarks>
        public static readonly int VideoDecodingThreadCount = -1;
    }
}
