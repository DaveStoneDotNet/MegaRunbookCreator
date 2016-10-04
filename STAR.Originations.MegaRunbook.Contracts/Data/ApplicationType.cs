using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace STAR.Originations.MegaRunbook.Contracts.Data
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/Capital/Markets/2014/09")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class ApplicationType
    {
        [DataMember] public string Code { get; set; }
        [DataMember] public string Description { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => String.Format("Code: {0}", this.Code);
    }
}
