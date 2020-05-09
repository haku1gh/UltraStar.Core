#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;

namespace UltraStar.Core.Voice
{
    /// <summary>
    /// The callback to process detected pitches from a pitch detector.
    /// </summary>
    /// <param name="handle">>The pitch detector from where this callback originates.</param>
    /// <param name="pitch">A deteted pitch.</param>
    public delegate void PitchDetectorCallback(PitchDetector handle, RecordPitch pitch);
}
