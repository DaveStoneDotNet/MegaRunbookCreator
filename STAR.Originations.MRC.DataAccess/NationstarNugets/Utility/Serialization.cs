// Decompiled with JetBrains decompiler
// Type: STAR.Framework.Utility.Serialization
// Assembly: STAR.Framework.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09c4f98813f6b683
// MVID: C0759794-9AB4-40FB-8B71-6E464E708E5C
// Assembly location: C:\Git\Repos\CapitalMarkets\Solutions\packages\STAR.Framework.Utility.1.0.0\lib\net40\STAR.Framework.Utility.dll

using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace STAR.Framework.Utility
{
    public static class Serialization
    {
        /// <summary>
        /// Gets the encoding from the 'SerializationEncoding' key in the app.config file.
        /// Returns 'ASCII' by default if the appp.config value does not exists or fails to
        /// parse successfully.
        /// </summary>
        /// <value>The encoding.</value>
        private static System.Text.Encoding Encoding
        {
            get
            {
                System.Text.Encoding encoding = System.Text.Encoding.ASCII;
                try
                {
                    if (ConfigurationManager.AppSettings["SerializationEncoding"] != null)
                    {
                        switch ((EncodingTypes)Enum.Parse(typeof(EncodingTypes), ConfigurationManager.AppSettings["SerializationEncoding"]))
                        {
                            case EncodingTypes.ASCII:
                                encoding = System.Text.Encoding.ASCII;
                                break;
                            case EncodingTypes.BigEndianUnicode:
                                encoding = System.Text.Encoding.BigEndianUnicode;
                                break;
                            case EncodingTypes.Unicode:
                                encoding = System.Text.Encoding.Unicode;
                                break;
                            case EncodingTypes.UTF7:
                                encoding = System.Text.Encoding.UTF7;
                                break;
                            case EncodingTypes.UTF8:
                                encoding = System.Text.Encoding.UTF8;
                                break;
                            case EncodingTypes.UTF32:
                                encoding = System.Text.Encoding.UTF32;
                                break;
                        }
                    }
                }
                catch (ArgumentException ex)
                {
                }
                return encoding;
            }
        }

        /// <summary>
        /// Utilize app.config 'SerializationEncoding' value to serialize an object to an xml string.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="throwExceptions"></param>
        /// <returns></returns>
        /// <remarks>
        /// The System.Text.Encoding used is base on the value of the 'SerializationEncoding' key
        /// in the app.config file. If the 'SerializationEncoding' key does not exist or fails to
        /// successfully parse, 'System.Text.Encoding.ASCII' is used by default.
        /// </remarks>
        public static string SerializeToXml(object instance, bool throwExceptions)
        {
            return STAR.Framework.Utility.Serialization.SerializeToXml(instance, STAR.Framework.Utility.Serialization.Encoding, throwExceptions);
        }

        /// <summary>
        /// Utilize app.config 'SerializationEncoding' value to deserialize an object to an xml string.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="type">The type.</param>
        /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <returns></returns>
        /// <remarks>
        /// The System.Text.Encoding used is base on the value of the 'SerializationEncoding' key
        /// in the app.config file. If the 'SerializationEncoding' key does not exist or fails to
        /// successfully parse, 'System.Text.Encoding.ASCII' is used by default.
        /// </remarks>
        public static object DeserializeToObject(string xml, Type type, bool throwExceptions)
        {
            return STAR.Framework.Utility.Serialization.DeserializeToObject(xml, type, STAR.Framework.Utility.Serialization.Encoding, throwExceptions);
        }

        /// <summary>
        /// Serialize an object to an xml string and optionally throws or ignores exceptions.
        /// </summary>
        /// <param name="instance">The object to serialize.</param>
        /// <param name="encoding">The System.Text.Encoding.</param>
        /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <returns></returns>
        /// <remarks>
        /// 
        /// </remarks>
        public static string SerializeToXml(object instance, System.Text.Encoding encoding, bool throwExceptions)
        {
            string str = string.Empty;
            try
            {
                if (instance != null)
                {
                    if (encoding != null)
                    {
                        using (MemoryStream memoryStream1 = new MemoryStream())
                        {
                            XmlSerializer xmlSerializer = new XmlSerializer(instance.GetType());
                            XmlTextWriter xmlTextWriter1 = new XmlTextWriter((Stream)memoryStream1, encoding);
                            XmlTextWriter xmlTextWriter2 = xmlTextWriter1;
                            object o = instance;
                            xmlSerializer.Serialize((XmlWriter)xmlTextWriter2, o);
                            using (MemoryStream memoryStream2 = (MemoryStream)xmlTextWriter1.BaseStream)
                                str = encoding.GetString(memoryStream2.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (throwExceptions)
                    throw new ArgumentException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Could not serialize the object. {0}", new object[1]
                    {
            (object) ex.Message
                    }));
            }
            return str;
        }

        /// <summary>Deserializes the XML to an object.</summary>
        /// <param name="xml">The XML.</param>
        /// <param name="type">The type.</param>
        /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <returns></returns>
        /// <remarks>
        /// 
        /// </remarks>
        public static object DeserializeToObject(string xml, Type type, System.Text.Encoding encoding, bool throwExceptions)
        {
            object obj = (object)null;
            try
            {
                if (!string.IsNullOrEmpty(xml))
                {
                    if (type != (Type)null)
                    {
                        if (encoding != null)
                        {
                            using (MemoryStream memoryStream = new MemoryStream(encoding.GetBytes(xml)))
                                obj = new XmlSerializer(type).Deserialize((Stream)memoryStream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (throwExceptions)
                    throw new ArgumentException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Could not deserialize the object. {0}", new object[1]
                    {
            (object) ex.Message
                    }));
            }
            return obj;
        }

        /// <summary>Clones the specified object without exceptions.</summary>
        /// <param name="objectToClone">The object to clone.</param>
        /// <returns></returns>
        public static object Clone(object instance)
        {
            return STAR.Framework.Utility.Serialization.Clone(instance, false);
        }

        /// <summary>Clones the specified object.</summary>
        /// <param name="instance">The object to clone.</param>
        /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <returns></returns>
        public static object Clone(object instance, bool throwExceptions)
        {
            if (instance != null)
                return STAR.Framework.Utility.Serialization.DeserializeToObject(STAR.Framework.Utility.Serialization.SerializeToXml(instance, false), instance.GetType(), throwExceptions);
            return instance;
        }

        /// <summary>Serializes an object to a Xml string</summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="myObject">Object to serialize</param>
        /// <returns></returns>
        [Obsolete("Call the SerializationUtility.SerializeObjectToXml() method instead.")]
        public static string ToXml<T>(T myObject)
        {
            string str = string.Empty;
            using (MemoryStream memoryStream1 = new MemoryStream())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(myObject.GetType());
                XmlTextWriter xmlTextWriter1 = new XmlTextWriter((Stream)memoryStream1, System.Text.Encoding.UTF8);
                XmlTextWriter xmlTextWriter2 = xmlTextWriter1;
                // ISSUE: variable of a boxed type
                var local = (object)myObject;
                xmlSerializer.Serialize((XmlWriter)xmlTextWriter2, (object)local);
                using (MemoryStream memoryStream2 = (MemoryStream)xmlTextWriter1.BaseStream)
                    return new UTF8Encoding().GetString(memoryStream2.ToArray());
            }
        }

        /// <summary>
        /// Serializes a WCF 'DataContract' to an XML string without throwing exceptions upon failures. See detailed remarks for
        /// comparsion to 'STAR.Framework.Serialization.SerializeToXml'.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        /// <remarks>
        /// WCF uses the 'System.Runtime.Serialization.DataContractSerializer', as opposed to the 'System.Xml.XmlTextWriter'.
        /// This distinction may be important when, for example, attempting to log 'Entity Framework' generated entities
        /// served over WCF. Auto-generated EF entities implement 'IDictionary' in its change tracking by default. 'IDictionary'
        /// cannot be serialized using the 'System.Xml.Serialization.XmlSerializer'. The exception reported is 'Cannot serialize
        /// member because it implements IDictionary.' You get this error because a 'Dictionary' isn't 'IXmlSerializable'. This is
        /// due to layering concerns: 'Dictionary' is in the 'mscorlib.dll' but 'IXmlSerializable' is in 'System.Xml.dll', and there
        /// can't be a dependency in that direction. Before .NET 3.0, you'd have to grow your own solution, such as sub-classing
        /// 'Dictionary' to implement 'IXmlSerializable'. Moving forward, the recommended solution is to use
        /// 'System.Runtime.Serialization.DataContractSerializer'. Note this is part of WCF in .NET 3.0. For more details on
        /// what is serializable using the data contract serializer, see: http://msdn2.microsoft.com/en-us/library/ms731923.aspx.
        /// </remarks>
        public static string SerializeDataContractToXml(object instance)
        {
            return STAR.Framework.Utility.Serialization.SerializeDataContractToXml(instance, false);
        }

        /// <summary>
        /// Serializes a WCF 'DataContract' to an XML string. See detailed remarks for comparsion to 'STAR.Framework.Serialization.SerializeToXml'.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <returns></returns>
        /// <remarks>
        /// WCF uses the 'System.Runtime.Serialization.DataContractSerializer', as opposed to the 'System.Xml.Serialization.XmlSerializer'.
        /// This distinction may be important when, for example, attempting to log 'Entity Framework' generated entities
        /// served over WCF. Auto-generated EF entities implement 'IDictionary' in its change tracking by default. 'IDictionary'
        /// cannot be serialized using the 'System.Xml.Serialization.XmlSerializer'. The exception reported is 'Cannot serialize member
        /// because it implements IDictionary.' You get this error because a 'Dictionary' isn't 'IXmlSerializable'. This is due to layering
        /// concerns: 'Dictionary' is in the 'mscorlib.dll' but 'IXmlSerializable' is in 'System.Xml.dll', and there can't be a dependency in
        /// that direction. Before .NET 3.0, you'd have to grow your own solution, such as sub-classing 'Dictionary' to implement 'IXmlSerializable'.
        /// Moving forward, the recommended solution is to use 'System.Runtime.Serialization.DataContractSerializer'. Note this is part of WCF
        /// in .NET 3.0. For more details on what is serializable using the data contract serializer, see: http://msdn2.microsoft.com/en-us/library/ms731923.aspx.
        /// </remarks>
        public static string SerializeDataContractToXml(object instance, bool throwExceptions)
        {
            string str = string.Empty;
            try
            {
                using (StringWriter stringWriter = new StringWriter())
                {
                    using (XmlTextWriter xmlTextWriter = new XmlTextWriter((TextWriter)stringWriter))
                    {
                        new DataContractSerializer(instance.GetType()).WriteObject((XmlWriter)xmlTextWriter, instance);
                        str = stringWriter.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                if (throwExceptions)
                    throw new ArgumentException(string.Format("Could not serialize the object. {0}", (object)ex.Message));
            }
            return str;
        }

        /// <summary>
        /// Deserializes an XML string to a WCF DataContract without throwing exceptions upon failures. See detailed remarks for
        /// comparsion to 'STAR.Framework.Serialization.DeserializeToObject'.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <remarks>
        /// WCF uses the 'System.Runtime.Serialization.DataContractSerializer', as opposed to the 'System.Xml.XmlTextWriter'.
        /// This distinction may be important when, for example, attempting to log 'Entity Framework' generated entities
        /// served over WCF. Auto-generated EF entities implement 'IDictionary' in its change tracking by default. 'IDictionary'
        /// cannot be serialized using the 'System.Xml.XmlTextWriter'. The exception reported is 'Cannot serialize member because it
        /// implements IDictionary.' You get this error because a 'Dictionary' isn't 'IXmlSerializable'. This is due to layering
        /// concerns: 'Dictionary' is in the 'mscorlib.dll' but 'IXmlSerializable' is in 'System.Xml.dll', and there can't
        /// be a dependency in that direction. Before .NET 3.0, you'd have to grow your own solution, such as sub-classing
        /// 'Dictionary' to implement 'IXmlSerializable'. Moving forward, the recommended solution is to use
        /// 'System.Runtime.Serialization.DataContractSerializer'. Note this is part of WCF in .NET 3.0. For more details on
        /// what is serializable using the data contract serializer, see: http://msdn2.microsoft.com/en-us/library/ms731923.aspx.
        /// </remarks>
        public static object DeserializeToDataContract(string xml, Type type)
        {
            return STAR.Framework.Utility.Serialization.DeserializeToDataContract(xml, type, false);
        }

        /// <summary>
        /// Deserializes an XML string to a WCF DataContract. See detailed remarks for comparsion to 'STAR.Framework.Serialization.DeserializeToObject'.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="type">The type.</param>
        /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <returns></returns>
        /// <remarks>
        /// WCF uses the 'System.Runtime.Serialization.DataContractSerializer', as opposed to the 'System.Xml.XmlTextWriter'.
        /// This distinction may be important when, for example, attempting to log 'Entity Framework' generated entities
        /// served over WCF. Auto-generated EF entities implement 'IDictionary' in its change tracking by default. 'IDictionary'
        /// cannot be serialized using the 'System.Xml.XmlTextWriter'. The exception reported is 'Cannot serialize member because it
        /// implements IDictionary.' You get this error because a 'Dictionary' isn't 'IXmlSerializable'. This is due to layering
        /// concerns: 'Dictionary' is in the 'mscorlib.dll' but 'IXmlSerializable' is in 'System.Xml.dll', and there can't
        /// be a dependency in that direction. Before .NET 3.0, you'd have to grow your own solution, such as sub-classing
        /// 'Dictionary' to implement 'IXmlSerializable'. Moving forward, the recommended solution is to use
        /// 'System.Runtime.Serialization.DataContractSerializer'. Note this is part of WCF in .NET 3.0. For more details on
        /// what is serializable using the data contract serializer, see: http://msdn2.microsoft.com/en-us/library/ms731923.aspx.
        /// </remarks>
        public static object DeserializeToDataContract(string xml, Type type, bool throwExceptions)
        {
            object obj = (object)null;
            try
            {
                if (!string.IsNullOrEmpty(xml))
                {
                    if (type != (Type)null)
                    {
                        using (MemoryStream memoryStream = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(xml)))
                        {
                            using (XmlDictionaryReader textReader = XmlDictionaryReader.CreateTextReader((Stream)memoryStream, XmlDictionaryReaderQuotas.Max))
                                obj = new DataContractSerializer(type).ReadObject(textReader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (throwExceptions)
                    throw new ArgumentException(string.Format("Could not deserialize the object. {0}", (object)ex.Message));
            }
            return obj;
        }
    }
}
