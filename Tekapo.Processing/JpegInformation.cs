namespace Tekapo.Processing
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using ExifLibrary;

    /// <summary>
    ///     The <see cref="Tekapo.Processing.JpegInformation" /> class is used to load and update the date and time a picture
    ///     was taken.
    /// </summary>
    public static class JpegInformation
    {
        /// <summary>
        ///     Defines the Id used to reference the Picture Taken value from a Jpeg meta data.
        /// </summary>
        private const int PictureTakenId = 0x9003;

        /// <summary>
        ///     Gets the picture taken.
        /// </summary>
        /// <param name="filePath">
        ///     The file path.
        /// </param>
        /// <returns>
        ///     A <see cref="DateTime" /> instance.
        /// </returns>
        public static DateTime GetPictureTaken(string filePath)
        {
            DateTime pictureTaken = default;

            // Check if the file exists
            if (File.Exists(filePath) == false)
            {
                return DateTime.Now;
            }

            try
            {
                string pictureTakenValue;

                var source = ImageFile.FromFile(filePath);

                var dateTakenProperty = source.Properties.FirstOrDefault(x => x.Tag == ExifTag.DateTimeOriginal);

                if (dateTakenProperty.Value is DateTime)
                {
                    pictureTaken = (DateTime) dateTakenProperty.Value;
                }
                else
                {
                    pictureTakenValue = dateTakenProperty.Value.ToString();

                    var dateCheck = new Regex(@"[0-9]{4}:[0-9]{2}:[0-9]{2}\s{1}[0-9]{2}:[0-9]{2}:[0-9]{2}");

                    // Check if the date value matches the expression
                    if (dateCheck.IsMatch(pictureTakenValue))
                    {
                        // Convert the date separators
                        pictureTakenValue = dateCheck.Match(pictureTakenValue).Value.Replace(" ", ":");

                        // Split the string using : as a delimiter
                        var textArray1 = pictureTakenValue.Split(':');

                        // Determine the month value
                        var monthValue = int.Parse(textArray1[1], CultureInfo.InvariantCulture) - 1;
                        var abbreviatedMonth =
                            CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[monthValue];

                        // Reconstruct the string
                        pictureTakenValue = string.Format(CultureInfo.CurrentCulture,
                            "{0}:{1}:{2} {3}/{4}/{5}",
                            textArray1[3],
                            textArray1[4],
                            textArray1[5],
                            textArray1[2],
                            abbreviatedMonth,
                            textArray1[0]);
                    }

                    if (DateTime.TryParse(pictureTakenValue, out pictureTaken) == false)
                    {
                        pictureTaken = default;
                    }
                }
            }
            catch (Exception)
            {
                // Fall through to default handling below
            }

            if (pictureTaken == default)
            {
                // The data stored in the image is not a valid date or no value is stored

                // Take the file created or last modified date as the picture taken date, whichever is earlier
                var fileDetails = new FileInfo(filePath);
                var creationTime = fileDetails.CreationTime;
                var lastWriteTime = fileDetails.LastWriteTimeUtc;

                if (creationTime < lastWriteTime)
                {
                    pictureTaken = creationTime;
                }
                else
                {
                    pictureTaken = lastWriteTime;
                }
            }

            // Return the date calculated
            return pictureTaken;
        }

        /// <summary>
        ///     Sets the picture taken.
        /// </summary>
        /// <param name="filePath">
        ///     The file path.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        public static void SetPictureTaken(string filePath, DateTime value)
        {
            // Exit if the file doesn't exist
            if (File.Exists(filePath) == false)
            {
                return;
            }

            // Create a temporary file
            var temporaryPath = Path.GetTempFileName();

            try
            {
                // Copy the file to the temporary location
                File.Copy(filePath, temporaryPath, true);

                using (var picture = Image.FromFile(temporaryPath))
                {
                    // Ensure that the image has properties
                    if (picture.PropertyItems.Length > 0)
                    {
                        PropertyItem propertyValue;

                        try
                        {
                            // Attempt to get the property value
                            propertyValue = picture.GetPropertyItem(0x9003);
                        }
                        catch (ArgumentException)
                        {
                            // Destory the Object should it exist
                            propertyValue = null;
                        }

                        // Check if the image had the property value
                        if (propertyValue == null)
                        {
                            // The image doesn't contain the property value
                            // Given that we can't instantiate a property, we will hijack an existing property
                            propertyValue = picture.PropertyItems[0];
                        }

                        // Get the date value as an array of bytes
                        var buffer1 =
                            Encoding.ASCII.GetBytes(value.ToString("yyyy:MM:dd HH:mm:ss",
                                CultureInfo.InvariantCulture));

                        // Assign the property values
                        propertyValue.Id = 0x9003;
                        propertyValue.Type = 2;
                        propertyValue.Len = buffer1.Length;
                        propertyValue.Value = buffer1;

                        // Assign the property value to the image
                        picture.SetPropertyItem(propertyValue);

                        // Save the image to the original path
                        picture.Save(filePath);
                    }
                }
            }
            finally
            {
                // Check if the temporary file path exists
                if (File.Exists(temporaryPath))
                {
                    // Delete the temporary file
                    File.Delete(temporaryPath);
                }
            }
        }
    }
}