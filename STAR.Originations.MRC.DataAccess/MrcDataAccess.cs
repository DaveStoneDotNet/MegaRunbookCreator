using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using STAR.Enterprise.Trace;
using STAR.Enterprise.Trace.Wcf;
using STAR.Framework.Utility;

using STAR.Originations.MegaRunbook.Contracts.PagingModels;
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

                if (request.ID > 0)
                {
                    query = query.Where(p => p.Id == request.ID);
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

                if (request.ID > 0)
                {
                    query = query.Where(p => p.Id == request.ID);
                }

                var data = await query.Select(a => a).FirstOrDefaultAsync().ConfigureAwait(false);

                var mapped = Mapper.Map<entities::RunbookStep, contracts::RunbookStep>(data);

                this.TraceSource.TraceEvent(TraceEventType.Information, String.Format("COMPLETE: Name: {0}.", Text.GetStringInfo(request.Name)), stopwatch.Elapsed, TraceStatus.Success, new Dictionary<String, object> { { "Request", request } });

                return mapped;
            }
        }
        #endregion GetRunbookStepAsync

        // ---

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
                                   .Include("ServiceLinks")
                                   .Include("ServiceLinks.EnvironmentLinks")
                                   .Include("ServiceLinks.EnvironmentLinks.Server")
                                   .Include("ServiceLinks.EnvironmentLinks.Server.Environment")
                                   .AsQueryable();
                if (!String.IsNullOrWhiteSpace(request.Name))
                {
                    query = query.Where(p => p.Name.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase));
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
