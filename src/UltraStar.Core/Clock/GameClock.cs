#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.Diagnostics;

namespace UltraStar.Core.Clock
{
    /// <summary>
    /// Respresents a clock for the Ultrastar game.
    /// </summary>
    internal class GameClock
    {
        // Private variables
        private Stopwatch watch;
        private uint delay;
        private long zeroOffset;

        /// <summary>
        /// Initializes a new instance of <see cref="GameClock"/>.
        /// </summary>
        /// <param name="delay">
        /// An additional delay in [us] for starting the game clock.
        /// With this delay it is possible to get a negative <see cref="Elapsed"/> time.
        /// </param>
        public GameClock(uint delay = 0)
        {
            watch = new Stopwatch();
            watch.Start();
            if (delay < 0) delay = 0;
            this.delay = delay;
            Running = false;
            Paused = false;
        }

        /// <summary>
        /// Starts the game clock.
        /// </summary>
        /// <param name="reference">
        /// A reference in [us] to the <see cref="GlobalClock"/> time.
        /// This will serve as an delay calculated as: <paramref name="reference"/>-<see cref="GlobalClock.Ticks"/>.
        /// With this reference it is possible to get a negative <see cref="Elapsed"/> time.
        /// It is also possible to contruct a game clock that starts with an <see cref="Elapsed"/> time of greater than 0.
        /// </param>
        public void Start(long reference)
        {
            zeroOffset = delay + reference - GlobalClock.Ticks;
            watch.Restart();
            Running = true;
            Paused = false;
        }

        /// <summary>
        /// Starts the game clock.
        /// </summary>
        public void Start()
        {
            zeroOffset = delay;
            watch.Restart();
            Running = true;
            Paused = false;
        }

        /// <summary>
        /// Pauses the game clock.
        /// </summary>
        public void Pause()
        {
            if (Running)
            {
                watch.Stop();
                Paused = true;
            }
        }

        /// <summary>
        /// Resumes the game clock.
        /// </summary>
        public void Resume()
        {
            if (Paused)
            {
                watch.Start();
                Paused = false;
            }
        }

        /// <summary>
        /// Stops the game clock.
        /// </summary>
        public void Stop()
        {
            if (Running)
            {
                watch.Stop();
                Running = false;
                Paused = false;
            }
        }

        /// <summary>
        /// Gets the elapsed micro seconds [us].
        /// </summary>
        public long Elapsed
        {
            get { return (1000000 * watch.ElapsedTicks / Stopwatch.Frequency) - zeroOffset; }
        }

        /// <summary>
        /// Gets the elapsed milli seconds [ms].
        /// </summary>
        public long ElapsedMilliseconds
        {
            get { return (1000 * watch.ElapsedTicks / Stopwatch.Frequency) - (zeroOffset / 1000); }
        }

        /// <summary>
        /// Gets the elapsed seconds [s].
        /// </summary>
        public float ElapsedSeconds
        {
            get { return (float)watch.ElapsedTicks / Stopwatch.Frequency - (zeroOffset / 1000000f); }
        }

        /// <summary>
        /// Gets or sets the delay of game clock.
        /// </summary>
        /// <remarks>
        /// Changes are only applied when the clock is restarted.
        /// </remarks>
        public uint Delay
        {
            get { return delay; }
            set
            {
                if (value < 0)
                    delay = 0;
                else
                    delay = value;
            }
        }

        /// <summary>
        /// Gets an indicator whether the clock is running.
        /// </summary>
        public bool Running { get; private set; }

        /// <summary>
        /// Gets an indicator whether the clock is paused.
        /// </summary>
        public bool Paused { get; private set; }
    }
}
