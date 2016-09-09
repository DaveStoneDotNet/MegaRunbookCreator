using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace STAR.Originations.MegaRunbook.Contracts
{
    [DataContract(Namespace = "http://services.nationstarmtg.net/Capital/Markets/2014/09")]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class AppInfo
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public string Version { get; set; }
        [DataMember] public string InformationalVersion { get; set; }
        [DataMember] public string FriendlyVersion { get; set; }
        [DataMember] public string WindowsUserName { get; set; }
        [DataMember] public string UserDomainName { get; set; }
        [DataMember] public string CLR { get; set; }
        [DataMember] public string OS { get; set; }
        [DataMember] public string TotalPhysicalMemory { get; set; }
        [DataMember] public string AvailablePhysicalMemory { get; set; }
        [DataMember] public string MachineName { get; set; }
        [DataMember] public string Sprint { get; set; }

        [DataMember] public DateTime BuildDate { get; set; }

        [DataMember] public int ProcessorCount { get; set; }

        [DataMember] public bool Is64BitOperatingSystem { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => String.Format("Name: {0}", this.Name);
    }
}
