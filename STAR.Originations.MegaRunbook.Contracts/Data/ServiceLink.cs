using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace STAR.Originations.MegaRunbook.Contracts.Data
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/originations/mrc/2016/01")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class ServiceLink
    {
        [DataMember] public int ID { get; set; }
        [DataMember] public string FolderName { get; set; }
        [DataMember] public string ServiceName { get; set; }
        [DataMember] public string Notes { get; set; }
        [DataMember] public List<Link> ServiceLinks { get; set; }
        [DataMember] public List<Link> FolderLinks { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => String.Format("FolderName: {0}", this.FolderName);
    }
}
