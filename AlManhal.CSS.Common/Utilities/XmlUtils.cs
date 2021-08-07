using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace System
{
    public static class XmlUtils
    {
        #region Methods

        public static string WriteObjectToXmlFile<T>(T obj, string path)
        {
            XmlWriterSettings xmlWriterSettings = new System.Xml.XmlWriterSettings()
            {
                CloseOutput = false,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false,
                Indent = true
            };

            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());

            using (Stream stream = new FileStream(path, FileMode.OpenOrCreate))
            using (XmlWriter xmlWriter = System.Xml.XmlWriter.Create(stream, xmlWriterSettings))
            {
                xmlSerializer.Serialize(xmlWriter, obj);
            }




            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(stringwriter, obj);
            return stringwriter.ToString();
        }

        #endregion
    }
}
