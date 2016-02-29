using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SpatialMaps
{
    public static class Helper
    {

        public static XDocument SerializeToXDoc<T>(this T source)
        {
            var result = new XDocument();
            var serializer = new XmlSerializer(source.GetType());
            using (var writer = result.CreateWriter())
            {
                serializer.Serialize(writer, source);
            }
            return result;
        }
        
        public static T DeserializeFromXml<T>(string xmlFileName)
        {
            using (TextReader reader = new StreamReader(xmlFileName))
            {
                var deserializer = new XmlSerializer(typeof(T));
                return (T)deserializer.Deserialize(reader);
            }
        }


    }
}
