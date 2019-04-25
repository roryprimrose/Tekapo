namespace Tekapo.Processing
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

    public class JpegMediaManager : IMediaManager
    {
        const int PictureTakenPropertyId = 0x9003;
        private static readonly Regex _dateTimeExpression = new Regex(@"[0-9]{4}:[0-9]{2}:[0-9]{2}\s{1}[0-9]{2}:[0-9]{2}:[0-9]{2}");

        public IEnumerable<string> GetSupportedFileTypes()
        {
            yield return ".jpg";
            yield return ".jpeg";
        }

        public bool IsSupported(Stream stream)
        {
            var image = ImageFile.FromStream(stream);

            if (image.Format == ImageFileFormat.JPEG)
            {
                return true;
            }

            return false;
        }

        public DateTime? ReadMediaCreatedDate(Stream stream)
        {
            var source = ImageFile.FromStream(stream);
            
            var dateTakenProperty = source.Properties?.FirstOrDefault(x => x.Tag == ExifTag.DateTimeOriginal);

            if (dateTakenProperty == null)
            {
                return null;
            }

            if (dateTakenProperty.Value is DateTime mediaCreatedDate)
            {
                return mediaCreatedDate;
            }

            var mediaCreatedValue = dateTakenProperty.Value.ToString();

            var match = _dateTimeExpression.Match(mediaCreatedValue);

            // Check if the date value matches the expression
            if (match.Success == false)
            {
                return null;
            }

            // Convert the date separators
            mediaCreatedValue = match.Value.Replace(" ", ":");

            // Split the string using : as a delimiter
            var parts = mediaCreatedValue.Split(':');

            // Determine the month value
            var monthValue = int.Parse(parts[1], CultureInfo.InvariantCulture) - 1;
            var abbreviatedMonth = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[monthValue];

            // Reconstruct the string
            mediaCreatedValue = string.Format(CultureInfo.CurrentCulture,
                "{0}:{1}:{2} {3}/{4}/{5}",
                parts[3],
                parts[4],
                parts[5],
                parts[2],
                abbreviatedMonth,
                parts[0]);

            if (DateTime.TryParse(mediaCreatedValue, out var mediaCreated))
            {
                return mediaCreated;
            }

            return null;
        }

        public Stream SetMediaCreatedDate(Stream stream, DateTime createdAt)
        {
            var picture = ImageFile.FromStream(stream);

            if (picture.Properties == null)
            {
                return null;
            }

            var exifProperty = picture.Properties.FirstOrDefault(x => x.Tag == ExifTag.DateTimeOriginal);

            if (exifProperty == null)
            {
                picture.Properties.Add(ExifTag.DateTimeOriginal, createdAt);
            }
            else
            {
                exifProperty.Value = createdAt;
            }

            var newStream = new MemoryStream();

            picture.Save(newStream);

            return newStream;
        }
    }
}