// Decompiled with JetBrains decompiler
// Type: STAR.Enterprise.Trace.EventDataBuilder
// Assembly: STAR.Enterprise.Trace, Version=1.1.5697.29972, Culture=neutral, PublicKeyToken=null
// MVID: 515909EF-777E-43BB-B206-39FDB6D66C81
// Assembly location: C:\Git\Repos\CapitalMarkets\Solutions\packages\STAR.Enterprise.Trace.1.1.0\lib\net451\STAR.Enterprise.Trace.dll

using STAR.Enterprise.Trace.Contracts.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace STAR.Enterprise.Trace
{
    internal class EventDataBuilder
    {
        private readonly TraceSerializer _serializer;

        public EventDataBuilder()
          : this(new TraceSerializer())
        {
        }

        public EventDataBuilder(TraceSerializer serializer)
        {
            this._serializer = serializer ?? new TraceSerializer();
        }

        public List<TraceEventData> CreateEventKeys(NameValueCollection keys)
        {
            if (keys == null || keys.Count == 0)
                return (List<TraceEventData>)null;
            return ((IEnumerable<string>)keys.AllKeys).Where<string>((Func<string, bool>)(key => !string.IsNullOrWhiteSpace(key))).Select<string, TraceEventData>((Func<string, TraceEventData>)(key =>
            {
                TraceEventData traceEventData = new TraceEventData();
                traceEventData.EventDataType = key;
                string str = keys[key];
                traceEventData.EventDataBody = str;
                return traceEventData;
            })).ToList<TraceEventData>();
        }

        public List<TraceEventData> CreateTraceEventData(Exception exception, IEnumerable<KeyValuePair<string, object>> blobsWithLabels)
        {
            if (exception == null && blobsWithLabels == null)
                return (List<TraceEventData>)null;
            List<TraceEventData> traceEventDataList = new List<TraceEventData>();
            TraceEventData traceEventData = this.ToTraceEventData(exception);
            if (traceEventData != null)
                traceEventDataList.Add(traceEventData);
            if (!string.IsNullOrWhiteSpace(exception != null ? exception.StackTrace : (string)null))
                traceEventDataList.Add(new TraceEventData()
                {
                    EventDataBody = exception.StackTrace,
                    EventDataType = "StackTrace"
                });
            if (blobsWithLabels != null)
                traceEventDataList.AddRange(blobsWithLabels.Where<KeyValuePair<string, object>>((Func<KeyValuePair<string, object>, bool>)(kv =>
                {
                    if (!string.IsNullOrWhiteSpace(kv.Key))
                        return kv.Value != null;
                    return false;
                })).Select<KeyValuePair<string, object>, TraceEventData>((Func<KeyValuePair<string, object>, TraceEventData>)(kv => new TraceEventData()
                {
                    EventDataBody = this._serializer.Serialize<object>(kv.Value),
                    EventDataType = kv.Key
                })).Take<TraceEventData>(16));
            return traceEventDataList;
        }

        private TraceEventData ToTraceEventData(Exception exception)
        {
            if (exception == null)
                return (TraceEventData)null;
            return new TraceEventData()
            {
                EventDataBody = this._serializer.SerializeException(exception),
                EventDataType = "ErrorMessage"
            };
        }
    }
}
