//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace STAR.Originations.MRC.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class RunbookStepPbi
    {
        public int RunbookStepId { get; set; }
        public int PbiNumber { get; set; }
    
        public virtual RunbookStep RunbookStep { get; set; }
    }
}
