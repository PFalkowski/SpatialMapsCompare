using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SpatialMapsCompare
{
    public static class Helper
    {
        public static XDocument SerializeToXDoc<T>(this T source)
            where T : new()
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
            where T : new()
        {
            T result = new T();
            var serializer = new XmlSerializer(result.GetType());
            using (var reader = new FileStream(xmlFileName, FileMode.Open))
            {
                serializer.Deserialize(reader);
            }
            return result;
        }
    }
}
