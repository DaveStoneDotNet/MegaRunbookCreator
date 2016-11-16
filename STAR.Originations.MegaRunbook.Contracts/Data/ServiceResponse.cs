using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace STAR.Originations.MegaRunbook.Contracts.Data
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/Capital/Markets/2014/09")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class ServiceResponse
    {
        [DataMember] public int RecordsAffected { get; set; }
        [DataMember] public string Result { get; set; }
        [DataMember] public bool IsSuccessful { get; set; }
        [DataMember] public List<String> Messages { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get { return String.Format("{0} | IsSuccessful: {1} | RecordsAffected: {2} | Messages: {3}", this.GetType().Name, this.IsSuccessful, this.RecordsAffected, this.Messages != null ? this.Messages.Count : 0); }
        }
    }
}
