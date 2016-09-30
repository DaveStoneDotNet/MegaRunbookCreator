using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace STAR.Originations.MegaRunbook.Contracts.Data
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/originations/mrc/2016/01")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class RunbookStep
    {
        [DataMember] public int ID { get; set; }
        [DataMember] public int GroupNumber { get; set; }
        [DataMember] public int StepNumber { get; set; }
        [DataMember] public int Duration { get; set; }

        [DataMember] public string Name { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public string Notes { get; set; }
        [DataMember] public string RunbookStepStatusCode { get; set; }
        [DataMember] public string RunbookStepTypeCode { get; set; }

        [DataMember] public bool IsHtml { get; set; }

        [DataMember] public DateTime Time { get; set; }

        [DataMember] public List<RunbookStepPbi> RunbookStepPbis { get; set; }

        [DataMember] public List<Team> Teams { get; set; }
        [DataMember] public List<Contact> Developers { get; set; }
        [DataMember] public List<Contact> Resources { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => String.Format("GroupNumber: {0}. StepNumber: {1}", this.GroupNumber, this.StepNumber);
    }
}
