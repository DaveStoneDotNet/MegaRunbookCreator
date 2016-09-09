using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace STAR.Originations.MegaRunbook.Contracts
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/originations/mrc/2016/01")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class RunbookStep
    {
        [DataMember] public int GroupNumber { get; set; }
        [DataMember] public int StepNumber { get; set; }
        [DataMember] public int Duration { get; set; }

        [DataMember] public string Team { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public string Notes { get; set; }
        [DataMember] public string Resource { get; set; }
        [DataMember] public string FormattedTime { get; set; }

        [DataMember] public string Time { get; set; }

        [DataMember] public Lookup StatusLookup { get; set; }

        [DataMember] public List<PBI> PBIs { get; set; }
        [DataMember] public List<Developer> Developers { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => String.Format("Number: {0}", this.StepNumber);
    }
}
