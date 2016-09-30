using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Text;

using STAR.Enterprise.Trace;

namespace STAR.Originations.MRC.DataAccess
{
    public partial class MrcDataEntities
    {
        private readonly StringBuilder logs = new StringBuilder();

        public string LogText => String.Join(Environment.NewLine, this.logs);

        #region EnableLogging
        public bool EnableLogging
        {
            get
            {
                return this.Database.Log != null;
            }
            set
            {
                if (value)
                {
                    this.Database.Log = this.LogMessage;
                }
                else
                {
                    this.Database.Log = null;
                }
            }
        }
        #endregion EnableLogging

        #region LogMessage
        private void LogMessage(string message)
        {
            this.logs.AppendLine(message);
        }
        #endregion LogMessage
    }

    public class MrcLoggingInterceptor : DbCommandInterceptor
    {
        public const string ApplicationName = "MRC DB Performance";

        #region Fields

        private TraceSource traceSource = null;
        private readonly Stopwatch stopwatch = new Stopwatch();

        #endregion Fields

        #region Properties

        public TraceSource TraceSource => this.traceSource ?? (this.traceSource = new TraceSource(MrcLoggingInterceptor.ApplicationName, SourceLevels.Information));

        #endregion Properties

        #region Methods

        #region ScalarExecuting
        public override void ScalarExecuting(DbCommand dbCommand, DbCommandInterceptionContext<object> interceptionContext)
        {
            base.ScalarExecuting(dbCommand, interceptionContext);
            this.stopwatch.Restart();
        }
        #endregion ScalarExecuting

        #region ScalarExecuted
        public override void ScalarExecuted(DbCommand dbCommand, DbCommandInterceptionContext<object> interceptionContext)
        {
            this.stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                this.TraceSource.TraceException(interceptionContext.Exception, TraceEventType.Error, "FAILED", this.stopwatch.Elapsed, TraceStatus.Failed, new Dictionary<String, Object> { { "SQL", dbCommand.CommandText } });
            }
            else
            {
                this.TraceSource.TraceEvent(TraceEventType.Information, "COMPLETE", this.stopwatch.Elapsed, TraceStatus.Success, new Dictionary<String, Object> { { "SQL", dbCommand.CommandText } });
            }
            base.ScalarExecuted(dbCommand, interceptionContext);
        }
        #endregion ScalarExecuted

        #region NonQueryExecuting
        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            base.NonQueryExecuting(command, interceptionContext);
            this.stopwatch.Restart();
        }
        #endregion NonQueryExecuting

        #region NonQueryExecuted
        public override void NonQueryExecuted(DbCommand dbCommand, DbCommandInterceptionContext<int> interceptionContext)
        {
            this.stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                this.TraceSource.TraceException(interceptionContext.Exception, TraceEventType.Error, "FAILED", this.stopwatch.Elapsed, TraceStatus.Failed, new Dictionary<String, Object> { { "SQL", dbCommand.CommandText } });
            }
            else
            {
                this.TraceSource.TraceEvent(TraceEventType.Information, "COMPLETE", this.stopwatch.Elapsed, TraceStatus.Success, new Dictionary<String, Object> { { "SQL", dbCommand.CommandText } });
            }
            base.NonQueryExecuted(dbCommand, interceptionContext);
        }
        #endregion NonQueryExecuted

        #region ReaderExecuting
        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            base.ReaderExecuting(command, interceptionContext);
            this.stopwatch.Restart();
        }
        #endregion ReaderExecuting

        #region ReaderExecuted
        public override void ReaderExecuted(DbCommand dbCommand, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            this.stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                this.TraceSource.TraceException(interceptionContext.Exception, TraceEventType.Error, "FAILED", this.stopwatch.Elapsed, TraceStatus.Failed, new Dictionary<String, Object> { { "SQL", dbCommand.CommandText } });
            }
            else
            {
                this.TraceSource.TraceEvent(TraceEventType.Information, "COMPLETE", this.stopwatch.Elapsed, TraceStatus.Success, new Dictionary<String, Object> { { "SQL", dbCommand.CommandText } });
            }
            base.ReaderExecuted(dbCommand, interceptionContext);
        }
        #endregion ReaderExecuted

        #endregion Methods
    }
}
