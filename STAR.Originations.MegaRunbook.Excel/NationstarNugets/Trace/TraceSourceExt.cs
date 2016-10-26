// Decompiled with JetBrains decompiler
// Type: STAR.Enterprise.Trace.TraceSourceExt
// Assembly: STAR.Enterprise.Trace, Version=1.1.5697.29972, Culture=neutral, PublicKeyToken=null
// MVID: 515909EF-777E-43BB-B206-39FDB6D66C81
// Assembly location: C:\Git\Repos\CapitalMarkets\Solutions\packages\STAR.Enterprise.Trace.1.1.0\lib\net451\STAR.Enterprise.Trace.dll

using STAR.Enterprise.Trace.Contracts.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;

namespace STAR.Enterprise.Trace
{
    public static class TraceSourceExt
    {
        private static readonly EventDataBuilder EventDataBuilder = new EventDataBuilder();
        private const string TraceKeyName = "STAR.Enterprise.Trace.Keys";

        public static void SetKeys(this TraceSource traceSource, NameValueCollection keys)
        {
            if (keys == null || keys.Count <= 0)
                return;
            CallContext.LogicalSetData("STAR.Enterprise.Trace.Keys", (object)keys);
        }

        private static NameValueCollection GetKeys()
        {
            return CallContext.LogicalGetData("STAR.Enterprise.Trace.Keys") as NameValueCollection;
        }

        /// <summary>
        /// Writes a trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="duration">The trace event duration.</param>
        /// <param name="status">One of the enumeration values that specifies the event status of the trace event.</param>
        /// <param name="exception">THe exception that to trace as part of the trace event.</param>
        /// <param name="data">The trace data.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        private static void TraceEventInternal(this TraceSource traceSource, TraceEventType traceEventType, string message, TimeSpan duration, TraceStatus status, Exception exception, object data, string methodName)
        {
            try
            {
                if (!TraceSourceExt.ShouldTrace(traceSource, traceEventType))
                    return;
                IEnumerable<KeyValuePair<string, object>> data1 = RawBlobParser.ParseItem(data);
                TraceSourceExt.TraceEventInternal(traceSource, traceEventType, message, duration, status, exception, data1, methodName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("An error occurred while tracing. {0}", (object)ex.Message));
            }
        }

        /// <summary>
        /// Writes a trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="duration">The trace event duration.</param>
        /// <param name="status">One of the enumeration values that specifies the event status of the trace event.</param>
        /// <param name="exception">THe exception that to trace as part of the trace event.</param>
        /// <param name="data">The trace data.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        private static void TraceEventWithDataArrayInternal(this TraceSource traceSource, TraceEventType traceEventType, string message, TimeSpan duration, TraceStatus status, Exception exception, object[] data, string methodName)
        {
            try
            {
                if (!TraceSourceExt.ShouldTrace(traceSource, traceEventType))
                    return;
                ICollection<KeyValuePair<string, object>> items = RawBlobParser.ParseItems((IList<object>)data);
                TraceSourceExt.TraceEventInternal(traceSource, traceEventType, message, duration, status, exception, (IEnumerable<KeyValuePair<string, object>>)items, methodName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("An error occurred while tracing. {0}", (object)ex.Message));
            }
        }

        /// <summary>
        /// Writes a trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="duration">The trace event duration.</param>
        /// <param name="status">One of the enumeration values that specifies the event status of the trace event.</param>
        /// <param name="exception">THe exception that to trace as part of the trace event.</param>
        /// <param name="data">The trace data.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        internal static void TraceEventInternal(this TraceSource traceSource, TraceEventType traceEventType, string message, TimeSpan duration, TraceStatus status, Exception exception, IEnumerable<KeyValuePair<string, object>> data, string methodName)
        {
            try
            {
                if (!TraceSourceExt.ShouldTrace(traceSource, traceEventType))
                    return;
                TraceEventEntry traceEventEntry = TraceEventEntryBuilder.IncludeEnvironmentData(new TraceEventEntry()
                {
                    ApplicationName = traceSource.Name,
                    EventData = TraceSourceExt.EventDataBuilder.CreateTraceEventData(exception, data),
                    EventDuration = duration,
                    EventKeys = TraceSourceExt.EventDataBuilder.CreateEventKeys(TraceSourceExt.GetKeys()),
                    EventType = traceEventType.ToString(),
                    Message = message,
                    MethodName = methodName,
                    Status = (int)status
                });
                traceSource.TraceData(traceEventType, 0, (object)traceEventEntry);
                traceSource.Flush();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("An error occurred while tracing. {0}", (object)ex.Message));
            }
        }

