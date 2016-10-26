// Decompiled with JetBrains decompiler
// Type: STAR.Enterprise.Trace.Serializer
// Assembly: STAR.Enterprise.Trace, Version=1.1.5697.29972, Culture=neutral, PublicKeyToken=null
// MVID: 515909EF-777E-43BB-B206-39FDB6D66C81
// Assembly location: C:\Git\Repos\CapitalMarkets\Solutions\packages\STAR.Enterprise.Trace.1.1.0\lib\net451\STAR.Enterprise.Trace.dll

using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace STAR.Enterprise.Trace
{
    internal class Serializer
    {
        public static string Serialize(object instance)
        {
            if (instance == null)
                return (string)null;
            if (!typeof(INotifyPropertyChanged).IsAssignableFrom(instance.GetType()))
                return Serializer.SerializeToXml(instance);
            return Serializer.SerializeDataContractToXml(instance);
        }

        private static string SerializeDataContractToXml(object instance)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter((TextWriter)stringWriter))
                {
                    new DataContractSerializer(instance.GetType()).WriteObject((XmlWriter)xmlTextWriter, instance);
                    xmlTextWriter.Flush();
                    stringWriter.Flush();
                    return stringWriter.ToString();
                }
            }
        }

        private static string SerializeToXml(object instance)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter((TextWriter)stringWriter))
                {
                    new XmlSerializer(instance.GetType()).Serialize((XmlWriter)xmlTextWriter, instance);
                    xmlTextWriter.Flush();
                    stringWriter.Flush();
                    return stringWriter.ToString();
                }
            }
        }
    }
}
