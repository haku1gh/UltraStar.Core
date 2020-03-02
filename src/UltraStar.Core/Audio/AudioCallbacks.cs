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
    /// The callback to process samples from an audio recording.
    /// </summary>
    /// <remarks>
    /// New samples always start at index 0 in the buffer.
    /// The buffer might be larger than length suggests. So always use the length parameter when looping through the data.
    /// This function should be executed as quick as possible.
    /// Do not call <see cref="AudioRecording.Stop"/> from within this function.
    /// </remarks>
    /// <param name="handle">The audio recording from where this callback originates.</param>
    /// <param name="buffer">The buffer containing the sample data.</param>
    /// <param name="length">The number of audio samples provided in the buffer.</param>
    delegate void AudioRecordingCallback(AudioRecording handle, float[] buffer, int length);

    /// <summary>
    /// The callback to provide samples for an audio playback.
    /// </summary>
    /// <remarks>
    /// Always start writing from index 0.
    /// The buffer might be larger than maxLength suggests. So always use the maxLength parameter when writing the data.
    /// This function should be executed as quick as possible.
    /// It is better to return less data quickly, rather than spending a long time delivering exactly the amount requested.
    /// Do not call <see cref="AudioPlayback.Stop"/> from within this function.
    /// </remarks>
    /// <param name="handle">The audio playback from where this callback originates.</param>
    /// <param name="buffer">The buffer to write new sample data.</param>
    /// <param name="maxLength">The maximum number of audio samples requested.</param>
    /// <returns>The number of audio samples written by the function.</returns>
    delegate int AudioPlaybackCallback(AudioPlayback handle, float[] buffer, int maxLength);
}
