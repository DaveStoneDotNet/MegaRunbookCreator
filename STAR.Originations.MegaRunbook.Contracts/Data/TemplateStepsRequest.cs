using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace STAR.Originations.MegaRunbook.Contracts.Data
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/originations/mrc/2016/01")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class TemplateStepsRequest
    {
        [DataMember] public int TemplateId { get; set; }
        [DataMember] public int OldRunbookStepId { get; set; }
        [DataMember] public int NewRunbookStepId { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => String.Format("TemplateId: {0}", this.TemplateId);
    }
}
