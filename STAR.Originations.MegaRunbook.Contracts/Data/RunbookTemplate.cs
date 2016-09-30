using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace STAR.Originations.MegaRunbook.Contracts.Data
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/originations/mrc/2016/01")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class RunbookTemplate : RequestBase
    {
        [DataMember] public int ID { get; set; }
        [DataMember] public int Number { get; set; }

        [DataMember] public string Name { get; set; }

        [DataMember] public List<RunbookStep> RunbookSteps { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => String.Format("Name: {0}", this.Name);
    }
}
