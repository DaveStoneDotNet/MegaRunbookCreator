using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using STAR.Enterprise.Trace;
using STAR.Enterprise.Trace.Wcf;
using STAR.Framework.Utility;

using STAR.Originations.MegaRunbook.Contracts.PagingModels;
using STAR.Originations.MegaRunbook.Excel;
using STAR.Originations.MRC.DataAccess.Query;

using entities = STAR.Originations.MRC.DataAccess;
using contracts = STAR.Originations.MegaRunbook.Contracts.Data;

namespace STAR.Originations.MRC.DataAccess
{
    public partial class MrcDataAccess : DataAccessBase
    {
        public const string ApplicationName = "MRC Data Access";

        #region Fields

        private readonly Func<entities::MrcDataEntities> contextCreator;

        #endregion Fields

        #region Constructor

        #region MrcDataAccess
        public MrcDataAccess() : this(MrcDataAccess.CreateContext)
        {

        }
        #endregion MrcDataAccess

        #region PoolTradeManagementData
        internal MrcDataAccess(Func<entities::MrcDataEntities> contextCreator)
        {
            this.traceSource = new TraceSource(MrcDataAccess.ApplicationName, SourceLevels.Information);

            if (contextCreator == null) throw new ArgumentNullException(nameof(contextCreator));

            this.contextCreator = contextCreator;
        }
        #endregion MrcDataAccess

        #endregion Constructor

        #region Methods

        #region GetRunbookTemplatesAsync
        public async Task<Page<contracts::RunbookTemplate>> GetRunbookTemplatesAsync(contracts::RunbookTemplate request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var stopwatch = DataAccessBase.StartStopwatch();
            using (var context = this.contextCreator())
            {
                var query = context.RunbookTemplates.AsQueryable();
                if (!String.IsNullOrWhiteSpace(request.Name))
                {
                    query = query.Where(p => p.Name.Contains(request.Name));
                }

                var totalRecordCount = query.Count();

                if (totalRecordCount == 0)
                {
                    return new Page<contracts::RunbookTemplate> { Items = new List<contracts::RunbookTemplate>(0) };
                }

                var sanitizedPagingInfo = request.Paging.SanitizePaging();

                var data = await query.Select(a => a).TakePage(sanitizedPagingInfo).ToListAsync().ConfigureAwait(false);
                var mapped = Mapper.Map<List<entities::RunbookTemplate>, List<contracts::RunbookTemplate>>(data);
                var page = mapped.ToPage(totalRecordCount, sanitizedPagingInfo);

                this.TraceSource.TraceEvent(TraceEventType.Information, String.Format("COMPLETE: Name: {0}.", Text.GetStringInfo(request.Name)), stopwatch.Elapsed, TraceStatus.Success, new Dictionary<String, object> { { "Request", request } });

                return page;
            }
        }
        #endregion GetRunbookTemplatesAsync

        #region GetRunbookTemplateAsync
        public async Task<contracts::RunbookTemplate> GetRunbookTemplateAsync(contracts::RunbookTemplate request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var stopwatch = DataAccessBase.StartStopwatch();
            using (var context = this.contextCreator())
            {
                var query = context.RunbookTemplates
                                   .Include("RunbookSteps")
                                   .Include("RunbookSteps.RunbookStepType")
                                   .Include("RunbookSteps.RunbookStepStatuses")
                                   .Include("RunbookSteps.RunbookStepPbis")
                                   .Include("RunbookSteps.Teams")
                                   .Include("RunbookSteps.Developers")
                                   .Include("RunbookSteps.Resources")
                                   .AsQueryable();

                if (request.Id > 0)
                {
                    query = query.Where(p => p.Id == request.Id);
                }

                var data = await query.Select(a => a).FirstOrDefaultAsync().ConfigureAwait(false);
                data.RunbookSteps = (from o in data.RunbookSteps select o).OrderBy(o => o.GroupNumber).ThenBy(o => o.StepNumber).ToList();

                var mapped = Mapper.Map<entities::RunbookTemplate, contracts::RunbookTemplate>(data);

                this.TraceSource.TraceEvent(TraceEventType.Information, String.Format("COMPLETE: Name: {0}.", Text.GetStringInfo(request.Name)), stopwatch.Elapsed, TraceStatus.Success, new Dictionary<String, object> { { "Request", request } });

                return mapped;
            }
        }
        #endregion GetRunbookTemplateAsync

