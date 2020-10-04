using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Halloumi.Common.Helpers
{
    /// <summary>
    /// Helper class for serializing objects
    /// </summary>
    /// <typeparam name="T">The class to be serialized/deserialized</typeparam>
    public static class SerializationHelper<T>
    {
        #region Public Methods

        /// <summary>
        /// Serializes the item to an utf-8 XML string.
        /// </summary>
        /// <param name="item">The item to be serialized.</param>
        /// <returns>The XML for the object</returns>
        public static string ToXmlString(T item)
        {
            // deserialize to string builder
            var serializer = new XmlSerializer(typeof(T));
            var builder = new StringBuilder();
            using (var writer = new StringWriter(builder))
            {
                serializer.Serialize(writer, item);
            }

            // convert to utf-8
            builder.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "<?xml version=\"1.0\" encoding=\"utf-8\"?>");

            // remove standard xml schema information (for space/readability reasons)
            builder.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            return builder.ToString();
        }

        /// <summary>
        /// Deserializes the an object from an XML string.
        /// </summary>
        /// <param name="xml">The XML to deserialize.</param>
        /// <returns>The object deserialized from the XML</returns>
        public static T FromXmlString(string xml)
        {
            return FromXmlString(xml, false);
        }

        /// <summary>
        /// Forms the XML string.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="quietMode">
        /// If set to true, will return null rather than 
        /// raising an error if the XML cannot be serialized.
        /// </param>
        /// <returns>The object deserialized from the XML</returns>
        public static T FromXmlString(string xml, bool quietMode)
        {
            // convert xml string to object and return (ignores any errors)
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(xml))
            {
                if (quietMode)
                {
                    // if quiet mode, deserilaize in try catch block and return null on error
                    try
                    {
                        return (T)serializer.Deserialize(reader);
                    }
                    catch
                    {
                        // return null if error
                        return default(T);
                    }
                }
                else
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
        }

        /// <summary>
        /// Serializes to a file.
        /// </summary>
        /// <param name="entity">The object to serialize.</param>
        /// <param name="filename">The filename to save to.</param>
        public static void ToXmlFile(T entity, string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Create))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                serializer.Serialize(stream, entity);
            }
        }

        /// <summary>
        /// Deserializes from file.
        /// </summary>
        /// <param name="filename">The filename to load from.</param>
        /// <returns>An object deserialized from the file</returns>
        public static T FromXmlFile(string filename)
        {
            T result;
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            var xml = File.ReadAllText(filename);
            using (var reader = new StringReader(xml))
            {
                result = (T)serializer.Deserialize(reader);
            }
            return result;
        }

        #endregion
    }
}
