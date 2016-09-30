using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace STAR.Originations.MegaRunbook.Contracts.PagingModels
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/Capital/Markets/2014/09")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Page<TItem>
    {
        [DataMember] public int TotalRecordCount { get; set; }
        [DataMember] public int TotalPageCount { get; set; }
        [DataMember] public int PageNumber { get; set; }
        [DataMember] public int RecordsPerPage { get; set; }
        
        [DataMember] public bool HasPrevious { get; set; }
        [DataMember] public bool HasNext { get; set; }
        
        [DataMember] public List<TItem> Items { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => String.Format("{0} Page({1} of {2})", typeof(TItem).Name, PageNumber, TotalPageCount);
    }
}
