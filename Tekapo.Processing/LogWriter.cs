using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Tekapo.Processing
{
    /// <summary>
    /// The <see cref="Tekapo.Processing.LogWriter"/> class is used to convert log information into an html report.
    /// </summary>
    public static class LogWriter
    {
        /// <summary>
        /// Saves the results log.
        /// </summary>
        /// <param name="document">
        /// The document.
        /// </param>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        public static void SaveResultsLog(Results document, String filePath)
        {
            String xml = ConvertResultsToXml(document);

            // Transform the xml
            String html = TransformXmlToHtml(xml);

            // Save the html to the file path
            File.WriteAllText(filePath, html);
        }

        /// <summary>
        /// Converts the results to XML.
        /// </summary>
        /// <param name="document">
        /// The document.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static String ConvertResultsToXml(Results document)
        {
            String xml;

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    // Serialize the document
                    XmlSerializer serializer = new XmlSerializer(typeof(Results));
                    serializer.Serialize(writer, document);

                    using (TextReader reader = new StreamReader(stream))
                    {
                        // Read the xml and close the reader
                        stream.Position = 0;
                        xml = reader.ReadToEnd();
                        reader.Close();
                    }

                    // Close the writer and the stream
                    writer.Close();
                    stream.Close();
                }
            }

            return xml;
        }

        /// <summary>
        /// Transforms the XML to HTML.
        /// </summary>
        /// <param name="xml">
        /// The XML to convert.
        /// </param>
        /// <returns>
        /// A <see cref="String"/> instance.
        /// </returns>
        private static String TransformXmlToHtml(String xml)
        {
            String xslt = Properties.Resources.ResultsLog;
            String html;

            XmlDocument xsltDocument = new XmlDocument();
            xsltDocument.LoadXml(xslt);

            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    XslCompiledTransform transform = new XslCompiledTransform();
                    transform.Load(xsltDocument);
                    transform.Transform(document, null, writer);

                    using (TextReader reader = new StreamReader(stream))
                    {
                        stream.Position = 0;
                        html = reader.ReadToEnd();
                        reader.Close();
                    }

                    // Close the writer and the stream
                    stream.Close();
                    writer.Close();
                }
            }

            return html;
        }
    }
}