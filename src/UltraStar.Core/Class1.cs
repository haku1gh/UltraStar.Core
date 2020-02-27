#region License
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
    internal class Class1
    {

        void asdsad()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        }
    }

    public class UsOptions
    {
        // Add here all possible options.
    }

    public class UsLogging
    {
        // Provide logging capabilities for view
    }

    public class UsImage
    {
        public float StartTimestamp;
        public int Width;
        public int Height;
        public byte[] Image;
    }

    public class UsLyricLine
    {
        public float StartTimestamp;
        public float Duration;
        public string Text;
        public UsLyricNote[] Notes;
    }

    public class UsLyricNote
    {
        public float StartTimestamp;
        public float Duration;
        public int Tone;
        public string Text;
        public bool Freestyle;
    }

    public class UsSong
    {
        /*
         * Contains everything necessary for a song.
         * 
         * UsImage GetImage(float timestamp) {}
         * bool IsDuet() {}
         * UsLyricLine[] GetLyrics(bool playerA = true) {}
         */
        // 
    }

    public enum UsGameMode
    {
        Karaoke,    // Sing
        Party,      // Challenge
        Jukebox     // Playback
    }

    public class UsGame
    {
        public UsGameMode GameMode { get; set; }
        public void Configure()
        {
            // 
        }

    }
}
