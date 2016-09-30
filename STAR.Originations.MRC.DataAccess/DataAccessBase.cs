using System;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

using STAR.Framework.Utility;

namespace STAR.Originations.MRC.DataAccess
{
    public abstract class DataAccessBase
    {
        #region Fields

        private const string ApplicationName = "MRC Data Access";

        private StringBuilder sqlLog;

        protected TraceSource traceSource = null;
        protected TraceSource loanReviewTraceSource = null;

        #endregion Fields

        #region Properties

        public string SqlLog => this.sqlLog?.ToString() ?? String.Empty;

        public Action<string> DatabaseLog { get; set; }

        public TraceSource TraceSource => this.traceSource ?? (this.traceSource = new TraceSource(DataAccessBase.ApplicationName, SourceLevels.Information));

        #endregion Properties

        #region Constructor

        #region DataAccessBase
        protected DataAccessBase()
        {
            this.traceSource = new TraceSource(DataAccessBase.ApplicationName, SourceLevels.Information);
            AutoMapperDataAccessConfiguration.Configure();
            this.DatabaseLog = this.AppendToSqlLog;
            this.InitializeDbInterceptor();
        }
        #endregion DataAccessBase

        #endregion Constructor

        #region Methods

        private static bool IsInterceptorInitialized = false;

        #region InitializeDbInterceptor
        private void InitializeDbInterceptor()
        {
            if (Conversion.Configuration.GetBoolean("LogDbPerformance"))
            {
                if (!DataAccessBase.IsInterceptorInitialized)
                {
                    DbInterception.Add(new MrcLoggingInterceptor());
                    DataAccessBase.IsInterceptorInitialized = true;
                }
            }
        }
        #endregion InitializeDbInterceptor

        #region AppendToSqlLog
        private void AppendToSqlLog(string sql)
        {
            if (this.sqlLog == null)
            {
                this.sqlLog = new StringBuilder();
            }
            this.sqlLog.AppendLine(sql);
        }
        #endregion AppendToSqlLog

        #region ExecuteAsync
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        /// <remarks>
        /// Usage:
        ///     return await this.ExecuteAsync(entities => entities.Database.ExecuteSqlCommandAsync("select * from Table where Id = @id", new SqlParameter { ParameterName = "@id", Value = 0 }));
        /// </remarks>
        protected async Task<T> ExecuteAsync<T>(Func<MrcDataEntities, Task<T>> method)
        {
            try
            {
                using (var entities = new MrcDataEntities())
                {
                    entities.Database.Log = this.DatabaseLog;
                    return await method(entities).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.SqlLog, ex);
            }
        }
        #endregion ExecuteAsync

        #region ExecuteAsync
        protected async Task ExecuteAsync(Func<MrcDataEntities, Task> method)
        {
            try
            {
                using (var entities = new MrcDataEntities())
                {
                    entities.Database.Log = this.DatabaseLog;
                    await method(entities).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.SqlLog, ex);
            }
        }
        #endregion ExecuteAsync

        #region Execute
        protected T Execute<T>(Func<MrcDataEntities, T> method)
        {
            try
            {
                using (var entities = new MrcDataEntities())
                {
                    entities.Database.Log = this.DatabaseLog;
                    return method(entities);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.SqlLog, ex);
            }
        }
        #endregion Execute

        #region Execute
        protected void Execute(Action<MrcDataEntities> method)
        {
            try
            {
                using (var entities = new MrcDataEntities())
                {
                    entities.Database.Log = this.DatabaseLog;
                    method(entities);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.SqlLog, ex);
            }
        }
        #endregion Execute

        #region StartStopwatch
        public static Stopwatch StartStopwatch()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            return stopwatch;
        }
        #endregion StartStopwatch

        #region GetNullableSqlParameter
        /// <summary>
        /// Returns a 'SqlParameter' with a 'DBNull.Value' when the value provided is NULL.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static SqlParameter GetNullableSqlParameter(string parameterName, object value)
        {
            return value != null ? new SqlParameter { ParameterName = parameterName, Value = value } : new SqlParameter { ParameterName = parameterName, Value = DBNull.Value };
        }
        #endregion GetNullableSqlParameter

        #endregion Methods
    }
}
