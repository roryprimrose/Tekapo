using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Tekapo.Processing
{
    /// <summary>
    /// The <see cref="ImageRenaming"/> class is used to process image renaming based on format masks.
    /// </summary>
    public static class ImageRenaming
    {
        /// <summary>
        /// Stores the renaming formats.
        /// </summary>
        private static readonly Dictionary<String, String> _formats = GenerateFormats();

        /// <summary>
        /// Creates the file path with increment.
        /// </summary>
        /// <param name="path">
        /// The path to process.
        /// </param>
        /// <param name="increment">
        /// The increment.
        /// </param>
        /// <param name="maxCollisionIncrement">
        /// The max collision increment.
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/> that contains the path modified with an increment.
        /// </returns>
        public static String CreateFilePathWithIncrement(String path, Int32 increment, Int32 maxCollisionIncrement)
        {
            // Store the parts of the path
            String baseFilePath = Path.GetDirectoryName(path);
            String baseFileName = Path.GetFileNameWithoutExtension(path);
            String fileExtension = Path.GetExtension(path);

            Int32 maxPaddingCount = maxCollisionIncrement.ToString(CultureInfo.InvariantCulture).Length;

            // Build the incremented path
            String incrementValue = "-" + increment.ToString(CultureInfo.InvariantCulture).PadLeft(maxPaddingCount, '0');

            // Combine the path with the increment value at the end of the new filename
            String incrementedFileName = String.Format(
                CultureInfo.CurrentCulture, "{0}{1}{2}", baseFileName, incrementValue, fileExtension);
            String incrementedPath = Path.Combine(baseFilePath, incrementedFileName);

            return incrementedPath;
        }

        /// <summary>
        /// Gets the renamed path.
        /// </summary>
        /// <param name="renamingFormat">
        /// The renaming format.
        /// </param>
        /// <param name="pictureTakenDate">
        /// The picture taken date.
        /// </param>
        /// <param name="originalFilePath">
        /// The original file path.
        /// </param>
        /// <param name="incrementOnCollision">
        /// If set to <c>true</c> add an increment to the filename on collision.
        /// </param>
        /// <param name="maxCollisionIncrement">
        /// The max collision increment.
        /// </param>
        /// <returns>
        /// An absolute or relative file path that has been renamed based on the date the picture was taken and a renaming format.
        /// </returns>
        public static String GetRenamedPath(
            String renamingFormat,
            DateTime pictureTakenDate,
            String originalFilePath,
            Boolean incrementOnCollision,
            Int32 maxCollisionIncrement)
        {
            String newName = ProcessFormat(renamingFormat, pictureTakenDate);

            // Strip leading and trailing slashes
            newName = StripSlashes(newName);

            String newPath = Path.GetPathRoot(newName);

            if (String.IsNullOrEmpty(newPath))
            {
                String directoryName = Path.GetDirectoryName(originalFilePath);

                newName = Path.Combine(directoryName, newName);
            }

            if (String.IsNullOrEmpty(Path.GetExtension(newName)))
            {
                newName = newName + Path.GetExtension(originalFilePath);
            }

            // We now have a fully qualified new path
            String firstIncrement = CreateFilePathWithIncrement(newName, 1, maxCollisionIncrement);

            // Check for naming collisions
            if (incrementOnCollision && (newName != originalFilePath)
                && (File.Exists(newName) || File.Exists(firstIncrement)))
            {
                for (Int32 index = 2; index <= maxCollisionIncrement; index++)
                {
                    String incrementedPath = CreateFilePathWithIncrement(newName, index, maxCollisionIncrement);

                    // Check if the path exists
                    if (File.Exists(incrementedPath) == false)
                    {
                        // The new incremented path doesn't exist

                        // Store the new path
                        newName = incrementedPath;

                        break;
                    }
                }
            }

            return newName;
        }

        /// <summary>
        /// Determines whether the specified naming format is valid.
        /// </summary>
        /// <param name="renamingFormat">
        /// The renaming format.
        /// </param>
        /// <returns>
        /// <c>true</c>if the specified format is valid; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsFormatValid(String renamingFormat)
        {
            String formatTest = renamingFormat;

            foreach (String key in _formats.Keys)
            {
                formatTest = formatTest.Replace("<" + key + ">", String.Empty);
            }

            // Check if there is a correct set of mask brackets
            if (Regex.IsMatch(formatTest, "[<>]"))
            {
                return false;
            }

            // Check if there is a mask that isn't supported
            if (Regex.IsMatch(formatTest, "[*\\?\"|]"))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Generates the formats.
        /// </summary>
        /// <returns>
        /// A <see cref="Dictionary{String, String}"/> instance.
        /// </returns>
        private static Dictionary<String, String> GenerateFormats()
        {
            Dictionary<String, String> formats = new Dictionary<String, String>();

            formats.Add("d", "Numeric day value without a leading zero");
            formats.Add("dd", "Numeric day value with a leading zero");
            formats.Add("ddd", "Abbreviated day name");
            formats.Add("dddd", "Full day name");
            formats.Add("M", "Numeric month value without a leading zero");
            formats.Add("MM", "Numeric month value with a leading zero");
            formats.Add("MMM", "Abbreviated month name");
            formats.Add("MMMM", "Full month name");
            formats.Add("y", "Single digit year value");
            formats.Add("yy", "Double digit year value");
            formats.Add("yyyy", "Four digit year value");
            formats.Add("h", "12 hour value without a leading zero");
            formats.Add("hh", "12 hour value with a leading zero");
            formats.Add("H", "24 hour value without a leading zero");
            formats.Add("HH", "24 hour value with a leading zero");
            formats.Add("m", "Minute value without a leading zero");
            formats.Add("mm", "Minute value with a leading zero");
            formats.Add("s", "Second value without a leading zero");
            formats.Add("ss", "Second value with a leading zero");
            formats.Add("t", "First character of an AM/PM value");
            formats.Add("tt", "AM/PM value");
            formats.Add("z", "Displays the time zone offset in whole hours without a leading zero");
            formats.Add("zz", "Displays the time zone offset in whole hours with a leading zero");
            formats.Add("zzz", "Displays the time zone offset in hours and minutes");

            return formats;
        }

        /// <summary>
        /// Processes the format.
        /// </summary>
        /// <param name="renamingFormat">
        /// The renaming format.
        /// </param>
        /// <param name="pictureTakenDate">
        /// The picture taken date.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static String ProcessFormat(String renamingFormat, DateTime pictureTakenDate)
        {
            String renameFormat = renamingFormat;

            foreach (String key in _formats.Keys)
            {
                renameFormat = renameFormat.Replace("<" + key + ">", "{0:" + key + "}");
            }

            // Return the formatted path
            return String.Format(CultureInfo.InvariantCulture, renameFormat, pictureTakenDate);
        }

        /// <summary>
        /// Strips the slashes.
        /// </summary>
        /// <param name="path">
        /// The path to process.
        /// </param>
        /// <returns>
        /// A path with the leading a trailing slashes removed as required.
        /// </returns>
        private static String StripSlashes(String path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return path;
            }

            String returnPath = path;

            // Check if there is a leading slash
            if ((returnPath.Substring(0, 1) == Path.DirectorySeparatorChar.ToString())
                || (returnPath.Substring(0, 1) == Path.AltDirectorySeparatorChar.ToString()))
            {
                // Check if the path starts with a double slash
                if ((returnPath.Length > 1) && (returnPath.Substring(1, 1) != Path.DirectorySeparatorChar.ToString())
                    && (returnPath.Substring(1, 1) != Path.AltDirectorySeparatorChar.ToString()))
                {
                    // Strip the single leading slash
                    returnPath = returnPath.Substring(1);
                }
            }

            // Check if there is a trailing slash
            if ((returnPath.Substring(returnPath.Length - 1, 1) == Path.DirectorySeparatorChar.ToString())
                || (returnPath.Substring(returnPath.Length - 1, 1) == Path.AltDirectorySeparatorChar.ToString()))
            {
                // Remove the trailing slash
                returnPath = returnPath.Substring(0, returnPath.Length - 1);
            }

            // Return the path
            return returnPath;
        }

        /// <summary>
        /// Gets the rename formats.
        /// </summary>
        /// <value>
        /// The rename formats.
        /// </value>
        public static Dictionary<String, String> RenameFormats
        {
            get
            {
                return _formats;
            }
        }
    }
}