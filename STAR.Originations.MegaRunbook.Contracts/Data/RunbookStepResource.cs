using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace STAR.Originations.MegaRunbook.Contracts.Data
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/Capital/Markets/2014/09")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class RunbookStepResource
    {
        [DataMember] public int RunbookStepId { get; set; }
        [DataMember] public int UserId { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => String.Format("RunbookStepId: {0}. UserId: {1}.", this.RunbookStepId, this.UserId);
    }
}
