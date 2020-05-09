#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using UltraStar.Core.Utils;

namespace UltraStar.Core.Voice
{
    /// <summary>
    /// Represents the classic pitch detector used in UltraStar WorldParty.
    /// </summary>
    /// <remarks>
    /// The code here is uses the basic mechanisms with auto correlation as used in UltraStar WorldParty.
    /// Nevertheless this implementation with the abstract base class offers a higher resolution of detected pitches and
    /// together with a fixed detection interval a more reliable pitch source.
    /// </remarks>
    public class ClassicPitchDetector : PitchDetector
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ClassicPitchDetector"/>.
        /// </summary>
        /// <param name="detectionInterval">The detection interval used in the pitch detector in number of samples.</param>
        /// <param name="samplerate">The sample rate of the audio data provided.</param>
        /// <param name="pitchDetectorCallback">A callback providing <see cref="RecordPitch"/>s.</param>
        public ClassicPitchDetector(int detectionInterval, int samplerate, PitchDetectorCallback pitchDetectorCallback) :
            base(detectionInterval, samplerate, pitchDetectorCallback)
        {
            // Start thread to decode frames
            startThread("Classic Pitch Detector Thread");
        }

        /// <summary>
        /// Analyses the audio sample buffer to detect a pitch.
        /// </summary>
        /// <param name="buffer">The buffer to analyse.</param>
        /// <param name="volume">The maximum volume detected.</param>
        /// <param name="pitchDetected">A preindication whether a pitch was detected. A modification of this value to <see langword="false"/> is allowed.</param>
        /// <param name="frequency">The frequency of the detected.</param>
        protected override void analyze(IRingBuffer<float> buffer, float volume, ref bool pitchDetected, out float frequency)
        {
            frequency = 0;
            // Return if pitch is already marked as not detected
            if (!pitchDetected && !LibrarySettings.PitchDetectorFullAnalysis) return;
            // Perform analysis
            double maxWeight = double.MinValue;
            for (int pitch = LibrarySettings.PitchDetectorMinimumPitch; pitch <= LibrarySettings.PitchDetectorMaximumPitch; pitch++)
            {
                float pitchFrequency = (float)Math.Pow(2, (double)(pitch - 69) / 12) * 440;
                double weight = performAutocorrelation(buffer, pitchFrequency);
                if (weight > maxWeight)
                {
                    maxWeight = weight;
                    frequency = pitchFrequency;
                }
            }
            //int toneAbs = AnalyzeByAutocorrelation(buffer.ToArray(correlationSampleCount));
            //frequency = (float)Math.Pow(2, (double)(toneAbs + 36 - 69) / 12) * 440;
        }

        /// <summary>
        /// Helpermethod to perform auto correlation as UltraStar is doing it.
        /// </summary>
        private double performAutocorrelation(IRingBuffer<float> buffer, float frequency)
        {
            // Initialize some variables
            double totalDistance = 0;
            int offset = (int)Math.Round(SampleRate / frequency); // The index of the sample one period ahead
            int offsetIndex = offset;
            // Compare correlating samples in the buffer
            for (int bufferIndex = 0; bufferIndex < (correlationSampleCount - offset); bufferIndex++)
            {
                // Calculate the distance (correlation: 1-dist) to the corresponding sample in the next period
                totalDistance += Math.Abs(buffer[bufferIndex] - buffer[offsetIndex++]) / 2.0;
            }
            // Return the "inverse" average distance (auto correlation)
            return 1 - (totalDistance / correlationSampleCount);
        }


        int AnalyzeByAutocorrelation(float[] buffer)
        {
            double baseToneFreq = 65.4064; // lowest (half-)tone to analyze (C2 = 65.4064 Hz)
            int numHalfTones = 46;      // C2-A5 (for Whitney and my high voice)
            int toneIndex;
            double curFreq;
            double curWeight;
            double maxWeight = -1d;
            int maxTone = 0;
            double halftoneBase = 1.05946309436;

            for (toneIndex = 0; toneIndex < numHalfTones; toneIndex++)
            {
                curFreq = baseToneFreq * Math.Pow(halftoneBase, toneIndex);
                curWeight = AnalyzeAutocorrelationFreq(buffer, curFreq);

                // TODO: prefer higher frequencies (use >= or use downto)
                if (curWeight > maxWeight)
                {
                    // this frequency has a higher weight
                    maxWeight = curWeight;
                    maxTone = toneIndex;
                }
            }

            return maxTone;
        }

        double AnalyzeAutocorrelationFreq(float[] buffer, double freq)
        {
            double dist;                // distance (0=equal .. 1=totally different) between correlated samples
            double accumDist = 0;       // accumulated distances
            int sampleIndex = 0;        // index of sample to analyze
            int correlatingSampleIndex; // index of sample one period ahead
            int samplesPerPeriod;       // samples in one period

            samplesPerPeriod = (int)Math.Round(48000d / freq);
            correlatingSampleIndex = sampleIndex + samplesPerPeriod;

            // compare correlating samples
            while (correlatingSampleIndex < buffer.Length)
            {
                // calc distance (correlation: 1-dist) to corresponding sample in next period
                dist = Math.Abs(buffer[sampleIndex] - buffer[correlatingSampleIndex]) / 2.0;
                accumDist = accumDist + dist;
                sampleIndex++;
                correlatingSampleIndex++;
            }

            // return "inverse" average distance (=correlation)
            return 1 - accumDist / buffer.Length;
        }
    }
}
