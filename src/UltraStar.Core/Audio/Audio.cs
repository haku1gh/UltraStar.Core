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
    abstract class AudioPlayback
    {
        // Normal output stream
        AudioPlayback(int frequency, int channels, AudioPlaybackCallback audioPlaybackCallback)
        {

        }
        // PUSH stream
        AudioPlayback(int frequency, int channels)
        {

        }

        public abstract void Start();
        public abstract void Stop();

        public static void GetDevices()
        {

        }
    }

    // Maybe makes sense?
    abstract class Audio
    {

    }
}
