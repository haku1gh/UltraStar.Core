#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.Runtime.InteropServices;
using UltraStar.Core.Utils;

namespace UltraStar.Core.Voice
{
    /// <summary>
    /// Represents a recording of a single (detected) pitch.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RecordPitch
    {
        private static readonly string[] baseNotes = new string[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        private byte pitchDetected;
        private byte pitch;
        private byte pitchRelative;
        private readonly byte loudness;
        private float frequency;
        private readonly float volume;

        /// <summary>
        /// Initializes a new instance of <see cref="RecordPitch"/>.
        /// </summary>
        /// <param name="pitchDetected">The indicator if a pitch had been detected or not.</param>
        /// <param name="frequency">The frequency of the pitch.</param>
        /// <param name="volume">The volume of the pitch.</param>
        public RecordPitch(bool pitchDetected, float frequency, float volume)
        {
            this.pitchDetected = pitchDetected ? (byte)1 : (byte)0;
            this.frequency = frequency;
            int intPitch = (int)Math.Round(69 + 12 * Math.Log(frequency / 440, 2));
            if (intPitch < 0 || intPitch > 99) // Mark pitch as invalid
            {
                this.pitchDetected = 0;
                pitch = 0;
                pitchRelative = 0;
            }
            else
            {
                pitch = (byte)intPitch;
                pitchRelative = (byte)(intPitch % 12);
            }
            this.volume = volume;
            loudness = (byte)(VolumeConversion.ConvertLinearToLoudness(volume) * 100);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RecordPitch"/>.
        /// </summary>
        /// <param name="pitchDetected">The indicator if a pitch had been detected or not.</param>
        /// <param name="pitch">The integer representation of a pitch.</param>
        /// <param name="volume">The volume of the pitch.</param>
        public RecordPitch(bool pitchDetected, int pitch, float volume)
        {
            this.pitchDetected = pitchDetected ? (byte)1 : (byte)0;
            if (pitch < 0 || pitch > 99) // Mark pitch as invalid
            {
                this.pitchDetected = 0;
                this.pitch = 0;
                pitchRelative = 0;
                frequency = 0f;
            }
            else
            {
                this.pitch = (byte)pitch;
                pitchRelative = (byte)(pitch % 12);
                frequency = (float)Math.Pow(2, (double)(this.pitch - 69) / 12) * 440;
            }
            this.volume = volume;
            loudness = (byte)(VolumeConversion.ConvertLinearToLoudness(volume) * 100);
        }

        /// <summary>
        /// Gets or sets the frequency of the pitch.
        /// </summary>
        public float Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        /// <summary>
        /// Gets or sets the integer representation of a pitch.
        /// </summary>
        public int Pitch
        {
            get { return pitch; }
            set
            {
                if (value < 0 || value > 99)
                    value = 0;
                pitch = (byte)value;
                pitchRelative = (byte)(pitch % 12);
            }
        }

        /// <summary>
        /// Gets the relative integer representation of a pitch.
        /// </summary>
        public int PitchRelative => pitchRelative;

        /// <summary>
        /// Gets the volume of the pitch.
        /// </summary>
        public float Volume => volume;

        /// <summary>
        /// Gets the perceived loudness of the pitch.
        /// </summary>
        public float Loudness => (float)loudness / 100;

        /// <summary>
        /// Gets the perceived loudness of the pitch in the scale from 0 to 100.
        /// </summary>
        public int LoudnessInt => loudness;

        /// <summary>
        /// Gets an indicator whether the pitch was detected correctly or not.
        /// </summary>
        public bool PitchDetected
        {
            get { return !(pitchDetected == 0); }
            set { pitchDetected = value ? (byte)1 : (byte)0; }
        }

        /// <summary>
        /// Gets the note in scientific pitch notation (EDV friendly version). E.g. "C#2".
        /// </summary>
        public string Note => baseNotes[pitchRelative] + (pitch / 12 - 1).ToString();

        /// <summary>
        /// Gets the relative note in scientific pitch notation (EDV friendly version). E.g. "C#". Note here that the octave information is missing.
        /// </summary>
        public string NoteRelative => baseNotes[pitchRelative];
    }
}
