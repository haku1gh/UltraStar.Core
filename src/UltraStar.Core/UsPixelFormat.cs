#region License (LGPL)
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
    /// <summary>
    /// Represents the pixel format of a video or image.
    /// </summary>
    public enum UsPixelFormat
    {
        /// <summary>
        /// No pixel format specified.
        /// </summary>
        None,
        /// <summary>
        /// Packed RGBA 8:8:8:8, 32bpp, RGBARGBA...
        /// </summary>
        RGBA,
        /// <summary>
        /// Packed RGB 8:8:8, 24bpp, BGRBGR...
        /// </summary>
        BGR24
    }
}
