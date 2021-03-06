﻿#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.Runtime.CompilerServices;

// Temporarily added here for testing access of project TestVideoConsole
[assembly: InternalsVisibleTo("TestAudioConsole")]
[assembly: InternalsVisibleTo("TestVideoConsole")]
[assembly: InternalsVisibleTo("TestGUI")]

namespace UltraStar.Core
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public class UsLogging
    {
        // Provide logging capabilities for view
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

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
