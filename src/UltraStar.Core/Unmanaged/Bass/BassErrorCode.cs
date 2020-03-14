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
    /// Bass error codes.
    /// </summary>
    public enum BassErrorCode
    {
        /// <summary>
        /// Some other mystery error.
        /// </summary>
        UnknownError = -1,

        /// <summary>
        /// No Error.
        /// </summary>
        Success = 0,

        /// <summary>
        /// Memory Error.
        /// </summary>
        MemoryError = 1,

        /// <summary>
        /// Can't open the file.
        /// </summary>
        FileOpenFailed = 2,

        /// <summary>
        /// Can't find a free/valid driver.
        /// </summary>
        DriverError = 3,

        /// <summary>
        /// The sample Buffer was lost.
        /// </summary>
        BufferLost = 4,

        /// <summary>
        /// Invalid Handle.
        /// </summary>
        InvalidHandle = 5,

        /// <summary>
        /// Unsupported sample format.
        /// </summary>
        UnsupportedSampleFormat = 6,

        /// <summary>
        /// Invalid playback position.
        /// </summary>
        InvalidPlaybackPosition = 7,

        /// <summary>
        /// BASS_Init has not been successfully called.
        /// </summary>
        NotInitialized = 8,

        /// <summary>
        /// BASS_Start has not been successfully called.
        /// </summary>
        NotStarted = 9,

        /// <summary>
        /// SSL/HTTPS support isn't available.
        /// </summary>
        SSLNotsupported = 10,

        /// <summary>
        /// Already initialized/paused/whatever.
        /// </summary>
        AlreadyDoneError = 14,

        /// <summary>
        /// Not an audio track.
        /// </summary>
        NotAnAudioTrack = 17,

        /// <summary>
        /// Can't get a free channel.
        /// </summary>
        NoFreeChannel = 18,

        /// <summary>
        /// An invalid Type was specified.
        /// </summary>
        InvalidType = 19,

        /// <summary>
        /// An invalid parameter was specified.
        /// </summary>
        InvalidParameter = 20,

        /// <summary>
        /// No 3D support.
        /// </summary>
        No3DSupport = 21,

        /// <summary>
        /// No EAX support.
        /// </summary>
        NoEAXSupport = 22,

        /// <summary>
        /// Invalid device number.
        /// </summary>
        InvalidDevice = 23,

        /// <summary>
        /// Not playing.
        /// </summary>
        NotPlaying = 24,

        /// <summary>
        /// Invalid sample rate.
        /// </summary>
        InvalidSampleRate = 25,

        /// <summary>
        /// The stream is not a file stream.
        /// </summary>
        NotFileStream = 27,

        /// <summary>
        /// No hardware voices available.
        /// </summary>
        NoHardwareVoicesAvailable = 29,

        /// <summary>
        /// The MOD music has no sequence data.
        /// </summary>
        NoMusicDataAvailable = 31,

        /// <summary>
        /// No internet connection could be opened.
        /// </summary>
        NoInternetConnection = 32,

        /// <summary>
        /// Couldn't create the file.
        /// </summary>
        CreateFileFailed = 33,

        /// <summary>
        /// Effects are not available.
        /// </summary>
        NoFXAvailable = 34,

        /// <summary>
        /// Requested data is not available.
        /// </summary>
        NoDataAvailable = 37,

        /// <summary>
        /// The channel is a 'Decoding Channel'.
        /// </summary>
        DecodeOnlyChannel = 38,

        /// <summary>
        /// A sufficient DirectX version is not installed.
        /// </summary>
        DirectXNotInstalled = 39,

        /// <summary>
        /// Connection timedout.
        /// </summary>
        ConnectionTimeout = 40,

        /// <summary>
        /// Unsupported file format.
        /// </summary>
        UnsupportedFileFormat = 41,

        /// <summary>
        /// Unavailable speaker.
        /// </summary>
        SpeakerUnavailable = 42,

        /// <summary>
        /// Invalid BASS version (used by add-ons).
        /// </summary>
        InvalidBassVersion = 43,

        /// <summary>
        /// Codec is not available/supported.
        /// </summary>
        CodecError = 44,

        /// <summary>
        /// The channel/file has ended.
        /// </summary>
        ChannelEnded = 45,

        /// <summary>
        /// The device is busy (eg. in "exclusive" use by another process).
        /// </summary>
        Busy = 46,

        /// <summary>
        /// Unstreamable file.
        /// </summary>
        UnstreamableFile = 47
    }
}
