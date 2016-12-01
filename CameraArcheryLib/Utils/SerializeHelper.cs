using System.IO;
using System.Xml.Serialization;

namespace CameraArcheryLib.Utils
{
    /// <summary>
    ///  helper to serialize and deserialize object in xml file
    /// </summary>
    public static class SerializeHelper
    {
        /// <summary>
        /// function to serialize object
        /// </summary>
        /// <typeparam name="T">type of the object to be serialize</typeparam>
        /// <param name="objet">object to be serialize</param>
        /// <param name="filePath">path of the file</param>
        public static void Serialization<T>(T objet, string filePath)
		{
            XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (TextWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, objet);
                }
		}

        /// <summary>
        /// Deserialize xml file
        /// </summary>
        /// <typeparam name="T"> type of the object</typeparam>
        /// <param name="filePath">path of the file to deserialize</param>
        /// <returns>object deserialized</returns>
        public static T Deserialization<T>( string filePath)
        {
            T obj = default(T);

            using(TextReader reader = new StreamReader(filePath))
            {
                if (!File.Exists(filePath))
                    return default(T);

                XmlSerializer deserializer = new XmlSerializer(typeof(T));
            
                 obj = (T)deserializer.Deserialize(reader);
            }

            return obj;
        }
    }
}
