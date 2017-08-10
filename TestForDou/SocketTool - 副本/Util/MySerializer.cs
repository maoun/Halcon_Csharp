using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Collections;
using SocketTool.Core;
using System.Text;

namespace SocketTool
{
    public class MySerializer
    {      

        public static void Serialize<T>(T value, string xmlFileName)
        {
            if (value == null)
            {
                return;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UnicodeEncoding(false, false);
            settings.Indent = false;
            settings.OmitXmlDeclaration = false;
            FileStream fs = new FileStream(xmlFileName, FileMode
                .OpenOrCreate);

            serializer.Serialize(fs, value);
            fs.Close();
        }

        public static T Deserialize<T>(string xmlFileName)
        {
            if (string.IsNullOrEmpty(xmlFileName))
            {
                return default(T);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            //XmlSerializer serializer = new XmlSerializer(typeof(ArrayList));
            XmlReaderSettings settings = new XmlReaderSettings();
            //settings.

            FileStream fs = null;
            try
            {
                fs = new FileStream(xmlFileName, FileMode.Open);

                // Deserialize the content of the XML file to a Contact array 
                // utilizing XMLReader
                XmlReader reader = new XmlTextReader(fs);
                T contacts = (T)serializer.Deserialize(reader);

                return contacts;
            }
            catch (FileNotFoundException)
            {
                // Do nothing if the file does not exists
            }
            finally
            {
                if (fs != null) fs.Close();
            }

            return default(T);
        }
    }
}
