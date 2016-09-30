// Decompiled with JetBrains decompiler
// Type: STAR.Enterprise.Trace.TraceSerializer
// Assembly: STAR.Enterprise.Trace, Version=1.1.5697.29972, Culture=neutral, PublicKeyToken=null
// MVID: 515909EF-777E-43BB-B206-39FDB6D66C81
// Assembly location: C:\Git\Repos\CapitalMarkets\Solutions\packages\STAR.Enterprise.Trace.1.1.0\lib\net451\STAR.Enterprise.Trace.dll

using System;
using System.Collections;
using System.Text;

namespace STAR.Enterprise.Trace
{
    internal class TraceSerializer
    {
        public string SerializeException(Exception exception)
        {
            StringBuilder buff = new StringBuilder();
            this.CollectErrorText(exception, buff, 32);
            return buff.ToString();
        }

        public string Serialize<T>(T item)
        {
            if ((object)item == null)
                return string.Empty;
            Type type = item.GetType();
            string str;
            try
            {
                if (typeof(string) == type || type.IsValueType)
                    str = item.ToString();
                else if (typeof(Exception).IsAssignableFrom(type))
                {
                    Exception exception = (object)item as Exception;
                    StringBuilder buff = new StringBuilder();
                    this.CollectErrorText(exception, buff, 32);
                    str = buff.ToString();
                }
                else
                    str = Serializer.Serialize((object)item);
            }
            catch (Exception ex)
            {
                str = string.Format("[ERROR: Failed to serialize: '{0}'.  {1}]", (object)type.FullName, (object)ex.Message);
            }
            return str ?? string.Empty;
        }

        private void CollectErrorDataText(IDictionary errorData, StringBuilder buff)
        {
            if (errorData == null || errorData.Keys.Count <= 0)
                return;
            buff.AppendLine().AppendLine("===== ERROR DATA =====");
            foreach (object key in (IEnumerable)errorData.Keys)
            {
                object obj = errorData[key];
                Exception exception = obj as Exception;
                string str = (exception != null ? exception.Message : (string)null) ?? this.Serialize<object>(obj);
                buff.AppendLine().AppendFormat("[{0}] =>\"{1}\"", key, (object)str).AppendLine().AppendLine("~");
            }
        }

        private void CollectErrorText(Exception exception, StringBuilder buff, int maxDepth)
        {
            if (exception == null || maxDepth <= 0)
                return;
            AggregateException exception1 = exception as AggregateException;
            if (exception1 != null)
            {
                this.CollectErrorText(exception1, buff, maxDepth - 1);
            }
            else
            {
                for (Exception exception2 = exception; exception2 != null; exception2 = exception2.InnerException)
                {
                    buff.AppendFormat("{0}:", (object)exception2.GetType().FullName).AppendLine().AppendLine(exception2.Message);
                    if (exception2.InnerException != null)
                        buff.AppendLine("----------------------------------------").AppendLine();
                }
                this.CollectErrorDataText(exception.Data, buff);
            }
        }

        private void CollectErrorText(AggregateException exception, StringBuilder buff, int maxDepth)
        {
            if (exception == null || maxDepth <= 0)
                return;
            exception = exception.Flatten();
            int count = exception.InnerExceptions.Count;
            buff.AppendFormat("{0} ({1} inner exceptions):", (object)exception.GetType().FullName, (object)count).AppendLine().AppendLine(exception.Message);
            buff.AppendLine("========================================").AppendLine();
            int num = 0;
            foreach (Exception innerException in exception.InnerExceptions)
            {
                ++num;
                if (num <= 15)
                {
                    buff.AppendLine().AppendFormat("===== EXCEPTION #{0} of {1} =====", (object)num, (object)count).AppendLine();
                    this.CollectErrorText(innerException, buff, maxDepth - 1);
                }
                else
                    break;
            }
            this.CollectErrorDataText(exception.Data, buff);
        }
    }
}
