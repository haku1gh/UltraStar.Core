#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using UltraStar.Core.ThirdParty.Serilog;
using UltraStar.Core.Unmanaged.FFmpeg;
using UltraStar.Core.Utils;

namespace UltraStar.Core.Audio
{
    /// <summary>
    /// Represents an audio decoder class using FFmpeg.
    /// </summary>
    public class FFmpegAudioDecoder : AudioDecoder
    {
        // Private variables
        private FFmpegAudioStreamDecoder decoder;
        private static readonly int sampleSize = 512;
        private float[] tempSampleStorage;
        private int tempSampleStoragePos;
        private bool eofReached = false;
        private int frameSamplePos = 0;
        private long totalSamplePos = 0;

        /// <summary>
        /// Initializes a new instance of <see cref="FFmpegAudioDecoder"/>.
        /// </summary>
        /// <param name="url">The URL of the media file.</param>
        public FFmpegAudioDecoder(string url) : base(0)
        {
            // Open audio file
            decoder = new FFmpegAudioStreamDecoder(url);
            // Set properties
            CodecName = decoder.CodecName;
            CodecLongName = decoder.CodecLongName;
            Duration = (float)decoder.Duration;
            StartTimestamp = (long)(decoder.StartTime * 1000000);
            Channels = decoder.Channels;
            if (Channels > 8) Channels = 8;
            SampleRate = decoder.SampleRate;
            // Resize buffer (each entry has 256*channels samples
            int minBufferSize = (int)Math.Round((double)decoder.SampleRate * UsOptions.AudioFilePreBufferLength / 1000 / sampleSize);
            if (minBufferSize < 4) minBufferSize = 4; // This should not be possible, but lets play safe. If this statement is reached, then we would have just a 2k samplerate with 500ms PreBuffer.
            resizeBuffer(minBufferSize);
            tempSampleStorage = new float[sampleSize * Channels];
            tempSampleStoragePos = 0;
            // Start thread to decode samples
            startThread("FFmpeg Audio Decoder Thread");
        }