        private static bool ShouldTrace(TraceSource traceSource, TraceEventType traceEventType)
        {
            if (traceSource.Switch == null || !traceSource.Switch.ShouldTrace(traceEventType))
                return false;
            TraceListenerCollection listeners = traceSource.Listeners;
            // ISSUE: explicit non-virtual call
            return (listeners != null ? listeners.Count : 0) > 0;
        }

        /// <summary>
        /// Writes a trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="duration">The trace event duration.</param>
        /// <param name="status">One of the enumeration values that specifies the event status of the trace event.</param>
        /// <param name="data">The trace data.</param>
        public static void TraceEvent(this TraceSource traceSource, TraceEventType traceEventType, string message, TimeSpan duration, TraceStatus status, params object[] data)
        {
            traceSource.TraceEventWithDataArrayInternal(traceEventType, message, duration, status, (Exception)null, data, (string)null);
        }

        /// <summary>
        /// Writes a trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="duration">The trace event duration.</param>
        /// <param name="status">One of the enumeration values that specifies the event status of the trace event.</param>
        /// <param name="data">The trace data.  The keys will be the Trace Data type and the values will be logged as the Trace Data.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        public static void TraceEvent(this TraceSource traceSource, TraceEventType traceEventType, string message, TimeSpan duration, TraceStatus status, Dictionary<string, object> data, [CallerMemberName] string methodName = null)
        {
            TraceSourceExt.TraceEventInternal(traceSource, traceEventType, message, duration, status, (Exception)null, (IEnumerable<KeyValuePair<string, object>>)data, methodName);
        }

        /// <summary>
        /// Writes a trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="duration">The trace event duration.</param>
        /// <param name="status">One of the enumeration values that specifies the event status of the trace event.</param>
        /// <param name="data">The trace data.  If this is an anonymous object, the member properties will be treated as the Trace Data and the property name will be the label.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        public static void TraceEvent(this TraceSource traceSource, TraceEventType traceEventType, string message, TimeSpan duration, TraceStatus status, object data, [CallerMemberName] string methodName = null)
        {
            traceSource.TraceEventInternal(traceEventType, message, duration, status, (Exception)null, data, methodName);
        }

        /// <summary>
        /// Writes a trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="duration">The trace event duration.</param>
        /// <param name="status">One of the enumeration values that specifies the event status of the trace event.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        public static void TraceEvent(this TraceSource traceSource, TraceEventType traceEventType, string message, TimeSpan duration, TraceStatus status, [CallerMemberName] string methodName = null)
        {
            TraceSourceExt.TraceEventInternal(traceSource, traceEventType, message, duration, status, (Exception)null, (IEnumerable<KeyValuePair<string, object>>)null, methodName);
        }

        /// <summary>
        /// Writes a trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="duration">The trace event duration.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        public static void TraceEvent(this TraceSource traceSource, TraceEventType traceEventType, string message, TimeSpan duration, [CallerMemberName] string methodName = null)
        {
            TraceSourceExt.TraceEventInternal(traceSource, traceEventType, message, duration, TraceStatus.Request, (Exception)null, (IEnumerable<KeyValuePair<string, object>>)null, methodName);
        }

        /// <summary>Posts a trace event to the TraceListener.</summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">The 'TraceTypeEvent'. Defaults to 'Information'.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        public static void TraceEvent(this TraceSource traceSource, TraceEventType traceEventType, string message, [CallerMemberName] string methodName = null)
        {
            TraceSourceExt.TraceEventInternal(traceSource, traceEventType, message, TimeSpan.Zero, TraceStatus.Request, (Exception)null, (IEnumerable<KeyValuePair<string, object>>)null, methodName);
        }

