
using System.Runtime.Serialization;

namespace STAR.Originations.MegaRunbook.Contracts.PagingModels
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/Capital/Markets/2014/09")]
    public enum SortOrder
    {
        [EnumMember] Ascending,
        [EnumMember] Descending,
    }
}
