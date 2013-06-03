using System.IO;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml.Serialization;

namespace WMPlayer.Web.Extensions
{
    public class Serializer
    {
        //---------------------------------
        // XML Methods
        //---------------------------------

        #region public static T FromXml<T>(string xml)

        public static T FromXml<T>(string xml)
        {
            // Return the type
            return FromXml<T>(Encoding.UTF8.GetBytes(xml));
        }

        #endregion

        #region public static T FromXml<T>(byte[] bytes)

        public static T FromXml<T>(byte[] bytes)
        {
            // Get a stream
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                // Return the type
                return FromXml<T>(stream);
            }
        }

        #endregion

        #region public static T FromXml<T>(Stream stream)

        public static T FromXml<T>(Stream stream)
        {
            // Create the serializer
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            // Return the settings
            return (T)serializer.Deserialize(stream);
        }

        #endregion

        #region public static string ToXml(object item)

        public static string ToXml(object item)
        {
            // Get the stream
            using (MemoryStream stream = new MemoryStream())
            {
                // Create the serializer
                XmlSerializer serializer = new XmlSerializer(item.GetType());

                // Serialize it
                serializer.Serialize(stream, item);

                // Return the item
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        #endregion

        //---------------------------------
        // JSON Methods
        //---------------------------------

        #region public static T FromJson<T>(string json)

        public static T FromJson<T>(string json)
        {
            // Create the converter
            JsonQueryStringConverter jsonConverter = new JsonQueryStringConverter();

            // Return the type
            return (T)jsonConverter.ConvertStringToValue(json, typeof(T));
        }

        #endregion

        #region public static T FromJson<T>(byte[] bytes)

        public static T FromJson<T>(byte[] bytes)
        {
            return FromJson<T>(Encoding.UTF8.GetString(bytes));
        }

        #endregion

        #region public static T FromJson<T>(Stream stream)

        public static T FromJson<T>(Stream stream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Copy the stream
                stream.CopyTo(memoryStream);

                // Return the array
                return FromJson<T>(memoryStream.ToArray());
            }
        }

        #endregion

        #region public static string ToJson(object item)

        public static string ToJson(object item)
        {
            // Create the converter
            JsonQueryStringConverter jsonConverter = new JsonQueryStringConverter();

            // Get the string
            return jsonConverter.ConvertValueToString(item, item.GetType());
        }

        #endregion
    }
}
