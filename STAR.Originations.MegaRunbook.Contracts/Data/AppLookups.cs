using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace STAR.Originations.MegaRunbook.Contracts.Data
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/Capital/Markets/2014/09")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class AppLookups
    {
        [DataMember] public List<Lookup> ApplicationGroups { get; set; }
        [DataMember] public List<Lookup> ApplicationTypes { get; set; }
        [DataMember] public List<Lookup> RunbookStetpTypes { get; set; }
        [DataMember] public List<Lookup> RunbookStepStatuses { get; set; }

        [DataMember] public List<Team> Teams { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => String.Format("{0} ", this.GetType().Name);
    }
}
