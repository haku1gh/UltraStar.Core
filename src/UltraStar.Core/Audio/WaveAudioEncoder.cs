#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.IO;
using System.Text;

namespace UltraStar.Core.Audio
{
    /// <summary>
    /// Represents an audio encoder using the WAVE file format specification.
    /// </summary>
    public class WaveAudioEncoder
    {
        // Private variables
        private BinaryWriter binWriter;
        private readonly WaveAudioFormat audioFormat;

        /// <summary>
        /// Indicator if this instance is disposed.
        /// </summary>
        protected bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of <see cref="WaveAudioEncoder"/>.
        /// </summary>
        /// <param name="filename">The file name where the WAVE audio output will be stored.</param>
        /// <param name="audioFormat">The audio format to be used for encoding.</param>
        /// <param name="channels">The number of channels to encode.</param>
        /// <param name="samplerate">The samplerate of the provided audio samples.</param>
        public WaveAudioEncoder(string filename, WaveAudioFormat audioFormat, int channels, int samplerate)
        {
            // Create binary writer
            Stream stream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            binWriter = new BinaryWriter(stream);
            // Store audio format
            this.audioFormat = audioFormat;
            // Create all headers
            generateRiffTag();
            generateFmtSubtag(audioFormat, channels, samplerate);
            generateDataSubtag();
        }

        /// <summary>
        /// Helpermethod to generate the "RIFF" tag.
        /// </summary>
        private void generateRiffTag()
        {
            // Write data
            binWriter.Write(Encoding.ASCII.GetBytes("RIFF")); // = Tag: "RIFF"
            binWriter.Write(0u);                              // = LengthOfTag: filesize - 8, unknown yet so set to 0
            binWriter.Write(Encoding.ASCII.GetBytes("WAVE")); // = RiffType: "WAVE"
        }

        /// <summary>
        /// Helpermethod to generate the "fmt " sub-tag.
        /// </summary>
        private void generateFmtSubtag(WaveAudioFormat audioFormat, int channels, int samplerate)
        {
            // Checks
            if (channels < 0 || channels > 0xFFFF)
                throw new ArgumentOutOfRangeException("channels");
            if (samplerate < 0)
                throw new ArgumentOutOfRangeException("channels");
            // Prepare some values
            ushort bitsPerSample = 16; // Default for PCM
            if (audioFormat == WaveAudioFormat.Float) bitsPerSample = 32;
            ushort blockAlign = (ushort)(channels * (bitsPerSample / 8));
            uint byteRate = (uint)(samplerate * blockAlign);
            // Write data
            binWriter.Write(Encoding.ASCII.GetBytes("fmt ")); // = TagType: "fmt "
            binWriter.Write(16u);                             // = LengthOfSubtag: 16 bytes for PCM and IEEE Float
            binWriter.Write((ushort)audioFormat);             // = AudioFormat: E.g. 1 (PCM) or 3 (IEE Float)
            binWriter.Write((ushort)channels);                // = Channels: E.g. 2
            binWriter.Write((uint)samplerate);                // = Samplerate: E.g. 48000
            binWriter.Write(byteRate);                        // = ByteRate: this is SampleRate * NumChannels * SampleSize
            binWriter.Write(blockAlign);                      // = BlockAlign: this is NumChannels * SampleSize
            binWriter.Write(bitsPerSample);                   // = BitsPerSample: this SampleSize in bits
        }

        /// <summary>
        /// Helpermethod to generate the "DATA" sub-tag.
        /// </summary>
        private void generateDataSubtag()
        {
            // Write data
            binWriter.Write(Encoding.ASCII.GetBytes("data")); // = TagType: "data"
            binWriter.Write(0u);                              // = LengthOfSubtag: filesize - 44, unknown yet so set to 0
        }

        /// <summary>
        /// Finalizes this instance.
        /// </summary>
        ~WaveAudioEncoder()
        {
            Dispose(false);
        }

        /// <summary>
        /// Closes the WAVE audio encoder.
        /// </summary>
        public void Close()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases the unmanaged resources used by this instance and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                // Special for Close() and Dispose()
                if (disposing)
                    GC.SuppressFinalize(this);
                if (binWriter != null && binWriter.BaseStream != null)
                {
                    // Calculate sizes for RIFF header and DATA header
                    long fileSize = binWriter.BaseStream.Length;
                    uint riffSize = (uint)(fileSize - 8);
                    uint dataSize = (uint)(fileSize - 44);
                    // Write sizes to headers
                    binWriter.BaseStream.Position = 4;
                    binWriter.Write(riffSize);
                    binWriter.BaseStream.Position = 40;
                    binWriter.Write(dataSize);
                    // Close file
                    binWriter.Flush();
                    binWriter.Close();
                }
                // Free other resources
                binWriter = null;
            }
        }

        /// <summary>
        /// Encodes sample data.
        /// </summary>
        /// <param name="data">An array containing the sample data.</param>
        /// <param name="length">The number of elements from the <paramref name="data"/> array to be encoded.</param>
        public void Encode(float[] data, int length)
        {
            // Check if disposed
            if (isDisposed) return;
            // Write data
            if (audioFormat == WaveAudioFormat.Float)
            {
                for (int i = 0; i < length; i++)
                {
                    binWriter.Write(data[i]);
                }
            }
            if (audioFormat == WaveAudioFormat.PCM)
            {
                for (int i = 0; i < length; i++)
                {
                    float value = data[i];
                    if (value < -1) value = -1;
                    if (value > 1) value = 1;
                    short sValue = (short)(value * 32767);
                    binWriter.Write(sValue);
                }
            }
        }

        /// <summary>
        /// Encodes sample data.
        /// </summary>
        /// <param name="data">An array containing the sample data.</param>
        /// <param name="length">The number of elements from the <paramref name="data"/> array to be encoded.</param>
        public void Encode(short[] data, int length)
        {
            // Check if disposed
            if (isDisposed) return;
            // Write data
            if (audioFormat == WaveAudioFormat.Float)
            {
                for (int i = 0; i < length; i++)
                {
                    float value = (float)data[i] / 32767;
                    if (value < -1) value = -1;
                    binWriter.Write(value);
                }
            }
            if (audioFormat == WaveAudioFormat.PCM)
            {
                for (int i = 0; i < length; i++)
                {
                    binWriter.Write(data[i]);
                }
            }
        }
    }
}
