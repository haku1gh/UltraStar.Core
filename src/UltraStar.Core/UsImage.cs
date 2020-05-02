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
    /// Represents an image.
    /// </summary>
    public struct UsImage
    {
        /// <summary>
        /// Initializes the struct <see cref="UsImage"/>.
        /// </summary>
        /// <param name="height">The height of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="pixelFormat">The pixel format of the image.</param>
        /// <param name="data">The image data.</param>
        /// <param name="codecName">The name of the codec used for decoding.</param>
        /// <param name="codecLongName">The codec long name.</param>
        internal UsImage(int height, int width, UsPixelFormat pixelFormat, byte[] data, string codecName, string codecLongName)
        {
            Height = height;
            Width = width;
            PixelFormat = pixelFormat;
            Data = data;
            CodecName = codecName;
            CodecLongName = codecLongName;
        }

        /// <summary>
        /// Gets the height of the image.
        /// </summary>
        public int Height { get; internal set; }

        /// <summary>
        /// Gets the width of the image.
        /// </summary>
        public int Width { get; internal set; }

        /// <summary>
        /// Gets the pixel format of the image.
        /// </summary>
        public UsPixelFormat PixelFormat { get; internal set; }

        /// <summary>
        /// Gets the image data.
        /// </summary>
        public byte[] Data { get; internal set; }

        /// <summary>
        /// Gets the name of the codec used for decoding.
        /// </summary>
        public string CodecName { get; internal set; }

        /// <summary>
        /// Gets the codec long name.
        /// </summary>
        public string CodecLongName { get; internal set; }
    }
}
