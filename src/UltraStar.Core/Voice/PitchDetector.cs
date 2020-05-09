#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.Collections.Generic;
using System.Threading;
using UltraStar.Core.Utils;
using UltraStar.Core.ThirdParty.Serilog;

namespace UltraStar.Core.Voice
{
    /// <summary>
    /// Represents an abstract pitch detector class.
    /// </summary>
    public abstract class PitchDetector : IDisposable
    {
        // Private variables
        private static readonly int pitchBufferSize = 4;    // Size depends on post analysis
        private static readonly int initialQueueSize = 10;
        private IRingBuffer<float> sampleBuffer;
        private IRingBuffer<RecordPitch> pitchBuffer;
        private Queue<float[]> queue;
        private Thread workerThread;
        private EventWaitHandle sleepWaitHandle;        // Sleep wait handle
        private PitchDetectorCallback pitchDetectorCallback;
        private int intervalPos;
        private readonly int sampleCountForVolumeCalculation;

        /// <summary>
        /// The sample count which should be used for detecting the pitch.
        /// </summary>
        /// <remarks>
        /// A pitch detector implementation class can of course us less samples.
        /// </remarks>
        protected static readonly int correlationSampleCount = 4096;

        /// <summary>
        /// Indicator if this instance is disposed.
        /// </summary>
        protected bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of <see cref="PitchDetector"/>.
        /// </summary>
        /// <param name="detectionInterval">The detection interval used in the pitch detector in number of samples.</param>
        /// <param name="samplerate">The sample rate of the audio data provided.</param>
        /// <param name="pitchDetectorCallback">A callback providing <see cref="RecordPitch"/>s.</param>
        public PitchDetector(int detectionInterval, int samplerate, PitchDetectorCallback pitchDetectorCallback)
        {
            // Check if callback is available
            if (pitchDetectorCallback == null)
                throw new VoiceException("No callback provided to the pitch detector.");
            // Initialize all variables
            DetectionInterval = detectionInterval;
            SampleRate = samplerate;
            this.pitchDetectorCallback = pitchDetectorCallback;
            sampleBuffer = new FastRingBuffer<float>(correlationSampleCount);
            pitchBuffer = new FastRingBuffer<RecordPitch>(pitchBufferSize);
            queue = new Queue<float[]>(initialQueueSize);
            intervalPos = 0;
            sampleCountForVolumeCalculation = detectionInterval * 2;
            if (sampleCountForVolumeCalculation < (samplerate / 60)) sampleCountForVolumeCalculation = (samplerate / 60);
            InitialSkippedAudioTime = 1000000L * (detectionInterval * 2) / samplerate;
        }

        /// <summary>
        /// Finalizes this instance.
        /// </summary>
        ~PitchDetector()
        {
            Dispose(false);
        }

