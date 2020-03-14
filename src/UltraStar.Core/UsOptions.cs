#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;

namespace UltraStar.Core
{
    /// <summary>
    /// This class contains options which can be changed by the user.
    /// </summary>
    /// <remarks>
    /// Non user related settings can be retrieved or set in <see cref="UsConfig"/>.
    /// </remarks>
    public static class UsOptions
    {
        /// <summary>
        /// Initializes <see cref="UsOptions"/>.
        /// </summary>
        static UsOptions()
        {
            AudioRecordingBufferLength = 1000;
            AudioRecordingDelay = UsAudioRecordingDelay.Minimum;
            AudioPlaybackBufferLength = 500;
            AudioFilePreBufferLength = 1000;
            VideoFilePreBufferLength = 500;
        }

        private static int audioRecordingBufferLength;
        /// <summary>
        /// Gets or sets the audio recording buffer length in [ms].
        /// </summary>
        /// <remarks>
        /// This value can range from 1000 to 5000 milliseconds. Default is 1000 milliseconds.
        /// Any values provided outside this range will be automatically capped.
        /// Higher values could increase the latency.
        /// </remarks>
        public static int AudioRecordingBufferLength
        {
            get { return audioRecordingBufferLength; }
            set
            {
                if (value < 1000) value = 1000;
                if (value > 5000) value = 5000;
                audioRecordingBufferLength = value;
            }
        }

        /// <summary>
        /// Gets or sets the audio recording delay.
        /// </summary>
        /// <remarks>
        /// Larger delays should lightly reduce CPU load,
        /// but negatively impact recording playback and visuals for audio recording (e.g. if a tone was correctly hit).
        /// </remarks>
        public static UsAudioRecordingDelay AudioRecordingDelay { get; set; }

        private static int audioPlaybackBufferLength;
        /// <summary>
        /// Gets or sets the audio playback buffer length in [ms].
        /// </summary>
        /// <remarks>
        /// This value can range from 100 to 5000 milliseconds. Default is 500 milliseconds.
        /// Any values provided outside this range will be automatically capped.
        /// Larger buffers will not cause additional delays.
        /// </remarks>
        public static int AudioPlaybackBufferLength
        {
            get { return audioPlaybackBufferLength; }
            set
            {
                if (value < 100) value = 100;
                if (value > 5000) value = 5000;
                audioPlaybackBufferLength = value;
            }
        }

        private static int audioFilePreBufferLength;
        /// <summary>
        /// Gets or sets the audio file pre-buffer length in [ms].
        /// </summary>
        /// <remarks>
        /// Any song file (e.g. an MP3) which is passed to an audio playback device is pre-buffered to ensure smooth playback.
        /// Depending on the location of the file, this value should be increased.
        /// E.g. if the file is located on another server/pc then it could make sense to increase the pre-buffer length.
        /// 
        /// This value can range from 500 to 60000 milliseconds. Default is 1000 milliseconds.
        /// Any values provided outside this range will be automatically capped.
        /// Larger buffers could cause a delay until the game starts.
        /// This buffer should always be greater than <see cref="AudioPlaybackBufferLength"/>.
        /// </remarks>
        public static int AudioFilePreBufferLength
        {
            get { return audioFilePreBufferLength; }
            set
            {
                if (value < 500) value = 500;
                if (value > 60000) value = 60000;
                audioFilePreBufferLength = value;
            }
        }

        private static int videoFilePreBufferLength;
        /// <summary>
        /// Gets or sets the video file pre-buffer length in [ms].
        /// </summary>
        /// <remarks>
        /// Any video file (e.g. an MP4 or MKV) which is passed to a display is pre-buffered to ensure smooth playback.
        /// Depending on the location of the file, this value should be increased.
        /// E.g. if the file is located on another server/pc then it could make sense to increase the pre-buffer length.
        /// 
        /// This value can range from 500 to 10000 milliseconds. Default is 500 milliseconds.
        /// Any values provided outside this range will be automatically capped.
        /// Larger buffers could cause a delay until the game starts.
        /// </remarks>
        public static int VideoFilePreBufferLength
        {
            get { return videoFilePreBufferLength; }
            set
            {
                if (value < 500) value = 500;
                if (value > 10000) value = 10000;
                videoFilePreBufferLength = value;
            }
        }
    }
}
