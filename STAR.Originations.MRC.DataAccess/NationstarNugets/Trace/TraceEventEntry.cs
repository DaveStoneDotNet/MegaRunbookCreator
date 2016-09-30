// Decompiled with JetBrains decompiler
// Type: STAR.Enterprise.Trace.Contracts.Data.TraceEventEntry
// Assembly: STAR.Enterprise.Trace.Contracts, Version=1.1.5701.32475, Culture=neutral, PublicKeyToken=null
// MVID: DA4C6A42-8C19-456C-A1BB-CD563F2378C2
// Assembly location: C:\Git\Repos\CapitalMarkets\Solutions\packages\STAR.Enterprise.Trace.Contracts.1.1.0\lib\net451\STAR.Enterprise.Trace.Contracts.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace STAR.Enterprise.Trace.Contracts.Data
{
    [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DataContract(Name = "TraceEventEntry", Namespace = "http://services.nationstar.net/DataContracts/2014/04")]
    public class TraceEventEntry
    {
        [DataMember]
        public string ApplicationName { get; set; }

        [DataMember]
        public string MethodName { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string MachineName { get; set; }

        [DataMember]
        public string Referrer { get; set; }

        [DataMember]
        public string EventId { get; set; }

        [DataMember]
        public string EventType { get; set; }

        [DataMember]
        public TimeSpan EventDuration { get; set; }

        [DataMember]
        public DateTime EventDate { get; set; }

        [DataMember]
        public DateTimeOffset? EventDateTimeOffset { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public List<TraceEventData> EventData { get; set; }

        [DataMember]
        public List<TraceEventData> EventKeys { get; set; }

        [DataMember]
        public TraceEventCache EventCache { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                return string.Format("{0}({1}, Message=\"{2}\")", (object)this.GetType().Name, (object)this.ApplicationName, (object)this.Message);
            }
        }
    }
}