        #region GetRunbookStepAsync
        public async Task<contracts::RunbookStep> GetRunbookStepAsync(contracts::RunbookStep request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var stopwatch = DataAccessBase.StartStopwatch();
            using (var context = this.contextCreator())
            {
                var query = context.RunbookSteps
                                   .Include("RunbookStepType")
                                   .Include("RunbookStepStatuses")
                                   .Include("RunbookStepPbis")
                                   .Include("Teams")
                                   .Include("Developers")
                                   .Include("Resources")
                                   .AsQueryable();

                if (request.Id > 0)
                {
                    query = query.Where(p => p.Id == request.Id);
                }

                var data = await query.Select(a => a).FirstOrDefaultAsync().ConfigureAwait(false);

                var mapped = Mapper.Map<entities::RunbookStep, contracts::RunbookStep>(data);

                this.TraceSource.TraceEvent(TraceEventType.Information, String.Format("COMPLETE: Name: {0}.", Text.GetStringInfo(request.Name)), stopwatch.Elapsed, TraceStatus.Success, new Dictionary<String, object> { { "Request", request } });

                return mapped;
            }
        }
        #endregion GetRunbookStepAsync

        // ---

        #region GetRfcsAsync
        public async Task<Page<contracts::Rfc>> GetRfcsAsync(contracts::Rfc request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var stopwatch = DataAccessBase.StartStopwatch();
            using (var context = this.contextCreator())
            {
                var query = context.Rfcs.AsQueryable();

                var number = Conversion.GetInt(request.Name);
                if (number > 0)
                {
                    request.Number = number;
                    request.Name = String.Empty;
                }

                if (request.Number > 0)
                {
                    query = query.Where(p => p.Number == request.Number);
                }
                if (!String.IsNullOrWhiteSpace(request.Name))
                {
                    query = query.Where(p => p.Name.Contains(request.Name));
                }

                var totalRecordCount = query.Count();

                if (totalRecordCount == 0)
                {
                    return new Page<contracts::Rfc> { Items = new List<contracts::Rfc>(0) };
                }

                var sanitizedPagingInfo = request.Paging.SanitizePaging();

                var data = await query.Select(a => a).TakePage(sanitizedPagingInfo).ToListAsync().ConfigureAwait(false);
                var mapped = Mapper.Map<List<entities::Rfc>, List<contracts::Rfc>>(data);
                var page = mapped.ToPage(totalRecordCount, sanitizedPagingInfo);

                this.TraceSource.TraceEvent(TraceEventType.Information, String.Format("COMPLETE: Name: {0}.", Text.GetStringInfo(request.Name)), stopwatch.Elapsed, TraceStatus.Success, new Dictionary<String, object> { { "Request", request } });

                return page;
            }
        }
        #endregion GetRfcsAsync

        #region GetRfcAsync
        public async Task<contracts::Rfc> GetRfcAsync(int id)
        {
            var stopwatch = DataAccessBase.StartStopwatch();
            using (var context = this.contextCreator())
            {
                var query = context.Rfcs
                                   .Include("RunbookSteps")
                                   .Include("Contact")
                                   .Include("RunbookSteps.RunbookStepType")
                                   .Include("RunbookSteps.RunbookStepStatuses")
                                   .Include("RunbookSteps.RunbookStepPbis")
                                   .Include("RunbookSteps.Teams")
                                   .Include("RunbookSteps.Developers")
                                   .Include("RunbookSteps.Resources")
                                   .AsQueryable();

                query = query.Where(p => p.Id == id);

                var data = await query.Select(a => a).FirstOrDefaultAsync().ConfigureAwait(false);
                data.RunbookSteps = (from o in data.RunbookSteps select o).OrderBy(o => o.GroupNumber).ThenBy(o => o.StepNumber).ToList();

                var mapped = Mapper.Map<entities::Rfc, contracts::Rfc>(data);

                this.TraceSource.TraceEvent(TraceEventType.Information, String.Format("COMPLETE: ID: {0}.", id), stopwatch.Elapsed, TraceStatus.Success);

                return mapped;
            }
        }
        #endregion GetRfcAsync

