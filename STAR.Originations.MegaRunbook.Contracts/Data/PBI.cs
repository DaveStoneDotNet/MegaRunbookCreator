using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace STAR.Originations.MegaRunbook.Contracts.Data
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/originations/mrc/2016/01")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class PBI
    {
        [DataMember] public int Number { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => String.Format("Number: {0}", this.Number);
    }
}
