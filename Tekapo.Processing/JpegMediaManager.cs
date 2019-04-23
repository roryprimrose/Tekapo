﻿namespace Tekapo.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using ExifLibrary;

    public class JpegMediaManager : FileMediaManager
    {
        public override IEnumerable<string> GetSupportedFileTypes()
        {
            yield return ".jpg";
            yield return ".jpeg";
        }

        public override void SetMediaCreatedDate(string filePath, DateTime createdAt)
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
                            // Destroy the Object should it exist
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
                            Encoding.ASCII.GetBytes(createdAt.ToString("yyyy:MM:dd HH:mm:ss",
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

        protected override DateTime? ResolveMediaCreatedDate(string filePath)
        {
            var source = ImageFile.FromFile(filePath);

            var dateTakenProperty = source.Properties.FirstOrDefault(x => x.Tag == ExifTag.DateTimeOriginal);

            if (dateTakenProperty == null)
            {
                return null;
            }

            if (dateTakenProperty.Value is DateTime mediaCreatedDate)
            {
                return mediaCreatedDate;
            }

            var mediaCreatedValue = dateTakenProperty.Value.ToString();

            var dateCheck = new Regex(@"[0-9]{4}:[0-9]{2}:[0-9]{2}\s{1}[0-9]{2}:[0-9]{2}:[0-9]{2}");

            // Check if the date value matches the expression
            if (dateCheck.IsMatch(mediaCreatedValue))
            {
                // Convert the date separators
                mediaCreatedValue = dateCheck.Match(mediaCreatedValue).Value.Replace(" ", ":");

                // Split the string using : as a delimiter
                var textArray1 = mediaCreatedValue.Split(':');

                // Determine the month value
                var monthValue = int.Parse(textArray1[1], CultureInfo.InvariantCulture) - 1;
                var abbreviatedMonth = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[monthValue];

                // Reconstruct the string
                mediaCreatedValue = string.Format(CultureInfo.CurrentCulture,
                    "{0}:{1}:{2} {3}/{4}/{5}",
                    textArray1[3],
                    textArray1[4],
                    textArray1[5],
                    textArray1[2],
                    abbreviatedMonth,
                    textArray1[0]);
            }

            if (DateTime.TryParse(mediaCreatedValue, out var mediaCreated))
            {
                return mediaCreated;
            }

            return null;
        }
    }
}