        #region GetApplicationGroupsAsync
        public async Task<Page<contracts::ApplicationGroup>> GetApplicationGroupsAsync(contracts::ApplicationGroup request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var stopwatch = DataAccessBase.StartStopwatch();
            using (var context = this.contextCreator())
            {
                var query = context.ApplicationGroups.AsQueryable();
                if (!String.IsNullOrWhiteSpace(request.Name))
                {
                    query = query.Where(p => p.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase));
                }

                var totalRecordCount = query.Count();

                if (totalRecordCount == 0)
                {
                    return new Page<contracts::ApplicationGroup> { Items = new List<contracts::ApplicationGroup>(0) };
                }

                var sanitizedPagingInfo = request.Paging.SanitizePaging();

                var data = await query.Select(a => a).TakePage(sanitizedPagingInfo).ToListAsync().ConfigureAwait(false);
                var mapped = Mapper.Map<List<entities::ApplicationGroup>, List<contracts::ApplicationGroup>>(data);
                var page = mapped.ToPage(totalRecordCount, sanitizedPagingInfo);

                this.TraceSource.TraceEvent(TraceEventType.Information, String.Format("COMPLETE: Name: {0}.", Text.GetStringInfo(request.Name)), stopwatch.Elapsed, TraceStatus.Success, new Dictionary<String, object> { { "Request", request } });

                return page;
            }
        }
        #endregion GetApplicationGroupsAsync

        #region GetApplicationLinksAsync
        public async Task<Page<contracts::ApplicationLink>> GetApplicationLinksAsync(contracts::ApplicationLink request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var stopwatch = DataAccessBase.StartStopwatch();
            using (var context = this.contextCreator())
            {
                var query = context.ApplicationLinks
                                   .Include("ApplicationGroup")
                                   .Include("ApplicationType")
                                   .Include("ServiceLinks")
                                   .Include("ServiceLinks.EnvironmentLinks")
                                   .Include("ServiceLinks.EnvironmentLinks.Server")
                                   .Include("ServiceLinks.EnvironmentLinks.Server.Environment")
                                   .AsQueryable();

                if (!String.IsNullOrWhiteSpace(request.Name))
                {
                    query = query.Where(p => p.Name.Contains(request.Name) || p.ApplicationType.Description.Contains(request.Name));
                }

                if (request.ApplicationType != null)
                {
                    if (!String.IsNullOrWhiteSpace(request.ApplicationType.Code))
                    {
                        query = query.Where(p => p.ApplicationType.Code.Equals(request.ApplicationType.Code));
                    }
                }

                if (request?.ApplicationGroupId > 0)
                {
                    query = query.Where(p => p.ApplicationGroupId == request.ApplicationGroupId);
                }

                var totalRecordCount = query.Count();

                if (totalRecordCount == 0)
                {
                    return new Page<contracts::ApplicationLink> { Items = new List<contracts::ApplicationLink>(0) };
                }

                var sanitizedPagingInfo = request.Paging.SanitizePaging();

                var data = await query.Select(a => a).TakePage(sanitizedPagingInfo).ToListAsync().ConfigureAwait(false);
                var mapped = Mapper.Map<List<entities::ApplicationLink>, List<contracts::ApplicationLink>>(data);
                var page = mapped.ToPage(totalRecordCount, sanitizedPagingInfo);

                this.TraceSource.TraceEvent(TraceEventType.Information, String.Format("COMPLETE: Name: {0}.", Text.GetStringInfo(request.Name)), stopwatch.Elapsed, TraceStatus.Success, new Dictionary<String, object> { { "Request", request } });

                return page;
            }
        }
        #endregion GetApplicationLinksAsync

        // ---

        #region GetAppLookupsAsync
        public async Task<contracts::AppLookups> GetAppLookupsAsync()
        {
            var stopwatch = DataAccessBase.StartStopwatch();
            var appLookups = new contracts::AppLookups();

            using (var context = this.contextCreator())
            {
                var applicationGroups = await context.ApplicationGroups.OrderBy(o => o.Name).ToListAsync().ConfigureAwait(false);
                var applicationTypes = await context.ApplicationTypes.OrderBy(o => o.SortOrder).ToListAsync().ConfigureAwait(false);
                var runbookStepStatuses = await context.RunbookStepStatus.OrderBy(o => o.SortOrder).ToListAsync().ConfigureAwait(false);
                var runbookStepTypes = await context.RunbookStepTypes.OrderBy(o => o.SortOrder).ToListAsync().ConfigureAwait(false);
                var teams = await context.Teams.OrderBy(o => o.Name).ToListAsync().ConfigureAwait(false);

                appLookups.ApplicationGroups = Mapper.Map<List<entities::ApplicationGroup>, List<contracts::Lookup>>(applicationGroups);
                appLookups.ApplicationTypes = Mapper.Map<List<entities::ApplicationType>, List<contracts::Lookup>>(applicationTypes);
                appLookups.RunbookStepStatuses = Mapper.Map<List<entities::RunbookStepStatus>, List<contracts::Lookup>>(runbookStepStatuses);
                appLookups.RunbookStetpTypes = Mapper.Map<List<entities::RunbookStepType>, List<contracts::Lookup>>(runbookStepTypes);
                appLookups.Teams = Mapper.Map<List<entities::Team>, List<contracts::Team>>(teams);

                this.TraceSource.TraceEvent(TraceEventType.Information, "COMPLETE", stopwatch.Elapsed, TraceStatus.Success);

                return appLookups;
            }
        }
        #endregion GetAppLookupsAsync