        /// <summary>Posts a trace event to the TraceListener.</summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        public static void TraceEvent(this TraceSource traceSource, string message, [CallerMemberName] string methodName = null)
        {
            TraceSourceExt.TraceEventInternal(traceSource, TraceEventType.Verbose, message, TimeSpan.Zero, TraceStatus.Request, (Exception)null, (IEnumerable<KeyValuePair<string, object>>)null, methodName);
        }

        /// <summary>
        /// Writes a exception trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="exception">The exception data.</param>
        /// <param name="duration">The trace event duration.</param>
        /// <param name="status">One of the enumeration values that specifies the event status of the trace event.</param>
        /// <param name="data">The trace data.</param>
        public static void TraceException(this TraceSource traceSource, Exception exception, TraceEventType traceEventType, string message, TimeSpan duration, TraceStatus status, params object[] data)
        {
            traceSource.TraceEventWithDataArrayInternal(traceEventType, message, duration, status, exception, data, (string)null);
        }

        /// <summary>
        /// Writes a exception trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="exception">The exception data.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="duration">The trace event duration.</param>
        /// <param name="status">One of the enumeration values that specifies the event status of the trace event.</param>
        /// <param name="data">The trace data.  The keys will be the Trace Data type and the values will be logged as the Trace Data.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        public static void TraceException(this TraceSource traceSource, Exception exception, TraceEventType traceEventType, string message, TimeSpan duration, TraceStatus status, Dictionary<string, object> data, [CallerMemberName] string methodName = null)
        {
            TraceSourceExt.TraceEventInternal(traceSource, traceEventType, message, duration, status, exception, (IEnumerable<KeyValuePair<string, object>>)data, methodName);
        }

        /// <summary>
        /// Writes a exception trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="exception">The exception data.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="duration">The trace event duration.</param>
        /// <param name="status">One of the enumeration values that specifies the event status of the trace event.</param>
        /// <param name="data">The trace data.  If this is an anonymous object, the member properties will be treated as the Trace Data and the property name will be the label.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        public static void TraceException(this TraceSource traceSource, Exception exception, TraceEventType traceEventType, string message, TimeSpan duration, TraceStatus status, object data, [CallerMemberName] string methodName = null)
        {
            traceSource.TraceEventInternal(traceEventType, message, duration, status, exception, data, methodName);
        }

        /// <summary>
        /// Writes a exception trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="exception">The exception data.</param>
        /// <param name="duration">The trace event duration.</param>
        /// <param name="status">One of the enumeration values that specifies the event status of the trace event.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        public static void TraceException(this TraceSource traceSource, Exception exception, TraceEventType traceEventType, string message, TimeSpan duration, TraceStatus status, [CallerMemberName] string methodName = null)
        {
            TraceSourceExt.TraceEventInternal(traceSource, traceEventType, message, duration, status, exception, (IEnumerable<KeyValuePair<string, object>>)null, methodName);
        }

        /// <summary>
        /// Writes a exception trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="exception">The exception data.</param>
        /// <param name="duration">The trace event duration.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        public static void TraceException(this TraceSource traceSource, Exception exception, TraceEventType traceEventType, string message, TimeSpan duration, [CallerMemberName] string methodName = null)
        {
            TraceSourceExt.TraceEventInternal(traceSource, traceEventType, message, duration, TraceStatus.Failed, exception, (IEnumerable<KeyValuePair<string, object>>)null, methodName);
        }

        /// <summary>
        /// Writes a exception trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="exception">The exception data.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        public static void TraceException(this TraceSource traceSource, Exception exception, TraceEventType traceEventType, string message, [CallerMemberName] string methodName = null)
        {
            TraceSourceExt.TraceEventInternal(traceSource, traceEventType, message, TimeSpan.Zero, TraceStatus.Failed, exception, (IEnumerable<KeyValuePair<string, object>>)null, methodName);
        }

