#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;

namespace UltraStar.Core.Video
{
    /// <summary>
    /// Represents an abstract image decoder class.
    /// </summary>
    public abstract class ImageDecoder
    {
        /// <summary>
        /// Decodes an image into an <see cref="UsImage"/>.
        /// </summary>
        /// <remarks>
        /// If the decoding fails, a <see langword="default"/> <see cref="UsImage"/> will be returned.
        /// The easiest way to recognize this is that <see cref="UsImage.Data"/> is <see langword="null"/>.
        /// </remarks>
        /// <param name="url">The URL of the media file.</param>
        /// <returns>A <see cref="UsImage"/>.</returns>
        public abstract UsImage DecodeImage(string url);
    }
}