        #region GetContactsAsync
        public async Task<List<contracts::Contact>> GetContactsAsync(contracts::Contact request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (String.IsNullOrWhiteSpace(request.DisplayName) && request.UserId == 0) return new List<contracts::Contact> { };

            var stopwatch = DataAccessBase.StartStopwatch();
            using (var context = this.contextCreator())
            {
                var query = context.Contacts.AsQueryable();

                if (!String.IsNullOrWhiteSpace(request.DisplayName))
                {
                    query = query.Where(p => p.DisplayName.Contains(request.DisplayName));
                }

                if (request.UserId > 0)
                {
                    query = query.Where(p => p.UserId == request.UserId);
                }

                var totalRecordCount = query.Count();

                if (totalRecordCount == 0)
                {
                    return new List<contracts::Contact> {  };
                }

                var data = await query.Select(a => a).OrderBy(o => o.DisplayName).ToListAsync().ConfigureAwait(false);
                var mapped = Mapper.Map<List<entities::Contact>, List<contracts::Contact>>(data);

                this.TraceSource.TraceEvent(TraceEventType.Information, String.Format("COMPLETE: Name: {0}.", Text.GetStringInfo(request.DisplayName)), stopwatch.Elapsed, TraceStatus.Success, new Dictionary<String, object> { { "Request", request } });

                return mapped;
            }
        }
        #endregion GetContactsAsync

        #region InsertRfcAsync
        public async Task<contracts::ServiceResponse> InsertRfcAsync(contracts::Rfc request)
        {
            var stopwatch = DataAccessBase.StartStopwatch();

            var serviceResponse = new contracts::ServiceResponse();

            // Convert Start and End Times
            // Convert Templates to Runbook Steps
            //  - Set the new RFC ID
            //  - Set Template ID's to ZERO

            using (var context = this.contextCreator())
            {
                // RFC
                //  - Save the RFC and get the ID

                var rfc = new Rfc()
                {
                    Number = request.Number,
                    Name = request.Name,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    ContactId = request.Contact.Id
                };

                context.Rfcs.Add(rfc);
                serviceResponse.RecordsAffected = await context.SaveChangesAsync().ConfigureAwait(false);

                // Runbook Steps
                //  - Save the Runbook Steps with the RFC ID

                foreach (var runbookTemplate in request.Templates)
                {
                    // Don't rely on the template coming from the UI - get from the database instead. Only the ID is really necessary - may refactor later.
                    var template = await this.GetRunbookTemplateAsync(new contracts.RunbookTemplate { Id = runbookTemplate.Id });

                    // Map the Contract Steps in the Template to the Entity Steps in the RFC
                    foreach (var c in template.RunbookSteps)
                    {
                        var runbookStep = new entities::RunbookStep
                        {
                            RfcId = rfc.Id,
                            GroupNumber = c.GroupNumber,
                            StepNumber = c.StepNumber,
                            Duration = c.Duration,
                            Name = c.Name,
                            Description = c.Description,
                            Notes = c.Notes,
                            RunbookStepStatusCode = c.RunbookStepStatusCode,
                            RunbookStepTypeCode = c.RunbookStepTypeCode,
                            IsHtml = c.IsHtml,
                            Time = c.Time,
                        };
                        context.RunbookSteps.Add(runbookStep);
                        serviceResponse.RecordsAffected = await context.SaveChangesAsync().ConfigureAwait(false);

                        var templateStepsRequest = new contracts::TemplateStepsRequest { TemplateId = runbookTemplate.Id, OldRunbookStepId = c.Id, NewRunbookStepId = runbookStep.Id };

                        // Save associated RunbookStep Developers, Pbis, Resources, and Teams...

                        serviceResponse = await this.InsertRunbookStepDevelopersAsync(templateStepsRequest);
                        serviceResponse = await this.InsertRunbookStepPbisAsync(templateStepsRequest);
                        serviceResponse = await this.InsertRunbookStepResourcesAsync(templateStepsRequest);
                        serviceResponse = await this.InsertRunbookStepTeamsAsync(templateStepsRequest);
                    }
                }

                serviceResponse.RecordsAffected = await context.SaveChangesAsync().ConfigureAwait(false);
                serviceResponse.IsSuccessful = true;

                this.TraceSource.TraceEvent(TraceEventType.Information, String.Format("COMPLETE: Name: {0}.", Text.GetStringInfo(request.Name)), stopwatch.Elapsed, TraceStatus.Success, new Dictionary<String, object> { { "Request", request } });
            }

            return serviceResponse;
        }
        #endregion InsertRfcAsync