        /// <summary>
        /// Writes a exception trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="exception">The exception data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        [Obsolete("This method is not easily accessible for it's intended use")]
        public static void TraceException(this TraceSource traceSource, Exception exception, string message, [CallerMemberName] string methodName = null)
        {
            TraceSourceExt.TraceEventInternal(traceSource, TraceEventType.Error, message, TimeSpan.Zero, TraceStatus.Failed, exception, (IEnumerable<KeyValuePair<string, object>>)null, methodName);
        }

        /// <summary>
        /// Writes a exception trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="exception">The exception data.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        public static void TraceException(this TraceSource traceSource, Exception exception, [CallerMemberName] string methodName = null)
        {
            string message = (exception != null ? exception.Message : (string)null) ?? string.Empty;
            TraceSourceExt.TraceEventInternal(traceSource, TraceEventType.Error, message, TimeSpan.Zero, TraceStatus.Failed, exception, (IEnumerable<KeyValuePair<string, object>>)null, methodName);
        }

        /// <summary>
        /// Writes a exception trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="exception">The exception data.</param>
        /// <param name="duration">The trace event duration.</param>
        /// <param name="status">One of the enumeration values that specifies the event status of the trace event.</param>
        /// <param name="data">The trace data.</param>
        [Obsolete("Use version that passes in the exception as the first parameter", false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void TraceException(this TraceSource traceSource, TraceEventType traceEventType, string message, Exception exception, TimeSpan duration, TraceStatus status, params object[] data)
        {
            traceSource.TraceEventWithDataArrayInternal(traceEventType, message, duration, status, exception, data, (string)null);
        }

        /// <summary>
        /// Writes a exception trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="exception">The exception data.</param>
        /// <param name="duration">The trace event duration.</param>
        /// <param name="status">One of the enumeration values that specifies the event status of the trace event.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        [Obsolete("Use version that passes in the exception as the first parameter", false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void TraceException(this TraceSource traceSource, TraceEventType traceEventType, string message, Exception exception, TimeSpan duration, TraceStatus status, [CallerMemberName] string methodName = null)
        {
            TraceSourceExt.TraceEventInternal(traceSource, traceEventType, message, duration, status, exception, (IEnumerable<KeyValuePair<string, object>>)null, methodName);
        }

        /// <summary>
        /// Writes a exception trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="exception">The exception data.</param>
        /// <param name="duration">The trace event duration.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        [Obsolete("Use version that passes in the exception as the first parameter", false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void TraceException(this TraceSource traceSource, TraceEventType traceEventType, string message, Exception exception, TimeSpan duration, [CallerMemberName] string methodName = null)
        {
            TraceSourceExt.TraceEventInternal(traceSource, traceEventType, message, duration, TraceStatus.Failed, exception, (IEnumerable<KeyValuePair<string, object>>)null, methodName);
        }

        /// <summary>
        /// Writes a exception trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="traceEventType">One of the enumeration values that specifies the event type of the trace data.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="exception">The exception data.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        [Obsolete("Use version that passes in the exception as the first parameter", false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void TraceException(this TraceSource traceSource, TraceEventType traceEventType, string message, Exception exception, [CallerMemberName] string methodName = null)
        {
            TraceSourceExt.TraceEventInternal(traceSource, traceEventType, message, TimeSpan.Zero, TraceStatus.Failed, exception, (IEnumerable<KeyValuePair<string, object>>)null, methodName);
        }

        /// <summary>
        /// Writes a exception trace event message to the trace listeners in the Listeners collection using the specified event type and event identifier.
        /// </summary>
        /// <param name="traceSource">The 'TraceSource' instance.</param>
        /// <param name="message">The informative message to write.</param>
        /// <param name="exception">The exception data.</param>
        /// <param name="methodName">The calling method where the log originated from</param>
        [Obsolete("Use version that passes in the exception as the first parameter", false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void TraceException(this TraceSource traceSource, string message, Exception exception, [CallerMemberName] string methodName = null)
        {
            TraceSourceExt.TraceEventInternal(traceSource, TraceEventType.Error, message, TimeSpan.Zero, TraceStatus.Failed, exception, (IEnumerable<KeyValuePair<string, object>>)null, methodName);
        }
    }
}
