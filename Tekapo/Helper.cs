namespace Tekapo
{
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    ///     The <see cref="Helper" />
    ///     class is used to provide common functionality.
    /// </summary>
    internal static class Helper
    {
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
        ///     Serializes the list.
        /// </summary>
        /// <param name="list">
        ///     The list to serialize.
        /// </param>
        /// <returns>
        ///     A <see cref="string" /> instance.
        /// </returns>
        [SuppressMessage("Microsoft.Usage",
            "CA2202:Do not dispose objects multiple times",
            Justification = "There shouldn't be an assumption here that the StreamReader will dispose the stream.")]
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
                }
            }

            return serializedValue;
        }
    }
}