        #region InsertRunbookStepDevelopersAsync
        public async Task<contracts::ServiceResponse> InsertRunbookStepDevelopersAsync(contracts::TemplateStepsRequest request)
        {
            var stopwatch = DataAccessBase.StartStopwatch();

            var serviceResponse = new contracts::ServiceResponse();

            using (var context = this.contextCreator())
            {
                    const string sql = @"
                                                insert into [Runbook].[RunbookStepDeveloper]
                                                (
                                                      [RunbookStepId], 
                                                      [ContactId]
                                                )
                                               select @NewRunbookStepId, 
                                                      [ContactId]
                                                 from [Runbook].[RunbookStepDeveloper]
                                                where [RunbookStepId] = @OldRunbookStepId
                                        ";

                    object[] parameters =
                    {
                        new SqlParameter { ParameterName = "@NewRunbookStepId", Value = request.NewRunbookStepId },
                        new SqlParameter { ParameterName = "@OldRunbookStepId", Value = request.OldRunbookStepId }
                    };

                    serviceResponse.RecordsAffected = await this.ExecuteAsync(entities => entities.Database.ExecuteSqlCommandAsync(sql, parameters));
                    serviceResponse.IsSuccessful = true;

                serviceResponse.RecordsAffected = await context.SaveChangesAsync().ConfigureAwait(false);
                serviceResponse.IsSuccessful = true;

                this.TraceSource.TraceEvent(TraceEventType.Information, String.Format("COMPLETE: Template: {0}.", request.TemplateId), stopwatch.Elapsed, TraceStatus.Success, new Dictionary<String, object> { { "Request", request } });
            }

            return serviceResponse;
        }
        #endregion InsertRunbookStepDevelopersAsync

        #region InsertRunbookStepPbisAsync
        public async Task<contracts::ServiceResponse> InsertRunbookStepPbisAsync(contracts::TemplateStepsRequest request)
        {
            var stopwatch = DataAccessBase.StartStopwatch();

            var serviceResponse = new contracts::ServiceResponse();

            using (var context = this.contextCreator())
            {
                const string sql = @"
                                            insert into [Runbook].[RunbookStepPbi]
                                            (
                                                  [RunbookStepId], 
                                                  [PbiNumber]
                                            )
                                           select @NewRunbookStepId, 
                                                  [PbiNumber]
                                             from [Runbook].[RunbookStepPbi]
                                            where [RunbookStepId] = @OldRunbookStepId
                                    ";

                object[] parameters =
                {
                        new SqlParameter { ParameterName = "@NewRunbookStepId", Value = request.NewRunbookStepId },
                        new SqlParameter { ParameterName = "@OldRunbookStepId", Value = request.OldRunbookStepId }
                    };

                serviceResponse.RecordsAffected = await this.ExecuteAsync(entities => entities.Database.ExecuteSqlCommandAsync(sql, parameters));
                serviceResponse.IsSuccessful = true;

                serviceResponse.RecordsAffected = await context.SaveChangesAsync().ConfigureAwait(false);
                serviceResponse.IsSuccessful = true;

                this.TraceSource.TraceEvent(TraceEventType.Information, String.Format("COMPLETE: Template: {0}.", request.TemplateId), stopwatch.Elapsed, TraceStatus.Success, new Dictionary<String, object> { { "Request", request } });
            }

            return serviceResponse;
        }
        #endregion InsertRunbookStepPbisAsync

