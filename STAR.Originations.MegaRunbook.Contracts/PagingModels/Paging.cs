using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace STAR.Originations.MegaRunbook.Contracts.PagingModels
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/Capital/Markets/2014/09")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Paging
    {
        [DataMember] public int PageNumber { get; set; }
        [DataMember] public int PageSize { get; set; }

        [DataMember] public List<SortInfo> SortInfo { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => String.Format("{0}(PageNumber={1}; PageSize={2})", this.GetType().Name, PageNumber, PageSize);
    }
}
