// Decompiled with JetBrains decompiler
// Type: STAR.Enterprise.Trace.RawBlobParser
// Assembly: STAR.Enterprise.Trace, Version=1.1.5697.29972, Culture=neutral, PublicKeyToken=null
// MVID: 515909EF-777E-43BB-B206-39FDB6D66C81
// Assembly location: C:\Git\Repos\CapitalMarkets\Solutions\packages\STAR.Enterprise.Trace.1.1.0\lib\net451\STAR.Enterprise.Trace.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace STAR.Enterprise.Trace
{
    internal static class RawBlobParser
    {
        public static ICollection<KeyValuePair<string, object>> ParseItems(IList<object> blobs)
        {
            if (blobs == null)
                return (ICollection<KeyValuePair<string, object>>)null;
            List<KeyValuePair<string, object>> keyValuePairList = (List<KeyValuePair<string, object>>)null;
            foreach (object blob in (IEnumerable<object>)blobs)
            {
                if (blob != null)
                {
                    IEnumerable<KeyValuePair<string, object>> keyValuePairs = RawBlobParser.ParseItem(blob);
                    if (keyValuePairs != null)
                    {
                        foreach (KeyValuePair<string, object> keyValuePair in keyValuePairs)
                        {
                            if (keyValuePairList == null)
                                keyValuePairList = new List<KeyValuePair<string, object>>();
                            keyValuePairList.Add(keyValuePair);
                        }
                    }
                }
            }
            return (ICollection<KeyValuePair<string, object>>)keyValuePairList;
        }

        public static IEnumerable<KeyValuePair<string, object>> ParseItem(object item)
        {
            if (item == null)
                return (IEnumerable<KeyValuePair<string, object>>)null;
            IDictionary<string, object> dictionary = item as IDictionary<string, object>;
            if (dictionary != null)
                return (IEnumerable<KeyValuePair<string, object>>)dictionary;
            Type type = item.GetType();
            if (RawBlobParser.IsAnonymousType(type))
                return ((IEnumerable<PropertyInfo>)item.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty)).Where<PropertyInfo>((Func<PropertyInfo, bool>)(propertyInfo => !((IEnumerable<ParameterInfo>)propertyInfo.GetIndexParameters()).Any<ParameterInfo>())).Select(propertyInfo =>
                {
                    PropertyInfo propertyInfo1 = propertyInfo;
                    object obj1 = item;
                    // ISSUE: variable of the null type
                    object local = null;
                    object obj2 = propertyInfo1.GetValue(obj1, (object[])local);
                    return new
                    {
                        propertyInfo = propertyInfo1,
                        unwrappedItem = obj2
                    };
                }).Where(param0 => param0.unwrappedItem != null).Select(param0 => new KeyValuePair<string, object>(param0.propertyInfo.Name, param0.unwrappedItem));
            return (IEnumerable<KeyValuePair<string, object>>)new KeyValuePair<string, object>[1]
            {
        new KeyValuePair<string, object>(type.Name, item)
            };
        }

        private static bool IsAnonymousType(Type type)
        {
            if (type == (Type)null || !Attribute.IsDefined((MemberInfo)type, typeof(CompilerGeneratedAttribute), false) || (!type.IsGenericType || !type.Name.Contains("AnonymousType")) || !type.Name.StartsWith("<>") && !type.Name.StartsWith("VB$"))
                return false;
            return type.Attributes.HasFlag((Enum)TypeAttributes.NotPublic);
        }
    }
}
