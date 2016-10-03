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
        [DataMember] public int Id { get; set; }
        [DataMember] public int ApplicationId { get; set; }

        [DataMember] public string ServiceName { get; set; }

        [DataMember] public List<EnvironmentLink> EnvironmentLinks { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => String.Format("ServiceName: {0}", this.ServiceName);
    }
}
