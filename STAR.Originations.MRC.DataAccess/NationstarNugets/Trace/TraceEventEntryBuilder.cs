// Decompiled with JetBrains decompiler
// Type: STAR.Enterprise.Trace.TraceEventEntryBuilder
// Assembly: STAR.Enterprise.Trace, Version=1.1.5697.29972, Culture=neutral, PublicKeyToken=null
// MVID: 515909EF-777E-43BB-B206-39FDB6D66C81
// Assembly location: C:\Git\Repos\CapitalMarkets\Solutions\packages\STAR.Enterprise.Trace.1.1.0\lib\net451\STAR.Enterprise.Trace.dll

using STAR.Enterprise.Trace.Contracts.Data;
using System;

namespace STAR.Enterprise.Trace
{
    internal static class TraceEventEntryBuilder
    {
        public static TraceEventEntry IncludeEnvironmentData(TraceEventEntry traceEventEntry)
        {
            if (traceEventEntry == null)
                return (TraceEventEntry)null;
            TraceEventEntry traceEventEntry1 = traceEventEntry;
            string str1 = traceEventEntry1.EventId ?? TraceEventEntryBuilder.GetEventId();
            traceEventEntry1.EventId = str1;
            traceEventEntry.EventDate = traceEventEntry.EventDate == new DateTime() ? DateTime.Now : traceEventEntry.EventDate;
            TraceEventEntry traceEventEntry2 = traceEventEntry;
            DateTimeOffset? nullable = new DateTimeOffset?(traceEventEntry2.EventDateTimeOffset ?? DateTimeOffset.Now);
            traceEventEntry2.EventDateTimeOffset = nullable;
            TraceEventEntry traceEventEntry3 = traceEventEntry;
            string str2 = traceEventEntry3.MachineName ?? Environment.MachineName;
            traceEventEntry3.MachineName = str2;
            TraceEventEntry traceEventEntry4 = traceEventEntry;
            string str3 = traceEventEntry4.MethodName ?? TraceEventEntryBuilder.GetSourceMethodName(traceEventEntry.MethodName);
            traceEventEntry4.MethodName = str3;
            TraceEventEntry traceEventEntry5 = traceEventEntry;
            string str4 = traceEventEntry5.Referrer ?? TraceEventEntryBuilder.GetClientIp();
            traceEventEntry5.Referrer = str4;
            return traceEventEntry;
        }

        private static string GetEventId()
        {
            if (System.Diagnostics.Trace.CorrelationManager.ActivityId == Guid.Empty)
                System.Diagnostics.Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            return System.Diagnostics.Trace.CorrelationManager.ActivityId.ToString();
        }

        private static string GetClientIp()
        {
            return TraceEventEntryBuilder.GetSafe<string>((Func<string>)(() =>
            {
                    return (string)null;
            })) ?? TraceEventEntryBuilder.GetSafe<string>((Func<string>)(() =>
            {
                    return (string)null;
            }));
        }

        private static T GetSafe<T>(Func<T> factory)
        {
            try
            {
                return factory();
            }
            catch
            {
                return default(T);
            }
        }

        private static string GetSourceMethodName(string methodName)
        {
            if (!string.IsNullOrWhiteSpace(methodName))
            {
                return methodName;
            }
            else
            {
                return "None";
            }
        }
    }
}
