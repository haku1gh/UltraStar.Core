#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.Reflection;
using UltraStar.Core.Utils;

namespace UltraStar.Core.Video
{
    /// <summary>
    /// Represents an abstract image decoder class.
    /// </summary>
    public abstract class ImageDecoder
    {
        /// <summary>
        /// Opens a new audio decoder.
        /// </summary>
        /// <param name="pixelFormat">The pixel format of the image.</param>
        /// <param name="maxWidth">The maximum width of the output image.</param>
        /// <param name="aspectRatio">The aspect ratio of the output image. Set to -1 to keep the existing aspect ratio.</param>
        public static ImageDecoder Open(UsPixelFormat pixelFormat, int maxWidth = 1920, float aspectRatio = -1.0f)
        {
            // Get the type where the class ImageDecoder is implemented
            Type imageDecoderClass = Type.GetType(LibrarySettings.ImageDecoderClassName, true);
            // Get the constructor
            ConstructorInfo cInfo = imageDecoderClass.GetConstructor(new Type[] { typeof(UsPixelFormat), typeof(int), typeof(float) });
            // Return the newly created object
            return (ImageDecoder)cInfo.Invoke(new object[] { pixelFormat, maxWidth, aspectRatio });
        }

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
