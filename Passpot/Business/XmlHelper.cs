using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace Passpot.Business
{
    public class XmlHelper
    {
       
        public static string InternalSerializer<T>(T t)
        {
            var xmlSerializer2 = new XmlSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Encoding = Encoding.UTF8;
            XmlWriter w = XmlWriter.Create(ms, ws);
            xmlSerializer2.Serialize(w, t);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(ms);
            // GET STRING 
            return reader.ReadToEnd();

        }

        public static T Deserialize<T>(string xmlstring)
        {
            if (string.IsNullOrEmpty(xmlstring))
            {
                return default(T);
            }
            using (var sr = new StringReader(xmlstring))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                using (var reader = XmlReader.Create(sr))
                {
                    return (T)xmlSerializer.Deserialize(reader);
                }
            }
        }





        //рабочая!!!
        //var xmlSerializer = new XmlSerializer(typeof(T));


        //MemoryStream ms = new MemoryStream();
        //using (var textWriter = new StringWriter())
        //{
        //    xmlSerializer.Serialize(textWriter, t);

        //    return textWriter.ToString();
        //}            




        //string xmlString = Encoding.UTF8.GetString(textWriter, 0, ms.ToArray().Length);

        //StringWriter writer = new StringWriter();
        //XmlSerializer serializer = new XmlSerializer(typeof(T));
        //serializer.Serialize(writer, t);
        //return writer.ToString();

        //MemoryStream ms = new MemoryStream();
        //ser.WriteObject(ms, t);
        //string jsonString = Encoding.UTF8.GetString(ms.ToArray(), 0, ms.ToArray().Length);
        //ms.Close();

        //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        //MemoryStream ms = new MemoryStream();
        //ser.WriteObject(ms, t);
        //string jsonString = Encoding.UTF8.GetString(ms.ToArray(), 0, ms.ToArray().Length);
        //ms.Close();
        //return jsonString;


        //var xmlSerializer2 = new XmlSerializer(typeof(T));
        //XmlWriterSettings settings = new XmlWriterSettings();

        // MemoryStream stream = new MemoryStream();
        ////XmlWriter xml = new XmlWriter(stream, new UTF8Encoding());

        // XmlWriterSettings ws = new XmlWriterSettings();


        // XmlWriter xml = XmlWriter.Create(stream, settings);
        // xmlSerializer2.Serialize(xml, t);
        //stream.Seek(0, SeekOrigin.Begin);
        //StreamReader reader = new StreamReader(stream);
        //// GET STRING 
        //string requestXML = reader.ReadToEnd();

        //public static string SerializeToString<T>(T t)
        //{
        //    return InternalSerializer<T>(T t);
        //}

        //public static string SerializeToString<T>(T content)
        //{
        //    return InternalSerializer(typeof(T), content);
        //}


        //public static object DeserializeFromString(Type type, string content)
        //{
        //    using (StringReader reader = new StringReader(content))
        //    {
        //        XmlSerializer serializer = new XmlSerializer(type);
        //        return serializer.Deserialize(reader);
        //    }
        //}

        //public static T DeserializeFromString<T>(string content)
        //{
        //    using (StringReader reader = new StringReader(content))
        //    {
        //        XmlSerializer serializer = new XmlSerializer(typeof(T));
        //        return (T)serializer.Deserialize(reader);
        //    }
        //}

    }
}