        /// <summary>
        /// Closes the pitch detector.
        /// </summary>
        public virtual void Close()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public virtual void Dispose()
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
                stopThread();
                sampleBuffer = null;
                pitchBuffer = null;
                queue = null;
                workerThread = null;
                sleepWaitHandle = null;
                pitchDetectorCallback = null;
            }
        }

        /// <summary>
        /// Starts the internal thread.
        /// </summary>
        protected void startThread(string threadName)
        {
            // Initialize wait handles, queues and threads
            sleepWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            workerThread = new Thread(backgroundTask)
            {
                IsBackground = true,
                Priority = ThreadPriority.Normal,
                Name = threadName
            };
            // Start threads
            lock (queue) Running = true;
            workerThread.Start();
        }

        /// <summary>
        /// Stops the internal thread.
        /// </summary>
        /// <returns><see langword="true"/> if the thread is stopped; otherwise <see langword="false"/> to indicate an error.</returns>
        protected bool stopThread()
        {
            if ((workerThread.ThreadState == ThreadState.Stopped) || (workerThread.ThreadState == ThreadState.Unstarted))
                return true;
            bool returnValue = false;
            lock (queue) Running = false;
            try
            {
                lock (sleepWaitHandle)
                {
                    sleepWaitHandle.Set(); // This means, wake up there is something to do
                }
                returnValue = workerThread.Join(500);
            }
            catch { }
            return returnValue;
        }

        /// <summary>
        /// The background task running in a separate thread.
        /// </summary>
        private void backgroundTask()
        {
            try
            {
                while (true)
                {
                    // Check if queue is empty, if yes sleep
                    bool queueEmpty = false;
                    lock (queue)
                    {
                        if (!Running) break;
                        queueEmpty = (queue.Count == 0);
                        if (queueEmpty)
                        {
                            lock (sleepWaitHandle)
                            {
                                sleepWaitHandle.Reset();
                            }
                        }
                    }
                    if (queueEmpty)
                    {
                        sleepWaitHandle.WaitOne();
                        continue;
                    }
                    // Get next audio sample
                    float[] nextAudioSamples = null;
                    lock (queue)
                    {
                        if (!Running) break;
                        nextAudioSamples = queue.Dequeue();
                    }
                    // Process audio samples
                    foreach (float nextAudioSample in nextAudioSamples)
                    {
                        // Update buffer
                        sampleBuffer.Push(nextAudioSample);
                        intervalPos++;
                        if (intervalPos == DetectionInterval) intervalPos = 0;
                        // Perform analysis if interval is reached
                        if (intervalPos == 0)
                        {
                            // Calculate maximum volume
                            float volume = getMaxVolume();
                            // Get first indicator whether the pitch could be detected
                            bool pitchDetected = (volume >= ((float)UsOptions.MicrophoneThreshold / 100));
                            // Analyze the sample buffer
                            analyze(sampleBuffer, volume, ref pitchDetected, out float frequency);
                            pitchBuffer.Push(new RecordPitch(pitchDetected, frequency, volume));

                            if (pitchBuffer.Count >= pitchBufferSize)
                            {
                                // Do some post analysis with previous pitches
                                postOptimization();
                                // Raise callback
                                pitchDetectorCallback(this, pitchBuffer[1]);
                            }
                        }
                    }
                }
            }
            // Will be raised when the thread terminates
            catch (ThreadAbortException)
            { }
            // Will be raised when the thread is interrupted while waiting
            catch (ThreadInterruptedException)
            { }
            // For any other kind of exceptions
            catch (Exception e)
            {
                // TODO: Add error logic here
                Log.Error("Unresolvable error occurred in {ThreadName}. Exception: {Exception}. Stacktrace: {Stacktrace}.", workerThread.Name, e.Message, e.StackTrace);
            }
            lock (sampleBuffer) Running = false;
        }

        /// <summary>
        /// Helpermethod to get the maximum volume from a set of audio samples.
        /// </summary>
        private float getMaxVolume()
        {
            // Initialize some variables
            float volume, maxVolume = 0;
            // Find maximum volume
            for (int i = 0; i < sampleCountForVolumeCalculation; i++)
            {
                volume = Math.Abs(sampleBuffer[i]);
                if (volume > maxVolume) maxVolume = volume;
            }
            // Return volume
            return maxVolume;
        }

        /// <summary>
        /// Helpermethod to perform some optimization on the pitch buffer.
        /// </summary>
        private void postOptimization()
        {
            // TODO
        }

        /// <summary>
        /// Analyses the audio sample buffer to detect a pitch.
        /// </summary>
        /// <param name="buffer">The buffer to analyse.</param>
        /// <param name="volume">The maximum volume detected.</param>
        /// <param name="pitchDetected">A preindication whether a pitch was detected. A modification of this value to <see langword="false"/> is allowed.</param>
        /// <param name="frequency">The frequency of the detected.</param>
        protected abstract void analyze(IRingBuffer<float> buffer, float volume, ref bool pitchDetected, out float frequency);

        /// <summary>
        /// Gets an indicator whether the internal pitch detector is running or has stopped.
        /// </summary>
        public bool Running { get; private set; }

        /// <summary>
        /// Adds a new audio sample to the pitch detector.
        /// </summary>
        /// <param name="audioSample">The audio sample to process.</param>
        public void Add(float audioSample)
        {
            lock(queue)
            {
                queue.Enqueue(new float[] { audioSample });
            }
            lock (sleepWaitHandle)
            {
                sleepWaitHandle.Set(); // This means, wake up there is something to do
            }
        }

        /// <summary>
        /// Adds new audio samples to the pitch detector.
        /// </summary>
        /// <param name="audioSamples">The audio samples to process.</param>
        /// <param name="length">The number of samples provided in <paramref name="audioSamples"/>.</param>
        public void Add(float[] audioSamples, int length)
        {
            float[] samples = new float[length];
            Buffer.BlockCopy(audioSamples, 0, samples, 0, length * 4);
            lock (queue)
            {
                queue.Enqueue(samples);
            }
            lock (sleepWaitHandle)
            {
                sleepWaitHandle.Set(); // This means, wake up there is something to do
            }
        }

        /// <summary>
        /// Gets the detection interval used in the pitch detector in number of samples.
        /// </summary>
        public int DetectionInterval { get; }

        /// <summary>
        /// Gets the sample rate of the audio data provided.
        /// </summary>
        public int SampleRate { get; }

        /// <summary>
        /// Gets the initially skipped audio time in micro seconds [us].
        /// </summary>
        /// <remarks>
        /// The internal buffers need to fill up before they can even start detecting a pitch.
        /// </remarks>
        public long InitialSkippedAudioTime { get; }
    }
}
