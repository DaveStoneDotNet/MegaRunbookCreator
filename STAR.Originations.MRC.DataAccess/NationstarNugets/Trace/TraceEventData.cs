// Decompiled with JetBrains decompiler
// Type: STAR.Enterprise.Trace.Contracts.Data.TraceEventData
// Assembly: STAR.Enterprise.Trace.Contracts, Version=1.1.5701.32475, Culture=neutral, PublicKeyToken=null
// MVID: DA4C6A42-8C19-456C-A1BB-CD563F2378C2
// Assembly location: C:\Git\Repos\CapitalMarkets\Solutions\packages\STAR.Enterprise.Trace.Contracts.1.1.0\lib\net451\STAR.Enterprise.Trace.Contracts.dll

using System.Diagnostics;
using System.Runtime.Serialization;

namespace STAR.Enterprise.Trace.Contracts.Data
{
    [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DataContract(Name = "TraceEventData", Namespace = "http://services.nationstar.net/DataContracts/2014/04")]
    public class TraceEventData
    {
        [DataMember]
        public string EventDataType { get; set; }

        [DataMember]
        public string EventDataBody { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                return string.Format("{0}({1})", (object)this.GetType().Name, (object)this.EventDataType);
            }
        }
    }
}
