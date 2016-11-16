using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace STAR.Originations.MegaRunbook.Contracts.Data
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/originations/mrc/2016/01")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Rfc
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public int Number { get; set; }

        [DataMember] public string Name { get; set; }

        [DataMember] public DateTime? StartTime { get; set; }
        [DataMember] public DateTime? EndTime { get; set; }

        [DataMember] public Contact Contact { get; set; }
        [DataMember] public List<RunbookStep> RunbookSteps { get; set; }
        [DataMember] public List<RunbookTemplate> Templates { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => String.Format("Name: {0}", this.Name);
    }
}