        /// <summary>
        /// Finalizes this instance.
        /// </summary>
        ~FFmpegAudioDecoder()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases the unmanaged resources used by this instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                // Dispose base class first here
                base.Dispose(disposing);
                // Close decoder
                decoder.Dispose();
                decoder = null;
                tempSampleStorage = null;
            }
        }

        /// <summary>
        /// Adds a new item to the buffer.
        /// </summary>
        /// <param name="entry">The old entry at the position where the item will be added. This entry shall be modified by this method.</param>
        /// <returns>
        /// <see langword="true"/> if the next item could be set;
        /// otherwise <see langword="false"/> to indicate an error or EOF and the termination of the thread.
        /// </returns>
        protected override unsafe bool addItemToBuffer(ref TimestampItem<float[]> entry)
        {
            if (eofReached) return false;
            // Get current frame
            AVFrame* frame = decoder.CurrentFrame;
            // Create new entry
            if (entry.Item == null) entry.Item = new float[sampleSize * Channels];
            entry.Timestamp = (totalSamplePos * 1000000 / SampleRate) + StartTimestamp;
            int entryPos = 0;
            // Store result
            do
            {
                // Copy data from frame
                entryPos = copyData(frame, entry.Item, entryPos, decoder.SampleFormat);
                if (entryPos == -1)
                {
                    Log.Error("FFmpeg Audio Decoder stopped working for file {FileName}. SampleFormat {Format} is not supported.", decoder.URL, decoder.SampleFormat);
                    return false;
                }
                if (entryPos == entry.Item.Length) break; // The buffer is full, we can return
                // Decode next frame
                bool success = decoder.DecodeNextFrame(out int errorCode);
                if (!success)
                {
                    if (errorCode != 0)
                    {
                        Log.Error("FFmpeg Audio Decoder stopped working for file {FileName} with {ErrorCode}: {ErrorDescription}", decoder.URL, errorCode, FFmpeg.AVGetErrorDescription(errorCode));
                        return false;
                    }
                    else
                    {
                        Log.Information("FFmpeg Audio Decoder reached end of file for file {FileName}.", decoder.URL);
                        eofReached = true;
                        float[] newItem = new float[entryPos];
                        Buffer.BlockCopy(entry.Item, 0, newItem, 0, entryPos * 4);
                        entry.Item = newItem;
                        break;
                    }
                }
                else
                    frameSamplePos = 0;
                frame = decoder.CurrentFrame;
            } while (true);
            // Return success
            return true;
        }

        /// <summary>
        /// Helpermethod to copy frame data.
        /// </summary>
        private unsafe int copyData(AVFrame* frame, float[] item, int itemPos, AVSampleFormat sampleFormat)
        {
            switch(sampleFormat)
            {
                case AVSampleFormat.AV_SAMPLE_FMT_FLTP:
                    return copyFltpData(frame, item, itemPos);
                case AVSampleFormat.AV_SAMPLE_FMT_S16P:
                    return copyS16pData(frame, item, itemPos);
                case AVSampleFormat.AV_SAMPLE_FMT_U8P:
                    return copyU8pData(frame, item, itemPos);
                default:
                    return -1;
            }
        }

        /// <summary>
        /// Helpermethod to copy frame data in the sample format "float, panar".
        /// </summary>
        private unsafe int copyFltpData(AVFrame* frame, float[] item, int itemPos)
        {
            float*[] _pFrameDataArray = frame->Data.ToFloatPtrArray();
            for (; frameSamplePos < frame->SamplesCount; frameSamplePos++)
            {
                for (uint ch = 0; ch < Channels; ch++)
                {
                    float* _pFData = _pFrameDataArray[ch] + frameSamplePos;
                    item[itemPos++] = *_pFData;
                }
                totalSamplePos++;
                if (itemPos == item.Length) break;
            }
            return itemPos;
        }

        /// <summary>
        /// Helpermethod to copy frame data in the sample format "short, panar".
        /// </summary>
        private unsafe int copyS16pData(AVFrame* frame, float[] item, int itemPos)
        {
            short*[] _pFrameDataArray = frame->Data.ToShortPtrArray();
            for (; frameSamplePos < frame->SamplesCount; frameSamplePos++)
            {
                for (uint ch = 0; ch < Channels; ch++)
                {
                    short* _pSData = _pFrameDataArray[ch] + frameSamplePos;
                    float fData = (float)*_pSData / short.MaxValue;
                    if (fData < -1.0f) fData = -1.0f;
                    item[itemPos++] = fData;
                }
                totalSamplePos++;
                if (itemPos == item.Length) break;
            }
            return itemPos;
        }

        /// <summary>
        /// Helpermethod to copy frame data in the sample format "uint8, panar".
        /// </summary>
        private unsafe int copyU8pData(AVFrame* frame, float[] item, int itemPos)
        {
            byte*[] _pFrameDataArray = frame->Data.ToBytePtrArray();
            for (; frameSamplePos < frame->SamplesCount; frameSamplePos++)
            {
                for (uint ch = 0; ch < Channels; ch++)
                {
                    byte* _pBData = _pFrameDataArray[ch] + frameSamplePos;
                    float fData = ((float)*_pBData / byte.MaxValue * 2) - 1;
                    item[itemPos++] = fData;
                }
                totalSamplePos++;
                if (itemPos == item.Length) break;
            }
            return itemPos;
        }

        /// <summary>
        /// Gets an audio playback callback from the decoder.
        /// </summary>
        /// <returns>A <see cref="AudioPlaybackCallback"/> delegate which can be called to retrieve audio samples.</returns>
        public override AudioPlaybackCallback GetAudioPlaybackCallback()
        {
            return audioPlaybackCallback;
        }

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
        private int audioPlaybackCallback(AudioPlayback handle, float[] buffer, int maxLength)
        {
            if (!base.DecoderRunning && !base.ItemsAvailable && tempSampleStoragePos == 0) return 0;
            int bufferPos = 0;
            int maxRemainingSamples = maxLength;
            // 
            if (tempSampleStoragePos > 0)
            {
                int count = Math.Min(tempSampleStoragePos, maxRemainingSamples);
                Buffer.BlockCopy(tempSampleStorage, 0, buffer, 0, count * 4);
                bufferPos = count;
                maxRemainingSamples -= count;
                tempSampleStoragePos -= count;
                if (tempSampleStoragePos > 0)
                {
                    Buffer.BlockCopy(tempSampleStorage, count * 4, tempSampleStorage, 0, tempSampleStoragePos * 4);
                    return bufferPos;
                }
            }
            do
            {
                if (base.ItemsAvailable)
                {
                    TimestampItem<float[]> item = base.NextItem();
                    int count = Math.Min(item.Item.Length, maxRemainingSamples);
                    Buffer.BlockCopy(item.Item, 0, buffer, bufferPos * 4, count * 4);
                    bufferPos += count;
                    maxRemainingSamples -= count;
                    if (count < item.Item.Length)
                    {
                        tempSampleStoragePos = item.Item.Length - count;
                        Buffer.BlockCopy(item.Item, count * 4, tempSampleStorage, 0, tempSampleStoragePos * 4);
                        return bufferPos;
                    }
                }
                else
                    return bufferPos;
            } while (maxRemainingSamples > 0);
            return bufferPos;
        }

        /// <summary>
        /// Gets the name of the codec used for decoding.
        /// </summary>
        public override string CodecName { get; protected set; }

        /// <summary>
        /// Gets the codec long name.
        /// </summary>
        public override string CodecLongName { get; protected set; }

        /// <summary>
        /// Gets the duration of the video.
        /// </summary>
        public override float Duration { get; protected set; }

        /// <summary>
        /// Gets the start time of the first video frame.
        /// </summary>
        public override long StartTimestamp { get; protected set; }

        /// <summary>
        /// Gets the number of channels in the audio.
        /// </summary>
        public override int Channels { get; protected set; }

        /// <summary>
        /// Gets the sample rate of the audio.
        /// </summary>
        public override int SampleRate { get; protected set; }
    }
}