        #region InsertRunbookStepResourcesAsync
        public async Task<contracts::ServiceResponse> InsertRunbookStepResourcesAsync(contracts::TemplateStepsRequest request)
        {
            var stopwatch = DataAccessBase.StartStopwatch();

            var serviceResponse = new contracts::ServiceResponse();

            using (var context = this.contextCreator())
            {
                const string sql = @"
                                            insert into [Runbook].[RunbookStepResource]
                                            (
                                                  [RunbookStepId], 
                                                  [ContactId]
                                            )
                                           select @NewRunbookStepId, 
                                                  [ContactId]
                                             from [Runbook].[RunbookStepResource]
                                            where [RunbookStepId] = @OldRunbookStepId
                                    ";

                object[] parameters =
                {
                        new SqlParameter { ParameterName = "@NewRunbookStepId", Value = request.NewRunbookStepId },
                        new SqlParameter { ParameterName = "@OldRunbookStepId", Value = request.OldRunbookStepId }
                    };

                serviceResponse.RecordsAffected = await this.ExecuteAsync(entities => entities.Database.ExecuteSqlCommandAsync(sql, parameters));
                serviceResponse.IsSuccessful = true;

                serviceResponse.RecordsAffected = await context.SaveChangesAsync().ConfigureAwait(false);
                serviceResponse.IsSuccessful = true;

                this.TraceSource.TraceEvent(TraceEventType.Information, String.Format("COMPLETE: Template: {0}.", request.TemplateId), stopwatch.Elapsed, TraceStatus.Success, new Dictionary<String, object> { { "Request", request } });
            }

            return serviceResponse;
        }
        #endregion InsertRunbookStepResourcesAsync

        #region InsertRunbookStepTeamsAsync
        public async Task<contracts::ServiceResponse> InsertRunbookStepTeamsAsync(contracts::TemplateStepsRequest request)
        {
            var stopwatch = DataAccessBase.StartStopwatch();

            var serviceResponse = new contracts::ServiceResponse();

            using (var context = this.contextCreator())
            {
                const string sql = @"
                                            insert into [Runbook].[RunbookStepTeam]
                                            (
                                                   [RunbookStepId], 
                                                   [TeamId]
                                            )
                                            select @NewRunbookStepId, 
                                                   [TeamId]
                                              from [Runbook].[RunbookStepTeam]
                                             where [RunbookStepId] = @OldRunbookStepId
                                    ";

                object[] parameters =
                {
                        new SqlParameter { ParameterName = "@NewRunbookStepId", Value = request.NewRunbookStepId },
                        new SqlParameter { ParameterName = "@OldRunbookStepId", Value = request.OldRunbookStepId }
                    };

                serviceResponse.RecordsAffected = await this.ExecuteAsync(entities => entities.Database.ExecuteSqlCommandAsync(sql, parameters));
                serviceResponse.IsSuccessful = true;

                serviceResponse.RecordsAffected = await context.SaveChangesAsync().ConfigureAwait(false);
                serviceResponse.IsSuccessful = true;

                this.TraceSource.TraceEvent(TraceEventType.Information, String.Format("COMPLETE: Template: {0}.", request.TemplateId), stopwatch.Elapsed, TraceStatus.Success, new Dictionary<String, object> { { "Request", request } });
            }

            return serviceResponse;
        }
        #endregion InsertRunbookStepTeamsAsync

        #endregion Methods

        #region CreateContext
        /// <summary>
        /// Initializes and returns the context to the 'PoolTradeManagementEntities'.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// A configuration setting was created to turn EF logging on and off to help troubleshoot ongoing 
        /// application performance issues. After having EF logging turned off by default for several months, 
        /// no noticeable performance gains were observed, so EF logging is being turned back on in the config.
        /// </remarks>
        private static entities::MrcDataEntities CreateContext()
        {
            var isEfLoggingEnabled = Conversion.Configuration.GetBoolean("IsEfLoggingEnabled", true);
            var context = new entities::MrcDataEntities { EnableLogging = isEfLoggingEnabled };
            context.Configuration.ProxyCreationEnabled = false;
            var traceContext = TraceContext.Current;

            traceContext?.Capture("SQL", () => context.LogText);

            return context;
        }
        #endregion CreateContext
    }
}
