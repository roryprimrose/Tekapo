namespace Tekapo.Processing
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Xml.Xsl;
    using Tekapo.Processing.Properties;

    /// <summary>
    ///     The <see cref="Tekapo.Processing.LogWriter" /> class is used to convert log information into an html report.
    /// </summary>
    public static class LogWriter
    {
        /// <summary>
        ///     Saves the results log.
        /// </summary>
        /// <param name="document">
        ///     The document.
        /// </param>
        /// <param name="filePath">
        ///     The file path.
        /// </param>
        public static void SaveResultsLog(Results document, string filePath)
        {
            var xml = ConvertResultsToXml(document);

            // Transform the xml
            var html = TransformXmlToHtml(xml);

            // Save the html to the file path
            File.WriteAllText(filePath, html);
        }

        /// <summary>
        ///     Converts the results to XML.
        /// </summary>
        /// <param name="document">
        ///     The document.
        /// </param>
        /// <returns>
        ///     A <see cref="string" /> instance.
        /// </returns>
        [SuppressMessage("Microsoft.Usage",
            "CA2202:Do not dispose objects multiple times",
            Justification = "There shouldn't be an assumption here that the StreamReader will dispose the stream.")]
        private static string ConvertResultsToXml(Results document)
        {
            string xml;

            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    // Serialize the document
                    var serializer = new XmlSerializer(typeof(Results));
                    serializer.Serialize(writer, document);

                    using (TextReader reader = new StreamReader(stream))
                    {
                        // Read the xml and close the reader
                        stream.Position = 0;
                        xml = reader.ReadToEnd();
                    }
                }
            }

            return xml;
        }

        /// <summary>
        ///     Transforms the XML to HTML.
        /// </summary>
        /// <param name="xml">
        ///     The XML to convert.
        /// </param>
        /// <returns>
        ///     A <see cref="string" /> instance.
        /// </returns>
        [SuppressMessage("Microsoft.Usage",
            "CA2202:Do not dispose objects multiple times",
            Justification = "There shouldn't be an assumption here that the StreamReader will dispose the stream.")]
        private static string TransformXmlToHtml(string xml)
        {
            var xslt = Resources.ResultsLog;
            string html;

            var xsltDocument = new XmlDocument();
            xsltDocument.LoadXml(xslt);

            var document = new XmlDocument();
            document.LoadXml(xml);

            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    var transform = new XslCompiledTransform();
                    transform.Load(xsltDocument);
                    transform.Transform(document, null, writer);

                    using (TextReader reader = new StreamReader(stream))
                    {
                        stream.Position = 0;
                        html = reader.ReadToEnd();
                    }
                }
            }

            return html;
        }
    }
}