#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.Reflection;
using UltraStar.Core.Utils;

namespace UltraStar.Core.Audio
{
    /// <summary>
    /// Represents an abstract audio decoder class.
    /// </summary>
    public abstract class AudioDecoder : Decoder<TimestampItem<float[]>>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AudioDecoder"/>.
        /// </summary>
        /// <param name="minimumBufferSize">The minimum size of the internal buffer.</param>
        public AudioDecoder(int minimumBufferSize) : base(minimumBufferSize, LibrarySettings.AudioDecoderNonOverwritingItems)
        { }

        /// <summary>
        /// Opens a new audio decoder.
        /// </summary>
        /// <param name="url">The URL of the media file.</param>
        public static AudioDecoder Open(string url)
        {
            // Get the type where the class AudioDecoder is implemented
            Type audioDecoderClass = Type.GetType(LibrarySettings.AudioDecoderClassName, true);
            // Get the constructor
            ConstructorInfo cInfo = audioDecoderClass.GetConstructor(new Type[] { typeof(string) });
            // Return the newly created object
            return (AudioDecoder)cInfo.Invoke(new object[] { url });
        }

        /// <summary>
        /// Gets an audio playback callback from the decoder.
        /// </summary>
        /// <returns>A <see cref="AudioPlaybackCallback"/> delegate which can be called to retrieve audio samples.</returns>
        public abstract AudioPlaybackCallback GetAudioPlaybackCallback();

        /// <summary>
        /// Gets the name of the codec used for decoding.
        /// </summary>
        public abstract string CodecName { get; protected set; }

        /// <summary>
        /// Gets the codec long name.
        /// </summary>
        public abstract string CodecLongName { get; protected set; }

        /// <summary>
        /// Gets the duration of the audio.
        /// </summary>
        public abstract float Duration { get; protected set; }

        /// <summary>
        /// Gets the start time of the first audio sample.
        /// </summary>
        public abstract long StartTimestamp { get; protected set; }

        /// <summary>
        /// Gets the number of channels in the audio.
        /// </summary>
        public abstract int Channels { get; protected set; }

        /// <summary>
        /// Gets the sample rate of the audio.
        /// </summary>
        public abstract int SampleRate { get; protected set; }
    }
}
