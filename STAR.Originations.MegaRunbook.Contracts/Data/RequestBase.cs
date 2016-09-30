using System.Diagnostics;
using System.Runtime.Serialization;

using STAR.Originations.MegaRunbook.Contracts.PagingModels;

namespace STAR.Originations.MegaRunbook.Contracts.Data
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/originations/mrc/2016/01")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class RequestBase
    {
        [DataMember] public Paging Paging { get; set; }
    }
}
