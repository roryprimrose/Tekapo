namespace Tekapo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Xml.Serialization;
    using Tekapo.Properties;

    /// <summary>
    ///     The <see cref="Helper" />
    ///     class is used to provide common functionality.
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        ///     Stores the supported file types.
        /// </summary>
        private static List<string> _supportedFileTypes;

        /// <summary>
        ///     Deserializes the list.
        /// </summary>
        /// <param name="serializedValue">
        ///     The serialized value.
        /// </param>
        /// <returns>
        ///     A <see cref="string" /> instance.
        /// </returns>
        public static BindingList<string> DeserializeList(string serializedValue)
        {
            // check if there is a value
            if (string.IsNullOrEmpty(serializedValue))
            {
                return new BindingList<string>();
            }

            using (var reader = new StringReader(serializedValue))
            {
                // Create an instance of the XmlSerializer class;
                // specify the type of Object to be deserialized.
                var serializer = new XmlSerializer(typeof(BindingList<string>));

                return (BindingList<string>) serializer.Deserialize(reader);
            }
        }

        /// <summary>
        ///     Gets the file extension.
        /// </summary>
        /// <param name="path">
        ///     The path to process.
        /// </param>
        /// <returns>
        ///     A <see cref="string" /> instance.
        /// </returns>
        public static string GetFileExtension(string path)
        {
            // Get the extension of the path
            var extension = Path.GetExtension(path);

            if (string.IsNullOrEmpty(extension))
            {
                return string.Empty;
            }

            return extension.ToLower(CultureInfo.CurrentCulture).Substring(1);
        }

        /// <summary>
        ///     Gets the supported file types.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> instance.
        /// </returns>
        public static List<string> GetSupportedFileTypes()
        {
            // Check if the file types have already been calculated
            if (_supportedFileTypes != null)
            {
                return _supportedFileTypes;
            }

            var supportedTypesValue = Settings.Default.SupportedFileTypes;

            // Ensure that there is a value
            if (string.IsNullOrEmpty(supportedTypesValue))
            {
                // Set the default value
                supportedTypesValue = Resources.DefaultSupportedFileTypes;
            }

            supportedTypesValue = supportedTypesValue.ToLower(CultureInfo.CurrentCulture);

            // Load the supported file types
            _supportedFileTypes = new List<string>(supportedTypesValue.Split(','));

            // Loop through each file type
            for (var typeIndex = 0; typeIndex < _supportedFileTypes.Count; typeIndex++)
            {
                // Ensure that the file type doesn't start with .
                if (_supportedFileTypes[typeIndex].StartsWith(".", StringComparison.OrdinalIgnoreCase))
                {
                    // Strip the leading . character
                    _supportedFileTypes[typeIndex] = _supportedFileTypes[typeIndex].Substring(1);
                }
            }

            // Return the supported types
            return _supportedFileTypes;
        }

        /// <summary>
        ///     Determines whether [is file supported] [the specified path].
        /// </summary>
        /// <param name="path">
        ///     The path to test.
        /// </param>
        /// <returns>
        ///     <c>true</c>if [is file supported] [the specified path]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFileSupported(string path)
        {
            var extension = GetFileExtension(path);

            return GetSupportedFileTypes().Contains(extension);
        }

        /// <summary>
        ///     Serializes the list.
        /// </summary>
        /// <param name="list">
        ///     The list to serialize.
        /// </param>
        /// <returns>
        ///     A <see cref="string" /> instance.
        /// </returns>
        public static string SerializeList(BindingList<string> list)
        {
            // Check if there is a list
            if (list == null)
            {
                return string.Empty;
            }

            string serializedValue;

            // Create an instance of the XmlSerializer class;
            // specify the type of Object to serialize.
            var serializer = new XmlSerializer(typeof(BindingList<string>));

            using (var stream = new MemoryStream())
            {
                // Serialize the purchase order, and close the TextWriter.
                serializer.Serialize(stream, list);

                stream.Position = 0;

                using (TextReader reader = new StreamReader(stream))
                {
                    serializedValue = reader.ReadToEnd();

                    reader.Close();
                }

                stream.Close();
            }

            return serializedValue;
        }
    }
}