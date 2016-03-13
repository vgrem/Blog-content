using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace SharePoint.NintexDeployment.Utilities
{

    /// <summary>
    /// Serializer wrapper
    /// </summary>
    public static class Serializer
    {


        /// <summary>
        /// Deserialize String into Type 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                XmlSerializer serializer = SerializerCache.GetSerializer(typeof(T));
                XmlTextReader reader = new XmlTextReader(new StringReader(s));
                return (T)serializer.Deserialize(reader);
            }
            return default(T);
        }



        /// <summary>
        /// Serialize Object To String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObjectString<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializer = SerializerCache.GetSerializer(typeof(T));
                serializer.Serialize(stream, obj);
                stream.Position = 0;
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                return Encoding.UTF8.GetString(bytes);
            }
        }



    }



    public static class SerializerCache
    {

        private static Dictionary<string, XmlSerializer> _serializers;
        private static readonly object _syncRoot = new object();


        public static XmlSerializer GetSerializer(Type type)
        {
            return GetSerializer(type, "", delegate
            {
                return new XmlSerializer(type);
            });
        }

        public static XmlSerializer GetSerializer(Type type, string customKey, SerializerFactory factory)
        {
            string key = customKey + type.AssemblyQualifiedName;
            if (Serializers.ContainsKey(key))
            {
                return Serializers[key];
            }
            XmlSerializer serializer = factory();
            if (Serializers.ContainsKey(key))
            {
                return Serializers[key];
            }
            lock (_syncRoot)
            {
                Serializers.Add(key, serializer);
            }
            return serializer;
        }


        private static Dictionary<string, XmlSerializer> Serializers
        {
            get
            {
                lock (_syncRoot)
                {
                    return (_serializers = _serializers ?? new Dictionary<string, XmlSerializer>());
                }
            }
        }


        public delegate XmlSerializer SerializerFactory();
    }
}
