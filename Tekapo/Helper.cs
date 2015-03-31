using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace Tekapo
{
    /// <summary>
    /// The <see cref="Helper"/>
    /// class is used to provide common functionality.
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Stores the supported file types.
        /// </summary>
        private static List<String> _supportedFileTypes;

        /// <summary>
        /// Deserializes the list.
        /// </summary>
        /// <param name="serializedValue">
        /// The serialized value.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        public static BindingList<String> DeserializeList(String serializedValue)
        {
            // check if there is a value
            if (String.IsNullOrEmpty(serializedValue))
            {
                return new BindingList<String>();
            }

            using (StringReader reader = new StringReader(serializedValue))
            {
                // Create an instance of the XmlSerializer class;
                // specify the type of Object to be deserialized.
                XmlSerializer serializer = new XmlSerializer(typeof(BindingList<String>));

                return (BindingList<String>)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        /// <param name="path">
        /// The path to process.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        public static String GetFileExtension(String path)
        {
            // Get the extension of the path
            String extension = Path.GetExtension(path);

            if (String.IsNullOrEmpty(extension))
            {
                return String.Empty;
            }

            return extension.ToLower(CultureInfo.CurrentCulture).Substring(1);
        }

        /// <summary>
        /// Gets the supported file types.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        public static List<String> GetSupportedFileTypes()
        {
            // Check if the file types have already been calculated
            if (_supportedFileTypes != null)
            {
                return _supportedFileTypes;
            }

            String supportedTypesValue = Properties.Settings.Default.SupportedFileTypes;

            // Ensure that there is a value
            if (String.IsNullOrEmpty(supportedTypesValue))
            {
                // Set the default value
                supportedTypesValue = Properties.Resources.DefaultSupportedFileTypes;
            }

            supportedTypesValue = supportedTypesValue.ToLower(CultureInfo.CurrentCulture);

            // Load the supported file types
            _supportedFileTypes = new List<String>(
                supportedTypesValue.Split(
                    new[]
                        {
                            ','
                        }));

            // Loop through each file type
            for (Int32 typeIndex = 0; typeIndex < _supportedFileTypes.Count; typeIndex++)
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
        /// Determines whether [is file supported] [the specified path].
        /// </summary>
        /// <param name="path">
        /// The path to test.
        /// </param>
        /// <returns>
        /// <c>true</c>if [is file supported] [the specified path]; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsFileSupported(String path)
        {
            String extension = GetFileExtension(path);

            return GetSupportedFileTypes().Contains(extension);
        }

        /// <summary>
        /// Serializes the list.
        /// </summary>
        /// <param name="list">
        /// The list to serialize.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        public static String SerializeList(BindingList<String> list)
        {
            // Check if there is a list
            if (list == null)
            {
                return String.Empty;
            }

            String serializedValue;

            // Create an instance of the XmlSerializer class;
            // specify the type of Object to serialize.
            XmlSerializer serializer = new XmlSerializer(typeof(BindingList<String>));

            using (MemoryStream stream = new MemoryStream())
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