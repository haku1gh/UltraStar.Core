#region License (LGPL)
/*
 * This file is part of UltraStar.Core.
 * 
 * You should have received a copy of the GNU Lesser General Public License 3
 * along with UltraStar.Core. If not, see <http://www.gnu.org/licenses/lgpl-3.0>.
 */
#endregion License

using System;
using System.IO;

namespace UltraStar.Core
{
    /// <summary>
    /// This class contains configs which can be changed by calling programs.
    /// </summary>
    /// <remarks>
    /// These configurations are not intended to be displayed to the user.
    /// All user releated settings can be retrieved or set in <see cref="UsOptions"/>.
    /// </remarks>
    public static class UsConfig
    {
        /// <summary>
        /// Initializes <see cref="UsConfig"/>.
        /// </summary>
        static UsConfig()
        {
            LibraryRootPath = "." + Path.DirectorySeparatorChar + "libs" + Path.DirectorySeparatorChar;
            LibrarySubFoldersExisting = true;
            LibrarySearchWithoutRootPath = true;
        }

        /// <summary>
        /// Gets or sets the root path to the native libraries.
        /// </summary>
        public static string LibraryRootPath { get; set; }

        /// <summary>
        /// Gets or sets an indication whether the libraries are found in the respective sub-directories
        /// or directly in the root path.
        /// </summary>
        /// <remarks>
        /// Per default it is assumed that all libraries reside in their sub-directories.
        /// If this value is <see langword="true"/>, then libaries will be searched in: "[rootPath]/[platform]/[architecture]/".
        /// If this value is <see langword="false"/>, then libaries will be searched in: "[rootPath]/".
        /// </remarks>
        public static bool LibrarySubFoldersExisting { get; set; }

        /// <summary>
        /// Gets or sets an indication whether the libraries could be found without a root path specified.
        /// </summary>
        /// <remarks>
        /// If this value is <see langword="true"/>, then libaries will be searched by the operating systems search strategy;
        /// otherwise this will be skipped.
        /// This approach of loading libraies will only be tried when loading the library from the rootPath failed.
        /// </remarks>
        public static bool LibrarySearchWithoutRootPath { get; set; }
    }
}
