using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Tekapo.Processing
{
    /// <summary>
    /// The <see cref="Tekapo.Processing.JpegInformation"/> class is used to load and update the date and time a picture was taken.
    /// </summary>
    public static class JpegInformation
    {
        /// <summary>
        /// Defines the Id used to reference the Picture Taken value from a Jpeg meta data.
        /// </summary>
        private const Int32 PictureTakenId = 0x9003;

        /// <summary>
        /// Gets the picture taken.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> instance.
        /// </returns>
        public static DateTime GetPictureTaken(String filePath)
        {
            DateTime pictureTaken = DateTime.Now;

            // Check if the file exists
            if (File.Exists(filePath) == false)
            {
                return pictureTaken;
            }

            try
            {
                String pictureTakenValue;

                // Load the image
                using (Image picture = Image.FromFile(filePath))
                {
                    // Get the picture taken date
                    PropertyItem propertyValue = picture.GetPropertyItem(PictureTakenId);
                    pictureTakenValue = Encoding.ASCII.GetString(propertyValue.Value);
                }

                Regex dateCheck = new Regex(@"[0-9]{4}:[0-9]{2}:[0-9]{2}\s{1}[0-9]{2}:[0-9]{2}:[0-9]{2}");

                // Check if the date value matches the expression
                if (dateCheck.IsMatch(pictureTakenValue))
                {
                    // Convert the date separators
                    pictureTakenValue = dateCheck.Match(pictureTakenValue).Value.Replace(" ", ":");

                    // Split the string using : as a delimiter
                    String[] textArray1 = pictureTakenValue.Split(
                        new[]
                            {
                                ':'
                            });

                    // Determine the month value
                    Int32 monthValue = Int32.Parse(textArray1[1], CultureInfo.InvariantCulture) - 1;
                    String abbreviatedMonth =
                        CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[monthValue];

                    // Reconstruct the string
                    pictureTakenValue = String.Format(
                        CultureInfo.CurrentCulture,
                        "{0}:{1}:{2} {3}/{4}/{5}",
                        textArray1[3],
                        textArray1[4],
                        textArray1[5],
                        textArray1[2],
                        abbreviatedMonth,
                        textArray1[0]);
                }

                // Check if a date can be correctly parsed from the string
                if (DateTime.TryParse(pictureTakenValue, out pictureTaken) == false)
                {
                    // The data stored in the image is not a valid date

                    // Take the file created date as the current date
                    FileInfo fileDetails = new FileInfo(filePath);
                    pictureTaken = fileDetails.CreationTime;
                }
            }
            catch (IOException)
            {
                // Default to the current date
                pictureTaken = DateTime.Now;
            }
            catch (ArgumentException)
            {
                // Default to the current date
                pictureTaken = DateTime.Now;
            }

            // Return the date calculated
            return pictureTaken;
        }

        /// <summary>
        /// Sets the picture taken.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void SetPictureTaken(String filePath, DateTime value)
        {
            // Exit if the file doesn't exist
            if (File.Exists(filePath) == false)
            {
                return;
            }

            // Create a temporary file
            String temporaryPath = Path.GetTempFileName();

            try
            {
                // Copy the file to the temporary location
                File.Copy(filePath, temporaryPath, true);

                using (Image picture = Image.FromFile(temporaryPath))
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
                        Byte[] buffer1 = Encoding.ASCII.GetBytes(value.ToString("yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture));

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