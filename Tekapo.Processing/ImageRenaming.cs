namespace Tekapo.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;

    public static class ImageRenaming
    {
        private static readonly Dictionary<string, string> _formats = GenerateFormats();

        public static string CreateFilePathWithIncrement(string path, int increment, int maxCollisionIncrement)
        {
            // Store the parts of the path
            var baseFilePath = Path.GetDirectoryName(path);
            var baseFileName = Path.GetFileNameWithoutExtension(path);
            var fileExtension = Path.GetExtension(path);

            var maxPaddingCount = maxCollisionIncrement.ToString(CultureInfo.InvariantCulture).Length;

            // Build the incremented path
            var incrementValue =
                "-" + increment.ToString(CultureInfo.InvariantCulture).PadLeft(maxPaddingCount, '0');

            // Combine the path with the increment value at the end of the new filename
            var incrementedFileName = string.Format(CultureInfo.CurrentCulture,
                "{0}{1}{2}",
                baseFileName,
                incrementValue,
                fileExtension);
            var incrementedPath = Path.Combine(baseFilePath, incrementedFileName);

            return incrementedPath;
        }

        public static string GetRenamedPath(
            string renamingFormat,
            DateTime mediaCreatedDate,
            string originalFilePath,
            bool incrementOnCollision,
            int maxCollisionIncrement)
        {
            var newName = ProcessFormat(renamingFormat, mediaCreatedDate);

            // Strip leading and trailing slashes
            newName = StripSlashes(newName);

            var newPath = Path.GetPathRoot(newName);

            if (string.IsNullOrEmpty(newPath))
            {
                var directoryName = Path.GetDirectoryName(originalFilePath);

                newName = Path.Combine(directoryName, newName);
            }

            if (string.IsNullOrEmpty(Path.GetExtension(newName)))
            {
                newName = newName + Path.GetExtension(originalFilePath);
            }

            // We now have a fully qualified new path
            var firstIncrement = CreateFilePathWithIncrement(newName, 1, maxCollisionIncrement);

            // Check for naming collisions
            if (incrementOnCollision
                && newName != originalFilePath
                && (File.Exists(newName) || File.Exists(firstIncrement)))
            {
                for (var index = 2; index <= maxCollisionIncrement; index++)
                {
                    var incrementedPath = CreateFilePathWithIncrement(newName, index, maxCollisionIncrement);

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

        public static bool IsFormatValid(string renamingFormat)
        {
            var formatTest = renamingFormat;

            foreach (var key in _formats.Keys)
            {
                formatTest = formatTest.Replace("<" + key + ">", string.Empty);
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

        private static Dictionary<string, string> GenerateFormats()
        {
            var formats = new Dictionary<string, string>();

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

        private static string ProcessFormat(string renamingFormat, DateTime mediaCreatedDate)
        {
            var renameFormat = renamingFormat;

            foreach (var key in _formats.Keys)
            {
                renameFormat = renameFormat.Replace("<" + key + ">", "{0:" + key + "}");
            }

            // Return the formatted path
            return string.Format(CultureInfo.InvariantCulture, renameFormat, mediaCreatedDate);
        }

        private static string StripSlashes(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            var returnPath = path;

            // Check if there is a leading slash
            if (returnPath.Substring(0, 1) == Path.DirectorySeparatorChar.ToString()
                || returnPath.Substring(0, 1) == Path.AltDirectorySeparatorChar.ToString())
            {
                // Check if the path starts with a double slash
                if (returnPath.Length > 1
                    && returnPath.Substring(1, 1) != Path.DirectorySeparatorChar.ToString()
                    && returnPath.Substring(1, 1) != Path.AltDirectorySeparatorChar.ToString())
                {
                    // Strip the single leading slash
                    returnPath = returnPath.Substring(1);
                }
            }

            // Check if there is a trailing slash
            if (returnPath.Substring(returnPath.Length - 1, 1) == Path.DirectorySeparatorChar.ToString()
                || returnPath.Substring(returnPath.Length - 1, 1) == Path.AltDirectorySeparatorChar.ToString())
            {
                // Remove the trailing slash
                returnPath = returnPath.Substring(0, returnPath.Length - 1);
            }

            // Return the path
            return returnPath;
        }

        public static Dictionary<string, string> RenameFormats => _formats;
    }
}