﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MrcDataEntities : DbContext
    {
        public MrcDataEntities()
            : base("name=MrcDataEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<RunbookTemplate> RunbookTemplates { get; set; }
        public virtual DbSet<RunbookStepStatus> RunbookStepStatus { get; set; }
        public virtual DbSet<RunbookStep> RunbookSteps { get; set; }
        public virtual DbSet<RunbookStepPbi> RunbookStepPbis { get; set; }
        public virtual DbSet<RunbookStepType> RunbookStepTypes { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<ApplicationGroup> ApplicationGroups { get; set; }
        public virtual DbSet<ApplicationLink> ApplicationLinks { get; set; }
        public virtual DbSet<Environment> Environments { get; set; }
        public virtual DbSet<EnvironmentLink> EnvironmentLinks { get; set; }
        public virtual DbSet<Server> Servers { get; set; }
        public virtual DbSet<ServiceLink> ServiceLinks { get; set; }
    }
}
