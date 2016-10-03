using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace STAR.Originations.MegaRunbook.Contracts.Data
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/originations/mrc/2016/01")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class EnvironmentLink
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int ServiceLinkId { get; set; }
        [DataMember] public int ServerId { get; set; }

        [DataMember] public string Url { get; set; }
        [DataMember] public string FolderPath { get; set; }

        [DataMember] public Server Server { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => String.Format("FolderPath: {0}", this.FolderPath);
    }
}
